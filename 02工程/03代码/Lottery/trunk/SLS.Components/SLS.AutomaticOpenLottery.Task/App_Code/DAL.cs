using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections;

using Shove.Database;

namespace DAL
{
    /*
    Program Name: Shove.DAL.30
    Program Version: 3.0
    Writer By: 3km.shovesoft.shove (zhou changjun)
    Release Time: 2008.9.1

    System Request: Shove.dll
    All Rights saved.
    */

    public class Tables
    {
        public class T_ActiveAllBuyStar : MSSQL.TableBase
        {
            public MSSQL.Field ID;
            public MSSQL.Field LotterieID;
            public MSSQL.Field UserList;
            public MSSQL.Field Order;

            public T_ActiveAllBuyStar()
            {
                TableName = "T_ActiveAllBuyStar";

                ID = new MSSQL.Field(this, "ID", "ID", SqlDbType.Int, true);
                LotterieID = new MSSQL.Field(this, "LotterieID", "LotterieID", SqlDbType.Int, false);
                UserList = new MSSQL.Field(this, "UserList", "UserList", SqlDbType.VarChar, false);
                Order = new MSSQL.Field(this, "Order", "Order", SqlDbType.Int, false);
            }
        }

        public class T_Activities21CN : MSSQL.TableBase
        {
            public MSSQL.Field ID;
            public MSSQL.Field UserID;
            public MSSQL.Field DateTime;
            public MSSQL.Field AlipayName;
            public MSSQL.Field IsReward1;
            public MSSQL.Field DayBalanceAdd;
            public MSSQL.Field IsReward2;
            public MSSQL.Field DaySchemeMoney;
            public MSSQL.Field IsReward10;
            public MSSQL.Field DayWinMoney;
            public MSSQL.Field IsReward200;

            public T_Activities21CN()
            {
                TableName = "T_Activities21CN";

                ID = new MSSQL.Field(this, "ID", "ID", SqlDbType.BigInt, true);
                UserID = new MSSQL.Field(this, "UserID", "UserID", SqlDbType.BigInt, false);
                DateTime = new MSSQL.Field(this, "DateTime", "DateTime", SqlDbType.DateTime, false);
                AlipayName = new MSSQL.Field(this, "AlipayName", "AlipayName", SqlDbType.VarChar, false);
                IsReward1 = new MSSQL.Field(this, "IsReward1", "IsReward1", SqlDbType.Bit, false);
                DayBalanceAdd = new MSSQL.Field(this, "DayBalanceAdd", "DayBalanceAdd", SqlDbType.Money, false);
                IsReward2 = new MSSQL.Field(this, "IsReward2", "IsReward2", SqlDbType.Bit, false);
                DaySchemeMoney = new MSSQL.Field(this, "DaySchemeMoney", "DaySchemeMoney", SqlDbType.Money, false);
                IsReward10 = new MSSQL.Field(this, "IsReward10", "IsReward10", SqlDbType.Bit, false);
                DayWinMoney = new MSSQL.Field(this, "DayWinMoney", "DayWinMoney", SqlDbType.Money, false);
                IsReward200 = new MSSQL.Field(this, "IsReward200", "IsReward200", SqlDbType.Bit, false);
            }
        }

        public class T_Activities360 : MSSQL.TableBase
        {
            public MSSQL.Field ID;
            public MSSQL.Field UserID;
            public MSSQL.Field DateTime;
            public MSSQL.Field AlipayName;
            public MSSQL.Field IsReward1;
            public MSSQL.Field DayBalanceAdd;
            public MSSQL.Field IsReward2;
            public MSSQL.Field DaySchemeMoney;
            public MSSQL.Field IsReward10;
            public MSSQL.Field DayWinMoney;
            public MSSQL.Field IsReward200;

            public T_Activities360()
            {
                TableName = "T_Activities360";

                ID = new MSSQL.Field(this, "ID", "ID", SqlDbType.BigInt, true);
                UserID = new MSSQL.Field(this, "UserID", "UserID", SqlDbType.BigInt, false);
                DateTime = new MSSQL.Field(this, "DateTime", "DateTime", SqlDbType.DateTime, false);
                AlipayName = new MSSQL.Field(this, "AlipayName", "AlipayName", SqlDbType.VarChar, false);
                IsReward1 = new MSSQL.Field(this, "IsReward1", "IsReward1", SqlDbType.Bit, false);
                DayBalanceAdd = new MSSQL.Field(this, "DayBalanceAdd", "DayBalanceAdd", SqlDbType.Money, false);
                IsReward2 = new MSSQL.Field(this, "IsReward2", "IsReward2", SqlDbType.Bit, false);
                DaySchemeMoney = new MSSQL.Field(this, "DaySchemeMoney", "DaySchemeMoney", SqlDbType.Money, false);
                IsReward10 = new MSSQL.Field(this, "IsReward10", "IsReward10", SqlDbType.Bit, false);
                DayWinMoney = new MSSQL.Field(this, "DayWinMoney", "DayWinMoney", SqlDbType.Money, false);
                IsReward200 = new MSSQL.Field(this, "IsReward200", "IsReward200", SqlDbType.Bit, false);
            }
        }

        public class T_ActivitiesAlipay : MSSQL.TableBase
        {
            public MSSQL.Field ID;
            public MSSQL.Field UserID;
            public MSSQL.Field DateTime;
            public MSSQL.Field AlipayName;
            public MSSQL.Field IsReward1;
            public MSSQL.Field DayBalanceAdd;
            public MSSQL.Field IsReward2;
            public MSSQL.Field DaySchemeMoney;
            public MSSQL.Field IsReward10;
            public MSSQL.Field DayWinMoney;
            public MSSQL.Field IsReward200;

            public T_ActivitiesAlipay()
            {
                TableName = "T_ActivitiesAlipay";

                ID = new MSSQL.Field(this, "ID", "ID", SqlDbType.BigInt, true);
                UserID = new MSSQL.Field(this, "UserID", "UserID", SqlDbType.BigInt, false);
                DateTime = new MSSQL.Field(this, "DateTime", "DateTime", SqlDbType.DateTime, false);
                AlipayName = new MSSQL.Field(this, "AlipayName", "AlipayName", SqlDbType.VarChar, false);
                IsReward1 = new MSSQL.Field(this, "IsReward1", "IsReward1", SqlDbType.Bit, false);
                DayBalanceAdd = new MSSQL.Field(this, "DayBalanceAdd", "DayBalanceAdd", SqlDbType.Money, false);
                IsReward2 = new MSSQL.Field(this, "IsReward2", "IsReward2", SqlDbType.Bit, false);
                DaySchemeMoney = new MSSQL.Field(this, "DaySchemeMoney", "DaySchemeMoney", SqlDbType.Money, false);
                IsReward10 = new MSSQL.Field(this, "IsReward10", "IsReward10", SqlDbType.Bit, false);
                DayWinMoney = new MSSQL.Field(this, "DayWinMoney", "DayWinMoney", SqlDbType.Money, false);
                IsReward200 = new MSSQL.Field(this, "IsReward200", "IsReward200", SqlDbType.Bit, false);
            }
        }

        public class T_ActivitiesMytv365 : MSSQL.TableBase
        {
            public MSSQL.Field ID;
            public MSSQL.Field UserID;
            public MSSQL.Field DateTime;
            public MSSQL.Field AlipayName;
            public MSSQL.Field IsReward1;
            public MSSQL.Field DayBalanceAdd;
            public MSSQL.Field IsReward2;
            public MSSQL.Field DaySchemeMoney;
            public MSSQL.Field IsReward10;
            public MSSQL.Field DayWinMoney;
            public MSSQL.Field IsReward200;

            public T_ActivitiesMytv365()
            {
                TableName = "T_ActivitiesMytv365";

                ID = new MSSQL.Field(this, "ID", "ID", SqlDbType.BigInt, true);
                UserID = new MSSQL.Field(this, "UserID", "UserID", SqlDbType.BigInt, false);
                DateTime = new MSSQL.Field(this, "DateTime", "DateTime", SqlDbType.DateTime, false);
                AlipayName = new MSSQL.Field(this, "AlipayName", "AlipayName", SqlDbType.VarChar, false);
                IsReward1 = new MSSQL.Field(this, "IsReward1", "IsReward1", SqlDbType.Bit, false);
                DayBalanceAdd = new MSSQL.Field(this, "DayBalanceAdd", "DayBalanceAdd", SqlDbType.Money, false);
                IsReward2 = new MSSQL.Field(this, "IsReward2", "IsReward2", SqlDbType.Bit, false);
                DaySchemeMoney = new MSSQL.Field(this, "DaySchemeMoney", "DaySchemeMoney", SqlDbType.Money, false);
                IsReward10 = new MSSQL.Field(this, "IsReward10", "IsReward10", SqlDbType.Bit, false);
                DayWinMoney = new MSSQL.Field(this, "DayWinMoney", "DayWinMoney", SqlDbType.Money, false);
                IsReward200 = new MSSQL.Field(this, "IsReward200", "IsReward200", SqlDbType.Bit, false);
            }
        }

        public class T_ActivitiesZJL : MSSQL.TableBase
        {
            public MSSQL.Field ID;
            public MSSQL.Field UserID;
            public MSSQL.Field DateTime;
            public MSSQL.Field AlipayName;
            public MSSQL.Field IsReward1;
            public MSSQL.Field DayBalanceAdd;
            public MSSQL.Field IsReward2;
            public MSSQL.Field DaySchemeMoney;
            public MSSQL.Field IsReward10;
            public MSSQL.Field DayWinMoney;
            public MSSQL.Field IsReward200;

            public T_ActivitiesZJL()
            {
                TableName = "T_ActivitiesZJL";

                ID = new MSSQL.Field(this, "ID", "ID", SqlDbType.BigInt, true);
                UserID = new MSSQL.Field(this, "UserID", "UserID", SqlDbType.BigInt, false);
                DateTime = new MSSQL.Field(this, "DateTime", "DateTime", SqlDbType.DateTime, false);
                AlipayName = new MSSQL.Field(this, "AlipayName", "AlipayName", SqlDbType.VarChar, false);
                IsReward1 = new MSSQL.Field(this, "IsReward1", "IsReward1", SqlDbType.Bit, false);
                DayBalanceAdd = new MSSQL.Field(this, "DayBalanceAdd", "DayBalanceAdd", SqlDbType.Money, false);
                IsReward2 = new MSSQL.Field(this, "IsReward2", "IsReward2", SqlDbType.Bit, false);
                DaySchemeMoney = new MSSQL.Field(this, "DaySchemeMoney", "DaySchemeMoney", SqlDbType.Money, false);
                IsReward10 = new MSSQL.Field(this, "IsReward10", "IsReward10", SqlDbType.Bit, false);
                DayWinMoney = new MSSQL.Field(this, "DayWinMoney", "DayWinMoney", SqlDbType.Money, false);
                IsReward200 = new MSSQL.Field(this, "IsReward200", "IsReward200", SqlDbType.Bit, false);
            }
        }

        public class T_Advertisements : MSSQL.TableBase
        {
            public MSSQL.Field ID;
            public MSSQL.Field LotteryID;
            public MSSQL.Field Name;
            public MSSQL.Field Title;
            public MSSQL.Field Url;
            public MSSQL.Field DateTime;
            public MSSQL.Field Order;
            public MSSQL.Field isShow;

            public T_Advertisements()
            {
                TableName = "T_Advertisements";

                ID = new MSSQL.Field(this, "ID", "ID", SqlDbType.Int, true);
                LotteryID = new MSSQL.Field(this, "LotteryID", "LotteryID", SqlDbType.Int, false);
                Name = new MSSQL.Field(this, "Name", "Name", SqlDbType.VarChar, false);
                Title = new MSSQL.Field(this, "Title", "Title", SqlDbType.VarChar, false);
                Url = new MSSQL.Field(this, "Url", "Url", SqlDbType.VarChar, false);
                DateTime = new MSSQL.Field(this, "DateTime", "DateTime", SqlDbType.DateTime, false);
                Order = new MSSQL.Field(this, "Order", "Order", SqlDbType.Int, false);
                isShow = new MSSQL.Field(this, "isShow", "isShow", SqlDbType.Bit, false);
            }
        }

        public class T_AlipayBuyTemp : MSSQL.TableBase
        {
            public MSSQL.Field ID;
            public MSSQL.Field SiteID;
            public MSSQL.Field UserID;
            public MSSQL.Field DateTime;
            public MSSQL.Field Money;
            public MSSQL.Field HandleResult;
            public MSSQL.Field SchemeID;
            public MSSQL.Field ChaseTaskID;
            public MSSQL.Field IsChase;
            public MSSQL.Field IsCoBuy;
            public MSSQL.Field LotteryID;
            public MSSQL.Field IsuseID;
            public MSSQL.Field PlayTypeID;
            public MSSQL.Field StopwhenwinMoney;
            public MSSQL.Field AdditionasXml;
            public MSSQL.Field Title;
            public MSSQL.Field Multiple;
            public MSSQL.Field BuyMoney;
            public MSSQL.Field SumMoney;
            public MSSQL.Field AssureMoney;
            public MSSQL.Field Share;
            public MSSQL.Field BuyShare;
            public MSSQL.Field AssureShare;
            public MSSQL.Field SecrecyLevel;
            public MSSQL.Field Description;
            public MSSQL.Field LotteryNumber;
            public MSSQL.Field UpdateloadFileContent;
            public MSSQL.Field OpenUsers;
            public MSSQL.Field Number;

            public T_AlipayBuyTemp()
            {
                TableName = "T_AlipayBuyTemp";

                ID = new MSSQL.Field(this, "ID", "ID", SqlDbType.BigInt, true);
                SiteID = new MSSQL.Field(this, "SiteID", "SiteID", SqlDbType.BigInt, false);
                UserID = new MSSQL.Field(this, "UserID", "UserID", SqlDbType.BigInt, false);
                DateTime = new MSSQL.Field(this, "DateTime", "DateTime", SqlDbType.DateTime, false);
                Money = new MSSQL.Field(this, "Money", "Money", SqlDbType.Money, false);
                HandleResult = new MSSQL.Field(this, "HandleResult", "HandleResult", SqlDbType.SmallInt, false);
                SchemeID = new MSSQL.Field(this, "SchemeID", "SchemeID", SqlDbType.BigInt, false);
                ChaseTaskID = new MSSQL.Field(this, "ChaseTaskID", "ChaseTaskID", SqlDbType.BigInt, false);
                IsChase = new MSSQL.Field(this, "IsChase", "IsChase", SqlDbType.Bit, false);
                IsCoBuy = new MSSQL.Field(this, "IsCoBuy", "IsCoBuy", SqlDbType.Bit, false);
                LotteryID = new MSSQL.Field(this, "LotteryID", "LotteryID", SqlDbType.Int, false);
                IsuseID = new MSSQL.Field(this, "IsuseID", "IsuseID", SqlDbType.BigInt, false);
                PlayTypeID = new MSSQL.Field(this, "PlayTypeID", "PlayTypeID", SqlDbType.Int, false);
                StopwhenwinMoney = new MSSQL.Field(this, "StopwhenwinMoney", "StopwhenwinMoney", SqlDbType.Money, false);
                AdditionasXml = new MSSQL.Field(this, "AdditionasXml", "AdditionasXml", SqlDbType.NText, false);
                Title = new MSSQL.Field(this, "Title", "Title", SqlDbType.VarChar, false);
                Multiple = new MSSQL.Field(this, "Multiple", "Multiple", SqlDbType.Int, false);
                BuyMoney = new MSSQL.Field(this, "BuyMoney", "BuyMoney", SqlDbType.Money, false);
                SumMoney = new MSSQL.Field(this, "SumMoney", "SumMoney", SqlDbType.Money, false);
                AssureMoney = new MSSQL.Field(this, "AssureMoney", "AssureMoney", SqlDbType.Money, false);
                Share = new MSSQL.Field(this, "Share", "Share", SqlDbType.Int, false);
                BuyShare = new MSSQL.Field(this, "BuyShare", "BuyShare", SqlDbType.Int, false);
                AssureShare = new MSSQL.Field(this, "AssureShare", "AssureShare", SqlDbType.Int, false);
                SecrecyLevel = new MSSQL.Field(this, "SecrecyLevel", "SecrecyLevel", SqlDbType.SmallInt, false);
                Description = new MSSQL.Field(this, "Description", "Description", SqlDbType.VarChar, false);
                LotteryNumber = new MSSQL.Field(this, "LotteryNumber", "LotteryNumber", SqlDbType.VarChar, false);
                UpdateloadFileContent = new MSSQL.Field(this, "UpdateloadFileContent", "UpdateloadFileContent", SqlDbType.VarChar, false);
                OpenUsers = new MSSQL.Field(this, "OpenUsers", "OpenUsers", SqlDbType.VarChar, false);
                Number = new MSSQL.Field(this, "Number", "Number", SqlDbType.Int, false);
            }
        }

        public class T_AlipayRegDonate : MSSQL.TableBase
        {
            public MSSQL.Field ID;
            public MSSQL.Field UserID;
            public MSSQL.Field DateTime;
            public MSSQL.Field AlipayName;
            public MSSQL.Field HandleResult;

            public T_AlipayRegDonate()
            {
                TableName = "T_AlipayRegDonate";

                ID = new MSSQL.Field(this, "ID", "ID", SqlDbType.BigInt, true);
                UserID = new MSSQL.Field(this, "UserID", "UserID", SqlDbType.BigInt, false);
                DateTime = new MSSQL.Field(this, "DateTime", "DateTime", SqlDbType.DateTime, false);
                AlipayName = new MSSQL.Field(this, "AlipayName", "AlipayName", SqlDbType.VarChar, false);
                HandleResult = new MSSQL.Field(this, "HandleResult", "HandleResult", SqlDbType.SmallInt, false);
            }
        }

        public class T_BankDetails : MSSQL.TableBase
        {
            public MSSQL.Field ID;
            public MSSQL.Field ProvinceName;
            public MSSQL.Field CityName;
            public MSSQL.Field BankTypeName;
            public MSSQL.Field BankName;

            public T_BankDetails()
            {
                TableName = "T_BankDetails";

                ID = new MSSQL.Field(this, "ID", "ID", SqlDbType.BigInt, true);
                ProvinceName = new MSSQL.Field(this, "ProvinceName", "ProvinceName", SqlDbType.VarChar, false);
                CityName = new MSSQL.Field(this, "CityName", "CityName", SqlDbType.VarChar, false);
                BankTypeName = new MSSQL.Field(this, "BankTypeName", "BankTypeName", SqlDbType.VarChar, false);
                BankName = new MSSQL.Field(this, "BankName", "BankName", SqlDbType.VarChar, false);
            }
        }

        public class T_Banks : MSSQL.TableBase
        {
            public MSSQL.Field ID;
            public MSSQL.Field Name;
            public MSSQL.Field Order;

            public T_Banks()
            {
                TableName = "T_Banks";

                ID = new MSSQL.Field(this, "ID", "ID", SqlDbType.SmallInt, false);
                Name = new MSSQL.Field(this, "Name", "Name", SqlDbType.VarChar, false);
                Order = new MSSQL.Field(this, "Order", "Order", SqlDbType.SmallInt, false);
            }
        }

        public class T_BuyDetails : MSSQL.TableBase
        {
            public MSSQL.Field ID;
            public MSSQL.Field SiteID;
            public MSSQL.Field UserID;
            public MSSQL.Field DateTime;
            public MSSQL.Field SchemeID;
            public MSSQL.Field Share;
            public MSSQL.Field QuashStatus;
            public MSSQL.Field isWhenInitiate;
            public MSSQL.Field WinMoneyNoWithTax;
            public MSSQL.Field isAutoFollowScheme;
            public MSSQL.Field DetailMoney;

            public T_BuyDetails()
            {
                TableName = "T_BuyDetails";

                ID = new MSSQL.Field(this, "ID", "ID", SqlDbType.BigInt, true);
                SiteID = new MSSQL.Field(this, "SiteID", "SiteID", SqlDbType.BigInt, false);
                UserID = new MSSQL.Field(this, "UserID", "UserID", SqlDbType.BigInt, false);
                DateTime = new MSSQL.Field(this, "DateTime", "DateTime", SqlDbType.DateTime, false);
                SchemeID = new MSSQL.Field(this, "SchemeID", "SchemeID", SqlDbType.BigInt, false);
                Share = new MSSQL.Field(this, "Share", "Share", SqlDbType.Int, false);
                QuashStatus = new MSSQL.Field(this, "QuashStatus", "QuashStatus", SqlDbType.SmallInt, false);
                isWhenInitiate = new MSSQL.Field(this, "isWhenInitiate", "isWhenInitiate", SqlDbType.Bit, false);
                WinMoneyNoWithTax = new MSSQL.Field(this, "WinMoneyNoWithTax", "WinMoneyNoWithTax", SqlDbType.Money, false);
                isAutoFollowScheme = new MSSQL.Field(this, "isAutoFollowScheme", "isAutoFollowScheme", SqlDbType.Bit, false);
                DetailMoney = new MSSQL.Field(this, "DetailMoney", "DetailMoney", SqlDbType.Money, false);
            }
        }

        public class T_CardPasswordAgentDetails : MSSQL.TableBase
        {
            public MSSQL.Field ID;
            public MSSQL.Field AgentID;
            public MSSQL.Field DateTime;
            public MSSQL.Field OperatorType;
            public MSSQL.Field Amount;
            public MSSQL.Field Memo;

            public T_CardPasswordAgentDetails()
            {
                TableName = "T_CardPasswordAgentDetails";

                ID = new MSSQL.Field(this, "ID", "ID", SqlDbType.BigInt, true);
                AgentID = new MSSQL.Field(this, "AgentID", "AgentID", SqlDbType.BigInt, false);
                DateTime = new MSSQL.Field(this, "DateTime", "DateTime", SqlDbType.DateTime, false);
                OperatorType = new MSSQL.Field(this, "OperatorType", "OperatorType", SqlDbType.SmallInt, false);
                Amount = new MSSQL.Field(this, "Amount", "Amount", SqlDbType.Money, false);
                Memo = new MSSQL.Field(this, "Memo", "Memo", SqlDbType.VarChar, false);
            }
        }

        public class T_CardPasswordAgents : MSSQL.TableBase
        {
            public MSSQL.Field ID;
            public MSSQL.Field Name;
            public MSSQL.Field Key;
            public MSSQL.Field Password;
            public MSSQL.Field Company;
            public MSSQL.Field Url;
            public MSSQL.Field State;
            public MSSQL.Field IPAddressLimit;
            public MSSQL.Field Balance;

            public T_CardPasswordAgents()
            {
                TableName = "T_CardPasswordAgents";

                ID = new MSSQL.Field(this, "ID", "ID", SqlDbType.Int, false);
                Name = new MSSQL.Field(this, "Name", "Name", SqlDbType.VarChar, false);
                Key = new MSSQL.Field(this, "Key", "Key", SqlDbType.VarChar, false);
                Password = new MSSQL.Field(this, "Password", "Password", SqlDbType.VarChar, false);
                Company = new MSSQL.Field(this, "Company", "Company", SqlDbType.VarChar, false);
                Url = new MSSQL.Field(this, "Url", "Url", SqlDbType.VarChar, false);
                State = new MSSQL.Field(this, "State", "State", SqlDbType.SmallInt, false);
                IPAddressLimit = new MSSQL.Field(this, "IPAddressLimit", "IPAddressLimit", SqlDbType.VarChar, false);
                Balance = new MSSQL.Field(this, "Balance", "Balance", SqlDbType.Money, false);
            }
        }

        public class T_CardPasswordAgentsTrys : MSSQL.TableBase
        {
            public MSSQL.Field ID;
            public MSSQL.Field Name;
            public MSSQL.Field Departments;
            public MSSQL.Field Money;
            public MSSQL.Field Type;
            public MSSQL.Field AgentTitle;
            public MSSQL.Field Place;
            public MSSQL.Field SchemeDetails;
            public MSSQL.Field InitUserCount;
            public MSSQL.Field InitUserMoneyCount;
            public MSSQL.Field AddedUserByDay;
            public MSSQL.Field AddedMoneyByMonth;
            public MSSQL.Field State;

            public T_CardPasswordAgentsTrys()
            {
                TableName = "T_CardPasswordAgentsTrys";

                ID = new MSSQL.Field(this, "ID", "ID", SqlDbType.Int, true);
                Name = new MSSQL.Field(this, "Name", "Name", SqlDbType.VarChar, false);
                Departments = new MSSQL.Field(this, "Departments", "Departments", SqlDbType.VarChar, false);
                Money = new MSSQL.Field(this, "Money", "Money", SqlDbType.Money, false);
                Type = new MSSQL.Field(this, "Type", "Type", SqlDbType.VarChar, false);
                AgentTitle = new MSSQL.Field(this, "AgentTitle", "AgentTitle", SqlDbType.VarChar, false);
                Place = new MSSQL.Field(this, "Place", "Place", SqlDbType.VarChar, false);
                SchemeDetails = new MSSQL.Field(this, "SchemeDetails", "SchemeDetails", SqlDbType.VarChar, false);
                InitUserCount = new MSSQL.Field(this, "InitUserCount", "InitUserCount", SqlDbType.BigInt, false);
                InitUserMoneyCount = new MSSQL.Field(this, "InitUserMoneyCount", "InitUserMoneyCount", SqlDbType.Money, false);
                AddedUserByDay = new MSSQL.Field(this, "AddedUserByDay", "AddedUserByDay", SqlDbType.BigInt, false);
                AddedMoneyByMonth = new MSSQL.Field(this, "AddedMoneyByMonth", "AddedMoneyByMonth", SqlDbType.Money, false);
                State = new MSSQL.Field(this, "State", "State", SqlDbType.SmallInt, false);
            }
        }

        public class T_CardPasswords : MSSQL.TableBase
        {
            public MSSQL.Field ID;
            public MSSQL.Field AgentID;
            public MSSQL.Field DateTime;
            public MSSQL.Field Period;
            public MSSQL.Field Money;
            public MSSQL.Field State;
            public MSSQL.Field UserID;
            public MSSQL.Field UseDateTime;

            public T_CardPasswords()
            {
                TableName = "T_CardPasswords";

                ID = new MSSQL.Field(this, "ID", "ID", SqlDbType.BigInt, true);
                AgentID = new MSSQL.Field(this, "AgentID", "AgentID", SqlDbType.Int, false);
                DateTime = new MSSQL.Field(this, "DateTime", "DateTime", SqlDbType.DateTime, false);
                Period = new MSSQL.Field(this, "Period", "Period", SqlDbType.DateTime, false);
                Money = new MSSQL.Field(this, "Money", "Money", SqlDbType.NChar, false);
                State = new MSSQL.Field(this, "State", "State", SqlDbType.SmallInt, false);
                UserID = new MSSQL.Field(this, "UserID", "UserID", SqlDbType.BigInt, false);
                UseDateTime = new MSSQL.Field(this, "UseDateTime", "UseDateTime", SqlDbType.DateTime, false);
            }
        }

        public class T_CardPasswordsValid : MSSQL.TableBase
        {
            public MSSQL.Field ID;
            public MSSQL.Field DateTime;
            public MSSQL.Field UserID;
            public MSSQL.Field Mobile;
            public MSSQL.Field CardPasswordsNum;

            public T_CardPasswordsValid()
            {
                TableName = "T_CardPasswordsValid";

                ID = new MSSQL.Field(this, "ID", "ID", SqlDbType.BigInt, true);
                DateTime = new MSSQL.Field(this, "DateTime", "DateTime", SqlDbType.DateTime, false);
                UserID = new MSSQL.Field(this, "UserID", "UserID", SqlDbType.BigInt, false);
                Mobile = new MSSQL.Field(this, "Mobile", "Mobile", SqlDbType.VarChar, false);
                CardPasswordsNum = new MSSQL.Field(this, "CardPasswordsNum", "CardPasswordsNum", SqlDbType.VarChar, false);
            }
        }

        public class T_CardPasswordTryErrors : MSSQL.TableBase
        {
            public MSSQL.Field ID;
            public MSSQL.Field UserID;
            public MSSQL.Field DateTime;
            public MSSQL.Field Number;

            public T_CardPasswordTryErrors()
            {
                TableName = "T_CardPasswordTryErrors";

                ID = new MSSQL.Field(this, "ID", "ID", SqlDbType.BigInt, true);
                UserID = new MSSQL.Field(this, "UserID", "UserID", SqlDbType.BigInt, false);
                DateTime = new MSSQL.Field(this, "DateTime", "DateTime", SqlDbType.DateTime, false);
                Number = new MSSQL.Field(this, "Number", "Number", SqlDbType.VarChar, false);
            }
        }

        public class T_CelebComments : MSSQL.TableBase
        {
            public MSSQL.Field ID;
            public MSSQL.Field CelebID;
            public MSSQL.Field DateTime;
            public MSSQL.Field CommentserID;
            public MSSQL.Field CommentserName;
            public MSSQL.Field isShow;
            public MSSQL.Field Content;

            public T_CelebComments()
            {
                TableName = "T_CelebComments";

                ID = new MSSQL.Field(this, "ID", "ID", SqlDbType.BigInt, true);
                CelebID = new MSSQL.Field(this, "CelebID", "CelebID", SqlDbType.BigInt, false);
                DateTime = new MSSQL.Field(this, "DateTime", "DateTime", SqlDbType.DateTime, false);
                CommentserID = new MSSQL.Field(this, "CommentserID", "CommentserID", SqlDbType.BigInt, false);
                CommentserName = new MSSQL.Field(this, "CommentserName", "CommentserName", SqlDbType.VarChar, false);
                isShow = new MSSQL.Field(this, "isShow", "isShow", SqlDbType.Bit, false);
                Content = new MSSQL.Field(this, "Content", "Content", SqlDbType.VarChar, false);
            }
        }

        public class T_Celebs : MSSQL.TableBase
        {
            public MSSQL.Field ID;
            public MSSQL.Field SiteID;
            public MSSQL.Field UserID;
            public MSSQL.Field Title;
            public MSSQL.Field Order;
            public MSSQL.Field isRecommended;
            public MSSQL.Field Intro;
            public MSSQL.Field Say;
            public MSSQL.Field Comment;
            public MSSQL.Field Score;

            public T_Celebs()
            {
                TableName = "T_Celebs";

                ID = new MSSQL.Field(this, "ID", "ID", SqlDbType.BigInt, true);
                SiteID = new MSSQL.Field(this, "SiteID", "SiteID", SqlDbType.BigInt, false);
                UserID = new MSSQL.Field(this, "UserID", "UserID", SqlDbType.BigInt, false);
                Title = new MSSQL.Field(this, "Title", "Title", SqlDbType.VarChar, false);
                Order = new MSSQL.Field(this, "Order", "Order", SqlDbType.BigInt, false);
                isRecommended = new MSSQL.Field(this, "isRecommended", "isRecommended", SqlDbType.Bit, false);
                Intro = new MSSQL.Field(this, "Intro", "Intro", SqlDbType.VarChar, false);
                Say = new MSSQL.Field(this, "Say", "Say", SqlDbType.VarChar, false);
                Comment = new MSSQL.Field(this, "Comment", "Comment", SqlDbType.VarChar, false);
                Score = new MSSQL.Field(this, "Score", "Score", SqlDbType.VarChar, false);
            }
        }

        public class T_ChaseLotteryNumber : MSSQL.TableBase
        {
            public MSSQL.Field ID;
            public MSSQL.Field ChaseID;
            public MSSQL.Field LotteryNumber;

            public T_ChaseLotteryNumber()
            {
                TableName = "T_ChaseLotteryNumber";

                ID = new MSSQL.Field(this, "ID", "ID", SqlDbType.BigInt, true);
                ChaseID = new MSSQL.Field(this, "ChaseID", "ChaseID", SqlDbType.BigInt, false);
                LotteryNumber = new MSSQL.Field(this, "LotteryNumber", "LotteryNumber", SqlDbType.VarChar, false);
            }
        }

        public class T_Chases : MSSQL.TableBase
        {
            public MSSQL.Field ID;
            public MSSQL.Field UserID;
            public MSSQL.Field LotteryID;
            public MSSQL.Field Type;
            public MSSQL.Field StartTime;
            public MSSQL.Field EndTime;
            public MSSQL.Field DateTime;
            public MSSQL.Field IsuseCount;
            public MSSQL.Field Multiple;
            public MSSQL.Field Nums;
            public MSSQL.Field BetType;
            public MSSQL.Field LotteryNumber;
            public MSSQL.Field StopTypeWhenWin;
            public MSSQL.Field StopTypeWhenWinMoney;
            public MSSQL.Field QuashStatus;
            public MSSQL.Field Money;
            public MSSQL.Field Title;
            public MSSQL.Field PlayTypeID;
            public MSSQL.Field Price;

            public T_Chases()
            {
                TableName = "T_Chases";

                ID = new MSSQL.Field(this, "ID", "ID", SqlDbType.BigInt, true);
                UserID = new MSSQL.Field(this, "UserID", "UserID", SqlDbType.BigInt, false);
                LotteryID = new MSSQL.Field(this, "LotteryID", "LotteryID", SqlDbType.Int, false);
                Type = new MSSQL.Field(this, "Type", "Type", SqlDbType.SmallInt, false);
                StartTime = new MSSQL.Field(this, "StartTime", "StartTime", SqlDbType.DateTime, false);
                EndTime = new MSSQL.Field(this, "EndTime", "EndTime", SqlDbType.DateTime, false);
                DateTime = new MSSQL.Field(this, "DateTime", "DateTime", SqlDbType.DateTime, false);
                IsuseCount = new MSSQL.Field(this, "IsuseCount", "IsuseCount", SqlDbType.Int, false);
                Multiple = new MSSQL.Field(this, "Multiple", "Multiple", SqlDbType.Int, false);
                Nums = new MSSQL.Field(this, "Nums", "Nums", SqlDbType.Int, false);
                BetType = new MSSQL.Field(this, "BetType", "BetType", SqlDbType.SmallInt, false);
                LotteryNumber = new MSSQL.Field(this, "LotteryNumber", "LotteryNumber", SqlDbType.VarChar, false);
                StopTypeWhenWin = new MSSQL.Field(this, "StopTypeWhenWin", "StopTypeWhenWin", SqlDbType.SmallInt, false);
                StopTypeWhenWinMoney = new MSSQL.Field(this, "StopTypeWhenWinMoney", "StopTypeWhenWinMoney", SqlDbType.Money, false);
                QuashStatus = new MSSQL.Field(this, "QuashStatus", "QuashStatus", SqlDbType.SmallInt, false);
                Money = new MSSQL.Field(this, "Money", "Money", SqlDbType.Money, false);
                Title = new MSSQL.Field(this, "Title", "Title", SqlDbType.VarChar, false);
                PlayTypeID = new MSSQL.Field(this, "PlayTypeID", "PlayTypeID", SqlDbType.Int, false);
                Price = new MSSQL.Field(this, "Price", "Price", SqlDbType.Int, false);
            }
        }

        public class T_ChaseTaskDetailRedundancy : MSSQL.TableBase
        {
            public MSSQL.Field ChaseTaskDetailID;

            public T_ChaseTaskDetailRedundancy()
            {
                TableName = "T_ChaseTaskDetailRedundancy";

                ChaseTaskDetailID = new MSSQL.Field(this, "ChaseTaskDetailID", "ChaseTaskDetailID", SqlDbType.BigInt, false);
            }
        }

        public class T_ChaseTaskDetails : MSSQL.TableBase
        {
            public MSSQL.Field ID;
            public MSSQL.Field SiteID;
            public MSSQL.Field ChaseTaskID;
            public MSSQL.Field DateTime;
            public MSSQL.Field IsuseID;
            public MSSQL.Field PlayTypeID;
            public MSSQL.Field Multiple;
            public MSSQL.Field Money;
            public MSSQL.Field QuashStatus;
            public MSSQL.Field Executed;
            public MSSQL.Field SchemeID;
            public MSSQL.Field SecrecyLevel;
            public MSSQL.Field LotteryNumber;
            public MSSQL.Field Share;
            public MSSQL.Field BuyedShare;
            public MSSQL.Field AssureShare;

            public T_ChaseTaskDetails()
            {
                TableName = "T_ChaseTaskDetails";

                ID = new MSSQL.Field(this, "ID", "ID", SqlDbType.BigInt, true);
                SiteID = new MSSQL.Field(this, "SiteID", "SiteID", SqlDbType.BigInt, false);
                ChaseTaskID = new MSSQL.Field(this, "ChaseTaskID", "ChaseTaskID", SqlDbType.BigInt, false);
                DateTime = new MSSQL.Field(this, "DateTime", "DateTime", SqlDbType.DateTime, false);
                IsuseID = new MSSQL.Field(this, "IsuseID", "IsuseID", SqlDbType.BigInt, false);
                PlayTypeID = new MSSQL.Field(this, "PlayTypeID", "PlayTypeID", SqlDbType.Int, false);
                Multiple = new MSSQL.Field(this, "Multiple", "Multiple", SqlDbType.Int, false);
                Money = new MSSQL.Field(this, "Money", "Money", SqlDbType.Money, false);
                QuashStatus = new MSSQL.Field(this, "QuashStatus", "QuashStatus", SqlDbType.SmallInt, false);
                Executed = new MSSQL.Field(this, "Executed", "Executed", SqlDbType.Bit, false);
                SchemeID = new MSSQL.Field(this, "SchemeID", "SchemeID", SqlDbType.BigInt, false);
                SecrecyLevel = new MSSQL.Field(this, "SecrecyLevel", "SecrecyLevel", SqlDbType.SmallInt, false);
                LotteryNumber = new MSSQL.Field(this, "LotteryNumber", "LotteryNumber", SqlDbType.VarChar, false);
                Share = new MSSQL.Field(this, "Share", "Share", SqlDbType.Int, false);
                BuyedShare = new MSSQL.Field(this, "BuyedShare", "BuyedShare", SqlDbType.Int, false);
                AssureShare = new MSSQL.Field(this, "AssureShare", "AssureShare", SqlDbType.Int, false);
            }
        }

        public class T_ChaseTasks : MSSQL.TableBase
        {
            public MSSQL.Field ID;
            public MSSQL.Field SiteID;
            public MSSQL.Field UserID;
            public MSSQL.Field DateTime;
            public MSSQL.Field Title;
            public MSSQL.Field LotteryID;
            public MSSQL.Field QuashStatus;
            public MSSQL.Field StopWhenWinMoney;
            public MSSQL.Field Description;
            public MSSQL.Field SchemeBonusScale;

            public T_ChaseTasks()
            {
                TableName = "T_ChaseTasks";

                ID = new MSSQL.Field(this, "ID", "ID", SqlDbType.BigInt, true);
                SiteID = new MSSQL.Field(this, "SiteID", "SiteID", SqlDbType.BigInt, false);
                UserID = new MSSQL.Field(this, "UserID", "UserID", SqlDbType.BigInt, false);
                DateTime = new MSSQL.Field(this, "DateTime", "DateTime", SqlDbType.DateTime, false);
                Title = new MSSQL.Field(this, "Title", "Title", SqlDbType.VarChar, false);
                LotteryID = new MSSQL.Field(this, "LotteryID", "LotteryID", SqlDbType.Int, false);
                QuashStatus = new MSSQL.Field(this, "QuashStatus", "QuashStatus", SqlDbType.SmallInt, false);
                StopWhenWinMoney = new MSSQL.Field(this, "StopWhenWinMoney", "StopWhenWinMoney", SqlDbType.Money, false);
                Description = new MSSQL.Field(this, "Description", "Description", SqlDbType.VarChar, false);
                SchemeBonusScale = new MSSQL.Field(this, "SchemeBonusScale", "SchemeBonusScale", SqlDbType.Float, false);
            }
        }

        public class T_Citys : MSSQL.TableBase
        {
            public MSSQL.Field ID;
            public MSSQL.Field ProvinceID;
            public MSSQL.Field Name;

            public T_Citys()
            {
                TableName = "T_Citys";

                ID = new MSSQL.Field(this, "ID", "ID", SqlDbType.Int, false);
                ProvinceID = new MSSQL.Field(this, "ProvinceID", "ProvinceID", SqlDbType.Int, false);
                Name = new MSSQL.Field(this, "Name", "Name", SqlDbType.VarChar, false);
            }
        }

        public class T_Competences : MSSQL.TableBase
        {
            public MSSQL.Field ID;
            public MSSQL.Field Name;
            public MSSQL.Field Code;
            public MSSQL.Field Description;

            public T_Competences()
            {
                TableName = "T_Competences";

                ID = new MSSQL.Field(this, "ID", "ID", SqlDbType.SmallInt, false);
                Name = new MSSQL.Field(this, "Name", "Name", SqlDbType.VarChar, false);
                Code = new MSSQL.Field(this, "Code", "Code", SqlDbType.VarChar, false);
                Description = new MSSQL.Field(this, "Description", "Description", SqlDbType.VarChar, false);
            }
        }

        public class T_CompetencesOfGroups : MSSQL.TableBase
        {
            public MSSQL.Field GroupID;
            public MSSQL.Field CompetenceID;

            public T_CompetencesOfGroups()
            {
                TableName = "T_CompetencesOfGroups";

                GroupID = new MSSQL.Field(this, "GroupID", "GroupID", SqlDbType.SmallInt, false);
                CompetenceID = new MSSQL.Field(this, "CompetenceID", "CompetenceID", SqlDbType.SmallInt, false);
            }
        }

        public class T_CompetencesOfUsers : MSSQL.TableBase
        {
            public MSSQL.Field UserID;
            public MSSQL.Field CompetenceID;

            public T_CompetencesOfUsers()
            {
                TableName = "T_CompetencesOfUsers";

                UserID = new MSSQL.Field(this, "UserID", "UserID", SqlDbType.BigInt, false);
                CompetenceID = new MSSQL.Field(this, "CompetenceID", "CompetenceID", SqlDbType.SmallInt, false);
            }
        }

        public class T_Cps : MSSQL.TableBase
        {
            public MSSQL.Field ID;
            public MSSQL.Field SiteID;
            public MSSQL.Field OwnerUserID;
            public MSSQL.Field Name;
            public MSSQL.Field DateTime;
            public MSSQL.Field Url;
            public MSSQL.Field LogoUrl;
            public MSSQL.Field BonusScale;
            public MSSQL.Field ON;
            public MSSQL.Field Company;
            public MSSQL.Field Address;
            public MSSQL.Field PostCode;
            public MSSQL.Field ResponsiblePerson;
            public MSSQL.Field ContactPerson;
            public MSSQL.Field Telephone;
            public MSSQL.Field Fax;
            public MSSQL.Field Mobile;
            public MSSQL.Field Email;
            public MSSQL.Field QQ;
            public MSSQL.Field ServiceTelephone;
            public MSSQL.Field MD5Key;
            public MSSQL.Field Type;
            public MSSQL.Field DomainName;
            public MSSQL.Field ParentID;
            public MSSQL.Field OperatorID;
            public MSSQL.Field CommendID;
            public MSSQL.Field IsShow;
            public MSSQL.Field PageTitleName;
            public MSSQL.Field PageHeadConctroFilelName;
            public MSSQL.Field PageFootConctrolFilelName;

            public T_Cps()
            {
                TableName = "T_Cps";

                ID = new MSSQL.Field(this, "ID", "ID", SqlDbType.BigInt, true);
                SiteID = new MSSQL.Field(this, "SiteID", "SiteID", SqlDbType.BigInt, false);
                OwnerUserID = new MSSQL.Field(this, "OwnerUserID", "OwnerUserID", SqlDbType.BigInt, false);
                Name = new MSSQL.Field(this, "Name", "Name", SqlDbType.VarChar, false);
                DateTime = new MSSQL.Field(this, "DateTime", "DateTime", SqlDbType.DateTime, false);
                Url = new MSSQL.Field(this, "Url", "Url", SqlDbType.VarChar, false);
                LogoUrl = new MSSQL.Field(this, "LogoUrl", "LogoUrl", SqlDbType.VarChar, false);
                BonusScale = new MSSQL.Field(this, "BonusScale", "BonusScale", SqlDbType.Float, false);
                ON = new MSSQL.Field(this, "ON", "ON", SqlDbType.Bit, false);
                Company = new MSSQL.Field(this, "Company", "Company", SqlDbType.VarChar, false);
                Address = new MSSQL.Field(this, "Address", "Address", SqlDbType.VarChar, false);
                PostCode = new MSSQL.Field(this, "PostCode", "PostCode", SqlDbType.VarChar, false);
                ResponsiblePerson = new MSSQL.Field(this, "ResponsiblePerson", "ResponsiblePerson", SqlDbType.VarChar, false);
                ContactPerson = new MSSQL.Field(this, "ContactPerson", "ContactPerson", SqlDbType.VarChar, false);
                Telephone = new MSSQL.Field(this, "Telephone", "Telephone", SqlDbType.VarChar, false);
                Fax = new MSSQL.Field(this, "Fax", "Fax", SqlDbType.VarChar, false);
                Mobile = new MSSQL.Field(this, "Mobile", "Mobile", SqlDbType.VarChar, false);
                Email = new MSSQL.Field(this, "Email", "Email", SqlDbType.VarChar, false);
                QQ = new MSSQL.Field(this, "QQ", "QQ", SqlDbType.VarChar, false);
                ServiceTelephone = new MSSQL.Field(this, "ServiceTelephone", "ServiceTelephone", SqlDbType.VarChar, false);
                MD5Key = new MSSQL.Field(this, "MD5Key", "MD5Key", SqlDbType.VarChar, false);
                Type = new MSSQL.Field(this, "Type", "Type", SqlDbType.SmallInt, false);
                DomainName = new MSSQL.Field(this, "DomainName", "DomainName", SqlDbType.VarChar, false);
                ParentID = new MSSQL.Field(this, "ParentID", "ParentID", SqlDbType.BigInt, false);
                OperatorID = new MSSQL.Field(this, "OperatorID", "OperatorID", SqlDbType.BigInt, false);
                CommendID = new MSSQL.Field(this, "CommendID", "CommendID", SqlDbType.BigInt, false);
                IsShow = new MSSQL.Field(this, "IsShow", "IsShow", SqlDbType.Bit, false);
                PageTitleName = new MSSQL.Field(this, "PageTitleName", "PageTitleName", SqlDbType.VarChar, false);
                PageHeadConctroFilelName = new MSSQL.Field(this, "PageHeadConctroFilelName", "PageHeadConctroFilelName", SqlDbType.VarChar, false);
                PageFootConctrolFilelName = new MSSQL.Field(this, "PageFootConctrolFilelName", "PageFootConctrolFilelName", SqlDbType.VarChar, false);
            }
        }

        public class T_Cps_Help : MSSQL.TableBase
        {
            public MSSQL.Field ID;
            public MSSQL.Field Title;
            public MSSQL.Field Content;

            public T_Cps_Help()
            {
                TableName = "T_Cps_Help";

                ID = new MSSQL.Field(this, "ID", "ID", SqlDbType.Int, true);
                Title = new MSSQL.Field(this, "Title", "Title", SqlDbType.VarChar, false);
                Content = new MSSQL.Field(this, "Content", "Content", SqlDbType.VarChar, false);
            }
        }

        public class T_CPS_SNS_SiteManager : MSSQL.TableBase
        {
            public MSSQL.Field ID;
            public MSSQL.Field Pid;
            public MSSQL.Field CpsID;
            public MSSQL.Field Url;
            public MSSQL.Field UnShowClub;

            public T_CPS_SNS_SiteManager()
            {
                TableName = "T_CPS_SNS_SiteManager";

                ID = new MSSQL.Field(this, "ID", "ID", SqlDbType.Int, true);
                Pid = new MSSQL.Field(this, "Pid", "Pid", SqlDbType.Int, false);
                CpsID = new MSSQL.Field(this, "CpsID", "CpsID", SqlDbType.Int, false);
                Url = new MSSQL.Field(this, "Url", "Url", SqlDbType.VarChar, false);
                UnShowClub = new MSSQL.Field(this, "UnShowClub", "UnShowClub", SqlDbType.Bit, false);
            }
        }

        public class T_CpsAccountRevenue : MSSQL.TableBase
        {
            public MSSQL.Field ID;
            public MSSQL.Field CpsID;
            public MSSQL.Field DayTime;
            public MSSQL.Field TotalUserCount;
            public MSSQL.Field DayNewUserCount;
            public MSSQL.Field DayNewUserPayCount;
            public MSSQL.Field DayNewUserPaySum;
            public MSSQL.Field CpsBonus;
            public MSSQL.Field CpsWithSiteMoneySum;

            public T_CpsAccountRevenue()
            {
                TableName = "T_CpsAccountRevenue";

                ID = new MSSQL.Field(this, "ID", "ID", SqlDbType.BigInt, true);
                CpsID = new MSSQL.Field(this, "CpsID", "CpsID", SqlDbType.Int, false);
                DayTime = new MSSQL.Field(this, "DayTime", "DayTime", SqlDbType.DateTime, false);
                TotalUserCount = new MSSQL.Field(this, "TotalUserCount", "TotalUserCount", SqlDbType.Int, false);
                DayNewUserCount = new MSSQL.Field(this, "DayNewUserCount", "DayNewUserCount", SqlDbType.Int, false);
                DayNewUserPayCount = new MSSQL.Field(this, "DayNewUserPayCount", "DayNewUserPayCount", SqlDbType.Int, false);
                DayNewUserPaySum = new MSSQL.Field(this, "DayNewUserPaySum", "DayNewUserPaySum", SqlDbType.Money, false);
                CpsBonus = new MSSQL.Field(this, "CpsBonus", "CpsBonus", SqlDbType.Money, false);
                CpsWithSiteMoneySum = new MSSQL.Field(this, "CpsWithSiteMoneySum", "CpsWithSiteMoneySum", SqlDbType.Money, false);
            }
        }

        public class T_CpsAdminAccountByMonth : MSSQL.TableBase
        {
            public MSSQL.Field AccountMonth;
            public MSSQL.Field BuyMoney;
            public MSSQL.Field PayBonus;
            public MSSQL.Field CpsCount;
            public MSSQL.Field IsPayOff;
            public MSSQL.Field UpdateTime;
            public MSSQL.Field MonthCpsCount;
            public MSSQL.Field MonthTradeCpsCount;
            public MSSQL.Field CpsMemberCount;
            public MSSQL.Field MonthNewCpsMemberCount;

            public T_CpsAdminAccountByMonth()
            {
                TableName = "T_CpsAdminAccountByMonth";

                AccountMonth = new MSSQL.Field(this, "AccountMonth", "AccountMonth", SqlDbType.VarChar, false);
                BuyMoney = new MSSQL.Field(this, "BuyMoney", "BuyMoney", SqlDbType.Money, false);
                PayBonus = new MSSQL.Field(this, "PayBonus", "PayBonus", SqlDbType.Money, false);
                CpsCount = new MSSQL.Field(this, "CpsCount", "CpsCount", SqlDbType.Int, false);
                IsPayOff = new MSSQL.Field(this, "IsPayOff", "IsPayOff", SqlDbType.Bit, false);
                UpdateTime = new MSSQL.Field(this, "UpdateTime", "UpdateTime", SqlDbType.DateTime, false);
                MonthCpsCount = new MSSQL.Field(this, "MonthCpsCount", "MonthCpsCount", SqlDbType.Int, false);
                MonthTradeCpsCount = new MSSQL.Field(this, "MonthTradeCpsCount", "MonthTradeCpsCount", SqlDbType.Int, false);
                CpsMemberCount = new MSSQL.Field(this, "CpsMemberCount", "CpsMemberCount", SqlDbType.Int, false);
                MonthNewCpsMemberCount = new MSSQL.Field(this, "MonthNewCpsMemberCount", "MonthNewCpsMemberCount", SqlDbType.Int, false);
            }
        }

        public class T_CpsBonusDetails : MSSQL.TableBase
        {
            public MSSQL.Field ID;
            public MSSQL.Field OwnerUserID;
            public MSSQL.Field DateTime;
            public MSSQL.Field Money;
            public MSSQL.Field BonusScale;
            public MSSQL.Field IsAddInAllowBonus;
            public MSSQL.Field OperatorType;
            public MSSQL.Field FromUserID;
            public MSSQL.Field FromUserCpsID;
            public MSSQL.Field SchemeID;
            public MSSQL.Field BuyDetailID;
            public MSSQL.Field PayNumber;
            public MSSQL.Field PayBank;
            public MSSQL.Field OperatorID;
            public MSSQL.Field Memo;
            public MSSQL.Field PrintOutDatetime;
            public MSSQL.Field LotteryID;
            public MSSQL.Field PlayTypeID;

            public T_CpsBonusDetails()
            {
                TableName = "T_CpsBonusDetails";

                ID = new MSSQL.Field(this, "ID", "ID", SqlDbType.BigInt, true);
                OwnerUserID = new MSSQL.Field(this, "OwnerUserID", "OwnerUserID", SqlDbType.BigInt, false);
                DateTime = new MSSQL.Field(this, "DateTime", "DateTime", SqlDbType.DateTime, false);
                Money = new MSSQL.Field(this, "Money", "Money", SqlDbType.Money, false);
                BonusScale = new MSSQL.Field(this, "BonusScale", "BonusScale", SqlDbType.Float, false);
                IsAddInAllowBonus = new MSSQL.Field(this, "IsAddInAllowBonus", "IsAddInAllowBonus", SqlDbType.Bit, false);
                OperatorType = new MSSQL.Field(this, "OperatorType", "OperatorType", SqlDbType.SmallInt, false);
                FromUserID = new MSSQL.Field(this, "FromUserID", "FromUserID", SqlDbType.BigInt, false);
                FromUserCpsID = new MSSQL.Field(this, "FromUserCpsID", "FromUserCpsID", SqlDbType.BigInt, false);
                SchemeID = new MSSQL.Field(this, "SchemeID", "SchemeID", SqlDbType.BigInt, false);
                BuyDetailID = new MSSQL.Field(this, "BuyDetailID", "BuyDetailID", SqlDbType.BigInt, false);
                PayNumber = new MSSQL.Field(this, "PayNumber", "PayNumber", SqlDbType.VarChar, false);
                PayBank = new MSSQL.Field(this, "PayBank", "PayBank", SqlDbType.VarChar, false);
                OperatorID = new MSSQL.Field(this, "OperatorID", "OperatorID", SqlDbType.BigInt, false);
                Memo = new MSSQL.Field(this, "Memo", "Memo", SqlDbType.VarChar, false);
                PrintOutDatetime = new MSSQL.Field(this, "PrintOutDatetime", "PrintOutDatetime", SqlDbType.DateTime, false);
                LotteryID = new MSSQL.Field(this, "LotteryID", "LotteryID", SqlDbType.BigInt, false);
                PlayTypeID = new MSSQL.Field(this, "PlayTypeID", "PlayTypeID", SqlDbType.BigInt, false);
            }
        }

        public class T_CpsBonusScale : MSSQL.TableBase
        {
            public MSSQL.Field OwnerUserID;
            public MSSQL.Field LotteryID;
            public MSSQL.Field BonusScale;

            public T_CpsBonusScale()
            {
                TableName = "T_CpsBonusScale";

                OwnerUserID = new MSSQL.Field(this, "OwnerUserID", "OwnerUserID", SqlDbType.BigInt, false);
                LotteryID = new MSSQL.Field(this, "LotteryID", "LotteryID", SqlDbType.BigInt, false);
                BonusScale = new MSSQL.Field(this, "BonusScale", "BonusScale", SqlDbType.Float, false);
            }
        }

        public class T_CpsBonusScaleSetting : MSSQL.TableBase
        {
            public MSSQL.Field LotteryID;
            public MSSQL.Field LotteryName;
            public MSSQL.Field TypeID;
            public MSSQL.Field UnionBonusScale;
            public MSSQL.Field SiteBonusScale;
            public MSSQL.Field CommenderBonusScale;
            public MSSQL.Field CommendSiteBonusScale;
            public MSSQL.Field Memo;

            public T_CpsBonusScaleSetting()
            {
                TableName = "T_CpsBonusScaleSetting";

                LotteryID = new MSSQL.Field(this, "LotteryID", "LotteryID", SqlDbType.BigInt, false);
                LotteryName = new MSSQL.Field(this, "LotteryName", "LotteryName", SqlDbType.NVarChar, false);
                TypeID = new MSSQL.Field(this, "TypeID", "TypeID", SqlDbType.BigInt, false);
                UnionBonusScale = new MSSQL.Field(this, "UnionBonusScale", "UnionBonusScale", SqlDbType.Float, false);
                SiteBonusScale = new MSSQL.Field(this, "SiteBonusScale", "SiteBonusScale", SqlDbType.Float, false);
                CommenderBonusScale = new MSSQL.Field(this, "CommenderBonusScale", "CommenderBonusScale", SqlDbType.Float, false);
                CommendSiteBonusScale = new MSSQL.Field(this, "CommendSiteBonusScale", "CommendSiteBonusScale", SqlDbType.Float, false);
                Memo = new MSSQL.Field(this, "Memo", "Memo", SqlDbType.VarChar, false);
            }
        }

        public class T_CpsBonusScaleType : MSSQL.TableBase
        {
            public MSSQL.Field ID;
            public MSSQL.Field ParentID;
            public MSSQL.Field Name;
            public MSSQL.Field TypePath;

            public T_CpsBonusScaleType()
            {
                TableName = "T_CpsBonusScaleType";

                ID = new MSSQL.Field(this, "ID", "ID", SqlDbType.BigInt, true);
                ParentID = new MSSQL.Field(this, "ParentID", "ParentID", SqlDbType.BigInt, false);
                Name = new MSSQL.Field(this, "Name", "Name", SqlDbType.NVarChar, false);
                TypePath = new MSSQL.Field(this, "TypePath", "TypePath", SqlDbType.VarChar, false);
            }
        }

        public class T_CpsIncomeByDay : MSSQL.TableBase
        {
            public MSSQL.Field ID;
            public MSSQL.Field CpsID;
            public MSSQL.Field DayTime;
            public MSSQL.Field TotalUserCount;
            public MSSQL.Field DayNewUserCount;
            public MSSQL.Field DayNewUserPayCount;
            public MSSQL.Field DayNewUserTradeMoney;
            public MSSQL.Field DayMemberTradeMoney;
            public MSSQL.Field DaySiteTradeMoney;
            public MSSQL.Field DayBonus;

            public T_CpsIncomeByDay()
            {
                TableName = "T_CpsIncomeByDay";

                ID = new MSSQL.Field(this, "ID", "ID", SqlDbType.BigInt, true);
                CpsID = new MSSQL.Field(this, "CpsID", "CpsID", SqlDbType.Int, false);
                DayTime = new MSSQL.Field(this, "DayTime", "DayTime", SqlDbType.DateTime, false);
                TotalUserCount = new MSSQL.Field(this, "TotalUserCount", "TotalUserCount", SqlDbType.Int, false);
                DayNewUserCount = new MSSQL.Field(this, "DayNewUserCount", "DayNewUserCount", SqlDbType.Int, false);
                DayNewUserPayCount = new MSSQL.Field(this, "DayNewUserPayCount", "DayNewUserPayCount", SqlDbType.Int, false);
                DayNewUserTradeMoney = new MSSQL.Field(this, "DayNewUserTradeMoney", "DayNewUserTradeMoney", SqlDbType.Money, false);
                DayMemberTradeMoney = new MSSQL.Field(this, "DayMemberTradeMoney", "DayMemberTradeMoney", SqlDbType.Money, false);
                DaySiteTradeMoney = new MSSQL.Field(this, "DaySiteTradeMoney", "DaySiteTradeMoney", SqlDbType.Money, false);
                DayBonus = new MSSQL.Field(this, "DayBonus", "DayBonus", SqlDbType.Money, false);
            }
        }

        public class T_CpsIncomeByMonth : MSSQL.TableBase
        {
            public MSSQL.Field CpsID;
            public MSSQL.Field YearMonth;
            public MSSQL.Field BuyMoney;
            public MSSQL.Field Bonus;
            public MSSQL.Field IsGetMoney;
            public MSSQL.Field CreateDatetime;
            public MSSQL.Field Memo;

            public T_CpsIncomeByMonth()
            {
                TableName = "T_CpsIncomeByMonth";

                CpsID = new MSSQL.Field(this, "CpsID", "CpsID", SqlDbType.BigInt, false);
                YearMonth = new MSSQL.Field(this, "YearMonth", "YearMonth", SqlDbType.VarChar, false);
                BuyMoney = new MSSQL.Field(this, "BuyMoney", "BuyMoney", SqlDbType.Money, false);
                Bonus = new MSSQL.Field(this, "Bonus", "Bonus", SqlDbType.Money, false);
                IsGetMoney = new MSSQL.Field(this, "IsGetMoney", "IsGetMoney", SqlDbType.Bit, false);
                CreateDatetime = new MSSQL.Field(this, "CreateDatetime", "CreateDatetime", SqlDbType.DateTime, false);
                Memo = new MSSQL.Field(this, "Memo", "Memo", SqlDbType.VarChar, false);
            }
        }

        public class T_CpsLog : MSSQL.TableBase
        {
            public MSSQL.Field ID;
            public MSSQL.Field Datetime;
            public MSSQL.Field LogContent;

            public T_CpsLog()
            {
                TableName = "T_CpsLog";

                ID = new MSSQL.Field(this, "ID", "ID", SqlDbType.BigInt, true);
                Datetime = new MSSQL.Field(this, "Datetime", "Datetime", SqlDbType.DateTime, false);
                LogContent = new MSSQL.Field(this, "LogContent", "LogContent", SqlDbType.VarChar, false);
            }
        }

        public class T_CpsTrys : MSSQL.TableBase
        {
            public MSSQL.Field ID;
            public MSSQL.Field SiteID;
            public MSSQL.Field UserID;
            public MSSQL.Field DateTime;
            public MSSQL.Field HandleResult;
            public MSSQL.Field HandlelDateTime;
            public MSSQL.Field Name;
            public MSSQL.Field Url;
            public MSSQL.Field LogoUrl;
            public MSSQL.Field Company;
            public MSSQL.Field Address;
            public MSSQL.Field PostCode;
            public MSSQL.Field ResponsiblePerson;
            public MSSQL.Field ContactPerson;
            public MSSQL.Field Telephone;
            public MSSQL.Field Fax;
            public MSSQL.Field Mobile;
            public MSSQL.Field Email;
            public MSSQL.Field QQ;
            public MSSQL.Field ServiceTelephone;
            public MSSQL.Field MD5Key;
            public MSSQL.Field Type;
            public MSSQL.Field DomainName;
            public MSSQL.Field ParentID;
            public MSSQL.Field BonusScale;
            public MSSQL.Field CommendID;
            public MSSQL.Field Content;

            public T_CpsTrys()
            {
                TableName = "T_CpsTrys";

                ID = new MSSQL.Field(this, "ID", "ID", SqlDbType.BigInt, false);
                SiteID = new MSSQL.Field(this, "SiteID", "SiteID", SqlDbType.BigInt, false);
                UserID = new MSSQL.Field(this, "UserID", "UserID", SqlDbType.BigInt, false);
                DateTime = new MSSQL.Field(this, "DateTime", "DateTime", SqlDbType.DateTime, false);
                HandleResult = new MSSQL.Field(this, "HandleResult", "HandleResult", SqlDbType.SmallInt, false);
                HandlelDateTime = new MSSQL.Field(this, "HandlelDateTime", "HandlelDateTime", SqlDbType.DateTime, false);
                Name = new MSSQL.Field(this, "Name", "Name", SqlDbType.VarChar, false);
                Url = new MSSQL.Field(this, "Url", "Url", SqlDbType.VarChar, false);
                LogoUrl = new MSSQL.Field(this, "LogoUrl", "LogoUrl", SqlDbType.VarChar, false);
                Company = new MSSQL.Field(this, "Company", "Company", SqlDbType.VarChar, false);
                Address = new MSSQL.Field(this, "Address", "Address", SqlDbType.VarChar, false);
                PostCode = new MSSQL.Field(this, "PostCode", "PostCode", SqlDbType.VarChar, false);
                ResponsiblePerson = new MSSQL.Field(this, "ResponsiblePerson", "ResponsiblePerson", SqlDbType.VarChar, false);
                ContactPerson = new MSSQL.Field(this, "ContactPerson", "ContactPerson", SqlDbType.VarChar, false);
                Telephone = new MSSQL.Field(this, "Telephone", "Telephone", SqlDbType.VarChar, false);
                Fax = new MSSQL.Field(this, "Fax", "Fax", SqlDbType.VarChar, false);
                Mobile = new MSSQL.Field(this, "Mobile", "Mobile", SqlDbType.VarChar, false);
                Email = new MSSQL.Field(this, "Email", "Email", SqlDbType.VarChar, false);
                QQ = new MSSQL.Field(this, "QQ", "QQ", SqlDbType.VarChar, false);
                ServiceTelephone = new MSSQL.Field(this, "ServiceTelephone", "ServiceTelephone", SqlDbType.VarChar, false);
                MD5Key = new MSSQL.Field(this, "MD5Key", "MD5Key", SqlDbType.VarChar, false);
                Type = new MSSQL.Field(this, "Type", "Type", SqlDbType.SmallInt, false);
                DomainName = new MSSQL.Field(this, "DomainName", "DomainName", SqlDbType.VarChar, false);
                ParentID = new MSSQL.Field(this, "ParentID", "ParentID", SqlDbType.BigInt, false);
                BonusScale = new MSSQL.Field(this, "BonusScale", "BonusScale", SqlDbType.Float, false);
                CommendID = new MSSQL.Field(this, "CommendID", "CommendID", SqlDbType.BigInt, false);
                Content = new MSSQL.Field(this, "Content", "Content", SqlDbType.VarChar, false);
            }
        }

        public class T_CpsType : MSSQL.TableBase
        {
            public MSSQL.Field ID;
            public MSSQL.Field Name;

            public T_CpsType()
            {
                TableName = "T_CpsType";

                ID = new MSSQL.Field(this, "ID", "ID", SqlDbType.SmallInt, true);
                Name = new MSSQL.Field(this, "Name", "Name", SqlDbType.VarChar, false);
            }
        }

        public class T_CustomFollowSchemes : MSSQL.TableBase
        {
            public MSSQL.Field ID;
            public MSSQL.Field SiteID;
            public MSSQL.Field UserID;
            public MSSQL.Field DateTime;
            public MSSQL.Field FollowSchemeID;
            public MSSQL.Field MoneyStart;
            public MSSQL.Field MoneyEnd;
            public MSSQL.Field BuyShareStart;
            public MSSQL.Field BuyShareEnd;
            public MSSQL.Field Type;

            public T_CustomFollowSchemes()
            {
                TableName = "T_CustomFollowSchemes";

                ID = new MSSQL.Field(this, "ID", "ID", SqlDbType.BigInt, true);
                SiteID = new MSSQL.Field(this, "SiteID", "SiteID", SqlDbType.BigInt, false);
                UserID = new MSSQL.Field(this, "UserID", "UserID", SqlDbType.BigInt, false);
                DateTime = new MSSQL.Field(this, "DateTime", "DateTime", SqlDbType.DateTime, false);
                FollowSchemeID = new MSSQL.Field(this, "FollowSchemeID", "FollowSchemeID", SqlDbType.BigInt, false);
                MoneyStart = new MSSQL.Field(this, "MoneyStart", "MoneyStart", SqlDbType.Money, false);
                MoneyEnd = new MSSQL.Field(this, "MoneyEnd", "MoneyEnd", SqlDbType.Money, false);
                BuyShareStart = new MSSQL.Field(this, "BuyShareStart", "BuyShareStart", SqlDbType.Int, false);
                BuyShareEnd = new MSSQL.Field(this, "BuyShareEnd", "BuyShareEnd", SqlDbType.Int, false);
                Type = new MSSQL.Field(this, "Type", "Type", SqlDbType.SmallInt, false);
            }
        }

        public class T_CustomFriendFollowSchemes : MSSQL.TableBase
        {
            public MSSQL.Field ID;
            public MSSQL.Field SiteID;
            public MSSQL.Field UserID;
            public MSSQL.Field FollowUserID;
            public MSSQL.Field LotteryID;
            public MSSQL.Field PlayTypeID;
            public MSSQL.Field MoneyStart;
            public MSSQL.Field MoneyEnd;
            public MSSQL.Field BuyShareStart;
            public MSSQL.Field BuyShareEnd;
            public MSSQL.Field Type;
            public MSSQL.Field DateTime;

            public T_CustomFriendFollowSchemes()
            {
                TableName = "T_CustomFriendFollowSchemes";

                ID = new MSSQL.Field(this, "ID", "ID", SqlDbType.BigInt, true);
                SiteID = new MSSQL.Field(this, "SiteID", "SiteID", SqlDbType.BigInt, false);
                UserID = new MSSQL.Field(this, "UserID", "UserID", SqlDbType.BigInt, false);
                FollowUserID = new MSSQL.Field(this, "FollowUserID", "FollowUserID", SqlDbType.BigInt, false);
                LotteryID = new MSSQL.Field(this, "LotteryID", "LotteryID", SqlDbType.Int, false);
                PlayTypeID = new MSSQL.Field(this, "PlayTypeID", "PlayTypeID", SqlDbType.Int, false);
                MoneyStart = new MSSQL.Field(this, "MoneyStart", "MoneyStart", SqlDbType.Money, false);
                MoneyEnd = new MSSQL.Field(this, "MoneyEnd", "MoneyEnd", SqlDbType.Money, false);
                BuyShareStart = new MSSQL.Field(this, "BuyShareStart", "BuyShareStart", SqlDbType.Int, false);
                BuyShareEnd = new MSSQL.Field(this, "BuyShareEnd", "BuyShareEnd", SqlDbType.Int, false);
                Type = new MSSQL.Field(this, "Type", "Type", SqlDbType.SmallInt, false);
                DateTime = new MSSQL.Field(this, "DateTime", "DateTime", SqlDbType.DateTime, false);
            }
        }

        public class T_Downloads : MSSQL.TableBase
        {
            public MSSQL.Field ID;
            public MSSQL.Field SiteID;
            public MSSQL.Field DateTime;
            public MSSQL.Field Title;
            public MSSQL.Field FileUrl;
            public MSSQL.Field isShow;

            public T_Downloads()
            {
                TableName = "T_Downloads";

                ID = new MSSQL.Field(this, "ID", "ID", SqlDbType.BigInt, true);
                SiteID = new MSSQL.Field(this, "SiteID", "SiteID", SqlDbType.BigInt, false);
                DateTime = new MSSQL.Field(this, "DateTime", "DateTime", SqlDbType.DateTime, false);
                Title = new MSSQL.Field(this, "Title", "Title", SqlDbType.VarChar, false);
                FileUrl = new MSSQL.Field(this, "FileUrl", "FileUrl", SqlDbType.VarChar, false);
                isShow = new MSSQL.Field(this, "isShow", "isShow", SqlDbType.Bit, false);
            }
        }

        public class T_ElectronTicketAgentDetails : MSSQL.TableBase
        {
            public MSSQL.Field ID;
            public MSSQL.Field AgentID;
            public MSSQL.Field DateTime;
            public MSSQL.Field OperatorType;
            public MSSQL.Field Amount;
            public MSSQL.Field Memo;

            public T_ElectronTicketAgentDetails()
            {
                TableName = "T_ElectronTicketAgentDetails";

                ID = new MSSQL.Field(this, "ID", "ID", SqlDbType.BigInt, true);
                AgentID = new MSSQL.Field(this, "AgentID", "AgentID", SqlDbType.BigInt, false);
                DateTime = new MSSQL.Field(this, "DateTime", "DateTime", SqlDbType.DateTime, false);
                OperatorType = new MSSQL.Field(this, "OperatorType", "OperatorType", SqlDbType.SmallInt, false);
                Amount = new MSSQL.Field(this, "Amount", "Amount", SqlDbType.Money, false);
                Memo = new MSSQL.Field(this, "Memo", "Memo", SqlDbType.VarChar, false);
            }
        }

        public class T_ElectronTicketAgents : MSSQL.TableBase
        {
            public MSSQL.Field ID;
            public MSSQL.Field Name;
            public MSSQL.Field Key;
            public MSSQL.Field Password;
            public MSSQL.Field Company;
            public MSSQL.Field Url;
            public MSSQL.Field Balance;
            public MSSQL.Field State;
            public MSSQL.Field UseLotteryList;
            public MSSQL.Field IPAddressLimit;

            public T_ElectronTicketAgents()
            {
                TableName = "T_ElectronTicketAgents";

                ID = new MSSQL.Field(this, "ID", "ID", SqlDbType.BigInt, true);
                Name = new MSSQL.Field(this, "Name", "Name", SqlDbType.VarChar, false);
                Key = new MSSQL.Field(this, "Key", "Key", SqlDbType.VarChar, false);
                Password = new MSSQL.Field(this, "Password", "Password", SqlDbType.VarChar, false);
                Company = new MSSQL.Field(this, "Company", "Company", SqlDbType.VarChar, false);
                Url = new MSSQL.Field(this, "Url", "Url", SqlDbType.VarChar, false);
                Balance = new MSSQL.Field(this, "Balance", "Balance", SqlDbType.Money, false);
                State = new MSSQL.Field(this, "State", "State", SqlDbType.SmallInt, false);
                UseLotteryList = new MSSQL.Field(this, "UseLotteryList", "UseLotteryList", SqlDbType.VarChar, false);
                IPAddressLimit = new MSSQL.Field(this, "IPAddressLimit", "IPAddressLimit", SqlDbType.VarChar, false);
            }
        }

        public class T_ElectronTicketAgentSchemeDetails : MSSQL.TableBase
        {
            public MSSQL.Field ID;
            public MSSQL.Field SchemeID;
            public MSSQL.Field Name;
            public MSSQL.Field AlipayName;
            public MSSQL.Field RealityName;
            public MSSQL.Field IDCard;
            public MSSQL.Field Telephone;
            public MSSQL.Field Mobile;
            public MSSQL.Field Email;
            public MSSQL.Field Share;
            public MSSQL.Field Amount;

            public T_ElectronTicketAgentSchemeDetails()
            {
                TableName = "T_ElectronTicketAgentSchemeDetails";

                ID = new MSSQL.Field(this, "ID", "ID", SqlDbType.BigInt, true);
                SchemeID = new MSSQL.Field(this, "SchemeID", "SchemeID", SqlDbType.BigInt, false);
                Name = new MSSQL.Field(this, "Name", "Name", SqlDbType.VarChar, false);
                AlipayName = new MSSQL.Field(this, "AlipayName", "AlipayName", SqlDbType.VarChar, false);
                RealityName = new MSSQL.Field(this, "RealityName", "RealityName", SqlDbType.VarChar, false);
                IDCard = new MSSQL.Field(this, "IDCard", "IDCard", SqlDbType.VarChar, false);
                Telephone = new MSSQL.Field(this, "Telephone", "Telephone", SqlDbType.VarChar, false);
                Mobile = new MSSQL.Field(this, "Mobile", "Mobile", SqlDbType.VarChar, false);
                Email = new MSSQL.Field(this, "Email", "Email", SqlDbType.VarChar, false);
                Share = new MSSQL.Field(this, "Share", "Share", SqlDbType.Int, false);
                Amount = new MSSQL.Field(this, "Amount", "Amount", SqlDbType.Money, false);
            }
        }

        public class T_ElectronTicketAgentSchemes : MSSQL.TableBase
        {
            public MSSQL.Field ID;
            public MSSQL.Field AgentID;
            public MSSQL.Field DateTime;
            public MSSQL.Field SchemeNumber;
            public MSSQL.Field LotteryID;
            public MSSQL.Field PlayTypeID;
            public MSSQL.Field IsuseID;
            public MSSQL.Field Amount;
            public MSSQL.Field Multiple;
            public MSSQL.Field Share;
            public MSSQL.Field InitiateName;
            public MSSQL.Field InitiateAlipayName;
            public MSSQL.Field InitiateAlipayID;
            public MSSQL.Field InitiateRealityName;
            public MSSQL.Field InitiateIDCard;
            public MSSQL.Field InitiateTelephone;
            public MSSQL.Field InitiateMobile;
            public MSSQL.Field InitiateEmail;
            public MSSQL.Field InitiateBonusScale;
            public MSSQL.Field InitiateBonusLimitLower;
            public MSSQL.Field InitiateBonusLimitUpper;
            public MSSQL.Field State;
            public MSSQL.Field BettingDescription;
            public MSSQL.Field WinMoney;
            public MSSQL.Field WinMoneyWithoutTax;
            public MSSQL.Field WinDescription;
            public MSSQL.Field Identifiers;
            public MSSQL.Field WriteOff;
            public MSSQL.Field LotteryNumber;

            public T_ElectronTicketAgentSchemes()
            {
                TableName = "T_ElectronTicketAgentSchemes";

                ID = new MSSQL.Field(this, "ID", "ID", SqlDbType.BigInt, true);
                AgentID = new MSSQL.Field(this, "AgentID", "AgentID", SqlDbType.BigInt, false);
                DateTime = new MSSQL.Field(this, "DateTime", "DateTime", SqlDbType.DateTime, false);
                SchemeNumber = new MSSQL.Field(this, "SchemeNumber", "SchemeNumber", SqlDbType.VarChar, false);
                LotteryID = new MSSQL.Field(this, "LotteryID", "LotteryID", SqlDbType.Int, false);
                PlayTypeID = new MSSQL.Field(this, "PlayTypeID", "PlayTypeID", SqlDbType.Int, false);
                IsuseID = new MSSQL.Field(this, "IsuseID", "IsuseID", SqlDbType.BigInt, false);
                Amount = new MSSQL.Field(this, "Amount", "Amount", SqlDbType.Money, false);
                Multiple = new MSSQL.Field(this, "Multiple", "Multiple", SqlDbType.Int, false);
                Share = new MSSQL.Field(this, "Share", "Share", SqlDbType.Int, false);
                InitiateName = new MSSQL.Field(this, "InitiateName", "InitiateName", SqlDbType.VarChar, false);
                InitiateAlipayName = new MSSQL.Field(this, "InitiateAlipayName", "InitiateAlipayName", SqlDbType.VarChar, false);
                InitiateAlipayID = new MSSQL.Field(this, "InitiateAlipayID", "InitiateAlipayID", SqlDbType.VarChar, false);
                InitiateRealityName = new MSSQL.Field(this, "InitiateRealityName", "InitiateRealityName", SqlDbType.VarChar, false);
                InitiateIDCard = new MSSQL.Field(this, "InitiateIDCard", "InitiateIDCard", SqlDbType.VarChar, false);
                InitiateTelephone = new MSSQL.Field(this, "InitiateTelephone", "InitiateTelephone", SqlDbType.VarChar, false);
                InitiateMobile = new MSSQL.Field(this, "InitiateMobile", "InitiateMobile", SqlDbType.VarChar, false);
                InitiateEmail = new MSSQL.Field(this, "InitiateEmail", "InitiateEmail", SqlDbType.VarChar, false);
                InitiateBonusScale = new MSSQL.Field(this, "InitiateBonusScale", "InitiateBonusScale", SqlDbType.Float, false);
                InitiateBonusLimitLower = new MSSQL.Field(this, "InitiateBonusLimitLower", "InitiateBonusLimitLower", SqlDbType.Money, false);
                InitiateBonusLimitUpper = new MSSQL.Field(this, "InitiateBonusLimitUpper", "InitiateBonusLimitUpper", SqlDbType.Money, false);
                State = new MSSQL.Field(this, "State", "State", SqlDbType.SmallInt, false);
                BettingDescription = new MSSQL.Field(this, "BettingDescription", "BettingDescription", SqlDbType.VarChar, false);
                WinMoney = new MSSQL.Field(this, "WinMoney", "WinMoney", SqlDbType.Money, false);
                WinMoneyWithoutTax = new MSSQL.Field(this, "WinMoneyWithoutTax", "WinMoneyWithoutTax", SqlDbType.Money, false);
                WinDescription = new MSSQL.Field(this, "WinDescription", "WinDescription", SqlDbType.VarChar, false);
                Identifiers = new MSSQL.Field(this, "Identifiers", "Identifiers", SqlDbType.VarChar, false);
                WriteOff = new MSSQL.Field(this, "WriteOff", "WriteOff", SqlDbType.Bit, false);
                LotteryNumber = new MSSQL.Field(this, "LotteryNumber", "LotteryNumber", SqlDbType.VarChar, false);
            }
        }

        public class T_ElectronTicketAgentSchemesElectronTickets : MSSQL.TableBase
        {
            public MSSQL.Field ID;
            public MSSQL.Field DateTime;
            public MSSQL.Field SchemeID;
            public MSSQL.Field PlayTypeID;
            public MSSQL.Field Money;
            public MSSQL.Field Multiple;
            public MSSQL.Field Sends;
            public MSSQL.Field HandleDateTime;
            public MSSQL.Field HandleResult;
            public MSSQL.Field HandleDescription;
            public MSSQL.Field Identifiers;
            public MSSQL.Field Ticket;

            public T_ElectronTicketAgentSchemesElectronTickets()
            {
                TableName = "T_ElectronTicketAgentSchemesElectronTickets";

                ID = new MSSQL.Field(this, "ID", "ID", SqlDbType.BigInt, false);
                DateTime = new MSSQL.Field(this, "DateTime", "DateTime", SqlDbType.DateTime, false);
                SchemeID = new MSSQL.Field(this, "SchemeID", "SchemeID", SqlDbType.BigInt, false);
                PlayTypeID = new MSSQL.Field(this, "PlayTypeID", "PlayTypeID", SqlDbType.Int, false);
                Money = new MSSQL.Field(this, "Money", "Money", SqlDbType.Money, false);
                Multiple = new MSSQL.Field(this, "Multiple", "Multiple", SqlDbType.Int, false);
                Sends = new MSSQL.Field(this, "Sends", "Sends", SqlDbType.SmallInt, false);
                HandleDateTime = new MSSQL.Field(this, "HandleDateTime", "HandleDateTime", SqlDbType.DateTime, false);
                HandleResult = new MSSQL.Field(this, "HandleResult", "HandleResult", SqlDbType.SmallInt, false);
                HandleDescription = new MSSQL.Field(this, "HandleDescription", "HandleDescription", SqlDbType.VarChar, false);
                Identifiers = new MSSQL.Field(this, "Identifiers", "Identifiers", SqlDbType.VarChar, false);
                Ticket = new MSSQL.Field(this, "Ticket", "Ticket", SqlDbType.VarChar, false);
            }
        }

        public class T_ElectronTicketAgentSchemesNumber : MSSQL.TableBase
        {
            public MSSQL.Field AgendID;
            public MSSQL.Field SchemeNumber;

            public T_ElectronTicketAgentSchemesNumber()
            {
                TableName = "T_ElectronTicketAgentSchemesNumber";

                AgendID = new MSSQL.Field(this, "AgendID", "AgendID", SqlDbType.BigInt, false);
                SchemeNumber = new MSSQL.Field(this, "SchemeNumber", "SchemeNumber", SqlDbType.VarChar, false);
            }
        }

        public class T_ElectronTicketAgentSchemesSendToCenter : MSSQL.TableBase
        {
            public MSSQL.Field ID;
            public MSSQL.Field DateTime;
            public MSSQL.Field SchemeID;
            public MSSQL.Field PlayTypeID;
            public MSSQL.Field Money;
            public MSSQL.Field Multiple;
            public MSSQL.Field Sends;
            public MSSQL.Field HandleDateTime;
            public MSSQL.Field HandleResult;
            public MSSQL.Field HandleDescription;
            public MSSQL.Field Identifiers;
            public MSSQL.Field Ticket;

            public T_ElectronTicketAgentSchemesSendToCenter()
            {
                TableName = "T_ElectronTicketAgentSchemesSendToCenter";

                ID = new MSSQL.Field(this, "ID", "ID", SqlDbType.BigInt, true);
                DateTime = new MSSQL.Field(this, "DateTime", "DateTime", SqlDbType.DateTime, false);
                SchemeID = new MSSQL.Field(this, "SchemeID", "SchemeID", SqlDbType.BigInt, false);
                PlayTypeID = new MSSQL.Field(this, "PlayTypeID", "PlayTypeID", SqlDbType.Int, false);
                Money = new MSSQL.Field(this, "Money", "Money", SqlDbType.Money, false);
                Multiple = new MSSQL.Field(this, "Multiple", "Multiple", SqlDbType.Int, false);
                Sends = new MSSQL.Field(this, "Sends", "Sends", SqlDbType.SmallInt, false);
                HandleDateTime = new MSSQL.Field(this, "HandleDateTime", "HandleDateTime", SqlDbType.DateTime, false);
                HandleResult = new MSSQL.Field(this, "HandleResult", "HandleResult", SqlDbType.SmallInt, false);
                HandleDescription = new MSSQL.Field(this, "HandleDescription", "HandleDescription", SqlDbType.VarChar, false);
                Identifiers = new MSSQL.Field(this, "Identifiers", "Identifiers", SqlDbType.VarChar, false);
                Ticket = new MSSQL.Field(this, "Ticket", "Ticket", SqlDbType.VarChar, false);
            }
        }

        public class T_ElectronTicketLog : MSSQL.TableBase
        {
            public MSSQL.Field id;
            public MSSQL.Field TransType;
            public MSSQL.Field datetime;
            public MSSQL.Field Send;
            public MSSQL.Field TransMessage;

            public T_ElectronTicketLog()
            {
                TableName = "T_ElectronTicketLog";

                id = new MSSQL.Field(this, "id", "id", SqlDbType.BigInt, true);
                TransType = new MSSQL.Field(this, "TransType", "TransType", SqlDbType.VarChar, false);
                datetime = new MSSQL.Field(this, "datetime", "datetime", SqlDbType.DateTime, false);
                Send = new MSSQL.Field(this, "Send", "Send", SqlDbType.Bit, false);
                TransMessage = new MSSQL.Field(this, "TransMessage", "TransMessage", SqlDbType.VarChar, false);
            }
        }

        public class T_ExecutedChases : MSSQL.TableBase
        {
            public MSSQL.Field ChaseID;
            public MSSQL.Field SchemeID;

            public T_ExecutedChases()
            {
                TableName = "T_ExecutedChases";

                ChaseID = new MSSQL.Field(this, "ChaseID", "ChaseID", SqlDbType.BigInt, false);
                SchemeID = new MSSQL.Field(this, "SchemeID", "SchemeID", SqlDbType.BigInt, false);
            }
        }

        public class T_Experts : MSSQL.TableBase
        {
            public MSSQL.Field ID;
            public MSSQL.Field SiteID;
            public MSSQL.Field UserID;
            public MSSQL.Field DateTime;
            public MSSQL.Field LotteryID;
            public MSSQL.Field Description;
            public MSSQL.Field isCanIssued;
            public MSSQL.Field MaxPrice;
            public MSSQL.Field BonusScale;
            public MSSQL.Field ON;
            public MSSQL.Field ReadCount;
            public MSSQL.Field isCommend;

            public T_Experts()
            {
                TableName = "T_Experts";

                ID = new MSSQL.Field(this, "ID", "ID", SqlDbType.BigInt, true);
                SiteID = new MSSQL.Field(this, "SiteID", "SiteID", SqlDbType.BigInt, false);
                UserID = new MSSQL.Field(this, "UserID", "UserID", SqlDbType.BigInt, false);
                DateTime = new MSSQL.Field(this, "DateTime", "DateTime", SqlDbType.DateTime, false);
                LotteryID = new MSSQL.Field(this, "LotteryID", "LotteryID", SqlDbType.Int, false);
                Description = new MSSQL.Field(this, "Description", "Description", SqlDbType.VarChar, false);
                isCanIssued = new MSSQL.Field(this, "isCanIssued", "isCanIssued", SqlDbType.Bit, false);
                MaxPrice = new MSSQL.Field(this, "MaxPrice", "MaxPrice", SqlDbType.Money, false);
                BonusScale = new MSSQL.Field(this, "BonusScale", "BonusScale", SqlDbType.Float, false);
                ON = new MSSQL.Field(this, "ON", "ON", SqlDbType.Bit, false);
                ReadCount = new MSSQL.Field(this, "ReadCount", "ReadCount", SqlDbType.Int, false);
                isCommend = new MSSQL.Field(this, "isCommend", "isCommend", SqlDbType.Bit, false);
            }
        }

        public class T_ExpertsCommendRead : MSSQL.TableBase
        {
            public MSSQL.Field ID;
            public MSSQL.Field SiteID;
            public MSSQL.Field ExpertsCommendID;
            public MSSQL.Field UserID;

            public T_ExpertsCommendRead()
            {
                TableName = "T_ExpertsCommendRead";

                ID = new MSSQL.Field(this, "ID", "ID", SqlDbType.BigInt, true);
                SiteID = new MSSQL.Field(this, "SiteID", "SiteID", SqlDbType.BigInt, false);
                ExpertsCommendID = new MSSQL.Field(this, "ExpertsCommendID", "ExpertsCommendID", SqlDbType.BigInt, false);
                UserID = new MSSQL.Field(this, "UserID", "UserID", SqlDbType.BigInt, false);
            }
        }

        public class T_ExpertsCommends : MSSQL.TableBase
        {
            public MSSQL.Field ID;
            public MSSQL.Field SiteID;
            public MSSQL.Field ExpertsID;
            public MSSQL.Field DateTime;
            public MSSQL.Field IsuseID;
            public MSSQL.Field Title;
            public MSSQL.Field Price;
            public MSSQL.Field ReadCount;
            public MSSQL.Field WinMoney;
            public MSSQL.Field isCommend;
            public MSSQL.Field Content;
            public MSSQL.Field Number;

            public T_ExpertsCommends()
            {
                TableName = "T_ExpertsCommends";

                ID = new MSSQL.Field(this, "ID", "ID", SqlDbType.BigInt, true);
                SiteID = new MSSQL.Field(this, "SiteID", "SiteID", SqlDbType.BigInt, false);
                ExpertsID = new MSSQL.Field(this, "ExpertsID", "ExpertsID", SqlDbType.BigInt, false);
                DateTime = new MSSQL.Field(this, "DateTime", "DateTime", SqlDbType.DateTime, false);
                IsuseID = new MSSQL.Field(this, "IsuseID", "IsuseID", SqlDbType.BigInt, false);
                Title = new MSSQL.Field(this, "Title", "Title", SqlDbType.VarChar, false);
                Price = new MSSQL.Field(this, "Price", "Price", SqlDbType.Money, false);
                ReadCount = new MSSQL.Field(this, "ReadCount", "ReadCount", SqlDbType.Int, false);
                WinMoney = new MSSQL.Field(this, "WinMoney", "WinMoney", SqlDbType.Money, false);
                isCommend = new MSSQL.Field(this, "isCommend", "isCommend", SqlDbType.Bit, false);
                Content = new MSSQL.Field(this, "Content", "Content", SqlDbType.VarChar, false);
                Number = new MSSQL.Field(this, "Number", "Number", SqlDbType.VarChar, false);
            }
        }

        public class T_ExpertsPredict : MSSQL.TableBase
        {
            public MSSQL.Field ID;
            public MSSQL.Field Name;
            public MSSQL.Field DateTime;
            public MSSQL.Field LotteryID;
            public MSSQL.Field Description;
            public MSSQL.Field ON;
            public MSSQL.Field URL;

            public T_ExpertsPredict()
            {
                TableName = "T_ExpertsPredict";

                ID = new MSSQL.Field(this, "ID", "ID", SqlDbType.Int, true);
                Name = new MSSQL.Field(this, "Name", "Name", SqlDbType.VarChar, false);
                DateTime = new MSSQL.Field(this, "DateTime", "DateTime", SqlDbType.DateTime, false);
                LotteryID = new MSSQL.Field(this, "LotteryID", "LotteryID", SqlDbType.Int, false);
                Description = new MSSQL.Field(this, "Description", "Description", SqlDbType.VarChar, false);
                ON = new MSSQL.Field(this, "ON", "ON", SqlDbType.Bit, false);
                URL = new MSSQL.Field(this, "URL", "URL", SqlDbType.VarChar, false);
            }
        }

        public class T_ExpertsPredictCommends : MSSQL.TableBase
        {
            public MSSQL.Field ID;
            public MSSQL.Field ExpertsPredictID;
            public MSSQL.Field CommendID;
            public MSSQL.Field IsShow;
            public MSSQL.Field Content;
            public MSSQL.Field DateTime;

            public T_ExpertsPredictCommends()
            {
                TableName = "T_ExpertsPredictCommends";

                ID = new MSSQL.Field(this, "ID", "ID", SqlDbType.Int, true);
                ExpertsPredictID = new MSSQL.Field(this, "ExpertsPredictID", "ExpertsPredictID", SqlDbType.Int, false);
                CommendID = new MSSQL.Field(this, "CommendID", "CommendID", SqlDbType.BigInt, false);
                IsShow = new MSSQL.Field(this, "IsShow", "IsShow", SqlDbType.Bit, false);
                Content = new MSSQL.Field(this, "Content", "Content", SqlDbType.VarChar, false);
                DateTime = new MSSQL.Field(this, "DateTime", "DateTime", SqlDbType.DateTime, false);
            }
        }

        public class T_ExpertsPredictNews : MSSQL.TableBase
        {
            public MSSQL.Field ID;
            public MSSQL.Field DateTime;
            public MSSQL.Field ExpertsPredictID;
            public MSSQL.Field Description;
            public MSSQL.Field ON;
            public MSSQL.Field URL;
            public MSSQL.Field isWinning;

            public T_ExpertsPredictNews()
            {
                TableName = "T_ExpertsPredictNews";

                ID = new MSSQL.Field(this, "ID", "ID", SqlDbType.BigInt, true);
                DateTime = new MSSQL.Field(this, "DateTime", "DateTime", SqlDbType.DateTime, false);
                ExpertsPredictID = new MSSQL.Field(this, "ExpertsPredictID", "ExpertsPredictID", SqlDbType.Int, false);
                Description = new MSSQL.Field(this, "Description", "Description", SqlDbType.VarChar, false);
                ON = new MSSQL.Field(this, "ON", "ON", SqlDbType.Bit, false);
                URL = new MSSQL.Field(this, "URL", "URL", SqlDbType.VarChar, false);
                isWinning = new MSSQL.Field(this, "isWinning", "isWinning", SqlDbType.Bit, false);
            }
        }

        public class T_ExpertsTrys : MSSQL.TableBase
        {
            public MSSQL.Field ID;
            public MSSQL.Field SiteID;
            public MSSQL.Field UserID;
            public MSSQL.Field DateTime;
            public MSSQL.Field LotteryID;
            public MSSQL.Field Description;
            public MSSQL.Field MaxPrice;
            public MSSQL.Field BonusScale;
            public MSSQL.Field HandleResult;
            public MSSQL.Field HandleDateTime;

            public T_ExpertsTrys()
            {
                TableName = "T_ExpertsTrys";

                ID = new MSSQL.Field(this, "ID", "ID", SqlDbType.BigInt, true);
                SiteID = new MSSQL.Field(this, "SiteID", "SiteID", SqlDbType.BigInt, false);
                UserID = new MSSQL.Field(this, "UserID", "UserID", SqlDbType.BigInt, false);
                DateTime = new MSSQL.Field(this, "DateTime", "DateTime", SqlDbType.DateTime, false);
                LotteryID = new MSSQL.Field(this, "LotteryID", "LotteryID", SqlDbType.Int, false);
                Description = new MSSQL.Field(this, "Description", "Description", SqlDbType.VarChar, false);
                MaxPrice = new MSSQL.Field(this, "MaxPrice", "MaxPrice", SqlDbType.Money, false);
                BonusScale = new MSSQL.Field(this, "BonusScale", "BonusScale", SqlDbType.Float, false);
                HandleResult = new MSSQL.Field(this, "HandleResult", "HandleResult", SqlDbType.SmallInt, false);
                HandleDateTime = new MSSQL.Field(this, "HandleDateTime", "HandleDateTime", SqlDbType.DateTime, false);
            }
        }

        public class T_ExpertsWinCommends : MSSQL.TableBase
        {
            public MSSQL.Field ID;
            public MSSQL.Field SiteID;
            public MSSQL.Field UserID;
            public MSSQL.Field DateTime;
            public MSSQL.Field IsuseID;
            public MSSQL.Field Title;
            public MSSQL.Field isShow;
            public MSSQL.Field ON;
            public MSSQL.Field ReadCount;
            public MSSQL.Field isCommend;
            public MSSQL.Field Content;

            public T_ExpertsWinCommends()
            {
                TableName = "T_ExpertsWinCommends";

                ID = new MSSQL.Field(this, "ID", "ID", SqlDbType.BigInt, true);
                SiteID = new MSSQL.Field(this, "SiteID", "SiteID", SqlDbType.BigInt, false);
                UserID = new MSSQL.Field(this, "UserID", "UserID", SqlDbType.BigInt, false);
                DateTime = new MSSQL.Field(this, "DateTime", "DateTime", SqlDbType.DateTime, false);
                IsuseID = new MSSQL.Field(this, "IsuseID", "IsuseID", SqlDbType.BigInt, false);
                Title = new MSSQL.Field(this, "Title", "Title", SqlDbType.VarChar, false);
                isShow = new MSSQL.Field(this, "isShow", "isShow", SqlDbType.Bit, false);
                ON = new MSSQL.Field(this, "ON", "ON", SqlDbType.Bit, false);
                ReadCount = new MSSQL.Field(this, "ReadCount", "ReadCount", SqlDbType.Int, false);
                isCommend = new MSSQL.Field(this, "isCommend", "isCommend", SqlDbType.Bit, false);
                Content = new MSSQL.Field(this, "Content", "Content", SqlDbType.VarChar, false);
            }
        }

        public class T_FloatNotify : MSSQL.TableBase
        {
            public MSSQL.Field ID;
            public MSSQL.Field Title;
            public MSSQL.Field Color;
            public MSSQL.Field Url;
            public MSSQL.Field DateTime;
            public MSSQL.Field Order;
            public MSSQL.Field isShow;

            public T_FloatNotify()
            {
                TableName = "T_FloatNotify";

                ID = new MSSQL.Field(this, "ID", "ID", SqlDbType.Int, true);
                Title = new MSSQL.Field(this, "Title", "Title", SqlDbType.VarChar, false);
                Color = new MSSQL.Field(this, "Color", "Color", SqlDbType.VarChar, false);
                Url = new MSSQL.Field(this, "Url", "Url", SqlDbType.VarChar, false);
                DateTime = new MSSQL.Field(this, "DateTime", "DateTime", SqlDbType.DateTime, false);
                Order = new MSSQL.Field(this, "Order", "Order", SqlDbType.Int, false);
                isShow = new MSSQL.Field(this, "isShow", "isShow", SqlDbType.Bit, false);
            }
        }

        public class T_FocusImageNews : MSSQL.TableBase
        {
            public MSSQL.Field ID;
            public MSSQL.Field Title;
            public MSSQL.Field ImageUrl;
            public MSSQL.Field Url;
            public MSSQL.Field DateTime;
            public MSSQL.Field IsBig;

            public T_FocusImageNews()
            {
                TableName = "T_FocusImageNews";

                ID = new MSSQL.Field(this, "ID", "ID", SqlDbType.Int, true);
                Title = new MSSQL.Field(this, "Title", "Title", SqlDbType.VarChar, false);
                ImageUrl = new MSSQL.Field(this, "ImageUrl", "ImageUrl", SqlDbType.VarChar, false);
                Url = new MSSQL.Field(this, "Url", "Url", SqlDbType.VarChar, false);
                DateTime = new MSSQL.Field(this, "DateTime", "DateTime", SqlDbType.DateTime, false);
                IsBig = new MSSQL.Field(this, "IsBig", "IsBig", SqlDbType.Bit, false);
            }
        }

        public class T_FocusNews : MSSQL.TableBase
        {
            public MSSQL.Field ID;
            public MSSQL.Field Title;
            public MSSQL.Field Url;
            public MSSQL.Field DateTime;
            public MSSQL.Field IsMaster;
            public MSSQL.Field Order;

            public T_FocusNews()
            {
                TableName = "T_FocusNews";

                ID = new MSSQL.Field(this, "ID", "ID", SqlDbType.Int, true);
                Title = new MSSQL.Field(this, "Title", "Title", SqlDbType.VarChar, false);
                Url = new MSSQL.Field(this, "Url", "Url", SqlDbType.VarChar, false);
                DateTime = new MSSQL.Field(this, "DateTime", "DateTime", SqlDbType.DateTime, false);
                IsMaster = new MSSQL.Field(this, "IsMaster", "IsMaster", SqlDbType.Bit, false);
                Order = new MSSQL.Field(this, "Order", "Order", SqlDbType.SmallInt, false);
            }
        }

        public class T_FootballLeagueTypes : MSSQL.TableBase
        {
            public MSSQL.Field ID;
            public MSSQL.Field Name;
            public MSSQL.Field Code;
            public MSSQL.Field MarkersColor;
            public MSSQL.Field Description;
            public MSSQL.Field Order;
            public MSSQL.Field isUse;

            public T_FootballLeagueTypes()
            {
                TableName = "T_FootballLeagueTypes";

                ID = new MSSQL.Field(this, "ID", "ID", SqlDbType.Int, true);
                Name = new MSSQL.Field(this, "Name", "Name", SqlDbType.VarChar, false);
                Code = new MSSQL.Field(this, "Code", "Code", SqlDbType.VarChar, false);
                MarkersColor = new MSSQL.Field(this, "MarkersColor", "MarkersColor", SqlDbType.VarChar, false);
                Description = new MSSQL.Field(this, "Description", "Description", SqlDbType.VarChar, false);
                Order = new MSSQL.Field(this, "Order", "Order", SqlDbType.Int, false);
                isUse = new MSSQL.Field(this, "isUse", "isUse", SqlDbType.Bit, false);
            }
        }

        public class T_FriendshipLinks : MSSQL.TableBase
        {
            public MSSQL.Field ID;
            public MSSQL.Field SiteID;
            public MSSQL.Field LinkName;
            public MSSQL.Field LogoUrl;
            public MSSQL.Field Url;
            public MSSQL.Field Order;
            public MSSQL.Field isShow;

            public T_FriendshipLinks()
            {
                TableName = "T_FriendshipLinks";

                ID = new MSSQL.Field(this, "ID", "ID", SqlDbType.Int, true);
                SiteID = new MSSQL.Field(this, "SiteID", "SiteID", SqlDbType.BigInt, false);
                LinkName = new MSSQL.Field(this, "LinkName", "LinkName", SqlDbType.VarChar, false);
                LogoUrl = new MSSQL.Field(this, "LogoUrl", "LogoUrl", SqlDbType.VarChar, false);
                Url = new MSSQL.Field(this, "Url", "Url", SqlDbType.VarChar, false);
                Order = new MSSQL.Field(this, "Order", "Order", SqlDbType.Int, false);
                isShow = new MSSQL.Field(this, "isShow", "isShow", SqlDbType.Bit, false);
            }
        }

        public class T_Helps : MSSQL.TableBase
        {
            public MSSQL.Field ID;
            public MSSQL.Field Title;
            public MSSQL.Field Content;

            public T_Helps()
            {
                TableName = "T_Helps";

                ID = new MSSQL.Field(this, "ID", "ID", SqlDbType.BigInt, true);
                Title = new MSSQL.Field(this, "Title", "Title", SqlDbType.VarChar, false);
                Content = new MSSQL.Field(this, "Content", "Content", SqlDbType.VarChar, false);
            }
        }

        public class T_IPAddress : MSSQL.TableBase
        {
            public MSSQL.Field IPStart;
            public MSSQL.Field IPEnd;
            public MSSQL.Field Country;
            public MSSQL.Field City;

            public T_IPAddress()
            {
                TableName = "T_IPAddress";

                IPStart = new MSSQL.Field(this, "IPStart", "IPStart", SqlDbType.Float, false);
                IPEnd = new MSSQL.Field(this, "IPEnd", "IPEnd", SqlDbType.Float, false);
                Country = new MSSQL.Field(this, "Country", "Country", SqlDbType.NVarChar, false);
                City = new MSSQL.Field(this, "City", "City", SqlDbType.NVarChar, false);
            }
        }

        public class T_IsShowedCustomFollowSchemesForIcaile : MSSQL.TableBase
        {
            public MSSQL.Field ID;

            public T_IsShowedCustomFollowSchemesForIcaile()
            {
                TableName = "T_IsShowedCustomFollowSchemesForIcaile";

                ID = new MSSQL.Field(this, "ID", "ID", SqlDbType.BigInt, false);
            }
        }

        public class T_IsuseBonuses : MSSQL.TableBase
        {
            public MSSQL.Field ID;
            public MSSQL.Field UserID;
            public MSSQL.Field IsuseID;
            public MSSQL.Field defaultMoney;
            public MSSQL.Field DefaultMoneyNoWithTax;

            public T_IsuseBonuses()
            {
                TableName = "T_IsuseBonuses";

                ID = new MSSQL.Field(this, "ID", "ID", SqlDbType.BigInt, true);
                UserID = new MSSQL.Field(this, "UserID", "UserID", SqlDbType.BigInt, false);
                IsuseID = new MSSQL.Field(this, "IsuseID", "IsuseID", SqlDbType.BigInt, false);
                defaultMoney = new MSSQL.Field(this, "defaultMoney", "defaultMoney", SqlDbType.Money, false);
                DefaultMoneyNoWithTax = new MSSQL.Field(this, "DefaultMoneyNoWithTax", "DefaultMoneyNoWithTax", SqlDbType.Money, false);
            }
        }

        public class T_IsuseForJQC : MSSQL.TableBase
        {
            public MSSQL.Field ID;
            public MSSQL.Field IsuseID;
            public MSSQL.Field No;
            public MSSQL.Field Team;
            public MSSQL.Field DateTime;

            public T_IsuseForJQC()
            {
                TableName = "T_IsuseForJQC";

                ID = new MSSQL.Field(this, "ID", "ID", SqlDbType.BigInt, true);
                IsuseID = new MSSQL.Field(this, "IsuseID", "IsuseID", SqlDbType.BigInt, false);
                No = new MSSQL.Field(this, "No", "No", SqlDbType.SmallInt, false);
                Team = new MSSQL.Field(this, "Team", "Team", SqlDbType.VarChar, false);
                DateTime = new MSSQL.Field(this, "DateTime", "DateTime", SqlDbType.VarChar, false);
            }
        }

        public class T_IsuseForLCBQC : MSSQL.TableBase
        {
            public MSSQL.Field ID;
            public MSSQL.Field IsuseID;
            public MSSQL.Field No;
            public MSSQL.Field HostTeam;
            public MSSQL.Field QuestTeam;
            public MSSQL.Field DateTime;

            public T_IsuseForLCBQC()
            {
                TableName = "T_IsuseForLCBQC";

                ID = new MSSQL.Field(this, "ID", "ID", SqlDbType.BigInt, true);
                IsuseID = new MSSQL.Field(this, "IsuseID", "IsuseID", SqlDbType.BigInt, false);
                No = new MSSQL.Field(this, "No", "No", SqlDbType.SmallInt, false);
                HostTeam = new MSSQL.Field(this, "HostTeam", "HostTeam", SqlDbType.VarChar, false);
                QuestTeam = new MSSQL.Field(this, "QuestTeam", "QuestTeam", SqlDbType.VarChar, false);
                DateTime = new MSSQL.Field(this, "DateTime", "DateTime", SqlDbType.VarChar, false);
            }
        }

        public class T_IsuseForLCDC : MSSQL.TableBase
        {
            public MSSQL.Field ID;
            public MSSQL.Field IsuseID;
            public MSSQL.Field No;
            public MSSQL.Field HostTeam;
            public MSSQL.Field QuestTeam;
            public MSSQL.Field DateTime;

            public T_IsuseForLCDC()
            {
                TableName = "T_IsuseForLCDC";

                ID = new MSSQL.Field(this, "ID", "ID", SqlDbType.BigInt, true);
                IsuseID = new MSSQL.Field(this, "IsuseID", "IsuseID", SqlDbType.BigInt, false);
                No = new MSSQL.Field(this, "No", "No", SqlDbType.SmallInt, false);
                HostTeam = new MSSQL.Field(this, "HostTeam", "HostTeam", SqlDbType.VarChar, false);
                QuestTeam = new MSSQL.Field(this, "QuestTeam", "QuestTeam", SqlDbType.VarChar, false);
                DateTime = new MSSQL.Field(this, "DateTime", "DateTime", SqlDbType.VarChar, false);
            }
        }

        public class T_IsuseForSFC : MSSQL.TableBase
        {
            public MSSQL.Field ID;
            public MSSQL.Field IsuseID;
            public MSSQL.Field No;
            public MSSQL.Field HostTeam;
            public MSSQL.Field QuestTeam;
            public MSSQL.Field DateTime;

            public T_IsuseForSFC()
            {
                TableName = "T_IsuseForSFC";

                ID = new MSSQL.Field(this, "ID", "ID", SqlDbType.BigInt, true);
                IsuseID = new MSSQL.Field(this, "IsuseID", "IsuseID", SqlDbType.BigInt, false);
                No = new MSSQL.Field(this, "No", "No", SqlDbType.SmallInt, false);
                HostTeam = new MSSQL.Field(this, "HostTeam", "HostTeam", SqlDbType.VarChar, false);
                QuestTeam = new MSSQL.Field(this, "QuestTeam", "QuestTeam", SqlDbType.VarChar, false);
                DateTime = new MSSQL.Field(this, "DateTime", "DateTime", SqlDbType.VarChar, false);
            }
        }

        public class T_IsuseForZCDC : MSSQL.TableBase
        {
            public MSSQL.Field ID;
            public MSSQL.Field IsuseID;
            public MSSQL.Field LeagueTypeID;
            public MSSQL.Field No;
            public MSSQL.Field HostTeam;
            public MSSQL.Field QuestTeam;
            public MSSQL.Field LetBall;
            public MSSQL.Field DateTime;
            public MSSQL.Field HalftimeResult;
            public MSSQL.Field Result;
            public MSSQL.Field SPFResult;
            public MSSQL.Field SPF_SP;
            public MSSQL.Field ZJQResult;
            public MSSQL.Field ZJQ_SP;
            public MSSQL.Field SXDSResult;
            public MSSQL.Field SXDS_SP;
            public MSSQL.Field ZQBFResult;
            public MSSQL.Field ZQBF_SP;
            public MSSQL.Field BQCSPFResult;
            public MSSQL.Field BQCSPF_SP;
            public MSSQL.Field AnalysisURL;

            public T_IsuseForZCDC()
            {
                TableName = "T_IsuseForZCDC";

                ID = new MSSQL.Field(this, "ID", "ID", SqlDbType.BigInt, true);
                IsuseID = new MSSQL.Field(this, "IsuseID", "IsuseID", SqlDbType.BigInt, false);
                LeagueTypeID = new MSSQL.Field(this, "LeagueTypeID", "LeagueTypeID", SqlDbType.Int, false);
                No = new MSSQL.Field(this, "No", "No", SqlDbType.SmallInt, false);
                HostTeam = new MSSQL.Field(this, "HostTeam", "HostTeam", SqlDbType.VarChar, false);
                QuestTeam = new MSSQL.Field(this, "QuestTeam", "QuestTeam", SqlDbType.VarChar, false);
                LetBall = new MSSQL.Field(this, "LetBall", "LetBall", SqlDbType.SmallInt, false);
                DateTime = new MSSQL.Field(this, "DateTime", "DateTime", SqlDbType.VarChar, false);
                HalftimeResult = new MSSQL.Field(this, "HalftimeResult", "HalftimeResult", SqlDbType.VarChar, false);
                Result = new MSSQL.Field(this, "Result", "Result", SqlDbType.VarChar, false);
                SPFResult = new MSSQL.Field(this, "SPFResult", "SPFResult", SqlDbType.VarChar, false);
                SPF_SP = new MSSQL.Field(this, "SPF_SP", "SPF_SP", SqlDbType.Float, false);
                ZJQResult = new MSSQL.Field(this, "ZJQResult", "ZJQResult", SqlDbType.VarChar, false);
                ZJQ_SP = new MSSQL.Field(this, "ZJQ_SP", "ZJQ_SP", SqlDbType.Float, false);
                SXDSResult = new MSSQL.Field(this, "SXDSResult", "SXDSResult", SqlDbType.VarChar, false);
                SXDS_SP = new MSSQL.Field(this, "SXDS_SP", "SXDS_SP", SqlDbType.Float, false);
                ZQBFResult = new MSSQL.Field(this, "ZQBFResult", "ZQBFResult", SqlDbType.VarChar, false);
                ZQBF_SP = new MSSQL.Field(this, "ZQBF_SP", "ZQBF_SP", SqlDbType.Float, false);
                BQCSPFResult = new MSSQL.Field(this, "BQCSPFResult", "BQCSPFResult", SqlDbType.VarChar, false);
                BQCSPF_SP = new MSSQL.Field(this, "BQCSPF_SP", "BQCSPF_SP", SqlDbType.Float, false);
                AnalysisURL = new MSSQL.Field(this, "AnalysisURL", "AnalysisURL", SqlDbType.VarChar, false);
            }
        }

        public class T_Isuses : MSSQL.TableBase
        {
            public MSSQL.Field ID;
            public MSSQL.Field LotteryID;
            public MSSQL.Field Name;
            public MSSQL.Field StartTime;
            public MSSQL.Field EndTime;
            public MSSQL.Field ChaseExecuted;
            public MSSQL.Field IsOpened;
            public MSSQL.Field WinLotteryNumber;
            public MSSQL.Field OpenOperatorID;
            public MSSQL.Field State;
            public MSSQL.Field StateUpdateTime;
            public MSSQL.Field OpenAffiche;

            public T_Isuses()
            {
                TableName = "T_Isuses";

                ID = new MSSQL.Field(this, "ID", "ID", SqlDbType.BigInt, true);
                LotteryID = new MSSQL.Field(this, "LotteryID", "LotteryID", SqlDbType.Int, false);
                Name = new MSSQL.Field(this, "Name", "Name", SqlDbType.VarChar, false);
                StartTime = new MSSQL.Field(this, "StartTime", "StartTime", SqlDbType.DateTime, false);
                EndTime = new MSSQL.Field(this, "EndTime", "EndTime", SqlDbType.DateTime, false);
                ChaseExecuted = new MSSQL.Field(this, "ChaseExecuted", "ChaseExecuted", SqlDbType.Bit, false);
                IsOpened = new MSSQL.Field(this, "IsOpened", "IsOpened", SqlDbType.Bit, false);
                WinLotteryNumber = new MSSQL.Field(this, "WinLotteryNumber", "WinLotteryNumber", SqlDbType.VarChar, false);
                OpenOperatorID = new MSSQL.Field(this, "OpenOperatorID", "OpenOperatorID", SqlDbType.BigInt, false);
                State = new MSSQL.Field(this, "State", "State", SqlDbType.SmallInt, false);
                StateUpdateTime = new MSSQL.Field(this, "StateUpdateTime", "StateUpdateTime", SqlDbType.DateTime, false);
                OpenAffiche = new MSSQL.Field(this, "OpenAffiche", "OpenAffiche", SqlDbType.VarChar, false);
            }
        }

        public class T_Lotteries : MSSQL.TableBase
        {
            public MSSQL.Field ID;
            public MSSQL.Field Name;
            public MSSQL.Field Code;
            public MSSQL.Field MaxChaseIsuse;
            public MSSQL.Field ChaseExecuteDeferMinute;
            public MSSQL.Field Order;
            public MSSQL.Field WinNumberExemple;
            public MSSQL.Field IntervalType;
            public MSSQL.Field PrintOutType;
            public MSSQL.Field TypeID;
            public MSSQL.Field Type2;
            public MSSQL.Field Agreement;
            public MSSQL.Field Explain;
            public MSSQL.Field SchemeExemple;
            public MSSQL.Field OpenAfficheTemplate;
            public MSSQL.Field QuashExecuteDeferMinute;

            public T_Lotteries()
            {
                TableName = "T_Lotteries";

                ID = new MSSQL.Field(this, "ID", "ID", SqlDbType.Int, false);
                Name = new MSSQL.Field(this, "Name", "Name", SqlDbType.VarChar, false);
                Code = new MSSQL.Field(this, "Code", "Code", SqlDbType.VarChar, false);
                MaxChaseIsuse = new MSSQL.Field(this, "MaxChaseIsuse", "MaxChaseIsuse", SqlDbType.VarChar, false);
                ChaseExecuteDeferMinute = new MSSQL.Field(this, "ChaseExecuteDeferMinute", "ChaseExecuteDeferMinute", SqlDbType.Int, false);
                Order = new MSSQL.Field(this, "Order", "Order", SqlDbType.Int, false);
                WinNumberExemple = new MSSQL.Field(this, "WinNumberExemple", "WinNumberExemple", SqlDbType.VarChar, false);
                IntervalType = new MSSQL.Field(this, "IntervalType", "IntervalType", SqlDbType.VarChar, false);
                PrintOutType = new MSSQL.Field(this, "PrintOutType", "PrintOutType", SqlDbType.SmallInt, false);
                TypeID = new MSSQL.Field(this, "TypeID", "TypeID", SqlDbType.SmallInt, false);
                Type2 = new MSSQL.Field(this, "Type2", "Type2", SqlDbType.SmallInt, false);
                Agreement = new MSSQL.Field(this, "Agreement", "Agreement", SqlDbType.VarChar, false);
                Explain = new MSSQL.Field(this, "Explain", "Explain", SqlDbType.VarChar, false);
                SchemeExemple = new MSSQL.Field(this, "SchemeExemple", "SchemeExemple", SqlDbType.VarChar, false);
                OpenAfficheTemplate = new MSSQL.Field(this, "OpenAfficheTemplate", "OpenAfficheTemplate", SqlDbType.VarChar, false);
                QuashExecuteDeferMinute = new MSSQL.Field(this, "QuashExecuteDeferMinute", "QuashExecuteDeferMinute", SqlDbType.Int, false);
            }
        }

        public class T_LotteryToolLinks : MSSQL.TableBase
        {
            public MSSQL.Field ID;
            public MSSQL.Field SiteID;
            public MSSQL.Field LotteryID;
            public MSSQL.Field LinkName;
            public MSSQL.Field LogoUrl;
            public MSSQL.Field Url;
            public MSSQL.Field Order;
            public MSSQL.Field isShow;

            public T_LotteryToolLinks()
            {
                TableName = "T_LotteryToolLinks";

                ID = new MSSQL.Field(this, "ID", "ID", SqlDbType.Int, true);
                SiteID = new MSSQL.Field(this, "SiteID", "SiteID", SqlDbType.BigInt, false);
                LotteryID = new MSSQL.Field(this, "LotteryID", "LotteryID", SqlDbType.Int, false);
                LinkName = new MSSQL.Field(this, "LinkName", "LinkName", SqlDbType.VarChar, false);
                LogoUrl = new MSSQL.Field(this, "LogoUrl", "LogoUrl", SqlDbType.VarChar, false);
                Url = new MSSQL.Field(this, "Url", "Url", SqlDbType.VarChar, false);
                Order = new MSSQL.Field(this, "Order", "Order", SqlDbType.Int, false);
                isShow = new MSSQL.Field(this, "isShow", "isShow", SqlDbType.Bit, false);
            }
        }

        public class T_LotteryType : MSSQL.TableBase
        {
            public MSSQL.Field ID;
            public MSSQL.Field ParentID;
            public MSSQL.Field Name;
            public MSSQL.Field Description;
            public MSSQL.Field Order;

            public T_LotteryType()
            {
                TableName = "T_LotteryType";

                ID = new MSSQL.Field(this, "ID", "ID", SqlDbType.SmallInt, true);
                ParentID = new MSSQL.Field(this, "ParentID", "ParentID", SqlDbType.SmallInt, false);
                Name = new MSSQL.Field(this, "Name", "Name", SqlDbType.VarChar, false);
                Description = new MSSQL.Field(this, "Description", "Description", SqlDbType.VarChar, false);
                Order = new MSSQL.Field(this, "Order", "Order", SqlDbType.SmallInt, false);
            }
        }

        public class T_LuckNumber : MSSQL.TableBase
        {
            public MSSQL.Field ID;
            public MSSQL.Field LotteryID;
            public MSSQL.Field Type;
            public MSSQL.Field Name;
            public MSSQL.Field LotteryNumber;
            public MSSQL.Field DateTime;

            public T_LuckNumber()
            {
                TableName = "T_LuckNumber";

                ID = new MSSQL.Field(this, "ID", "ID", SqlDbType.BigInt, true);
                LotteryID = new MSSQL.Field(this, "LotteryID", "LotteryID", SqlDbType.Int, false);
                Type = new MSSQL.Field(this, "Type", "Type", SqlDbType.SmallInt, false);
                Name = new MSSQL.Field(this, "Name", "Name", SqlDbType.VarChar, false);
                LotteryNumber = new MSSQL.Field(this, "LotteryNumber", "LotteryNumber", SqlDbType.VarChar, false);
                DateTime = new MSSQL.Field(this, "DateTime", "DateTime", SqlDbType.DateTime, false);
            }
        }

        public class T_MarketOutlook : MSSQL.TableBase
        {
            public MSSQL.Field ID;
            public MSSQL.Field DateTime;
            public MSSQL.Field Title;
            public MSSQL.Field isShow;
            public MSSQL.Field Content;

            public T_MarketOutlook()
            {
                TableName = "T_MarketOutlook";

                ID = new MSSQL.Field(this, "ID", "ID", SqlDbType.BigInt, true);
                DateTime = new MSSQL.Field(this, "DateTime", "DateTime", SqlDbType.DateTime, false);
                Title = new MSSQL.Field(this, "Title", "Title", SqlDbType.VarChar, false);
                isShow = new MSSQL.Field(this, "isShow", "isShow", SqlDbType.Bit, false);
                Content = new MSSQL.Field(this, "Content", "Content", SqlDbType.VarChar, false);
            }
        }

        public class T_MaxMultiple : MSSQL.TableBase
        {
            public MSSQL.Field ID;
            public MSSQL.Field IsuseID;
            public MSSQL.Field PlayTypeID;
            public MSSQL.Field MaxMultiple;

            public T_MaxMultiple()
            {
                TableName = "T_MaxMultiple";

                ID = new MSSQL.Field(this, "ID", "ID", SqlDbType.BigInt, true);
                IsuseID = new MSSQL.Field(this, "IsuseID", "IsuseID", SqlDbType.BigInt, false);
                PlayTypeID = new MSSQL.Field(this, "PlayTypeID", "PlayTypeID", SqlDbType.Int, false);
                MaxMultiple = new MSSQL.Field(this, "MaxMultiple", "MaxMultiple", SqlDbType.Int, false);
            }
        }

        public class T_News : MSSQL.TableBase
        {
            public MSSQL.Field ID;
            public MSSQL.Field SiteID;
            public MSSQL.Field TypeID;
            public MSSQL.Field DateTime;
            public MSSQL.Field Title;
            public MSSQL.Field ImageUrl;
            public MSSQL.Field isShow;
            public MSSQL.Field isHasImage;
            public MSSQL.Field isCanComments;
            public MSSQL.Field isCommend;
            public MSSQL.Field isHot;
            public MSSQL.Field ReadCount;
            public MSSQL.Field Content;
            public MSSQL.Field IsusesId;

            public T_News()
            {
                TableName = "T_News";

                ID = new MSSQL.Field(this, "ID", "ID", SqlDbType.BigInt, true);
                SiteID = new MSSQL.Field(this, "SiteID", "SiteID", SqlDbType.BigInt, false);
                TypeID = new MSSQL.Field(this, "TypeID", "TypeID", SqlDbType.Int, false);
                DateTime = new MSSQL.Field(this, "DateTime", "DateTime", SqlDbType.DateTime, false);
                Title = new MSSQL.Field(this, "Title", "Title", SqlDbType.VarChar, false);
                ImageUrl = new MSSQL.Field(this, "ImageUrl", "ImageUrl", SqlDbType.VarChar, false);
                isShow = new MSSQL.Field(this, "isShow", "isShow", SqlDbType.Bit, false);
                isHasImage = new MSSQL.Field(this, "isHasImage", "isHasImage", SqlDbType.Bit, false);
                isCanComments = new MSSQL.Field(this, "isCanComments", "isCanComments", SqlDbType.Bit, false);
                isCommend = new MSSQL.Field(this, "isCommend", "isCommend", SqlDbType.Bit, false);
                isHot = new MSSQL.Field(this, "isHot", "isHot", SqlDbType.Bit, false);
                ReadCount = new MSSQL.Field(this, "ReadCount", "ReadCount", SqlDbType.BigInt, false);
                Content = new MSSQL.Field(this, "Content", "Content", SqlDbType.VarChar, false);
                IsusesId = new MSSQL.Field(this, "IsusesId", "IsusesId", SqlDbType.Int, false);
            }
        }

        public class T_NewsComments : MSSQL.TableBase
        {
            public MSSQL.Field ID;
            public MSSQL.Field NewsID;
            public MSSQL.Field DateTime;
            public MSSQL.Field CommentserID;
            public MSSQL.Field CommentserName;
            public MSSQL.Field isShow;
            public MSSQL.Field Content;

            public T_NewsComments()
            {
                TableName = "T_NewsComments";

                ID = new MSSQL.Field(this, "ID", "ID", SqlDbType.BigInt, true);
                NewsID = new MSSQL.Field(this, "NewsID", "NewsID", SqlDbType.BigInt, false);
                DateTime = new MSSQL.Field(this, "DateTime", "DateTime", SqlDbType.DateTime, false);
                CommentserID = new MSSQL.Field(this, "CommentserID", "CommentserID", SqlDbType.BigInt, false);
                CommentserName = new MSSQL.Field(this, "CommentserName", "CommentserName", SqlDbType.VarChar, false);
                isShow = new MSSQL.Field(this, "isShow", "isShow", SqlDbType.Bit, false);
                Content = new MSSQL.Field(this, "Content", "Content", SqlDbType.VarChar, false);
            }
        }

        public class T_NewsPaperIsuses : MSSQL.TableBase
        {
            public MSSQL.Field ID;
            public MSSQL.Field Name;
            public MSSQL.Field StartTime;
            public MSSQL.Field EndTime;
            public MSSQL.Field NPMessage;

            public T_NewsPaperIsuses()
            {
                TableName = "T_NewsPaperIsuses";

                ID = new MSSQL.Field(this, "ID", "ID", SqlDbType.Int, true);
                Name = new MSSQL.Field(this, "Name", "Name", SqlDbType.VarChar, false);
                StartTime = new MSSQL.Field(this, "StartTime", "StartTime", SqlDbType.DateTime, false);
                EndTime = new MSSQL.Field(this, "EndTime", "EndTime", SqlDbType.DateTime, false);
                NPMessage = new MSSQL.Field(this, "NPMessage", "NPMessage", SqlDbType.VarChar, false);
            }
        }

        public class T_NewsPaperTypes : MSSQL.TableBase
        {
            public MSSQL.Field ID;
            public MSSQL.Field SiteID;
            public MSSQL.Field ParentID;
            public MSSQL.Field Name;
            public MSSQL.Field isShow;
            public MSSQL.Field isSystem;

            public T_NewsPaperTypes()
            {
                TableName = "T_NewsPaperTypes";

                ID = new MSSQL.Field(this, "ID", "ID", SqlDbType.Int, true);
                SiteID = new MSSQL.Field(this, "SiteID", "SiteID", SqlDbType.BigInt, false);
                ParentID = new MSSQL.Field(this, "ParentID", "ParentID", SqlDbType.Int, false);
                Name = new MSSQL.Field(this, "Name", "Name", SqlDbType.VarChar, false);
                isShow = new MSSQL.Field(this, "isShow", "isShow", SqlDbType.Bit, false);
                isSystem = new MSSQL.Field(this, "isSystem", "isSystem", SqlDbType.Bit, false);
            }
        }

        public class T_NewsType : MSSQL.TableBase
        {
            public MSSQL.Field ID;
            public MSSQL.Field SiteID;
            public MSSQL.Field ParentID;
            public MSSQL.Field Name;
            public MSSQL.Field isShow;
            public MSSQL.Field isSystem;

            public T_NewsType()
            {
                TableName = "T_NewsType";

                ID = new MSSQL.Field(this, "ID", "ID", SqlDbType.Int, true);
                SiteID = new MSSQL.Field(this, "SiteID", "SiteID", SqlDbType.BigInt, false);
                ParentID = new MSSQL.Field(this, "ParentID", "ParentID", SqlDbType.Int, false);
                Name = new MSSQL.Field(this, "Name", "Name", SqlDbType.VarChar, false);
                isShow = new MSSQL.Field(this, "isShow", "isShow", SqlDbType.Bit, false);
                isSystem = new MSSQL.Field(this, "isSystem", "isSystem", SqlDbType.Bit, false);
            }
        }

        public class T_NewsTypes : MSSQL.TableBase
        {
            public MSSQL.Field ID;
            public MSSQL.Field SiteID;
            public MSSQL.Field ParentID;
            public MSSQL.Field Name;
            public MSSQL.Field IsShow;
            public MSSQL.Field IsSystem;

            public T_NewsTypes()
            {
                TableName = "T_NewsTypes";

                ID = new MSSQL.Field(this, "ID", "ID", SqlDbType.Int, false);
                SiteID = new MSSQL.Field(this, "SiteID", "SiteID", SqlDbType.BigInt, false);
                ParentID = new MSSQL.Field(this, "ParentID", "ParentID", SqlDbType.Int, false);
                Name = new MSSQL.Field(this, "Name", "Name", SqlDbType.VarChar, false);
                IsShow = new MSSQL.Field(this, "IsShow", "IsShow", SqlDbType.Bit, false);
                IsSystem = new MSSQL.Field(this, "IsSystem", "IsSystem", SqlDbType.Bit, false);
            }
        }

        public class T_NotificationTypes : MSSQL.TableBase
        {
            public MSSQL.Field ID;
            public MSSQL.Field Name;
            public MSSQL.Field Code;
            public MSSQL.Field Description;
            public MSSQL.Field TemplateEmail;
            public MSSQL.Field TemplateStationSMS;
            public MSSQL.Field TemplateSMS;

            public T_NotificationTypes()
            {
                TableName = "T_NotificationTypes";

                ID = new MSSQL.Field(this, "ID", "ID", SqlDbType.SmallInt, false);
                Name = new MSSQL.Field(this, "Name", "Name", SqlDbType.VarChar, false);
                Code = new MSSQL.Field(this, "Code", "Code", SqlDbType.VarChar, false);
                Description = new MSSQL.Field(this, "Description", "Description", SqlDbType.VarChar, false);
                TemplateEmail = new MSSQL.Field(this, "TemplateEmail", "TemplateEmail", SqlDbType.VarChar, false);
                TemplateStationSMS = new MSSQL.Field(this, "TemplateStationSMS", "TemplateStationSMS", SqlDbType.VarChar, false);
                TemplateSMS = new MSSQL.Field(this, "TemplateSMS", "TemplateSMS", SqlDbType.VarChar, false);
            }
        }

        public class T_Options : MSSQL.TableBase
        {
            public MSSQL.Field ID;
            public MSSQL.Field Key;
            public MSSQL.Field Value;
            public MSSQL.Field Description;

            public T_Options()
            {
                TableName = "T_Options";

                ID = new MSSQL.Field(this, "ID", "ID", SqlDbType.SmallInt, false);
                Key = new MSSQL.Field(this, "Key", "Key", SqlDbType.VarChar, false);
                Value = new MSSQL.Field(this, "Value", "Value", SqlDbType.VarChar, false);
                Description = new MSSQL.Field(this, "Description", "Description", SqlDbType.VarChar, false);
            }
        }

        public class T_Personages : MSSQL.TableBase
        {
            public MSSQL.Field ID;
            public MSSQL.Field LotteryID;
            public MSSQL.Field UserName;
            public MSSQL.Field DateTime;
            public MSSQL.Field Order;
            public MSSQL.Field IsShow;

            public T_Personages()
            {
                TableName = "T_Personages";

                ID = new MSSQL.Field(this, "ID", "ID", SqlDbType.Int, true);
                LotteryID = new MSSQL.Field(this, "LotteryID", "LotteryID", SqlDbType.Int, false);
                UserName = new MSSQL.Field(this, "UserName", "UserName", SqlDbType.VarChar, false);
                DateTime = new MSSQL.Field(this, "DateTime", "DateTime", SqlDbType.DateTime, false);
                Order = new MSSQL.Field(this, "Order", "Order", SqlDbType.Int, false);
                IsShow = new MSSQL.Field(this, "IsShow", "IsShow", SqlDbType.Bit, false);
            }
        }

        public class T_PiaoPiaoRules : MSSQL.TableBase
        {
            public MSSQL.Field ID;
            public MSSQL.Field TypeName;
            public MSSQL.Field TimePeriod;
            public MSSQL.Field TimeUnit;
            public MSSQL.Field PPCount;
            public MSSQL.Field Scale;

            public T_PiaoPiaoRules()
            {
                TableName = "T_PiaoPiaoRules";

                ID = new MSSQL.Field(this, "ID", "ID", SqlDbType.Int, false);
                TypeName = new MSSQL.Field(this, "TypeName", "TypeName", SqlDbType.NVarChar, false);
                TimePeriod = new MSSQL.Field(this, "TimePeriod", "TimePeriod", SqlDbType.Int, false);
                TimeUnit = new MSSQL.Field(this, "TimeUnit", "TimeUnit", SqlDbType.SmallInt, false);
                PPCount = new MSSQL.Field(this, "PPCount", "PPCount", SqlDbType.Int, false);
                Scale = new MSSQL.Field(this, "Scale", "Scale", SqlDbType.SmallInt, false);
            }
        }

        public class T_PiaoPiaoToCpsUid : MSSQL.TableBase
        {
            public MSSQL.Field ID;
            public MSSQL.Field UID;
            public MSSQL.Field TypeID;
            public MSSQL.Field LastDateTime;
            public MSSQL.Field TotalPPCount;

            public T_PiaoPiaoToCpsUid()
            {
                TableName = "T_PiaoPiaoToCpsUid";

                ID = new MSSQL.Field(this, "ID", "ID", SqlDbType.BigInt, true);
                UID = new MSSQL.Field(this, "UID", "UID", SqlDbType.BigInt, false);
                TypeID = new MSSQL.Field(this, "TypeID", "TypeID", SqlDbType.Int, false);
                LastDateTime = new MSSQL.Field(this, "LastDateTime", "LastDateTime", SqlDbType.SmallDateTime, false);
                TotalPPCount = new MSSQL.Field(this, "TotalPPCount", "TotalPPCount", SqlDbType.Int, false);
            }
        }

        public class T_PlayTypes : MSSQL.TableBase
        {
            public MSSQL.Field ID;
            public MSSQL.Field LotteryID;
            public MSSQL.Field Name;
            public MSSQL.Field SystemEndAheadMinute;
            public MSSQL.Field Price;
            public MSSQL.Field BuyFileName;
            public MSSQL.Field MaxFollowSchemeNumberOf;
            public MSSQL.Field MaxMultiple;

            public T_PlayTypes()
            {
                TableName = "T_PlayTypes";

                ID = new MSSQL.Field(this, "ID", "ID", SqlDbType.Int, false);
                LotteryID = new MSSQL.Field(this, "LotteryID", "LotteryID", SqlDbType.Int, false);
                Name = new MSSQL.Field(this, "Name", "Name", SqlDbType.VarChar, false);
                SystemEndAheadMinute = new MSSQL.Field(this, "SystemEndAheadMinute", "SystemEndAheadMinute", SqlDbType.Int, false);
                Price = new MSSQL.Field(this, "Price", "Price", SqlDbType.Money, false);
                BuyFileName = new MSSQL.Field(this, "BuyFileName", "BuyFileName", SqlDbType.VarChar, false);
                MaxFollowSchemeNumberOf = new MSSQL.Field(this, "MaxFollowSchemeNumberOf", "MaxFollowSchemeNumberOf", SqlDbType.Int, false);
                MaxMultiple = new MSSQL.Field(this, "MaxMultiple", "MaxMultiple", SqlDbType.Int, false);
            }
        }

        public class T_PoliciesAndRegulations : MSSQL.TableBase
        {
            public MSSQL.Field ID;
            public MSSQL.Field DateTime;
            public MSSQL.Field Title;
            public MSSQL.Field isShow;
            public MSSQL.Field Content;

            public T_PoliciesAndRegulations()
            {
                TableName = "T_PoliciesAndRegulations";

                ID = new MSSQL.Field(this, "ID", "ID", SqlDbType.BigInt, true);
                DateTime = new MSSQL.Field(this, "DateTime", "DateTime", SqlDbType.DateTime, false);
                Title = new MSSQL.Field(this, "Title", "Title", SqlDbType.VarChar, false);
                isShow = new MSSQL.Field(this, "isShow", "isShow", SqlDbType.Bit, false);
                Content = new MSSQL.Field(this, "Content", "Content", SqlDbType.VarChar, false);
            }
        }

        public class T_Provinces : MSSQL.TableBase
        {
            public MSSQL.Field ID;
            public MSSQL.Field Name;

            public T_Provinces()
            {
                TableName = "T_Provinces";

                ID = new MSSQL.Field(this, "ID", "ID", SqlDbType.Int, false);
                Name = new MSSQL.Field(this, "Name", "Name", SqlDbType.VarChar, false);
            }
        }

        public class T_Questionnaire : MSSQL.TableBase
        {
            public MSSQL.Field ID;
            public MSSQL.Field Title;
            public MSSQL.Field Options1Content;
            public MSSQL.Field Options2Content;
            public MSSQL.Field Options3Content;
            public MSSQL.Field Options4Content;
            public MSSQL.Field Options1Count;
            public MSSQL.Field Options2Count;
            public MSSQL.Field Options3Count;
            public MSSQL.Field Options4Count;

            public T_Questionnaire()
            {
                TableName = "T_Questionnaire";

                ID = new MSSQL.Field(this, "ID", "ID", SqlDbType.Int, true);
                Title = new MSSQL.Field(this, "Title", "Title", SqlDbType.VarChar, false);
                Options1Content = new MSSQL.Field(this, "Options1Content", "Options1Content", SqlDbType.VarChar, false);
                Options2Content = new MSSQL.Field(this, "Options2Content", "Options2Content", SqlDbType.VarChar, false);
                Options3Content = new MSSQL.Field(this, "Options3Content", "Options3Content", SqlDbType.VarChar, false);
                Options4Content = new MSSQL.Field(this, "Options4Content", "Options4Content", SqlDbType.VarChar, false);
                Options1Count = new MSSQL.Field(this, "Options1Count", "Options1Count", SqlDbType.Int, false);
                Options2Count = new MSSQL.Field(this, "Options2Count", "Options2Count", SqlDbType.Int, false);
                Options3Count = new MSSQL.Field(this, "Options3Count", "Options3Count", SqlDbType.Int, false);
                Options4Count = new MSSQL.Field(this, "Options4Count", "Options4Count", SqlDbType.Int, false);
            }
        }

        public class T_QuestionnaireSurveyAnswer : MSSQL.TableBase
        {
            public MSSQL.Field ID;
            public MSSQL.Field Name;
            public MSSQL.Field QuestionID;
            public MSSQL.Field SelectCount;
            public MSSQL.Field IsSystem;

            public T_QuestionnaireSurveyAnswer()
            {
                TableName = "T_QuestionnaireSurveyAnswer";

                ID = new MSSQL.Field(this, "ID", "ID", SqlDbType.Int, true);
                Name = new MSSQL.Field(this, "Name", "Name", SqlDbType.VarChar, false);
                QuestionID = new MSSQL.Field(this, "QuestionID", "QuestionID", SqlDbType.Int, false);
                SelectCount = new MSSQL.Field(this, "SelectCount", "SelectCount", SqlDbType.Int, false);
                IsSystem = new MSSQL.Field(this, "IsSystem", "IsSystem", SqlDbType.Bit, false);
            }
        }

        public class T_QuestionnaireSurveyQuestions : MSSQL.TableBase
        {
            public MSSQL.Field ID;
            public MSSQL.Field Name;
            public MSSQL.Field IsCanSelectMuch;
            public MSSQL.Field IsCustomAnswer;

            public T_QuestionnaireSurveyQuestions()
            {
                TableName = "T_QuestionnaireSurveyQuestions";

                ID = new MSSQL.Field(this, "ID", "ID", SqlDbType.Int, true);
                Name = new MSSQL.Field(this, "Name", "Name", SqlDbType.VarChar, false);
                IsCanSelectMuch = new MSSQL.Field(this, "IsCanSelectMuch", "IsCanSelectMuch", SqlDbType.Bit, false);
                IsCustomAnswer = new MSSQL.Field(this, "IsCustomAnswer", "IsCustomAnswer", SqlDbType.Bit, false);
            }
        }

        public class T_QuestionnaireSurveySuggestions : MSSQL.TableBase
        {
            public MSSQL.Field ID;
            public MSSQL.Field Suggestions;

            public T_QuestionnaireSurveySuggestions()
            {
                TableName = "T_QuestionnaireSurveySuggestions";

                ID = new MSSQL.Field(this, "ID", "ID", SqlDbType.Int, true);
                Suggestions = new MSSQL.Field(this, "Suggestions", "Suggestions", SqlDbType.VarChar, false);
            }
        }

        public class T_Questions : MSSQL.TableBase
        {
            public MSSQL.Field ID;
            public MSSQL.Field SiteID;
            public MSSQL.Field UserID;
            public MSSQL.Field DateTime;
            public MSSQL.Field TypeID;
            public MSSQL.Field Telephone;
            public MSSQL.Field AnswerStatus;
            public MSSQL.Field HandleDateTime;
            public MSSQL.Field HandleOperatorID;
            public MSSQL.Field AnswerOperatorID;
            public MSSQL.Field AnswerDateTime;
            public MSSQL.Field Content;
            public MSSQL.Field Answer;

            public T_Questions()
            {
                TableName = "T_Questions";

                ID = new MSSQL.Field(this, "ID", "ID", SqlDbType.BigInt, true);
                SiteID = new MSSQL.Field(this, "SiteID", "SiteID", SqlDbType.BigInt, false);
                UserID = new MSSQL.Field(this, "UserID", "UserID", SqlDbType.BigInt, false);
                DateTime = new MSSQL.Field(this, "DateTime", "DateTime", SqlDbType.DateTime, false);
                TypeID = new MSSQL.Field(this, "TypeID", "TypeID", SqlDbType.SmallInt, false);
                Telephone = new MSSQL.Field(this, "Telephone", "Telephone", SqlDbType.VarChar, false);
                AnswerStatus = new MSSQL.Field(this, "AnswerStatus", "AnswerStatus", SqlDbType.SmallInt, false);
                HandleDateTime = new MSSQL.Field(this, "HandleDateTime", "HandleDateTime", SqlDbType.DateTime, false);
                HandleOperatorID = new MSSQL.Field(this, "HandleOperatorID", "HandleOperatorID", SqlDbType.BigInt, false);
                AnswerOperatorID = new MSSQL.Field(this, "AnswerOperatorID", "AnswerOperatorID", SqlDbType.BigInt, false);
                AnswerDateTime = new MSSQL.Field(this, "AnswerDateTime", "AnswerDateTime", SqlDbType.DateTime, false);
                Content = new MSSQL.Field(this, "Content", "Content", SqlDbType.VarChar, false);
                Answer = new MSSQL.Field(this, "Answer", "Answer", SqlDbType.VarChar, false);
            }
        }

        public class T_QuestionTypes : MSSQL.TableBase
        {
            public MSSQL.Field ID;
            public MSSQL.Field Name;
            public MSSQL.Field Description;
            public MSSQL.Field UseType;

            public T_QuestionTypes()
            {
                TableName = "T_QuestionTypes";

                ID = new MSSQL.Field(this, "ID", "ID", SqlDbType.SmallInt, false);
                Name = new MSSQL.Field(this, "Name", "Name", SqlDbType.VarChar, false);
                Description = new MSSQL.Field(this, "Description", "Description", SqlDbType.VarChar, false);
                UseType = new MSSQL.Field(this, "UseType", "UseType", SqlDbType.SmallInt, false);
            }
        }

        public class T_RecallingAllBuyStar : MSSQL.TableBase
        {
            public MSSQL.Field ID;
            public MSSQL.Field SchemesID;
            public MSSQL.Field InitiatorName;
            public MSSQL.Field SchemesMoney;
            public MSSQL.Field SchemesWinMoney;
            public MSSQL.Field ProfitIndex;
            public MSSQL.Field State;

            public T_RecallingAllBuyStar()
            {
                TableName = "T_RecallingAllBuyStar";

                ID = new MSSQL.Field(this, "ID", "ID", SqlDbType.Int, true);
                SchemesID = new MSSQL.Field(this, "SchemesID", "SchemesID", SqlDbType.Int, false);
                InitiatorName = new MSSQL.Field(this, "InitiatorName", "InitiatorName", SqlDbType.VarChar, false);
                SchemesMoney = new MSSQL.Field(this, "SchemesMoney", "SchemesMoney", SqlDbType.VarChar, false);
                SchemesWinMoney = new MSSQL.Field(this, "SchemesWinMoney", "SchemesWinMoney", SqlDbType.VarChar, false);
                ProfitIndex = new MSSQL.Field(this, "ProfitIndex", "ProfitIndex", SqlDbType.VarChar, false);
                State = new MSSQL.Field(this, "State", "State", SqlDbType.Int, false);
            }
        }

        public class T_SchemeChatContents : MSSQL.TableBase
        {
            public MSSQL.Field ID;
            public MSSQL.Field SiteID;
            public MSSQL.Field SchemeID;
            public MSSQL.Field DateTime;
            public MSSQL.Field FromUserID;
            public MSSQL.Field ToUserID;
            public MSSQL.Field Type;
            public MSSQL.Field Content;

            public T_SchemeChatContents()
            {
                TableName = "T_SchemeChatContents";

                ID = new MSSQL.Field(this, "ID", "ID", SqlDbType.BigInt, true);
                SiteID = new MSSQL.Field(this, "SiteID", "SiteID", SqlDbType.BigInt, false);
                SchemeID = new MSSQL.Field(this, "SchemeID", "SchemeID", SqlDbType.BigInt, false);
                DateTime = new MSSQL.Field(this, "DateTime", "DateTime", SqlDbType.DateTime, false);
                FromUserID = new MSSQL.Field(this, "FromUserID", "FromUserID", SqlDbType.BigInt, false);
                ToUserID = new MSSQL.Field(this, "ToUserID", "ToUserID", SqlDbType.BigInt, false);
                Type = new MSSQL.Field(this, "Type", "Type", SqlDbType.SmallInt, false);
                Content = new MSSQL.Field(this, "Content", "Content", SqlDbType.VarChar, false);
            }
        }

        public class T_SchemeChatContentsReaded : MSSQL.TableBase
        {
            public MSSQL.Field ContentID;
            public MSSQL.Field UserID;

            public T_SchemeChatContentsReaded()
            {
                TableName = "T_SchemeChatContentsReaded";

                ContentID = new MSSQL.Field(this, "ContentID", "ContentID", SqlDbType.BigInt, false);
                UserID = new MSSQL.Field(this, "UserID", "UserID", SqlDbType.BigInt, false);
            }
        }

        public class T_SchemeElectronTickets : MSSQL.TableBase
        {
            public MSSQL.Field ID;
            public MSSQL.Field DateTime;
            public MSSQL.Field SchemeID;
            public MSSQL.Field PlayTypeID;
            public MSSQL.Field Money;
            public MSSQL.Field Multiple;
            public MSSQL.Field Sends;
            public MSSQL.Field HandleDateTime;
            public MSSQL.Field HandleResult;
            public MSSQL.Field HandleDescription;
            public MSSQL.Field Identifiers;
            public MSSQL.Field Ticket;

            public T_SchemeElectronTickets()
            {
                TableName = "T_SchemeElectronTickets";

                ID = new MSSQL.Field(this, "ID", "ID", SqlDbType.BigInt, false);
                DateTime = new MSSQL.Field(this, "DateTime", "DateTime", SqlDbType.DateTime, false);
                SchemeID = new MSSQL.Field(this, "SchemeID", "SchemeID", SqlDbType.BigInt, false);
                PlayTypeID = new MSSQL.Field(this, "PlayTypeID", "PlayTypeID", SqlDbType.Int, false);
                Money = new MSSQL.Field(this, "Money", "Money", SqlDbType.Money, false);
                Multiple = new MSSQL.Field(this, "Multiple", "Multiple", SqlDbType.Int, false);
                Sends = new MSSQL.Field(this, "Sends", "Sends", SqlDbType.SmallInt, false);
                HandleDateTime = new MSSQL.Field(this, "HandleDateTime", "HandleDateTime", SqlDbType.DateTime, false);
                HandleResult = new MSSQL.Field(this, "HandleResult", "HandleResult", SqlDbType.SmallInt, false);
                HandleDescription = new MSSQL.Field(this, "HandleDescription", "HandleDescription", SqlDbType.VarChar, false);
                Identifiers = new MSSQL.Field(this, "Identifiers", "Identifiers", SqlDbType.VarChar, false);
                Ticket = new MSSQL.Field(this, "Ticket", "Ticket", SqlDbType.VarChar, false);
            }
        }

        public class T_SchemeIsCalcuteBonus : MSSQL.TableBase
        {
            public MSSQL.Field FromSystem;
            public MSSQL.Field SchemeID;

            public T_SchemeIsCalcuteBonus()
            {
                TableName = "T_SchemeIsCalcuteBonus";

                FromSystem = new MSSQL.Field(this, "FromSystem", "FromSystem", SqlDbType.Int, false);
                SchemeID = new MSSQL.Field(this, "SchemeID", "SchemeID", SqlDbType.BigInt, false);
            }
        }

        public class T_SchemeIsCalcuteScore : MSSQL.TableBase
        {
            public MSSQL.Field SchemeID;
            public MSSQL.Field ScoreType;

            public T_SchemeIsCalcuteScore()
            {
                TableName = "T_SchemeIsCalcuteScore";

                SchemeID = new MSSQL.Field(this, "SchemeID", "SchemeID", SqlDbType.BigInt, false);
                ScoreType = new MSSQL.Field(this, "ScoreType", "ScoreType", SqlDbType.Int, false);
            }
        }

        public class T_SchemeOpenUsers : MSSQL.TableBase
        {
            public MSSQL.Field SchemeID;
            public MSSQL.Field UserID;

            public T_SchemeOpenUsers()
            {
                TableName = "T_SchemeOpenUsers";

                SchemeID = new MSSQL.Field(this, "SchemeID", "SchemeID", SqlDbType.BigInt, false);
                UserID = new MSSQL.Field(this, "UserID", "UserID", SqlDbType.BigInt, false);
            }
        }

        public class T_Schemes : MSSQL.TableBase
        {
            public MSSQL.Field ID;
            public MSSQL.Field SiteID;
            public MSSQL.Field DateTime;
            public MSSQL.Field SchemeNumber;
            public MSSQL.Field Title;
            public MSSQL.Field InitiateUserID;
            public MSSQL.Field IsuseID;
            public MSSQL.Field PlayTypeID;
            public MSSQL.Field Multiple;
            public MSSQL.Field Money;
            public MSSQL.Field AssureMoney;
            public MSSQL.Field Share;
            public MSSQL.Field SecrecyLevel;
            public MSSQL.Field QuashStatus;
            public MSSQL.Field Buyed;
            public MSSQL.Field BuyOperatorID;
            public MSSQL.Field PrintOutType;
            public MSSQL.Field Identifiers;
            public MSSQL.Field isOpened;
            public MSSQL.Field OpenOperatorID;
            public MSSQL.Field WinMoney;
            public MSSQL.Field WinMoneyNoWithTax;
            public MSSQL.Field InitiateBonus;
            public MSSQL.Field AtTopStatus;
            public MSSQL.Field isCanChat;
            public MSSQL.Field PreWinMoney;
            public MSSQL.Field PreWinMoneyNoWithTax;
            public MSSQL.Field EditWinMoney;
            public MSSQL.Field EditWinMoneyNoWithTax;
            public MSSQL.Field BuyedShare;
            public MSSQL.Field Schedule;
            public MSSQL.Field ReSchedule;
            public MSSQL.Field IsSchemeCalculatedBonus;
            public MSSQL.Field Description;
            public MSSQL.Field LotteryNumber;
            public MSSQL.Field UploadFileContent;
            public MSSQL.Field WinDescription;
            public MSSQL.Field WinImage;
            public MSSQL.Field UpdateDatetime;
            public MSSQL.Field PrintOutDateTime;
            public MSSQL.Field Ot;
            public MSSQL.Field OutTo;
            public MSSQL.Field CorrelationSchemeID;
            public MSSQL.Field SchemeBonusScale;

            public T_Schemes()
            {
                TableName = "T_Schemes";

                ID = new MSSQL.Field(this, "ID", "ID", SqlDbType.BigInt, true);
                SiteID = new MSSQL.Field(this, "SiteID", "SiteID", SqlDbType.BigInt, false);
                DateTime = new MSSQL.Field(this, "DateTime", "DateTime", SqlDbType.DateTime, false);
                SchemeNumber = new MSSQL.Field(this, "SchemeNumber", "SchemeNumber", SqlDbType.VarChar, false);
                Title = new MSSQL.Field(this, "Title", "Title", SqlDbType.VarChar, false);
                InitiateUserID = new MSSQL.Field(this, "InitiateUserID", "InitiateUserID", SqlDbType.BigInt, false);
                IsuseID = new MSSQL.Field(this, "IsuseID", "IsuseID", SqlDbType.BigInt, false);
                PlayTypeID = new MSSQL.Field(this, "PlayTypeID", "PlayTypeID", SqlDbType.Int, false);
                Multiple = new MSSQL.Field(this, "Multiple", "Multiple", SqlDbType.Int, false);
                Money = new MSSQL.Field(this, "Money", "Money", SqlDbType.Money, false);
                AssureMoney = new MSSQL.Field(this, "AssureMoney", "AssureMoney", SqlDbType.Money, false);
                Share = new MSSQL.Field(this, "Share", "Share", SqlDbType.Int, false);
                SecrecyLevel = new MSSQL.Field(this, "SecrecyLevel", "SecrecyLevel", SqlDbType.SmallInt, false);
                QuashStatus = new MSSQL.Field(this, "QuashStatus", "QuashStatus", SqlDbType.SmallInt, false);
                Buyed = new MSSQL.Field(this, "Buyed", "Buyed", SqlDbType.Bit, false);
                BuyOperatorID = new MSSQL.Field(this, "BuyOperatorID", "BuyOperatorID", SqlDbType.BigInt, false);
                PrintOutType = new MSSQL.Field(this, "PrintOutType", "PrintOutType", SqlDbType.SmallInt, false);
                Identifiers = new MSSQL.Field(this, "Identifiers", "Identifiers", SqlDbType.VarChar, false);
                isOpened = new MSSQL.Field(this, "isOpened", "isOpened", SqlDbType.Bit, false);
                OpenOperatorID = new MSSQL.Field(this, "OpenOperatorID", "OpenOperatorID", SqlDbType.BigInt, false);
                WinMoney = new MSSQL.Field(this, "WinMoney", "WinMoney", SqlDbType.Money, false);
                WinMoneyNoWithTax = new MSSQL.Field(this, "WinMoneyNoWithTax", "WinMoneyNoWithTax", SqlDbType.Money, false);
                InitiateBonus = new MSSQL.Field(this, "InitiateBonus", "InitiateBonus", SqlDbType.Money, false);
                AtTopStatus = new MSSQL.Field(this, "AtTopStatus", "AtTopStatus", SqlDbType.SmallInt, false);
                isCanChat = new MSSQL.Field(this, "isCanChat", "isCanChat", SqlDbType.Bit, false);
                PreWinMoney = new MSSQL.Field(this, "PreWinMoney", "PreWinMoney", SqlDbType.Money, false);
                PreWinMoneyNoWithTax = new MSSQL.Field(this, "PreWinMoneyNoWithTax", "PreWinMoneyNoWithTax", SqlDbType.Money, false);
                EditWinMoney = new MSSQL.Field(this, "EditWinMoney", "EditWinMoney", SqlDbType.Money, false);
                EditWinMoneyNoWithTax = new MSSQL.Field(this, "EditWinMoneyNoWithTax", "EditWinMoneyNoWithTax", SqlDbType.Money, false);
                BuyedShare = new MSSQL.Field(this, "BuyedShare", "BuyedShare", SqlDbType.Int, false);
                Schedule = new MSSQL.Field(this, "Schedule", "Schedule", SqlDbType.Float, false);
                ReSchedule = new MSSQL.Field(this, "ReSchedule", "ReSchedule", SqlDbType.Float, false);
                IsSchemeCalculatedBonus = new MSSQL.Field(this, "IsSchemeCalculatedBonus", "IsSchemeCalculatedBonus", SqlDbType.Bit, false);
                Description = new MSSQL.Field(this, "Description", "Description", SqlDbType.VarChar, false);
                LotteryNumber = new MSSQL.Field(this, "LotteryNumber", "LotteryNumber", SqlDbType.VarChar, false);
                UploadFileContent = new MSSQL.Field(this, "UploadFileContent", "UploadFileContent", SqlDbType.VarChar, false);
                WinDescription = new MSSQL.Field(this, "WinDescription", "WinDescription", SqlDbType.VarChar, false);
                WinImage = new MSSQL.Field(this, "WinImage", "WinImage", SqlDbType.VarChar, false);
                UpdateDatetime = new MSSQL.Field(this, "UpdateDatetime", "UpdateDatetime", SqlDbType.DateTime, false);
                PrintOutDateTime = new MSSQL.Field(this, "PrintOutDateTime", "PrintOutDateTime", SqlDbType.DateTime, false);
                Ot = new MSSQL.Field(this, "Ot", "Ot", SqlDbType.SmallInt, false);
                OutTo = new MSSQL.Field(this, "OutTo", "OutTo", SqlDbType.SmallInt, false);
                CorrelationSchemeID = new MSSQL.Field(this, "CorrelationSchemeID", "CorrelationSchemeID", SqlDbType.BigInt, false);
                SchemeBonusScale = new MSSQL.Field(this, "SchemeBonusScale", "SchemeBonusScale", SqlDbType.Float, false);
            }
        }

        public class T_SchemesNumber : MSSQL.TableBase
        {
            public MSSQL.Field ID;
            public MSSQL.Field DateTime;
            public MSSQL.Field SchemeID;
            public MSSQL.Field Money;
            public MSSQL.Field Multiple;
            public MSSQL.Field LotteryNumber;

            public T_SchemesNumber()
            {
                TableName = "T_SchemesNumber";

                ID = new MSSQL.Field(this, "ID", "ID", SqlDbType.BigInt, true);
                DateTime = new MSSQL.Field(this, "DateTime", "DateTime", SqlDbType.DateTime, false);
                SchemeID = new MSSQL.Field(this, "SchemeID", "SchemeID", SqlDbType.BigInt, false);
                Money = new MSSQL.Field(this, "Money", "Money", SqlDbType.Money, false);
                Multiple = new MSSQL.Field(this, "Multiple", "Multiple", SqlDbType.Int, false);
                LotteryNumber = new MSSQL.Field(this, "LotteryNumber", "LotteryNumber", SqlDbType.VarChar, false);
            }
        }

        public class T_SchemesSendToCenter : MSSQL.TableBase
        {
            public MSSQL.Field ID;
            public MSSQL.Field DateTime;
            public MSSQL.Field SchemeID;
            public MSSQL.Field PlayTypeID;
            public MSSQL.Field Money;
            public MSSQL.Field Multiple;
            public MSSQL.Field Sends;
            public MSSQL.Field HandleDateTime;
            public MSSQL.Field HandleResult;
            public MSSQL.Field HandleDescription;
            public MSSQL.Field Identifiers;
            public MSSQL.Field Ticket;

            public T_SchemesSendToCenter()
            {
                TableName = "T_SchemesSendToCenter";

                ID = new MSSQL.Field(this, "ID", "ID", SqlDbType.BigInt, true);
                DateTime = new MSSQL.Field(this, "DateTime", "DateTime", SqlDbType.DateTime, false);
                SchemeID = new MSSQL.Field(this, "SchemeID", "SchemeID", SqlDbType.BigInt, false);
                PlayTypeID = new MSSQL.Field(this, "PlayTypeID", "PlayTypeID", SqlDbType.Int, false);
                Money = new MSSQL.Field(this, "Money", "Money", SqlDbType.Money, false);
                Multiple = new MSSQL.Field(this, "Multiple", "Multiple", SqlDbType.Int, false);
                Sends = new MSSQL.Field(this, "Sends", "Sends", SqlDbType.SmallInt, false);
                HandleDateTime = new MSSQL.Field(this, "HandleDateTime", "HandleDateTime", SqlDbType.DateTime, false);
                HandleResult = new MSSQL.Field(this, "HandleResult", "HandleResult", SqlDbType.SmallInt, false);
                HandleDescription = new MSSQL.Field(this, "HandleDescription", "HandleDescription", SqlDbType.VarChar, false);
                Identifiers = new MSSQL.Field(this, "Identifiers", "Identifiers", SqlDbType.VarChar, false);
                Ticket = new MSSQL.Field(this, "Ticket", "Ticket", SqlDbType.VarChar, false);
            }
        }

        public class T_SchemeSupperCobuy : MSSQL.TableBase
        {
            public MSSQL.Field ID;
            public MSSQL.Field SchemeID;
            public MSSQL.Field TypeState;

            public T_SchemeSupperCobuy()
            {
                TableName = "T_SchemeSupperCobuy";

                ID = new MSSQL.Field(this, "ID", "ID", SqlDbType.Int, true);
                SchemeID = new MSSQL.Field(this, "SchemeID", "SchemeID", SqlDbType.BigInt, false);
                TypeState = new MSSQL.Field(this, "TypeState", "TypeState", SqlDbType.Int, false);
            }
        }

        public class T_SchemeToTicketed : MSSQL.TableBase
        {
            public MSSQL.Field OurOrAgent;
            public MSSQL.Field SchemeID;

            public T_SchemeToTicketed()
            {
                TableName = "T_SchemeToTicketed";

                OurOrAgent = new MSSQL.Field(this, "OurOrAgent", "OurOrAgent", SqlDbType.SmallInt, false);
                SchemeID = new MSSQL.Field(this, "SchemeID", "SchemeID", SqlDbType.BigInt, false);
            }
        }

        public class T_SchemeUpload : MSSQL.TableBase
        {
            public MSSQL.Field LotteryID;
            public MSSQL.Field SchemeContent;

            public T_SchemeUpload()
            {
                TableName = "T_SchemeUpload";

                LotteryID = new MSSQL.Field(this, "LotteryID", "LotteryID", SqlDbType.BigInt, false);
                SchemeContent = new MSSQL.Field(this, "SchemeContent", "SchemeContent", SqlDbType.VarChar, false);
            }
        }

        public class T_ScoreChange : MSSQL.TableBase
        {
            public MSSQL.Field ID;
            public MSSQL.Field UserID;
            public MSSQL.Field ComodityID;
            public MSSQL.Field Type;
            public MSSQL.Field DateTime;
            public MSSQL.Field Score;
            public MSSQL.Field IsWin;

            public T_ScoreChange()
            {
                TableName = "T_ScoreChange";

                ID = new MSSQL.Field(this, "ID", "ID", SqlDbType.BigInt, true);
                UserID = new MSSQL.Field(this, "UserID", "UserID", SqlDbType.BigInt, false);
                ComodityID = new MSSQL.Field(this, "ComodityID", "ComodityID", SqlDbType.BigInt, false);
                Type = new MSSQL.Field(this, "Type", "Type", SqlDbType.SmallInt, false);
                DateTime = new MSSQL.Field(this, "DateTime", "DateTime", SqlDbType.DateTime, false);
                Score = new MSSQL.Field(this, "Score", "Score", SqlDbType.Int, false);
                IsWin = new MSSQL.Field(this, "IsWin", "IsWin", SqlDbType.Bit, false);
            }
        }

        public class T_ScoreChangeAddress : MSSQL.TableBase
        {
            public MSSQL.Field UserID;
            public MSSQL.Field Name;
            public MSSQL.Field Address;
            public MSSQL.Field PostCode;
            public MSSQL.Field Phone;
            public MSSQL.Field Mobile;
            public MSSQL.Field Memo;

            public T_ScoreChangeAddress()
            {
                TableName = "T_ScoreChangeAddress";

                UserID = new MSSQL.Field(this, "UserID", "UserID", SqlDbType.BigInt, false);
                Name = new MSSQL.Field(this, "Name", "Name", SqlDbType.VarChar, false);
                Address = new MSSQL.Field(this, "Address", "Address", SqlDbType.VarChar, false);
                PostCode = new MSSQL.Field(this, "PostCode", "PostCode", SqlDbType.VarChar, false);
                Phone = new MSSQL.Field(this, "Phone", "Phone", SqlDbType.VarChar, false);
                Mobile = new MSSQL.Field(this, "Mobile", "Mobile", SqlDbType.VarChar, false);
                Memo = new MSSQL.Field(this, "Memo", "Memo", SqlDbType.VarChar, false);
            }
        }

        public class T_ScoreCommodities : MSSQL.TableBase
        {
            public MSSQL.Field ID;
            public MSSQL.Field TypeID;
            public MSSQL.Field Name;
            public MSSQL.Field Qty;
            public MSSQL.Field ChangedScore;
            public MSSQL.Field DrawedScore;
            public MSSQL.Field Images;
            public MSSQL.Field Introduce;
            public MSSQL.Field IsCanChange;
            public MSSQL.Field IsCanDraw;
            public MSSQL.Field IsCommend;

            public T_ScoreCommodities()
            {
                TableName = "T_ScoreCommodities";

                ID = new MSSQL.Field(this, "ID", "ID", SqlDbType.BigInt, true);
                TypeID = new MSSQL.Field(this, "TypeID", "TypeID", SqlDbType.Int, false);
                Name = new MSSQL.Field(this, "Name", "Name", SqlDbType.VarChar, false);
                Qty = new MSSQL.Field(this, "Qty", "Qty", SqlDbType.Int, false);
                ChangedScore = new MSSQL.Field(this, "ChangedScore", "ChangedScore", SqlDbType.Int, false);
                DrawedScore = new MSSQL.Field(this, "DrawedScore", "DrawedScore", SqlDbType.Int, false);
                Images = new MSSQL.Field(this, "Images", "Images", SqlDbType.VarChar, false);
                Introduce = new MSSQL.Field(this, "Introduce", "Introduce", SqlDbType.VarChar, false);
                IsCanChange = new MSSQL.Field(this, "IsCanChange", "IsCanChange", SqlDbType.Bit, false);
                IsCanDraw = new MSSQL.Field(this, "IsCanDraw", "IsCanDraw", SqlDbType.Bit, false);
                IsCommend = new MSSQL.Field(this, "IsCommend", "IsCommend", SqlDbType.Bit, false);
            }
        }

        public class T_ScoreCommodityType : MSSQL.TableBase
        {
            public MSSQL.Field ID;
            public MSSQL.Field ParentID;
            public MSSQL.Field Name;

            public T_ScoreCommodityType()
            {
                TableName = "T_ScoreCommodityType";

                ID = new MSSQL.Field(this, "ID", "ID", SqlDbType.Int, true);
                ParentID = new MSSQL.Field(this, "ParentID", "ParentID", SqlDbType.Int, false);
                Name = new MSSQL.Field(this, "Name", "Name", SqlDbType.VarChar, false);
            }
        }

        public class T_ScoreGoldType : MSSQL.TableBase
        {
            public MSSQL.Field ID;
            public MSSQL.Field IsScore;
            public MSSQL.Field TypeId;
            public MSSQL.Field TypeName;

            public T_ScoreGoldType()
            {
                TableName = "T_ScoreGoldType";

                ID = new MSSQL.Field(this, "ID", "ID", SqlDbType.Int, true);
                IsScore = new MSSQL.Field(this, "IsScore", "IsScore", SqlDbType.Bit, false);
                TypeId = new MSSQL.Field(this, "TypeId", "TypeId", SqlDbType.Int, false);
                TypeName = new MSSQL.Field(this, "TypeName", "TypeName", SqlDbType.NVarChar, false);
            }
        }

        public class T_ScorePresentIn : MSSQL.TableBase
        {
            public MSSQL.Field ID;
            public MSSQL.Field PresentID;
            public MSSQL.Field Qty;
            public MSSQL.Field CreateTime;
            public MSSQL.Field OperatorID;

            public T_ScorePresentIn()
            {
                TableName = "T_ScorePresentIn";

                ID = new MSSQL.Field(this, "ID", "ID", SqlDbType.BigInt, true);
                PresentID = new MSSQL.Field(this, "PresentID", "PresentID", SqlDbType.BigInt, false);
                Qty = new MSSQL.Field(this, "Qty", "Qty", SqlDbType.Int, false);
                CreateTime = new MSSQL.Field(this, "CreateTime", "CreateTime", SqlDbType.SmallDateTime, false);
                OperatorID = new MSSQL.Field(this, "OperatorID", "OperatorID", SqlDbType.BigInt, false);
            }
        }

        public class T_ScorePresentInventory : MSSQL.TableBase
        {
            public MSSQL.Field ID;
            public MSSQL.Field TypeID;
            public MSSQL.Field Qty;
            public MSSQL.Field Price;
            public MSSQL.Field ShopID;
            public MSSQL.Field PresentName;
            public MSSQL.Field ProductImage;
            public MSSQL.Field ProductDetail;
            public MSSQL.Field PhotoDir;

            public T_ScorePresentInventory()
            {
                TableName = "T_ScorePresentInventory";

                ID = new MSSQL.Field(this, "ID", "ID", SqlDbType.BigInt, true);
                TypeID = new MSSQL.Field(this, "TypeID", "TypeID", SqlDbType.BigInt, false);
                Qty = new MSSQL.Field(this, "Qty", "Qty", SqlDbType.Int, false);
                Price = new MSSQL.Field(this, "Price", "Price", SqlDbType.Money, false);
                ShopID = new MSSQL.Field(this, "ShopID", "ShopID", SqlDbType.BigInt, false);
                PresentName = new MSSQL.Field(this, "PresentName", "PresentName", SqlDbType.NVarChar, false);
                ProductImage = new MSSQL.Field(this, "ProductImage", "ProductImage", SqlDbType.Image, false);
                ProductDetail = new MSSQL.Field(this, "ProductDetail", "ProductDetail", SqlDbType.NText, false);
                PhotoDir = new MSSQL.Field(this, "PhotoDir", "PhotoDir", SqlDbType.VarChar, false);
            }
        }

        public class T_ScorePresentOut : MSSQL.TableBase
        {
            public MSSQL.Field ID;
            public MSSQL.Field UserID;
            public MSSQL.Field UserName;
            public MSSQL.Field Qty;
            public MSSQL.Field ChangeID;
            public MSSQL.Field CreateDate;
            public MSSQL.Field Status;

            public T_ScorePresentOut()
            {
                TableName = "T_ScorePresentOut";

                ID = new MSSQL.Field(this, "ID", "ID", SqlDbType.BigInt, true);
                UserID = new MSSQL.Field(this, "UserID", "UserID", SqlDbType.BigInt, false);
                UserName = new MSSQL.Field(this, "UserName", "UserName", SqlDbType.NVarChar, false);
                Qty = new MSSQL.Field(this, "Qty", "Qty", SqlDbType.Int, false);
                ChangeID = new MSSQL.Field(this, "ChangeID", "ChangeID", SqlDbType.BigInt, false);
                CreateDate = new MSSQL.Field(this, "CreateDate", "CreateDate", SqlDbType.SmallDateTime, false);
                Status = new MSSQL.Field(this, "Status", "Status", SqlDbType.SmallInt, false);
            }
        }

        public class T_ScorePresentRule : MSSQL.TableBase
        {
            public MSSQL.Field ID;
            public MSSQL.Field PresentID;
            public MSSQL.Field ChangeType;
            public MSSQL.Field Qty;
            public MSSQL.Field ScoreNumber;
            public MSSQL.Field GoldNumber;
            public MSSQL.Field ChangeMemo;
            public MSSQL.Field PresentOrder;
            public MSSQL.Field Status;
            public MSSQL.Field IsScore;
            public MSSQL.Field AllowWinMember;
            public MSSQL.Field IsHot;
            public MSSQL.Field TimeStart;
            public MSSQL.Field TimeEnd;

            public T_ScorePresentRule()
            {
                TableName = "T_ScorePresentRule";

                ID = new MSSQL.Field(this, "ID", "ID", SqlDbType.BigInt, true);
                PresentID = new MSSQL.Field(this, "PresentID", "PresentID", SqlDbType.BigInt, false);
                ChangeType = new MSSQL.Field(this, "ChangeType", "ChangeType", SqlDbType.SmallInt, false);
                Qty = new MSSQL.Field(this, "Qty", "Qty", SqlDbType.Int, false);
                ScoreNumber = new MSSQL.Field(this, "ScoreNumber", "ScoreNumber", SqlDbType.Int, false);
                GoldNumber = new MSSQL.Field(this, "GoldNumber", "GoldNumber", SqlDbType.Int, false);
                ChangeMemo = new MSSQL.Field(this, "ChangeMemo", "ChangeMemo", SqlDbType.NVarChar, false);
                PresentOrder = new MSSQL.Field(this, "PresentOrder", "PresentOrder", SqlDbType.Int, false);
                Status = new MSSQL.Field(this, "Status", "Status", SqlDbType.SmallInt, false);
                IsScore = new MSSQL.Field(this, "IsScore", "IsScore", SqlDbType.SmallInt, false);
                AllowWinMember = new MSSQL.Field(this, "AllowWinMember", "AllowWinMember", SqlDbType.NVarChar, false);
                IsHot = new MSSQL.Field(this, "IsHot", "IsHot", SqlDbType.Bit, false);
                TimeStart = new MSSQL.Field(this, "TimeStart", "TimeStart", SqlDbType.SmallDateTime, false);
                TimeEnd = new MSSQL.Field(this, "TimeEnd", "TimeEnd", SqlDbType.SmallDateTime, false);
            }
        }

        public class T_ScorePresentShop : MSSQL.TableBase
        {
            public MSSQL.Field ID;
            public MSSQL.Field ShopName;
            public MSSQL.Field Telephone;
            public MSSQL.Field MobilePhone;
            public MSSQL.Field ContactName;

            public T_ScorePresentShop()
            {
                TableName = "T_ScorePresentShop";

                ID = new MSSQL.Field(this, "ID", "ID", SqlDbType.BigInt, true);
                ShopName = new MSSQL.Field(this, "ShopName", "ShopName", SqlDbType.NVarChar, false);
                Telephone = new MSSQL.Field(this, "Telephone", "Telephone", SqlDbType.VarChar, false);
                MobilePhone = new MSSQL.Field(this, "MobilePhone", "MobilePhone", SqlDbType.VarChar, false);
                ContactName = new MSSQL.Field(this, "ContactName", "ContactName", SqlDbType.NVarChar, false);
            }
        }

        public class T_ScorePresentType : MSSQL.TableBase
        {
            public MSSQL.Field ID;
            public MSSQL.Field ParentID;
            public MSSQL.Field TypeName;
            public MSSQL.Field CreateDate;
            public MSSQL.Field OperatorID;

            public T_ScorePresentType()
            {
                TableName = "T_ScorePresentType";

                ID = new MSSQL.Field(this, "ID", "ID", SqlDbType.BigInt, true);
                ParentID = new MSSQL.Field(this, "ParentID", "ParentID", SqlDbType.BigInt, false);
                TypeName = new MSSQL.Field(this, "TypeName", "TypeName", SqlDbType.NVarChar, false);
                CreateDate = new MSSQL.Field(this, "CreateDate", "CreateDate", SqlDbType.SmallDateTime, false);
                OperatorID = new MSSQL.Field(this, "OperatorID", "OperatorID", SqlDbType.BigInt, false);
            }
        }

        public class T_ScoreUserAddress : MSSQL.TableBase
        {
            public MSSQL.Field UserID;
            public MSSQL.Field PresentCityID;
            public MSSQL.Field PresentAddress;
            public MSSQL.Field PresentPostCode;
            public MSSQL.Field PresentPhone;
            public MSSQL.Field PresentMobile;
            public MSSQL.Field PresentContact;
            public MSSQL.Field PresentMemo;

            public T_ScoreUserAddress()
            {
                TableName = "T_ScoreUserAddress";

                UserID = new MSSQL.Field(this, "UserID", "UserID", SqlDbType.BigInt, false);
                PresentCityID = new MSSQL.Field(this, "PresentCityID", "PresentCityID", SqlDbType.NChar, false);
                PresentAddress = new MSSQL.Field(this, "PresentAddress", "PresentAddress", SqlDbType.NVarChar, false);
                PresentPostCode = new MSSQL.Field(this, "PresentPostCode", "PresentPostCode", SqlDbType.VarChar, false);
                PresentPhone = new MSSQL.Field(this, "PresentPhone", "PresentPhone", SqlDbType.VarChar, false);
                PresentMobile = new MSSQL.Field(this, "PresentMobile", "PresentMobile", SqlDbType.VarChar, false);
                PresentContact = new MSSQL.Field(this, "PresentContact", "PresentContact", SqlDbType.NVarChar, false);
                PresentMemo = new MSSQL.Field(this, "PresentMemo", "PresentMemo", SqlDbType.NVarChar, false);
            }
        }

        public class T_ScoreUserChange : MSSQL.TableBase
        {
            public MSSQL.Field ID;
            public MSSQL.Field UserID;
            public MSSQL.Field PresentRuleID;
            public MSSQL.Field ChangeType;
            public MSSQL.Field UseScore;
            public MSSQL.Field IsGoldScore;
            public MSSQL.Field IsGetPresent;
            public MSSQL.Field CreateDate;
            public MSSQL.Field Qty;

            public T_ScoreUserChange()
            {
                TableName = "T_ScoreUserChange";

                ID = new MSSQL.Field(this, "ID", "ID", SqlDbType.BigInt, true);
                UserID = new MSSQL.Field(this, "UserID", "UserID", SqlDbType.BigInt, false);
                PresentRuleID = new MSSQL.Field(this, "PresentRuleID", "PresentRuleID", SqlDbType.BigInt, false);
                ChangeType = new MSSQL.Field(this, "ChangeType", "ChangeType", SqlDbType.SmallInt, false);
                UseScore = new MSSQL.Field(this, "UseScore", "UseScore", SqlDbType.Int, false);
                IsGoldScore = new MSSQL.Field(this, "IsGoldScore", "IsGoldScore", SqlDbType.SmallInt, false);
                IsGetPresent = new MSSQL.Field(this, "IsGetPresent", "IsGetPresent", SqlDbType.Bit, false);
                CreateDate = new MSSQL.Field(this, "CreateDate", "CreateDate", SqlDbType.SmallDateTime, false);
                Qty = new MSSQL.Field(this, "Qty", "Qty", SqlDbType.Int, false);
            }
        }

        public class T_SendNoticeForQuashScheme : MSSQL.TableBase
        {
            public MSSQL.Field ID;
            public MSSQL.Field SchemeID;
            public MSSQL.Field UserID;
            public MSSQL.Field UserName;
            public MSSQL.Field IsEmailValided;
            public MSSQL.Field Email;
            public MSSQL.Field IsMobileValided;
            public MSSQL.Field Mobile;
            public MSSQL.Field Email_QuashScheme;
            public MSSQL.Field StationSMS_QuashScheme;
            public MSSQL.Field SMS_QuashScheme;
            public MSSQL.Field IsSend;

            public T_SendNoticeForQuashScheme()
            {
                TableName = "T_SendNoticeForQuashScheme";

                ID = new MSSQL.Field(this, "ID", "ID", SqlDbType.Int, true);
                SchemeID = new MSSQL.Field(this, "SchemeID", "SchemeID", SqlDbType.BigInt, false);
                UserID = new MSSQL.Field(this, "UserID", "UserID", SqlDbType.BigInt, false);
                UserName = new MSSQL.Field(this, "UserName", "UserName", SqlDbType.VarChar, false);
                IsEmailValided = new MSSQL.Field(this, "IsEmailValided", "IsEmailValided", SqlDbType.Bit, false);
                Email = new MSSQL.Field(this, "Email", "Email", SqlDbType.VarChar, false);
                IsMobileValided = new MSSQL.Field(this, "IsMobileValided", "IsMobileValided", SqlDbType.Bit, false);
                Mobile = new MSSQL.Field(this, "Mobile", "Mobile", SqlDbType.VarChar, false);
                Email_QuashScheme = new MSSQL.Field(this, "Email_QuashScheme", "Email_QuashScheme", SqlDbType.VarChar, false);
                StationSMS_QuashScheme = new MSSQL.Field(this, "StationSMS_QuashScheme", "StationSMS_QuashScheme", SqlDbType.VarChar, false);
                SMS_QuashScheme = new MSSQL.Field(this, "SMS_QuashScheme", "SMS_QuashScheme", SqlDbType.VarChar, false);
                IsSend = new MSSQL.Field(this, "IsSend", "IsSend", SqlDbType.Bit, false);
            }
        }

        public class T_SiteAffiches : MSSQL.TableBase
        {
            public MSSQL.Field ID;
            public MSSQL.Field SiteID;
            public MSSQL.Field DateTime;
            public MSSQL.Field Title;
            public MSSQL.Field isShow;
            public MSSQL.Field isCommend;
            public MSSQL.Field Content;

            public T_SiteAffiches()
            {
                TableName = "T_SiteAffiches";

                ID = new MSSQL.Field(this, "ID", "ID", SqlDbType.BigInt, true);
                SiteID = new MSSQL.Field(this, "SiteID", "SiteID", SqlDbType.BigInt, false);
                DateTime = new MSSQL.Field(this, "DateTime", "DateTime", SqlDbType.DateTime, false);
                Title = new MSSQL.Field(this, "Title", "Title", SqlDbType.VarChar, false);
                isShow = new MSSQL.Field(this, "isShow", "isShow", SqlDbType.Bit, false);
                isCommend = new MSSQL.Field(this, "isCommend", "isCommend", SqlDbType.Bit, false);
                Content = new MSSQL.Field(this, "Content", "Content", SqlDbType.VarChar, false);
            }
        }

        public class T_Sites : MSSQL.TableBase
        {
            public MSSQL.Field ID;
            public MSSQL.Field ParentID;
            public MSSQL.Field OwnerUserID;
            public MSSQL.Field Name;
            public MSSQL.Field LogoUrl;
            public MSSQL.Field Company;
            public MSSQL.Field Address;
            public MSSQL.Field PostCode;
            public MSSQL.Field ResponsiblePerson;
            public MSSQL.Field ContactPerson;
            public MSSQL.Field Telephone;
            public MSSQL.Field Fax;
            public MSSQL.Field Mobile;
            public MSSQL.Field Email;
            public MSSQL.Field QQ;
            public MSSQL.Field ServiceTelephone;
            public MSSQL.Field ICPCert;
            public MSSQL.Field Level;
            public MSSQL.Field ON;
            public MSSQL.Field BonusScale;
            public MSSQL.Field MaxSubSites;
            public MSSQL.Field UseLotteryListRestrictions;
            public MSSQL.Field UseLotteryList;
            public MSSQL.Field UseLotteryListQuickBuy;
            public MSSQL.Field Opt_BettingStationName;
            public MSSQL.Field Opt_BettingStationNumber;
            public MSSQL.Field Opt_BettingStationAddress;
            public MSSQL.Field Opt_BettingStationTelephone;
            public MSSQL.Field Opt_BettingStationContactPreson;
            public MSSQL.Field Opt_EmailServer_From;
            public MSSQL.Field Opt_EmailServer_EmailServer;
            public MSSQL.Field Opt_EmailServer_UserName;
            public MSSQL.Field Opt_EmailServer_Password;
            public MSSQL.Field Opt_ISP_HostName;
            public MSSQL.Field Opt_ISP_HostPort;
            public MSSQL.Field Opt_ISP_UserID;
            public MSSQL.Field Opt_ISP_UserPassword;
            public MSSQL.Field Opt_ISP_RegCode;
            public MSSQL.Field Opt_ISP_ServiceNumber;
            public MSSQL.Field Opt_ForumUrl;
            public MSSQL.Field Opt_MobileCheckCharset;
            public MSSQL.Field Opt_MobileCheckStringLength;
            public MSSQL.Field Opt_SMSPayType;
            public MSSQL.Field Opt_SMSPrice;
            public MSSQL.Field Opt_isUseCheckCode;
            public MSSQL.Field Opt_CheckCodeCharset;
            public MSSQL.Field Opt_isWriteLog;
            public MSSQL.Field Opt_InitiateSchemeBonusScale;
            public MSSQL.Field Opt_InitiateSchemeMinBuyScale;
            public MSSQL.Field Opt_InitiateSchemeMinBuyAndAssureScale;
            public MSSQL.Field Opt_InitiateSchemeMaxNum;
            public MSSQL.Field Opt_InitiateFollowSchemeMaxNum;
            public MSSQL.Field Opt_QuashSchemeMaxNum;
            public MSSQL.Field Opt_FullSchemeCanQuash;
            public MSSQL.Field Opt_SchemeMinMoney;
            public MSSQL.Field Opt_SchemeMaxMoney;
            public MSSQL.Field Opt_FirstPageUnionBuyMaxRows;
            public MSSQL.Field Opt_isFirstPageUnionBuyWithAll;
            public MSSQL.Field Opt_isBuyValidPasswordAdv;
            public MSSQL.Field Opt_MaxShowLotteryNumberRows;
            public MSSQL.Field Opt_LotteryCountOfMenuBarRow;
            public MSSQL.Field Opt_ScoringOfSelfBuy;
            public MSSQL.Field Opt_ScoringOfCommendBuy;
            public MSSQL.Field Opt_ScoringExchangeRate;
            public MSSQL.Field Opt_Scoring_Status_ON;
            public MSSQL.Field Opt_SchemeChatRoom_StopChatDaysAfterOpened;
            public MSSQL.Field Opt_SchemeChatRoom_MaxChatNumberOf;
            public MSSQL.Field Opt_isShowFloatAD;
            public MSSQL.Field Opt_MemberSharing_Alipay_Status_ON;
            public MSSQL.Field Opt_CpsBonusScale;
            public MSSQL.Field Opt_Cps_Status_ON;
            public MSSQL.Field Opt_Experts_Status_ON;
            public MSSQL.Field Opt_PageTitle;
            public MSSQL.Field Opt_PageKeywords;
            public MSSQL.Field Opt_DefaultFirstPageType;
            public MSSQL.Field Opt_DefaultLotteryFirstPageType;
            public MSSQL.Field Opt_LotteryChannelPage;
            public MSSQL.Field Opt_isShowSMSSubscriptionNavigate;
            public MSSQL.Field Opt_isShowChartNavigate;
            public MSSQL.Field Opt_RoomStyle;
            public MSSQL.Field Opt_RoomLogoUrl;
            public MSSQL.Field Opt_UpdateLotteryDateTime;
            public MSSQL.Field Opt_InitiateSchemeLimitLowerScaleMoney;
            public MSSQL.Field Opt_InitiateSchemeLimitLowerScale;
            public MSSQL.Field Opt_InitiateSchemeLimitUpperScaleMoney;
            public MSSQL.Field Opt_InitiateSchemeLimitUpperScale;
            public MSSQL.Field Opt_About;
            public MSSQL.Field Opt_RightFloatADContent;
            public MSSQL.Field Opt_ContactUS;
            public MSSQL.Field Opt_UserRegisterAgreement;
            public MSSQL.Field Opt_SurrogateFAQ;
            public MSSQL.Field Opt_OfficialAuthorization;
            public MSSQL.Field Opt_CompanyQualification;
            public MSSQL.Field Opt_ExpertsNote;
            public MSSQL.Field Opt_SMSSubscription;
            public MSSQL.Field Opt_LawAffirmsThat;
            public MSSQL.Field Opt_CpsPolicies;
            public MSSQL.Field TemplateEmail_Register;
            public MSSQL.Field TemplateEmail_RegisterAdv;
            public MSSQL.Field TemplateEmail_ForgetPassword;
            public MSSQL.Field TemplateEmail_UserEdit;
            public MSSQL.Field TemplateEmail_UserEditAdv;
            public MSSQL.Field TemplateEmail_InitiateScheme;
            public MSSQL.Field TemplateEmail_JoinScheme;
            public MSSQL.Field TemplateEmail_InitiateChaseTask;
            public MSSQL.Field TemplateEmail_ExecChaseTaskDetail;
            public MSSQL.Field TemplateEmail_TryDistill;
            public MSSQL.Field TemplateEmail_DistillAccept;
            public MSSQL.Field TemplateEmail_DistillNoAccept;
            public MSSQL.Field TemplateEmail_Quash;
            public MSSQL.Field TemplateEmail_QuashScheme;
            public MSSQL.Field TemplateEmail_QuashChaseTaskDetail;
            public MSSQL.Field TemplateEmail_QuashChaseTask;
            public MSSQL.Field TemplateEmail_Win;
            public MSSQL.Field TemplateEmail_MobileValid;
            public MSSQL.Field TemplateEmail_MobileValided;
            public MSSQL.Field TemplateStationSMS_Register;
            public MSSQL.Field TemplateStationSMS_RegisterAdv;
            public MSSQL.Field TemplateStationSMS_ForgetPassword;
            public MSSQL.Field TemplateStationSMS_UserEdit;
            public MSSQL.Field TemplateStationSMS_UserEditAdv;
            public MSSQL.Field TemplateStationSMS_InitiateScheme;
            public MSSQL.Field TemplateStationSMS_JoinScheme;
            public MSSQL.Field TemplateStationSMS_InitiateChaseTask;
            public MSSQL.Field TemplateStationSMS_ExecChaseTaskDetail;
            public MSSQL.Field TemplateStationSMS_TryDistill;
            public MSSQL.Field TemplateStationSMS_DistillAccept;
            public MSSQL.Field TemplateStationSMS_DistillNoAccept;
            public MSSQL.Field TemplateStationSMS_Quash;
            public MSSQL.Field TemplateStationSMS_QuashScheme;
            public MSSQL.Field TemplateStationSMS_QuashChaseTaskDetail;
            public MSSQL.Field TemplateStationSMS_QuashChaseTask;
            public MSSQL.Field TemplateStationSMS_Win;
            public MSSQL.Field TemplateStationSMS_MobileValid;
            public MSSQL.Field TemplateStationSMS_MobileValided;
            public MSSQL.Field TemplateSMS_Register;
            public MSSQL.Field TemplateSMS_RegisterAdv;
            public MSSQL.Field TemplateSMS_ForgetPassword;
            public MSSQL.Field TemplateSMS_UserEdit;
            public MSSQL.Field TemplateSMS_UserEditAdv;
            public MSSQL.Field TemplateSMS_InitiateScheme;
            public MSSQL.Field TemplateSMS_JoinScheme;
            public MSSQL.Field TemplateSMS_InitiateChaseTask;
            public MSSQL.Field TemplateSMS_ExecChaseTaskDetail;
            public MSSQL.Field TemplateSMS_TryDistill;
            public MSSQL.Field TemplateSMS_DistillAccept;
            public MSSQL.Field TemplateSMS_DistillNoAccept;
            public MSSQL.Field TemplateSMS_Quash;
            public MSSQL.Field TemplateSMS_QuashScheme;
            public MSSQL.Field TemplateSMS_QuashChaseTaskDetail;
            public MSSQL.Field TemplateSMS_QuashChaseTask;
            public MSSQL.Field TemplateSMS_Win;
            public MSSQL.Field TemplateSMS_MobileValid;
            public MSSQL.Field TemplateSMS_MobileValided;
            public MSSQL.Field Opt_CPSRegisterAgreement;
            public MSSQL.Field Opt_PromotionMemberBonusScale;
            public MSSQL.Field Opt_PromotionSiteBonusScale;
            public MSSQL.Field Opt_Promotion_Status_ON;
            public MSSQL.Field Opt_FloatNotifiesTime;
            public MSSQL.Field Opt_Score_Compendium;
            public MSSQL.Field Opt_Score_PrententType;
            public MSSQL.Field TemplateEmail_IntiateCustomChase;
            public MSSQL.Field TemplateStationSMS_IntiateCustomChase;
            public MSSQL.Field TemplateSMS_IntiateCustomChase;
            public MSSQL.Field TemplateEmail_CustomChaseWin;
            public MSSQL.Field TemplateStationSMS_CustomChaseWin;
            public MSSQL.Field TemplateSMS_CustomChaseWin;

            public T_Sites()
            {
                TableName = "T_Sites";

                ID = new MSSQL.Field(this, "ID", "ID", SqlDbType.BigInt, true);
                ParentID = new MSSQL.Field(this, "ParentID", "ParentID", SqlDbType.BigInt, false);
                OwnerUserID = new MSSQL.Field(this, "OwnerUserID", "OwnerUserID", SqlDbType.BigInt, false);
                Name = new MSSQL.Field(this, "Name", "Name", SqlDbType.VarChar, false);
                LogoUrl = new MSSQL.Field(this, "LogoUrl", "LogoUrl", SqlDbType.VarChar, false);
                Company = new MSSQL.Field(this, "Company", "Company", SqlDbType.VarChar, false);
                Address = new MSSQL.Field(this, "Address", "Address", SqlDbType.VarChar, false);
                PostCode = new MSSQL.Field(this, "PostCode", "PostCode", SqlDbType.VarChar, false);
                ResponsiblePerson = new MSSQL.Field(this, "ResponsiblePerson", "ResponsiblePerson", SqlDbType.VarChar, false);
                ContactPerson = new MSSQL.Field(this, "ContactPerson", "ContactPerson", SqlDbType.VarChar, false);
                Telephone = new MSSQL.Field(this, "Telephone", "Telephone", SqlDbType.VarChar, false);
                Fax = new MSSQL.Field(this, "Fax", "Fax", SqlDbType.VarChar, false);
                Mobile = new MSSQL.Field(this, "Mobile", "Mobile", SqlDbType.VarChar, false);
                Email = new MSSQL.Field(this, "Email", "Email", SqlDbType.VarChar, false);
                QQ = new MSSQL.Field(this, "QQ", "QQ", SqlDbType.VarChar, false);
                ServiceTelephone = new MSSQL.Field(this, "ServiceTelephone", "ServiceTelephone", SqlDbType.VarChar, false);
                ICPCert = new MSSQL.Field(this, "ICPCert", "ICPCert", SqlDbType.VarChar, false);
                Level = new MSSQL.Field(this, "Level", "Level", SqlDbType.SmallInt, false);
                ON = new MSSQL.Field(this, "ON", "ON", SqlDbType.Bit, false);
                BonusScale = new MSSQL.Field(this, "BonusScale", "BonusScale", SqlDbType.Float, false);
                MaxSubSites = new MSSQL.Field(this, "MaxSubSites", "MaxSubSites", SqlDbType.Int, false);
                UseLotteryListRestrictions = new MSSQL.Field(this, "UseLotteryListRestrictions", "UseLotteryListRestrictions", SqlDbType.VarChar, false);
                UseLotteryList = new MSSQL.Field(this, "UseLotteryList", "UseLotteryList", SqlDbType.VarChar, false);
                UseLotteryListQuickBuy = new MSSQL.Field(this, "UseLotteryListQuickBuy", "UseLotteryListQuickBuy", SqlDbType.VarChar, false);
                Opt_BettingStationName = new MSSQL.Field(this, "Opt_BettingStationName", "Opt_BettingStationName", SqlDbType.VarChar, false);
                Opt_BettingStationNumber = new MSSQL.Field(this, "Opt_BettingStationNumber", "Opt_BettingStationNumber", SqlDbType.VarChar, false);
                Opt_BettingStationAddress = new MSSQL.Field(this, "Opt_BettingStationAddress", "Opt_BettingStationAddress", SqlDbType.VarChar, false);
                Opt_BettingStationTelephone = new MSSQL.Field(this, "Opt_BettingStationTelephone", "Opt_BettingStationTelephone", SqlDbType.VarChar, false);
                Opt_BettingStationContactPreson = new MSSQL.Field(this, "Opt_BettingStationContactPreson", "Opt_BettingStationContactPreson", SqlDbType.VarChar, false);
                Opt_EmailServer_From = new MSSQL.Field(this, "Opt_EmailServer_From", "Opt_EmailServer_From", SqlDbType.VarChar, false);
                Opt_EmailServer_EmailServer = new MSSQL.Field(this, "Opt_EmailServer_EmailServer", "Opt_EmailServer_EmailServer", SqlDbType.VarChar, false);
                Opt_EmailServer_UserName = new MSSQL.Field(this, "Opt_EmailServer_UserName", "Opt_EmailServer_UserName", SqlDbType.VarChar, false);
                Opt_EmailServer_Password = new MSSQL.Field(this, "Opt_EmailServer_Password", "Opt_EmailServer_Password", SqlDbType.VarChar, false);
                Opt_ISP_HostName = new MSSQL.Field(this, "Opt_ISP_HostName", "Opt_ISP_HostName", SqlDbType.VarChar, false);
                Opt_ISP_HostPort = new MSSQL.Field(this, "Opt_ISP_HostPort", "Opt_ISP_HostPort", SqlDbType.VarChar, false);
                Opt_ISP_UserID = new MSSQL.Field(this, "Opt_ISP_UserID", "Opt_ISP_UserID", SqlDbType.VarChar, false);
                Opt_ISP_UserPassword = new MSSQL.Field(this, "Opt_ISP_UserPassword", "Opt_ISP_UserPassword", SqlDbType.VarChar, false);
                Opt_ISP_RegCode = new MSSQL.Field(this, "Opt_ISP_RegCode", "Opt_ISP_RegCode", SqlDbType.VarChar, false);
                Opt_ISP_ServiceNumber = new MSSQL.Field(this, "Opt_ISP_ServiceNumber", "Opt_ISP_ServiceNumber", SqlDbType.VarChar, false);
                Opt_ForumUrl = new MSSQL.Field(this, "Opt_ForumUrl", "Opt_ForumUrl", SqlDbType.VarChar, false);
                Opt_MobileCheckCharset = new MSSQL.Field(this, "Opt_MobileCheckCharset", "Opt_MobileCheckCharset", SqlDbType.SmallInt, false);
                Opt_MobileCheckStringLength = new MSSQL.Field(this, "Opt_MobileCheckStringLength", "Opt_MobileCheckStringLength", SqlDbType.SmallInt, false);
                Opt_SMSPayType = new MSSQL.Field(this, "Opt_SMSPayType", "Opt_SMSPayType", SqlDbType.SmallInt, false);
                Opt_SMSPrice = new MSSQL.Field(this, "Opt_SMSPrice", "Opt_SMSPrice", SqlDbType.Money, false);
                Opt_isUseCheckCode = new MSSQL.Field(this, "Opt_isUseCheckCode", "Opt_isUseCheckCode", SqlDbType.Bit, false);
                Opt_CheckCodeCharset = new MSSQL.Field(this, "Opt_CheckCodeCharset", "Opt_CheckCodeCharset", SqlDbType.SmallInt, false);
                Opt_isWriteLog = new MSSQL.Field(this, "Opt_isWriteLog", "Opt_isWriteLog", SqlDbType.Bit, false);
                Opt_InitiateSchemeBonusScale = new MSSQL.Field(this, "Opt_InitiateSchemeBonusScale", "Opt_InitiateSchemeBonusScale", SqlDbType.Float, false);
                Opt_InitiateSchemeMinBuyScale = new MSSQL.Field(this, "Opt_InitiateSchemeMinBuyScale", "Opt_InitiateSchemeMinBuyScale", SqlDbType.Float, false);
                Opt_InitiateSchemeMinBuyAndAssureScale = new MSSQL.Field(this, "Opt_InitiateSchemeMinBuyAndAssureScale", "Opt_InitiateSchemeMinBuyAndAssureScale", SqlDbType.Float, false);
                Opt_InitiateSchemeMaxNum = new MSSQL.Field(this, "Opt_InitiateSchemeMaxNum", "Opt_InitiateSchemeMaxNum", SqlDbType.SmallInt, false);
                Opt_InitiateFollowSchemeMaxNum = new MSSQL.Field(this, "Opt_InitiateFollowSchemeMaxNum", "Opt_InitiateFollowSchemeMaxNum", SqlDbType.SmallInt, false);
                Opt_QuashSchemeMaxNum = new MSSQL.Field(this, "Opt_QuashSchemeMaxNum", "Opt_QuashSchemeMaxNum", SqlDbType.SmallInt, false);
                Opt_FullSchemeCanQuash = new MSSQL.Field(this, "Opt_FullSchemeCanQuash", "Opt_FullSchemeCanQuash", SqlDbType.Bit, false);
                Opt_SchemeMinMoney = new MSSQL.Field(this, "Opt_SchemeMinMoney", "Opt_SchemeMinMoney", SqlDbType.Money, false);
                Opt_SchemeMaxMoney = new MSSQL.Field(this, "Opt_SchemeMaxMoney", "Opt_SchemeMaxMoney", SqlDbType.Money, false);
                Opt_FirstPageUnionBuyMaxRows = new MSSQL.Field(this, "Opt_FirstPageUnionBuyMaxRows", "Opt_FirstPageUnionBuyMaxRows", SqlDbType.SmallInt, false);
                Opt_isFirstPageUnionBuyWithAll = new MSSQL.Field(this, "Opt_isFirstPageUnionBuyWithAll", "Opt_isFirstPageUnionBuyWithAll", SqlDbType.Bit, false);
                Opt_isBuyValidPasswordAdv = new MSSQL.Field(this, "Opt_isBuyValidPasswordAdv", "Opt_isBuyValidPasswordAdv", SqlDbType.Bit, false);
                Opt_MaxShowLotteryNumberRows = new MSSQL.Field(this, "Opt_MaxShowLotteryNumberRows", "Opt_MaxShowLotteryNumberRows", SqlDbType.SmallInt, false);
                Opt_LotteryCountOfMenuBarRow = new MSSQL.Field(this, "Opt_LotteryCountOfMenuBarRow", "Opt_LotteryCountOfMenuBarRow", SqlDbType.SmallInt, false);
                Opt_ScoringOfSelfBuy = new MSSQL.Field(this, "Opt_ScoringOfSelfBuy", "Opt_ScoringOfSelfBuy", SqlDbType.Float, false);
                Opt_ScoringOfCommendBuy = new MSSQL.Field(this, "Opt_ScoringOfCommendBuy", "Opt_ScoringOfCommendBuy", SqlDbType.Float, false);
                Opt_ScoringExchangeRate = new MSSQL.Field(this, "Opt_ScoringExchangeRate", "Opt_ScoringExchangeRate", SqlDbType.Float, false);
                Opt_Scoring_Status_ON = new MSSQL.Field(this, "Opt_Scoring_Status_ON", "Opt_Scoring_Status_ON", SqlDbType.Bit, false);
                Opt_SchemeChatRoom_StopChatDaysAfterOpened = new MSSQL.Field(this, "Opt_SchemeChatRoom_StopChatDaysAfterOpened", "Opt_SchemeChatRoom_StopChatDaysAfterOpened", SqlDbType.SmallInt, false);
                Opt_SchemeChatRoom_MaxChatNumberOf = new MSSQL.Field(this, "Opt_SchemeChatRoom_MaxChatNumberOf", "Opt_SchemeChatRoom_MaxChatNumberOf", SqlDbType.SmallInt, false);
                Opt_isShowFloatAD = new MSSQL.Field(this, "Opt_isShowFloatAD", "Opt_isShowFloatAD", SqlDbType.Bit, false);
                Opt_MemberSharing_Alipay_Status_ON = new MSSQL.Field(this, "Opt_MemberSharing_Alipay_Status_ON", "Opt_MemberSharing_Alipay_Status_ON", SqlDbType.Bit, false);
                Opt_CpsBonusScale = new MSSQL.Field(this, "Opt_CpsBonusScale", "Opt_CpsBonusScale", SqlDbType.Float, false);
                Opt_Cps_Status_ON = new MSSQL.Field(this, "Opt_Cps_Status_ON", "Opt_Cps_Status_ON", SqlDbType.Bit, false);
                Opt_Experts_Status_ON = new MSSQL.Field(this, "Opt_Experts_Status_ON", "Opt_Experts_Status_ON", SqlDbType.Bit, false);
                Opt_PageTitle = new MSSQL.Field(this, "Opt_PageTitle", "Opt_PageTitle", SqlDbType.VarChar, false);
                Opt_PageKeywords = new MSSQL.Field(this, "Opt_PageKeywords", "Opt_PageKeywords", SqlDbType.VarChar, false);
                Opt_DefaultFirstPageType = new MSSQL.Field(this, "Opt_DefaultFirstPageType", "Opt_DefaultFirstPageType", SqlDbType.SmallInt, false);
                Opt_DefaultLotteryFirstPageType = new MSSQL.Field(this, "Opt_DefaultLotteryFirstPageType", "Opt_DefaultLotteryFirstPageType", SqlDbType.SmallInt, false);
                Opt_LotteryChannelPage = new MSSQL.Field(this, "Opt_LotteryChannelPage", "Opt_LotteryChannelPage", SqlDbType.VarChar, false);
                Opt_isShowSMSSubscriptionNavigate = new MSSQL.Field(this, "Opt_isShowSMSSubscriptionNavigate", "Opt_isShowSMSSubscriptionNavigate", SqlDbType.Bit, false);
                Opt_isShowChartNavigate = new MSSQL.Field(this, "Opt_isShowChartNavigate", "Opt_isShowChartNavigate", SqlDbType.Bit, false);
                Opt_RoomStyle = new MSSQL.Field(this, "Opt_RoomStyle", "Opt_RoomStyle", SqlDbType.SmallInt, false);
                Opt_RoomLogoUrl = new MSSQL.Field(this, "Opt_RoomLogoUrl", "Opt_RoomLogoUrl", SqlDbType.VarChar, false);
                Opt_UpdateLotteryDateTime = new MSSQL.Field(this, "Opt_UpdateLotteryDateTime", "Opt_UpdateLotteryDateTime", SqlDbType.DateTime, false);
                Opt_InitiateSchemeLimitLowerScaleMoney = new MSSQL.Field(this, "Opt_InitiateSchemeLimitLowerScaleMoney", "Opt_InitiateSchemeLimitLowerScaleMoney", SqlDbType.Money, false);
                Opt_InitiateSchemeLimitLowerScale = new MSSQL.Field(this, "Opt_InitiateSchemeLimitLowerScale", "Opt_InitiateSchemeLimitLowerScale", SqlDbType.Float, false);
                Opt_InitiateSchemeLimitUpperScaleMoney = new MSSQL.Field(this, "Opt_InitiateSchemeLimitUpperScaleMoney", "Opt_InitiateSchemeLimitUpperScaleMoney", SqlDbType.Money, false);
                Opt_InitiateSchemeLimitUpperScale = new MSSQL.Field(this, "Opt_InitiateSchemeLimitUpperScale", "Opt_InitiateSchemeLimitUpperScale", SqlDbType.Float, false);
                Opt_About = new MSSQL.Field(this, "Opt_About", "Opt_About", SqlDbType.VarChar, false);
                Opt_RightFloatADContent = new MSSQL.Field(this, "Opt_RightFloatADContent", "Opt_RightFloatADContent", SqlDbType.VarChar, false);
                Opt_ContactUS = new MSSQL.Field(this, "Opt_ContactUS", "Opt_ContactUS", SqlDbType.VarChar, false);
                Opt_UserRegisterAgreement = new MSSQL.Field(this, "Opt_UserRegisterAgreement", "Opt_UserRegisterAgreement", SqlDbType.VarChar, false);
                Opt_SurrogateFAQ = new MSSQL.Field(this, "Opt_SurrogateFAQ", "Opt_SurrogateFAQ", SqlDbType.VarChar, false);
                Opt_OfficialAuthorization = new MSSQL.Field(this, "Opt_OfficialAuthorization", "Opt_OfficialAuthorization", SqlDbType.VarChar, false);
                Opt_CompanyQualification = new MSSQL.Field(this, "Opt_CompanyQualification", "Opt_CompanyQualification", SqlDbType.VarChar, false);
                Opt_ExpertsNote = new MSSQL.Field(this, "Opt_ExpertsNote", "Opt_ExpertsNote", SqlDbType.VarChar, false);
                Opt_SMSSubscription = new MSSQL.Field(this, "Opt_SMSSubscription", "Opt_SMSSubscription", SqlDbType.VarChar, false);
                Opt_LawAffirmsThat = new MSSQL.Field(this, "Opt_LawAffirmsThat", "Opt_LawAffirmsThat", SqlDbType.VarChar, false);
                Opt_CpsPolicies = new MSSQL.Field(this, "Opt_CpsPolicies", "Opt_CpsPolicies", SqlDbType.VarChar, false);
                TemplateEmail_Register = new MSSQL.Field(this, "TemplateEmail_Register", "TemplateEmail_Register", SqlDbType.VarChar, false);
                TemplateEmail_RegisterAdv = new MSSQL.Field(this, "TemplateEmail_RegisterAdv", "TemplateEmail_RegisterAdv", SqlDbType.VarChar, false);
                TemplateEmail_ForgetPassword = new MSSQL.Field(this, "TemplateEmail_ForgetPassword", "TemplateEmail_ForgetPassword", SqlDbType.VarChar, false);
                TemplateEmail_UserEdit = new MSSQL.Field(this, "TemplateEmail_UserEdit", "TemplateEmail_UserEdit", SqlDbType.VarChar, false);
                TemplateEmail_UserEditAdv = new MSSQL.Field(this, "TemplateEmail_UserEditAdv", "TemplateEmail_UserEditAdv", SqlDbType.VarChar, false);
                TemplateEmail_InitiateScheme = new MSSQL.Field(this, "TemplateEmail_InitiateScheme", "TemplateEmail_InitiateScheme", SqlDbType.VarChar, false);
                TemplateEmail_JoinScheme = new MSSQL.Field(this, "TemplateEmail_JoinScheme", "TemplateEmail_JoinScheme", SqlDbType.VarChar, false);
                TemplateEmail_InitiateChaseTask = new MSSQL.Field(this, "TemplateEmail_InitiateChaseTask", "TemplateEmail_InitiateChaseTask", SqlDbType.VarChar, false);
                TemplateEmail_ExecChaseTaskDetail = new MSSQL.Field(this, "TemplateEmail_ExecChaseTaskDetail", "TemplateEmail_ExecChaseTaskDetail", SqlDbType.VarChar, false);
                TemplateEmail_TryDistill = new MSSQL.Field(this, "TemplateEmail_TryDistill", "TemplateEmail_TryDistill", SqlDbType.VarChar, false);
                TemplateEmail_DistillAccept = new MSSQL.Field(this, "TemplateEmail_DistillAccept", "TemplateEmail_DistillAccept", SqlDbType.VarChar, false);
                TemplateEmail_DistillNoAccept = new MSSQL.Field(this, "TemplateEmail_DistillNoAccept", "TemplateEmail_DistillNoAccept", SqlDbType.VarChar, false);
                TemplateEmail_Quash = new MSSQL.Field(this, "TemplateEmail_Quash", "TemplateEmail_Quash", SqlDbType.VarChar, false);
                TemplateEmail_QuashScheme = new MSSQL.Field(this, "TemplateEmail_QuashScheme", "TemplateEmail_QuashScheme", SqlDbType.VarChar, false);
                TemplateEmail_QuashChaseTaskDetail = new MSSQL.Field(this, "TemplateEmail_QuashChaseTaskDetail", "TemplateEmail_QuashChaseTaskDetail", SqlDbType.VarChar, false);
                TemplateEmail_QuashChaseTask = new MSSQL.Field(this, "TemplateEmail_QuashChaseTask", "TemplateEmail_QuashChaseTask", SqlDbType.VarChar, false);
                TemplateEmail_Win = new MSSQL.Field(this, "TemplateEmail_Win", "TemplateEmail_Win", SqlDbType.VarChar, false);
                TemplateEmail_MobileValid = new MSSQL.Field(this, "TemplateEmail_MobileValid", "TemplateEmail_MobileValid", SqlDbType.VarChar, false);
                TemplateEmail_MobileValided = new MSSQL.Field(this, "TemplateEmail_MobileValided", "TemplateEmail_MobileValided", SqlDbType.VarChar, false);
                TemplateStationSMS_Register = new MSSQL.Field(this, "TemplateStationSMS_Register", "TemplateStationSMS_Register", SqlDbType.VarChar, false);
                TemplateStationSMS_RegisterAdv = new MSSQL.Field(this, "TemplateStationSMS_RegisterAdv", "TemplateStationSMS_RegisterAdv", SqlDbType.VarChar, false);
                TemplateStationSMS_ForgetPassword = new MSSQL.Field(this, "TemplateStationSMS_ForgetPassword", "TemplateStationSMS_ForgetPassword", SqlDbType.VarChar, false);
                TemplateStationSMS_UserEdit = new MSSQL.Field(this, "TemplateStationSMS_UserEdit", "TemplateStationSMS_UserEdit", SqlDbType.VarChar, false);
                TemplateStationSMS_UserEditAdv = new MSSQL.Field(this, "TemplateStationSMS_UserEditAdv", "TemplateStationSMS_UserEditAdv", SqlDbType.VarChar, false);
                TemplateStationSMS_InitiateScheme = new MSSQL.Field(this, "TemplateStationSMS_InitiateScheme", "TemplateStationSMS_InitiateScheme", SqlDbType.VarChar, false);
                TemplateStationSMS_JoinScheme = new MSSQL.Field(this, "TemplateStationSMS_JoinScheme", "TemplateStationSMS_JoinScheme", SqlDbType.VarChar, false);
                TemplateStationSMS_InitiateChaseTask = new MSSQL.Field(this, "TemplateStationSMS_InitiateChaseTask", "TemplateStationSMS_InitiateChaseTask", SqlDbType.VarChar, false);
                TemplateStationSMS_ExecChaseTaskDetail = new MSSQL.Field(this, "TemplateStationSMS_ExecChaseTaskDetail", "TemplateStationSMS_ExecChaseTaskDetail", SqlDbType.VarChar, false);
                TemplateStationSMS_TryDistill = new MSSQL.Field(this, "TemplateStationSMS_TryDistill", "TemplateStationSMS_TryDistill", SqlDbType.VarChar, false);
                TemplateStationSMS_DistillAccept = new MSSQL.Field(this, "TemplateStationSMS_DistillAccept", "TemplateStationSMS_DistillAccept", SqlDbType.VarChar, false);
                TemplateStationSMS_DistillNoAccept = new MSSQL.Field(this, "TemplateStationSMS_DistillNoAccept", "TemplateStationSMS_DistillNoAccept", SqlDbType.VarChar, false);
                TemplateStationSMS_Quash = new MSSQL.Field(this, "TemplateStationSMS_Quash", "TemplateStationSMS_Quash", SqlDbType.VarChar, false);
                TemplateStationSMS_QuashScheme = new MSSQL.Field(this, "TemplateStationSMS_QuashScheme", "TemplateStationSMS_QuashScheme", SqlDbType.VarChar, false);
                TemplateStationSMS_QuashChaseTaskDetail = new MSSQL.Field(this, "TemplateStationSMS_QuashChaseTaskDetail", "TemplateStationSMS_QuashChaseTaskDetail", SqlDbType.VarChar, false);
                TemplateStationSMS_QuashChaseTask = new MSSQL.Field(this, "TemplateStationSMS_QuashChaseTask", "TemplateStationSMS_QuashChaseTask", SqlDbType.VarChar, false);
                TemplateStationSMS_Win = new MSSQL.Field(this, "TemplateStationSMS_Win", "TemplateStationSMS_Win", SqlDbType.VarChar, false);
                TemplateStationSMS_MobileValid = new MSSQL.Field(this, "TemplateStationSMS_MobileValid", "TemplateStationSMS_MobileValid", SqlDbType.VarChar, false);
                TemplateStationSMS_MobileValided = new MSSQL.Field(this, "TemplateStationSMS_MobileValided", "TemplateStationSMS_MobileValided", SqlDbType.VarChar, false);
                TemplateSMS_Register = new MSSQL.Field(this, "TemplateSMS_Register", "TemplateSMS_Register", SqlDbType.VarChar, false);
                TemplateSMS_RegisterAdv = new MSSQL.Field(this, "TemplateSMS_RegisterAdv", "TemplateSMS_RegisterAdv", SqlDbType.VarChar, false);
                TemplateSMS_ForgetPassword = new MSSQL.Field(this, "TemplateSMS_ForgetPassword", "TemplateSMS_ForgetPassword", SqlDbType.VarChar, false);
                TemplateSMS_UserEdit = new MSSQL.Field(this, "TemplateSMS_UserEdit", "TemplateSMS_UserEdit", SqlDbType.VarChar, false);
                TemplateSMS_UserEditAdv = new MSSQL.Field(this, "TemplateSMS_UserEditAdv", "TemplateSMS_UserEditAdv", SqlDbType.VarChar, false);
                TemplateSMS_InitiateScheme = new MSSQL.Field(this, "TemplateSMS_InitiateScheme", "TemplateSMS_InitiateScheme", SqlDbType.VarChar, false);
                TemplateSMS_JoinScheme = new MSSQL.Field(this, "TemplateSMS_JoinScheme", "TemplateSMS_JoinScheme", SqlDbType.VarChar, false);
                TemplateSMS_InitiateChaseTask = new MSSQL.Field(this, "TemplateSMS_InitiateChaseTask", "TemplateSMS_InitiateChaseTask", SqlDbType.VarChar, false);
                TemplateSMS_ExecChaseTaskDetail = new MSSQL.Field(this, "TemplateSMS_ExecChaseTaskDetail", "TemplateSMS_ExecChaseTaskDetail", SqlDbType.VarChar, false);
                TemplateSMS_TryDistill = new MSSQL.Field(this, "TemplateSMS_TryDistill", "TemplateSMS_TryDistill", SqlDbType.VarChar, false);
                TemplateSMS_DistillAccept = new MSSQL.Field(this, "TemplateSMS_DistillAccept", "TemplateSMS_DistillAccept", SqlDbType.VarChar, false);
                TemplateSMS_DistillNoAccept = new MSSQL.Field(this, "TemplateSMS_DistillNoAccept", "TemplateSMS_DistillNoAccept", SqlDbType.VarChar, false);
                TemplateSMS_Quash = new MSSQL.Field(this, "TemplateSMS_Quash", "TemplateSMS_Quash", SqlDbType.VarChar, false);
                TemplateSMS_QuashScheme = new MSSQL.Field(this, "TemplateSMS_QuashScheme", "TemplateSMS_QuashScheme", SqlDbType.VarChar, false);
                TemplateSMS_QuashChaseTaskDetail = new MSSQL.Field(this, "TemplateSMS_QuashChaseTaskDetail", "TemplateSMS_QuashChaseTaskDetail", SqlDbType.VarChar, false);
                TemplateSMS_QuashChaseTask = new MSSQL.Field(this, "TemplateSMS_QuashChaseTask", "TemplateSMS_QuashChaseTask", SqlDbType.VarChar, false);
                TemplateSMS_Win = new MSSQL.Field(this, "TemplateSMS_Win", "TemplateSMS_Win", SqlDbType.VarChar, false);
                TemplateSMS_MobileValid = new MSSQL.Field(this, "TemplateSMS_MobileValid", "TemplateSMS_MobileValid", SqlDbType.VarChar, false);
                TemplateSMS_MobileValided = new MSSQL.Field(this, "TemplateSMS_MobileValided", "TemplateSMS_MobileValided", SqlDbType.VarChar, false);
                Opt_CPSRegisterAgreement = new MSSQL.Field(this, "Opt_CPSRegisterAgreement", "Opt_CPSRegisterAgreement", SqlDbType.VarChar, false);
                Opt_PromotionMemberBonusScale = new MSSQL.Field(this, "Opt_PromotionMemberBonusScale", "Opt_PromotionMemberBonusScale", SqlDbType.Float, false);
                Opt_PromotionSiteBonusScale = new MSSQL.Field(this, "Opt_PromotionSiteBonusScale", "Opt_PromotionSiteBonusScale", SqlDbType.Float, false);
                Opt_Promotion_Status_ON = new MSSQL.Field(this, "Opt_Promotion_Status_ON", "Opt_Promotion_Status_ON", SqlDbType.Bit, false);
                Opt_FloatNotifiesTime = new MSSQL.Field(this, "Opt_FloatNotifiesTime", "Opt_FloatNotifiesTime", SqlDbType.SmallInt, false);
                Opt_Score_Compendium = new MSSQL.Field(this, "Opt_Score_Compendium", "Opt_Score_Compendium", SqlDbType.Decimal, false);
                Opt_Score_PrententType = new MSSQL.Field(this, "Opt_Score_PrententType", "Opt_Score_PrententType", SqlDbType.SmallInt, false);
                TemplateEmail_IntiateCustomChase = new MSSQL.Field(this, "TemplateEmail_IntiateCustomChase", "TemplateEmail_IntiateCustomChase", SqlDbType.VarChar, false);
                TemplateStationSMS_IntiateCustomChase = new MSSQL.Field(this, "TemplateStationSMS_IntiateCustomChase", "TemplateStationSMS_IntiateCustomChase", SqlDbType.VarChar, false);
                TemplateSMS_IntiateCustomChase = new MSSQL.Field(this, "TemplateSMS_IntiateCustomChase", "TemplateSMS_IntiateCustomChase", SqlDbType.VarChar, false);
                TemplateEmail_CustomChaseWin = new MSSQL.Field(this, "TemplateEmail_CustomChaseWin", "TemplateEmail_CustomChaseWin", SqlDbType.VarChar, false);
                TemplateStationSMS_CustomChaseWin = new MSSQL.Field(this, "TemplateStationSMS_CustomChaseWin", "TemplateStationSMS_CustomChaseWin", SqlDbType.VarChar, false);
                TemplateSMS_CustomChaseWin = new MSSQL.Field(this, "TemplateSMS_CustomChaseWin", "TemplateSMS_CustomChaseWin", SqlDbType.VarChar, false);
            }
        }

        public class T_SiteSendNotificationTypes : MSSQL.TableBase
        {
            public MSSQL.Field SiteID;
            public MSSQL.Field Manner;
            public MSSQL.Field NotificationTypeID;

            public T_SiteSendNotificationTypes()
            {
                TableName = "T_SiteSendNotificationTypes";

                SiteID = new MSSQL.Field(this, "SiteID", "SiteID", SqlDbType.BigInt, false);
                Manner = new MSSQL.Field(this, "Manner", "Manner", SqlDbType.SmallInt, false);
                NotificationTypeID = new MSSQL.Field(this, "NotificationTypeID", "NotificationTypeID", SqlDbType.SmallInt, false);
            }
        }

        public class T_SiteUrls : MSSQL.TableBase
        {
            public MSSQL.Field ID;
            public MSSQL.Field SiteID;
            public MSSQL.Field Url;

            public T_SiteUrls()
            {
                TableName = "T_SiteUrls";

                ID = new MSSQL.Field(this, "ID", "ID", SqlDbType.BigInt, true);
                SiteID = new MSSQL.Field(this, "SiteID", "SiteID", SqlDbType.BigInt, false);
                Url = new MSSQL.Field(this, "Url", "Url", SqlDbType.VarChar, false);
            }
        }

        public class T_SMS : MSSQL.TableBase
        {
            public MSSQL.Field ID;
            public MSSQL.Field SiteID;
            public MSSQL.Field SMSID;
            public MSSQL.Field From;
            public MSSQL.Field To;
            public MSSQL.Field DateTime;
            public MSSQL.Field Content;
            public MSSQL.Field IsSent;

            public T_SMS()
            {
                TableName = "T_SMS";

                ID = new MSSQL.Field(this, "ID", "ID", SqlDbType.BigInt, true);
                SiteID = new MSSQL.Field(this, "SiteID", "SiteID", SqlDbType.BigInt, false);
                SMSID = new MSSQL.Field(this, "SMSID", "SMSID", SqlDbType.BigInt, false);
                From = new MSSQL.Field(this, "From", "From", SqlDbType.VarChar, false);
                To = new MSSQL.Field(this, "To", "To", SqlDbType.VarChar, false);
                DateTime = new MSSQL.Field(this, "DateTime", "DateTime", SqlDbType.DateTime, false);
                Content = new MSSQL.Field(this, "Content", "Content", SqlDbType.VarChar, false);
                IsSent = new MSSQL.Field(this, "IsSent", "IsSent", SqlDbType.Bit, false);
            }
        }

        public class T_SmsBettings : MSSQL.TableBase
        {
            public MSSQL.Field ID;
            public MSSQL.Field SiteID;
            public MSSQL.Field SMSID;
            public MSSQL.Field From;
            public MSSQL.Field DateTime;
            public MSSQL.Field Content;
            public MSSQL.Field SchemeID;
            public MSSQL.Field HandleResult;
            public MSSQL.Field HandleDescription;

            public T_SmsBettings()
            {
                TableName = "T_SmsBettings";

                ID = new MSSQL.Field(this, "ID", "ID", SqlDbType.BigInt, true);
                SiteID = new MSSQL.Field(this, "SiteID", "SiteID", SqlDbType.BigInt, false);
                SMSID = new MSSQL.Field(this, "SMSID", "SMSID", SqlDbType.BigInt, false);
                From = new MSSQL.Field(this, "From", "From", SqlDbType.VarChar, false);
                DateTime = new MSSQL.Field(this, "DateTime", "DateTime", SqlDbType.DateTime, false);
                Content = new MSSQL.Field(this, "Content", "Content", SqlDbType.VarChar, false);
                SchemeID = new MSSQL.Field(this, "SchemeID", "SchemeID", SqlDbType.BigInt, false);
                HandleResult = new MSSQL.Field(this, "HandleResult", "HandleResult", SqlDbType.Bit, false);
                HandleDescription = new MSSQL.Field(this, "HandleDescription", "HandleDescription", SqlDbType.VarChar, false);
            }
        }

        public class T_SNSFeedTemplate : MSSQL.TableBase
        {
            public MSSQL.Field ID;
            public MSSQL.Field Title;
            public MSSQL.Field Content;
            public MSSQL.Field Type;
            public MSSQL.Field OperateID;

            public T_SNSFeedTemplate()
            {
                TableName = "T_SNSFeedTemplate";

                ID = new MSSQL.Field(this, "ID", "ID", SqlDbType.Int, true);
                Title = new MSSQL.Field(this, "Title", "Title", SqlDbType.VarChar, false);
                Content = new MSSQL.Field(this, "Content", "Content", SqlDbType.VarChar, false);
                Type = new MSSQL.Field(this, "Type", "Type", SqlDbType.VarChar, false);
                OperateID = new MSSQL.Field(this, "OperateID", "OperateID", SqlDbType.Int, false);
            }
        }

        public class T_SoftDownloads : MSSQL.TableBase
        {
            public MSSQL.Field ID;
            public MSSQL.Field SiteID;
            public MSSQL.Field LotteryID;
            public MSSQL.Field DateTime;
            public MSSQL.Field Title;
            public MSSQL.Field FileUrl;
            public MSSQL.Field ImageUrl;
            public MSSQL.Field isHot;
            public MSSQL.Field isCommend;
            public MSSQL.Field isShow;
            public MSSQL.Field ReadCount;
            public MSSQL.Field Content;

            public T_SoftDownloads()
            {
                TableName = "T_SoftDownloads";

                ID = new MSSQL.Field(this, "ID", "ID", SqlDbType.BigInt, true);
                SiteID = new MSSQL.Field(this, "SiteID", "SiteID", SqlDbType.BigInt, false);
                LotteryID = new MSSQL.Field(this, "LotteryID", "LotteryID", SqlDbType.Int, false);
                DateTime = new MSSQL.Field(this, "DateTime", "DateTime", SqlDbType.DateTime, false);
                Title = new MSSQL.Field(this, "Title", "Title", SqlDbType.VarChar, false);
                FileUrl = new MSSQL.Field(this, "FileUrl", "FileUrl", SqlDbType.VarChar, false);
                ImageUrl = new MSSQL.Field(this, "ImageUrl", "ImageUrl", SqlDbType.VarChar, false);
                isHot = new MSSQL.Field(this, "isHot", "isHot", SqlDbType.Bit, false);
                isCommend = new MSSQL.Field(this, "isCommend", "isCommend", SqlDbType.Bit, false);
                isShow = new MSSQL.Field(this, "isShow", "isShow", SqlDbType.Bit, false);
                ReadCount = new MSSQL.Field(this, "ReadCount", "ReadCount", SqlDbType.Int, false);
                Content = new MSSQL.Field(this, "Content", "Content", SqlDbType.VarChar, false);
            }
        }

        public class T_StationSMS : MSSQL.TableBase
        {
            public MSSQL.Field ID;
            public MSSQL.Field SiteID;
            public MSSQL.Field SourceID;
            public MSSQL.Field AimID;
            public MSSQL.Field Type;
            public MSSQL.Field DateTime;
            public MSSQL.Field isShow;
            public MSSQL.Field Content;
            public MSSQL.Field isRead;

            public T_StationSMS()
            {
                TableName = "T_StationSMS";

                ID = new MSSQL.Field(this, "ID", "ID", SqlDbType.BigInt, true);
                SiteID = new MSSQL.Field(this, "SiteID", "SiteID", SqlDbType.BigInt, false);
                SourceID = new MSSQL.Field(this, "SourceID", "SourceID", SqlDbType.BigInt, false);
                AimID = new MSSQL.Field(this, "AimID", "AimID", SqlDbType.BigInt, false);
                Type = new MSSQL.Field(this, "Type", "Type", SqlDbType.SmallInt, false);
                DateTime = new MSSQL.Field(this, "DateTime", "DateTime", SqlDbType.DateTime, false);
                isShow = new MSSQL.Field(this, "isShow", "isShow", SqlDbType.Bit, false);
                Content = new MSSQL.Field(this, "Content", "Content", SqlDbType.VarChar, false);
                isRead = new MSSQL.Field(this, "isRead", "isRead", SqlDbType.Bit, false);
            }
        }

        public class T_SurrogateNotifications : MSSQL.TableBase
        {
            public MSSQL.Field ID;
            public MSSQL.Field SiteID;
            public MSSQL.Field DateTime;
            public MSSQL.Field Title;
            public MSSQL.Field isShow;
            public MSSQL.Field Content;

            public T_SurrogateNotifications()
            {
                TableName = "T_SurrogateNotifications";

                ID = new MSSQL.Field(this, "ID", "ID", SqlDbType.BigInt, true);
                SiteID = new MSSQL.Field(this, "SiteID", "SiteID", SqlDbType.BigInt, false);
                DateTime = new MSSQL.Field(this, "DateTime", "DateTime", SqlDbType.DateTime, false);
                Title = new MSSQL.Field(this, "Title", "Title", SqlDbType.VarChar, false);
                isShow = new MSSQL.Field(this, "isShow", "isShow", SqlDbType.Bit, false);
                Content = new MSSQL.Field(this, "Content", "Content", SqlDbType.VarChar, false);
            }
        }

        public class T_SurrogateTrys : MSSQL.TableBase
        {
            public MSSQL.Field ID;
            public MSSQL.Field SiteID;
            public MSSQL.Field UserID;
            public MSSQL.Field DateTime;
            public MSSQL.Field HandleResult;
            public MSSQL.Field HandlelDateTime;
            public MSSQL.Field Name;
            public MSSQL.Field LogoUrl;
            public MSSQL.Field Company;
            public MSSQL.Field Address;
            public MSSQL.Field PostCode;
            public MSSQL.Field ResponsiblePerson;
            public MSSQL.Field ContactPerson;
            public MSSQL.Field Telephone;
            public MSSQL.Field Fax;
            public MSSQL.Field Mobile;
            public MSSQL.Field Email;
            public MSSQL.Field QQ;
            public MSSQL.Field ServiceTelephone;
            public MSSQL.Field UseLotteryList;
            public MSSQL.Field Urls;
            public MSSQL.Field Content;

            public T_SurrogateTrys()
            {
                TableName = "T_SurrogateTrys";

                ID = new MSSQL.Field(this, "ID", "ID", SqlDbType.BigInt, true);
                SiteID = new MSSQL.Field(this, "SiteID", "SiteID", SqlDbType.BigInt, false);
                UserID = new MSSQL.Field(this, "UserID", "UserID", SqlDbType.BigInt, false);
                DateTime = new MSSQL.Field(this, "DateTime", "DateTime", SqlDbType.DateTime, false);
                HandleResult = new MSSQL.Field(this, "HandleResult", "HandleResult", SqlDbType.SmallInt, false);
                HandlelDateTime = new MSSQL.Field(this, "HandlelDateTime", "HandlelDateTime", SqlDbType.DateTime, false);
                Name = new MSSQL.Field(this, "Name", "Name", SqlDbType.VarChar, false);
                LogoUrl = new MSSQL.Field(this, "LogoUrl", "LogoUrl", SqlDbType.VarChar, false);
                Company = new MSSQL.Field(this, "Company", "Company", SqlDbType.VarChar, false);
                Address = new MSSQL.Field(this, "Address", "Address", SqlDbType.VarChar, false);
                PostCode = new MSSQL.Field(this, "PostCode", "PostCode", SqlDbType.VarChar, false);
                ResponsiblePerson = new MSSQL.Field(this, "ResponsiblePerson", "ResponsiblePerson", SqlDbType.VarChar, false);
                ContactPerson = new MSSQL.Field(this, "ContactPerson", "ContactPerson", SqlDbType.VarChar, false);
                Telephone = new MSSQL.Field(this, "Telephone", "Telephone", SqlDbType.VarChar, false);
                Fax = new MSSQL.Field(this, "Fax", "Fax", SqlDbType.VarChar, false);
                Mobile = new MSSQL.Field(this, "Mobile", "Mobile", SqlDbType.VarChar, false);
                Email = new MSSQL.Field(this, "Email", "Email", SqlDbType.VarChar, false);
                QQ = new MSSQL.Field(this, "QQ", "QQ", SqlDbType.VarChar, false);
                ServiceTelephone = new MSSQL.Field(this, "ServiceTelephone", "ServiceTelephone", SqlDbType.VarChar, false);
                UseLotteryList = new MSSQL.Field(this, "UseLotteryList", "UseLotteryList", SqlDbType.VarChar, false);
                Urls = new MSSQL.Field(this, "Urls", "Urls", SqlDbType.VarChar, false);
                Content = new MSSQL.Field(this, "Content", "Content", SqlDbType.VarChar, false);
            }
        }

        public class T_SystemLog : MSSQL.TableBase
        {
            public MSSQL.Field ID;
            public MSSQL.Field SiteID;
            public MSSQL.Field UserID;
            public MSSQL.Field DateTime;
            public MSSQL.Field IPAddress;
            public MSSQL.Field Description;

            public T_SystemLog()
            {
                TableName = "T_SystemLog";

                ID = new MSSQL.Field(this, "ID", "ID", SqlDbType.BigInt, true);
                SiteID = new MSSQL.Field(this, "SiteID", "SiteID", SqlDbType.BigInt, false);
                UserID = new MSSQL.Field(this, "UserID", "UserID", SqlDbType.BigInt, false);
                DateTime = new MSSQL.Field(this, "DateTime", "DateTime", SqlDbType.DateTime, false);
                IPAddress = new MSSQL.Field(this, "IPAddress", "IPAddress", SqlDbType.VarChar, false);
                Description = new MSSQL.Field(this, "Description", "Description", SqlDbType.SmallInt, false);
            }
        }

        public class T_TestNumber : MSSQL.TableBase
        {
            public MSSQL.Field ID;
            public MSSQL.Field IsuseID;
            public MSSQL.Field TestNumber;

            public T_TestNumber()
            {
                TableName = "T_TestNumber";

                ID = new MSSQL.Field(this, "ID", "ID", SqlDbType.BigInt, true);
                IsuseID = new MSSQL.Field(this, "IsuseID", "IsuseID", SqlDbType.BigInt, false);
                TestNumber = new MSSQL.Field(this, "TestNumber", "TestNumber", SqlDbType.VarChar, false);
            }
        }

        public class T_TomActivities : MSSQL.TableBase
        {
            public MSSQL.Field ID;
            public MSSQL.Field UserID;
            public MSSQL.Field DateTime;
            public MSSQL.Field AlipayName;
            public MSSQL.Field IsReward1;
            public MSSQL.Field DayBalanceAdd;
            public MSSQL.Field IsReward2;
            public MSSQL.Field DaySchemeMoney;
            public MSSQL.Field IsReward10;
            public MSSQL.Field DayWinMoney;
            public MSSQL.Field IsReward200;

            public T_TomActivities()
            {
                TableName = "T_TomActivities";

                ID = new MSSQL.Field(this, "ID", "ID", SqlDbType.BigInt, true);
                UserID = new MSSQL.Field(this, "UserID", "UserID", SqlDbType.BigInt, false);
                DateTime = new MSSQL.Field(this, "DateTime", "DateTime", SqlDbType.DateTime, false);
                AlipayName = new MSSQL.Field(this, "AlipayName", "AlipayName", SqlDbType.VarChar, false);
                IsReward1 = new MSSQL.Field(this, "IsReward1", "IsReward1", SqlDbType.Bit, false);
                DayBalanceAdd = new MSSQL.Field(this, "DayBalanceAdd", "DayBalanceAdd", SqlDbType.Money, false);
                IsReward2 = new MSSQL.Field(this, "IsReward2", "IsReward2", SqlDbType.Bit, false);
                DaySchemeMoney = new MSSQL.Field(this, "DaySchemeMoney", "DaySchemeMoney", SqlDbType.Money, false);
                IsReward10 = new MSSQL.Field(this, "IsReward10", "IsReward10", SqlDbType.Bit, false);
                DayWinMoney = new MSSQL.Field(this, "DayWinMoney", "DayWinMoney", SqlDbType.Money, false);
                IsReward200 = new MSSQL.Field(this, "IsReward200", "IsReward200", SqlDbType.Bit, false);
            }
        }

        public class T_TotalMoney : MSSQL.TableBase
        {
            public MSSQL.Field ID;
            public MSSQL.Field IsuseID;
            public MSSQL.Field TotalMoney;

            public T_TotalMoney()
            {
                TableName = "T_TotalMoney";

                ID = new MSSQL.Field(this, "ID", "ID", SqlDbType.BigInt, true);
                IsuseID = new MSSQL.Field(this, "IsuseID", "IsuseID", SqlDbType.BigInt, false);
                TotalMoney = new MSSQL.Field(this, "TotalMoney", "TotalMoney", SqlDbType.VarChar, false);
            }
        }

        public class T_TransferDetails : MSSQL.TableBase
        {
            public MSSQL.Field ID;
            public MSSQL.Field SiteID;
            public MSSQL.Field UserID;
            public MSSQL.Field DateTime;
            public MSSQL.Field Money;
            public MSSQL.Field FormalitiesFees;
            public MSSQL.Field RelatedUserID;
            public MSSQL.Field AcceptDateTime;
            public MSSQL.Field Result;
            public MSSQL.Field SecurityAnswer;
            public MSSQL.Field Name;
            public MSSQL.Field NickName;
            public MSSQL.Field RelatedName;
            public MSSQL.Field RelatedNickName;
            public MSSQL.Field Memo;

            public T_TransferDetails()
            {
                TableName = "T_TransferDetails";

                ID = new MSSQL.Field(this, "ID", "ID", SqlDbType.BigInt, true);
                SiteID = new MSSQL.Field(this, "SiteID", "SiteID", SqlDbType.BigInt, false);
                UserID = new MSSQL.Field(this, "UserID", "UserID", SqlDbType.BigInt, false);
                DateTime = new MSSQL.Field(this, "DateTime", "DateTime", SqlDbType.DateTime, false);
                Money = new MSSQL.Field(this, "Money", "Money", SqlDbType.Money, false);
                FormalitiesFees = new MSSQL.Field(this, "FormalitiesFees", "FormalitiesFees", SqlDbType.Money, false);
                RelatedUserID = new MSSQL.Field(this, "RelatedUserID", "RelatedUserID", SqlDbType.BigInt, false);
                AcceptDateTime = new MSSQL.Field(this, "AcceptDateTime", "AcceptDateTime", SqlDbType.DateTime, false);
                Result = new MSSQL.Field(this, "Result", "Result", SqlDbType.SmallInt, false);
                SecurityAnswer = new MSSQL.Field(this, "SecurityAnswer", "SecurityAnswer", SqlDbType.NVarChar, false);
                Name = new MSSQL.Field(this, "Name", "Name", SqlDbType.VarChar, false);
                NickName = new MSSQL.Field(this, "NickName", "NickName", SqlDbType.VarChar, false);
                RelatedName = new MSSQL.Field(this, "RelatedName", "RelatedName", SqlDbType.VarChar, false);
                RelatedNickName = new MSSQL.Field(this, "RelatedNickName", "RelatedNickName", SqlDbType.VarChar, false);
                Memo = new MSSQL.Field(this, "Memo", "Memo", SqlDbType.VarChar, false);
            }
        }

        public class T_TrendCharts : MSSQL.TableBase
        {
            public MSSQL.Field ID;
            public MSSQL.Field LotteryID;
            public MSSQL.Field TrendChartName;
            public MSSQL.Field TrendChartUrl;
            public MSSQL.Field Order;

            public T_TrendCharts()
            {
                TableName = "T_TrendCharts";

                ID = new MSSQL.Field(this, "ID", "ID", SqlDbType.Int, true);
                LotteryID = new MSSQL.Field(this, "LotteryID", "LotteryID", SqlDbType.Int, false);
                TrendChartName = new MSSQL.Field(this, "TrendChartName", "TrendChartName", SqlDbType.VarChar, false);
                TrendChartUrl = new MSSQL.Field(this, "TrendChartUrl", "TrendChartUrl", SqlDbType.VarChar, false);
                Order = new MSSQL.Field(this, "Order", "Order", SqlDbType.Int, false);
            }
        }

        public class T_UnionLinkScale : MSSQL.TableBase
        {
            public MSSQL.Field ID;
            public MSSQL.Field UnionID;
            public MSSQL.Field SiteLinkPID;
            public MSSQL.Field BonusScale;

            public T_UnionLinkScale()
            {
                TableName = "T_UnionLinkScale";

                ID = new MSSQL.Field(this, "ID", "ID", SqlDbType.BigInt, true);
                UnionID = new MSSQL.Field(this, "UnionID", "UnionID", SqlDbType.BigInt, false);
                SiteLinkPID = new MSSQL.Field(this, "SiteLinkPID", "SiteLinkPID", SqlDbType.VarChar, false);
                BonusScale = new MSSQL.Field(this, "BonusScale", "BonusScale", SqlDbType.Decimal, false);
            }
        }

        public class T_UserAcceptNotificationTypes : MSSQL.TableBase
        {
            public MSSQL.Field UserID;
            public MSSQL.Field Manner;
            public MSSQL.Field NotificationTypeID;

            public T_UserAcceptNotificationTypes()
            {
                TableName = "T_UserAcceptNotificationTypes";

                UserID = new MSSQL.Field(this, "UserID", "UserID", SqlDbType.BigInt, false);
                Manner = new MSSQL.Field(this, "Manner", "Manner", SqlDbType.SmallInt, false);
                NotificationTypeID = new MSSQL.Field(this, "NotificationTypeID", "NotificationTypeID", SqlDbType.SmallInt, false);
            }
        }

        public class T_UserActions : MSSQL.TableBase
        {
            public MSSQL.Field ID;
            public MSSQL.Field DateTime;
            public MSSQL.Field UserID;
            public MSSQL.Field Action;
            public MSSQL.Field LastSchemeID;

            public T_UserActions()
            {
                TableName = "T_UserActions";

                ID = new MSSQL.Field(this, "ID", "ID", SqlDbType.BigInt, true);
                DateTime = new MSSQL.Field(this, "DateTime", "DateTime", SqlDbType.DateTime, false);
                UserID = new MSSQL.Field(this, "UserID", "UserID", SqlDbType.BigInt, false);
                Action = new MSSQL.Field(this, "Action", "Action", SqlDbType.VarChar, false);
                LastSchemeID = new MSSQL.Field(this, "LastSchemeID", "LastSchemeID", SqlDbType.BigInt, false);
            }
        }

        public class T_UserBankBindDetails : MSSQL.TableBase
        {
            public MSSQL.Field UserID;
            public MSSQL.Field BankType;
            public MSSQL.Field BankName;
            public MSSQL.Field BankCardNumber;
            public MSSQL.Field BankInProvinceName;
            public MSSQL.Field BankInCityName;
            public MSSQL.Field BankUserName;
            public MSSQL.Field BankTypeName;

            public T_UserBankBindDetails()
            {
                TableName = "T_UserBankBindDetails";

                UserID = new MSSQL.Field(this, "UserID", "UserID", SqlDbType.BigInt, false);
                BankType = new MSSQL.Field(this, "BankType", "BankType", SqlDbType.BigInt, false);
                BankName = new MSSQL.Field(this, "BankName", "BankName", SqlDbType.VarChar, false);
                BankCardNumber = new MSSQL.Field(this, "BankCardNumber", "BankCardNumber", SqlDbType.VarChar, false);
                BankInProvinceName = new MSSQL.Field(this, "BankInProvinceName", "BankInProvinceName", SqlDbType.VarChar, false);
                BankInCityName = new MSSQL.Field(this, "BankInCityName", "BankInCityName", SqlDbType.VarChar, false);
                BankUserName = new MSSQL.Field(this, "BankUserName", "BankUserName", SqlDbType.VarChar, false);
                BankTypeName = new MSSQL.Field(this, "BankTypeName", "BankTypeName", SqlDbType.VarChar, false);
            }
        }

        public class T_UserDetails : MSSQL.TableBase
        {
            public MSSQL.Field ID;
            public MSSQL.Field SiteID;
            public MSSQL.Field UserID;
            public MSSQL.Field DateTime;
            public MSSQL.Field OperatorType;
            public MSSQL.Field Money;
            public MSSQL.Field FormalitiesFees;
            public MSSQL.Field SchemeID;
            public MSSQL.Field RelatedUserID;
            public MSSQL.Field PayNumber;
            public MSSQL.Field PayBank;
            public MSSQL.Field Memo;
            public MSSQL.Field OperatorID;
            public MSSQL.Field AlipayID;
            public MSSQL.Field AlipayName;

            public T_UserDetails()
            {
                TableName = "T_UserDetails";

                ID = new MSSQL.Field(this, "ID", "ID", SqlDbType.BigInt, true);
                SiteID = new MSSQL.Field(this, "SiteID", "SiteID", SqlDbType.BigInt, false);
                UserID = new MSSQL.Field(this, "UserID", "UserID", SqlDbType.BigInt, false);
                DateTime = new MSSQL.Field(this, "DateTime", "DateTime", SqlDbType.DateTime, false);
                OperatorType = new MSSQL.Field(this, "OperatorType", "OperatorType", SqlDbType.SmallInt, false);
                Money = new MSSQL.Field(this, "Money", "Money", SqlDbType.Money, false);
                FormalitiesFees = new MSSQL.Field(this, "FormalitiesFees", "FormalitiesFees", SqlDbType.Money, false);
                SchemeID = new MSSQL.Field(this, "SchemeID", "SchemeID", SqlDbType.BigInt, false);
                RelatedUserID = new MSSQL.Field(this, "RelatedUserID", "RelatedUserID", SqlDbType.BigInt, false);
                PayNumber = new MSSQL.Field(this, "PayNumber", "PayNumber", SqlDbType.VarChar, false);
                PayBank = new MSSQL.Field(this, "PayBank", "PayBank", SqlDbType.VarChar, false);
                Memo = new MSSQL.Field(this, "Memo", "Memo", SqlDbType.VarChar, false);
                OperatorID = new MSSQL.Field(this, "OperatorID", "OperatorID", SqlDbType.BigInt, false);
                AlipayID = new MSSQL.Field(this, "AlipayID", "AlipayID", SqlDbType.VarChar, false);
                AlipayName = new MSSQL.Field(this, "AlipayName", "AlipayName", SqlDbType.VarChar, false);
            }
        }

        public class T_UserDistillPayByAlipayLog : MSSQL.TableBase
        {
            public MSSQL.Field ID;
            public MSSQL.Field DateTime;
            public MSSQL.Field Content;

            public T_UserDistillPayByAlipayLog()
            {
                TableName = "T_UserDistillPayByAlipayLog";

                ID = new MSSQL.Field(this, "ID", "ID", SqlDbType.BigInt, true);
                DateTime = new MSSQL.Field(this, "DateTime", "DateTime", SqlDbType.SmallDateTime, false);
                Content = new MSSQL.Field(this, "Content", "Content", SqlDbType.VarChar, false);
            }
        }

        public class T_UserDistillPaymentFileDetaills : MSSQL.TableBase
        {
            public MSSQL.Field ID;
            public MSSQL.Field PaymentFileID;
            public MSSQL.Field SequenceNumber;
            public MSSQL.Field DistillID;

            public T_UserDistillPaymentFileDetaills()
            {
                TableName = "T_UserDistillPaymentFileDetaills";

                ID = new MSSQL.Field(this, "ID", "ID", SqlDbType.BigInt, true);
                PaymentFileID = new MSSQL.Field(this, "PaymentFileID", "PaymentFileID", SqlDbType.BigInt, false);
                SequenceNumber = new MSSQL.Field(this, "SequenceNumber", "SequenceNumber", SqlDbType.BigInt, false);
                DistillID = new MSSQL.Field(this, "DistillID", "DistillID", SqlDbType.BigInt, false);
            }
        }

        public class T_UserDistillPaymentFiles : MSSQL.TableBase
        {
            public MSSQL.Field id;
            public MSSQL.Field FileName;
            public MSSQL.Field DateTime;
            public MSSQL.Field Result;
            public MSSQL.Field HandleOperatorID;
            public MSSQL.Field Type;

            public T_UserDistillPaymentFiles()
            {
                TableName = "T_UserDistillPaymentFiles";

                id = new MSSQL.Field(this, "id", "id", SqlDbType.BigInt, true);
                FileName = new MSSQL.Field(this, "FileName", "FileName", SqlDbType.VarChar, false);
                DateTime = new MSSQL.Field(this, "DateTime", "DateTime", SqlDbType.DateTime, false);
                Result = new MSSQL.Field(this, "Result", "Result", SqlDbType.Int, false);
                HandleOperatorID = new MSSQL.Field(this, "HandleOperatorID", "HandleOperatorID", SqlDbType.BigInt, false);
                Type = new MSSQL.Field(this, "Type", "Type", SqlDbType.Int, false);
            }
        }

        public class T_UserDistills : MSSQL.TableBase
        {
            public MSSQL.Field ID;
            public MSSQL.Field SiteID;
            public MSSQL.Field UserID;
            public MSSQL.Field DateTime;
            public MSSQL.Field Money;
            public MSSQL.Field FormalitiesFees;
            public MSSQL.Field Result;
            public MSSQL.Field HandleDateTime;
            public MSSQL.Field BankName;
            public MSSQL.Field BankCardNumber;
            public MSSQL.Field Memo;
            public MSSQL.Field HandleOperatorID;
            public MSSQL.Field BankUserName;
            public MSSQL.Field AlipayID;
            public MSSQL.Field AlipayName;
            public MSSQL.Field DistillType;
            public MSSQL.Field BankTypeName;
            public MSSQL.Field BankInProvince;
            public MSSQL.Field BankInCity;
            public MSSQL.Field IsCps;

            public T_UserDistills()
            {
                TableName = "T_UserDistills";

                ID = new MSSQL.Field(this, "ID", "ID", SqlDbType.BigInt, true);
                SiteID = new MSSQL.Field(this, "SiteID", "SiteID", SqlDbType.BigInt, false);
                UserID = new MSSQL.Field(this, "UserID", "UserID", SqlDbType.BigInt, false);
                DateTime = new MSSQL.Field(this, "DateTime", "DateTime", SqlDbType.DateTime, false);
                Money = new MSSQL.Field(this, "Money", "Money", SqlDbType.Money, false);
                FormalitiesFees = new MSSQL.Field(this, "FormalitiesFees", "FormalitiesFees", SqlDbType.Money, false);
                Result = new MSSQL.Field(this, "Result", "Result", SqlDbType.SmallInt, false);
                HandleDateTime = new MSSQL.Field(this, "HandleDateTime", "HandleDateTime", SqlDbType.DateTime, false);
                BankName = new MSSQL.Field(this, "BankName", "BankName", SqlDbType.VarChar, false);
                BankCardNumber = new MSSQL.Field(this, "BankCardNumber", "BankCardNumber", SqlDbType.VarChar, false);
                Memo = new MSSQL.Field(this, "Memo", "Memo", SqlDbType.VarChar, false);
                HandleOperatorID = new MSSQL.Field(this, "HandleOperatorID", "HandleOperatorID", SqlDbType.BigInt, false);
                BankUserName = new MSSQL.Field(this, "BankUserName", "BankUserName", SqlDbType.VarChar, false);
                AlipayID = new MSSQL.Field(this, "AlipayID", "AlipayID", SqlDbType.VarChar, false);
                AlipayName = new MSSQL.Field(this, "AlipayName", "AlipayName", SqlDbType.VarChar, false);
                DistillType = new MSSQL.Field(this, "DistillType", "DistillType", SqlDbType.Int, false);
                BankTypeName = new MSSQL.Field(this, "BankTypeName", "BankTypeName", SqlDbType.VarChar, false);
                BankInProvince = new MSSQL.Field(this, "BankInProvince", "BankInProvince", SqlDbType.VarChar, false);
                BankInCity = new MSSQL.Field(this, "BankInCity", "BankInCity", SqlDbType.VarChar, false);
                IsCps = new MSSQL.Field(this, "IsCps", "IsCps", SqlDbType.Bit, false);
            }
        }

        public class T_UserEditQuestionAnswer : MSSQL.TableBase
        {
            public MSSQL.Field ID;
            public MSSQL.Field UserID;
            public MSSQL.Field QuestionAnswerState;
            public MSSQL.Field ValidedCount;
            public MSSQL.Field DateTime;

            public T_UserEditQuestionAnswer()
            {
                TableName = "T_UserEditQuestionAnswer";

                ID = new MSSQL.Field(this, "ID", "ID", SqlDbType.Int, true);
                UserID = new MSSQL.Field(this, "UserID", "UserID", SqlDbType.Int, false);
                QuestionAnswerState = new MSSQL.Field(this, "QuestionAnswerState", "QuestionAnswerState", SqlDbType.Int, false);
                ValidedCount = new MSSQL.Field(this, "ValidedCount", "ValidedCount", SqlDbType.Int, false);
                DateTime = new MSSQL.Field(this, "DateTime", "DateTime", SqlDbType.DateTime, false);
            }
        }

        public class T_UserForInitiateFollowSchemeTrys : MSSQL.TableBase
        {
            public MSSQL.Field ID;
            public MSSQL.Field SiteID;
            public MSSQL.Field UserID;
            public MSSQL.Field DateTime;
            public MSSQL.Field PlayTypeID;
            public MSSQL.Field Description;
            public MSSQL.Field HandleResult;
            public MSSQL.Field HandleDateTime;

            public T_UserForInitiateFollowSchemeTrys()
            {
                TableName = "T_UserForInitiateFollowSchemeTrys";

                ID = new MSSQL.Field(this, "ID", "ID", SqlDbType.BigInt, true);
                SiteID = new MSSQL.Field(this, "SiteID", "SiteID", SqlDbType.BigInt, false);
                UserID = new MSSQL.Field(this, "UserID", "UserID", SqlDbType.BigInt, false);
                DateTime = new MSSQL.Field(this, "DateTime", "DateTime", SqlDbType.DateTime, false);
                PlayTypeID = new MSSQL.Field(this, "PlayTypeID", "PlayTypeID", SqlDbType.Int, false);
                Description = new MSSQL.Field(this, "Description", "Description", SqlDbType.VarChar, false);
                HandleResult = new MSSQL.Field(this, "HandleResult", "HandleResult", SqlDbType.SmallInt, false);
                HandleDateTime = new MSSQL.Field(this, "HandleDateTime", "HandleDateTime", SqlDbType.DateTime, false);
            }
        }

        public class T_UserGroups : MSSQL.TableBase
        {
            public MSSQL.Field ID;
            public MSSQL.Field Name;
            public MSSQL.Field Description;

            public T_UserGroups()
            {
                TableName = "T_UserGroups";

                ID = new MSSQL.Field(this, "ID", "ID", SqlDbType.SmallInt, false);
                Name = new MSSQL.Field(this, "Name", "Name", SqlDbType.VarChar, false);
                Description = new MSSQL.Field(this, "Description", "Description", SqlDbType.VarChar, false);
            }
        }

        public class T_UserHongbaoPromotion : MSSQL.TableBase
        {
            public MSSQL.Field ID;
            public MSSQL.Field UserID;
            public MSSQL.Field CreateDate;
            public MSSQL.Field Money;
            public MSSQL.Field AcceptUserID;
            public MSSQL.Field UseDate;
            public MSSQL.Field ExpiryDate;
            public MSSQL.Field URL;

            public T_UserHongbaoPromotion()
            {
                TableName = "T_UserHongbaoPromotion";

                ID = new MSSQL.Field(this, "ID", "ID", SqlDbType.BigInt, true);
                UserID = new MSSQL.Field(this, "UserID", "UserID", SqlDbType.BigInt, false);
                CreateDate = new MSSQL.Field(this, "CreateDate", "CreateDate", SqlDbType.DateTime, false);
                Money = new MSSQL.Field(this, "Money", "Money", SqlDbType.Money, false);
                AcceptUserID = new MSSQL.Field(this, "AcceptUserID", "AcceptUserID", SqlDbType.BigInt, false);
                UseDate = new MSSQL.Field(this, "UseDate", "UseDate", SqlDbType.DateTime, false);
                ExpiryDate = new MSSQL.Field(this, "ExpiryDate", "ExpiryDate", SqlDbType.DateTime, false);
                URL = new MSSQL.Field(this, "URL", "URL", SqlDbType.NVarChar, false);
            }
        }

        public class T_UserHongbaoPromotionUsed : MSSQL.TableBase
        {
            public MSSQL.Field PromotionID;

            public T_UserHongbaoPromotionUsed()
            {
                TableName = "T_UserHongbaoPromotionUsed";

                PromotionID = new MSSQL.Field(this, "PromotionID", "PromotionID", SqlDbType.BigInt, false);
            }
        }

        public class T_UserInGroups : MSSQL.TableBase
        {
            public MSSQL.Field UserID;
            public MSSQL.Field GroupID;

            public T_UserInGroups()
            {
                TableName = "T_UserInGroups";

                UserID = new MSSQL.Field(this, "UserID", "UserID", SqlDbType.BigInt, false);
                GroupID = new MSSQL.Field(this, "GroupID", "GroupID", SqlDbType.SmallInt, false);
            }
        }

        public class T_UserInSchemeChatRooms : MSSQL.TableBase
        {
            public MSSQL.Field UserID;
            public MSSQL.Field SchemeID;
            public MSSQL.Field LastAccessTime;

            public T_UserInSchemeChatRooms()
            {
                TableName = "T_UserInSchemeChatRooms";

                UserID = new MSSQL.Field(this, "UserID", "UserID", SqlDbType.BigInt, false);
                SchemeID = new MSSQL.Field(this, "SchemeID", "SchemeID", SqlDbType.BigInt, false);
                LastAccessTime = new MSSQL.Field(this, "LastAccessTime", "LastAccessTime", SqlDbType.DateTime, false);
            }
        }

        public class T_UserPayDetails : MSSQL.TableBase
        {
            public MSSQL.Field ID;
            public MSSQL.Field SiteID;
            public MSSQL.Field UserID;
            public MSSQL.Field DateTime;
            public MSSQL.Field PayType;
            public MSSQL.Field Money;
            public MSSQL.Field FormalitiesFees;
            public MSSQL.Field Result;

            public T_UserPayDetails()
            {
                TableName = "T_UserPayDetails";

                ID = new MSSQL.Field(this, "ID", "ID", SqlDbType.BigInt, true);
                SiteID = new MSSQL.Field(this, "SiteID", "SiteID", SqlDbType.BigInt, false);
                UserID = new MSSQL.Field(this, "UserID", "UserID", SqlDbType.BigInt, false);
                DateTime = new MSSQL.Field(this, "DateTime", "DateTime", SqlDbType.DateTime, false);
                PayType = new MSSQL.Field(this, "PayType", "PayType", SqlDbType.VarChar, false);
                Money = new MSSQL.Field(this, "Money", "Money", SqlDbType.Money, false);
                FormalitiesFees = new MSSQL.Field(this, "FormalitiesFees", "FormalitiesFees", SqlDbType.Money, false);
                Result = new MSSQL.Field(this, "Result", "Result", SqlDbType.SmallInt, false);
            }
        }

        public class T_UserPayNumberList : MSSQL.TableBase
        {
            public MSSQL.Field PayNumber;
            public MSSQL.Field Money;

            public T_UserPayNumberList()
            {
                TableName = "T_UserPayNumberList";

                PayNumber = new MSSQL.Field(this, "PayNumber", "PayNumber", SqlDbType.BigInt, false);
                Money = new MSSQL.Field(this, "Money", "Money", SqlDbType.Money, false);
            }
        }

        public class T_UserPayOutDetails_99Bill : MSSQL.TableBase
        {
            public MSSQL.Field ID;
            public MSSQL.Field DistillID;
            public MSSQL.Field DealCharge;
            public MSSQL.Field DebitCharge;
            public MSSQL.Field CreditCharge;
            public MSSQL.Field DealID;
            public MSSQL.Field ResultFlag;
            public MSSQL.Field FailureCause;

            public T_UserPayOutDetails_99Bill()
            {
                TableName = "T_UserPayOutDetails_99Bill";

                ID = new MSSQL.Field(this, "ID", "ID", SqlDbType.BigInt, true);
                DistillID = new MSSQL.Field(this, "DistillID", "DistillID", SqlDbType.BigInt, false);
                DealCharge = new MSSQL.Field(this, "DealCharge", "DealCharge", SqlDbType.Money, false);
                DebitCharge = new MSSQL.Field(this, "DebitCharge", "DebitCharge", SqlDbType.Money, false);
                CreditCharge = new MSSQL.Field(this, "CreditCharge", "CreditCharge", SqlDbType.Money, false);
                DealID = new MSSQL.Field(this, "DealID", "DealID", SqlDbType.VarChar, false);
                ResultFlag = new MSSQL.Field(this, "ResultFlag", "ResultFlag", SqlDbType.Bit, false);
                FailureCause = new MSSQL.Field(this, "FailureCause", "FailureCause", SqlDbType.VarChar, false);
            }
        }

        public class T_Users : MSSQL.TableBase
        {
            public MSSQL.Field ID;
            public MSSQL.Field SiteID;
            public MSSQL.Field Name;
            public MSSQL.Field RealityName;
            public MSSQL.Field Password;
            public MSSQL.Field PasswordAdv;
            public MSSQL.Field CityID;
            public MSSQL.Field Sex;
            public MSSQL.Field BirthDay;
            public MSSQL.Field IDCardNumber;
            public MSSQL.Field Address;
            public MSSQL.Field Email;
            public MSSQL.Field isEmailValided;
            public MSSQL.Field QQ;
            public MSSQL.Field Telephone;
            public MSSQL.Field Mobile;
            public MSSQL.Field isMobileValided;
            public MSSQL.Field isPrivacy;
            public MSSQL.Field isCanLogin;
            public MSSQL.Field RegisterTime;
            public MSSQL.Field LastLoginTime;
            public MSSQL.Field LastLoginIP;
            public MSSQL.Field LoginCount;
            public MSSQL.Field UserType;
            public MSSQL.Field BankType;
            public MSSQL.Field BankName;
            public MSSQL.Field BankCardNumber;
            public MSSQL.Field Balance;
            public MSSQL.Field Freeze;
            public MSSQL.Field ScoringOfSelfBuy;
            public MSSQL.Field ScoringOfCommendBuy;
            public MSSQL.Field Scoring;
            public MSSQL.Field Level;
            public MSSQL.Field CommenderID;
            public MSSQL.Field CpsID;
            public MSSQL.Field AlipayID;
            public MSSQL.Field Bonus;
            public MSSQL.Field Reward;
            public MSSQL.Field AlipayName;
            public MSSQL.Field isAlipayNameValided;
            public MSSQL.Field isAlipayCps;
            public MSSQL.Field IsCrossLogin;
            public MSSQL.Field ComeFrom;
            public MSSQL.Field Memo;
            public MSSQL.Field BonusThisMonth;
            public MSSQL.Field BonusAllow;
            public MSSQL.Field BonusUse;
            public MSSQL.Field PromotionMemberBonusScale;
            public MSSQL.Field PromotionSiteBonusScale;
            public MSSQL.Field MaxFollowNumber;
            public MSSQL.Field VisitSource;
            public MSSQL.Field Key;
            public MSSQL.Field HeadUrl;
            public MSSQL.Field FriendList;
            public MSSQL.Field NickName;
            public MSSQL.Field SecurityQuestion;
            public MSSQL.Field SecurityAnswer;
            public MSSQL.Field Reason;
            public MSSQL.Field IsQQValided;
            public MSSQL.Field IsAllowWinScore;

            public T_Users()
            {
                TableName = "T_Users";

                ID = new MSSQL.Field(this, "ID", "ID", SqlDbType.BigInt, false);
                SiteID = new MSSQL.Field(this, "SiteID", "SiteID", SqlDbType.BigInt, false);
                Name = new MSSQL.Field(this, "Name", "Name", SqlDbType.VarChar, false);
                RealityName = new MSSQL.Field(this, "RealityName", "RealityName", SqlDbType.VarChar, false);
                Password = new MSSQL.Field(this, "Password", "Password", SqlDbType.VarChar, false);
                PasswordAdv = new MSSQL.Field(this, "PasswordAdv", "PasswordAdv", SqlDbType.VarChar, false);
                CityID = new MSSQL.Field(this, "CityID", "CityID", SqlDbType.Int, false);
                Sex = new MSSQL.Field(this, "Sex", "Sex", SqlDbType.VarChar, false);
                BirthDay = new MSSQL.Field(this, "BirthDay", "BirthDay", SqlDbType.DateTime, false);
                IDCardNumber = new MSSQL.Field(this, "IDCardNumber", "IDCardNumber", SqlDbType.VarChar, false);
                Address = new MSSQL.Field(this, "Address", "Address", SqlDbType.VarChar, false);
                Email = new MSSQL.Field(this, "Email", "Email", SqlDbType.VarChar, false);
                isEmailValided = new MSSQL.Field(this, "isEmailValided", "isEmailValided", SqlDbType.Bit, false);
                QQ = new MSSQL.Field(this, "QQ", "QQ", SqlDbType.VarChar, false);
                Telephone = new MSSQL.Field(this, "Telephone", "Telephone", SqlDbType.VarChar, false);
                Mobile = new MSSQL.Field(this, "Mobile", "Mobile", SqlDbType.VarChar, false);
                isMobileValided = new MSSQL.Field(this, "isMobileValided", "isMobileValided", SqlDbType.Bit, false);
                isPrivacy = new MSSQL.Field(this, "isPrivacy", "isPrivacy", SqlDbType.Bit, false);
                isCanLogin = new MSSQL.Field(this, "isCanLogin", "isCanLogin", SqlDbType.Bit, false);
                RegisterTime = new MSSQL.Field(this, "RegisterTime", "RegisterTime", SqlDbType.DateTime, false);
                LastLoginTime = new MSSQL.Field(this, "LastLoginTime", "LastLoginTime", SqlDbType.DateTime, false);
                LastLoginIP = new MSSQL.Field(this, "LastLoginIP", "LastLoginIP", SqlDbType.VarChar, false);
                LoginCount = new MSSQL.Field(this, "LoginCount", "LoginCount", SqlDbType.Int, false);
                UserType = new MSSQL.Field(this, "UserType", "UserType", SqlDbType.SmallInt, false);
                BankType = new MSSQL.Field(this, "BankType", "BankType", SqlDbType.SmallInt, false);
                BankName = new MSSQL.Field(this, "BankName", "BankName", SqlDbType.VarChar, false);
                BankCardNumber = new MSSQL.Field(this, "BankCardNumber", "BankCardNumber", SqlDbType.VarChar, false);
                Balance = new MSSQL.Field(this, "Balance", "Balance", SqlDbType.Money, false);
                Freeze = new MSSQL.Field(this, "Freeze", "Freeze", SqlDbType.Money, false);
                ScoringOfSelfBuy = new MSSQL.Field(this, "ScoringOfSelfBuy", "ScoringOfSelfBuy", SqlDbType.Float, false);
                ScoringOfCommendBuy = new MSSQL.Field(this, "ScoringOfCommendBuy", "ScoringOfCommendBuy", SqlDbType.Float, false);
                Scoring = new MSSQL.Field(this, "Scoring", "Scoring", SqlDbType.Float, false);
                Level = new MSSQL.Field(this, "Level", "Level", SqlDbType.SmallInt, false);
                CommenderID = new MSSQL.Field(this, "CommenderID", "CommenderID", SqlDbType.BigInt, false);
                CpsID = new MSSQL.Field(this, "CpsID", "CpsID", SqlDbType.BigInt, false);
                AlipayID = new MSSQL.Field(this, "AlipayID", "AlipayID", SqlDbType.VarChar, false);
                Bonus = new MSSQL.Field(this, "Bonus", "Bonus", SqlDbType.Money, false);
                Reward = new MSSQL.Field(this, "Reward", "Reward", SqlDbType.Money, false);
                AlipayName = new MSSQL.Field(this, "AlipayName", "AlipayName", SqlDbType.VarChar, false);
                isAlipayNameValided = new MSSQL.Field(this, "isAlipayNameValided", "isAlipayNameValided", SqlDbType.Bit, false);
                isAlipayCps = new MSSQL.Field(this, "isAlipayCps", "isAlipayCps", SqlDbType.Bit, false);
                IsCrossLogin = new MSSQL.Field(this, "IsCrossLogin", "IsCrossLogin", SqlDbType.Bit, false);
                ComeFrom = new MSSQL.Field(this, "ComeFrom", "ComeFrom", SqlDbType.Int, false);
                Memo = new MSSQL.Field(this, "Memo", "Memo", SqlDbType.VarChar, false);
                BonusThisMonth = new MSSQL.Field(this, "BonusThisMonth", "BonusThisMonth", SqlDbType.Money, false);
                BonusAllow = new MSSQL.Field(this, "BonusAllow", "BonusAllow", SqlDbType.Money, false);
                BonusUse = new MSSQL.Field(this, "BonusUse", "BonusUse", SqlDbType.Money, false);
                PromotionMemberBonusScale = new MSSQL.Field(this, "PromotionMemberBonusScale", "PromotionMemberBonusScale", SqlDbType.Float, false);
                PromotionSiteBonusScale = new MSSQL.Field(this, "PromotionSiteBonusScale", "PromotionSiteBonusScale", SqlDbType.Float, false);
                MaxFollowNumber = new MSSQL.Field(this, "MaxFollowNumber", "MaxFollowNumber", SqlDbType.Int, false);
                VisitSource = new MSSQL.Field(this, "VisitSource", "VisitSource", SqlDbType.VarChar, false);
                Key = new MSSQL.Field(this, "Key", "Key", SqlDbType.VarChar, false);
                HeadUrl = new MSSQL.Field(this, "HeadUrl", "HeadUrl", SqlDbType.VarChar, false);
                FriendList = new MSSQL.Field(this, "FriendList", "FriendList", SqlDbType.VarChar, false);
                NickName = new MSSQL.Field(this, "NickName", "NickName", SqlDbType.VarChar, false);
                SecurityQuestion = new MSSQL.Field(this, "SecurityQuestion", "SecurityQuestion", SqlDbType.VarChar, false);
                SecurityAnswer = new MSSQL.Field(this, "SecurityAnswer", "SecurityAnswer", SqlDbType.NVarChar, false);
                Reason = new MSSQL.Field(this, "Reason", "Reason", SqlDbType.VarChar, false);
                IsQQValided = new MSSQL.Field(this, "IsQQValided", "IsQQValided", SqlDbType.Bit, false);
                IsAllowWinScore = new MSSQL.Field(this, "IsAllowWinScore", "IsAllowWinScore", SqlDbType.Bit, false);
            }
        }

        public class T_UserScoringDetails : MSSQL.TableBase
        {
            public MSSQL.Field ID;
            public MSSQL.Field SiteID;
            public MSSQL.Field UserID;
            public MSSQL.Field DateTime;
            public MSSQL.Field OperatorType;
            public MSSQL.Field Scoring;
            public MSSQL.Field SchemeID;
            public MSSQL.Field RelatedUserID;
            public MSSQL.Field Memo;

            public T_UserScoringDetails()
            {
                TableName = "T_UserScoringDetails";

                ID = new MSSQL.Field(this, "ID", "ID", SqlDbType.BigInt, true);
                SiteID = new MSSQL.Field(this, "SiteID", "SiteID", SqlDbType.BigInt, false);
                UserID = new MSSQL.Field(this, "UserID", "UserID", SqlDbType.BigInt, false);
                DateTime = new MSSQL.Field(this, "DateTime", "DateTime", SqlDbType.DateTime, false);
                OperatorType = new MSSQL.Field(this, "OperatorType", "OperatorType", SqlDbType.SmallInt, false);
                Scoring = new MSSQL.Field(this, "Scoring", "Scoring", SqlDbType.Float, false);
                SchemeID = new MSSQL.Field(this, "SchemeID", "SchemeID", SqlDbType.BigInt, false);
                RelatedUserID = new MSSQL.Field(this, "RelatedUserID", "RelatedUserID", SqlDbType.BigInt, false);
                Memo = new MSSQL.Field(this, "Memo", "Memo", SqlDbType.VarChar, false);
            }
        }

        public class T_UsersForInitiateFollowScheme : MSSQL.TableBase
        {
            public MSSQL.Field ID;
            public MSSQL.Field SiteID;
            public MSSQL.Field UserID;
            public MSSQL.Field DateTime;
            public MSSQL.Field PlayTypeID;
            public MSSQL.Field Description;
            public MSSQL.Field MaxNumberOf;

            public T_UsersForInitiateFollowScheme()
            {
                TableName = "T_UsersForInitiateFollowScheme";

                ID = new MSSQL.Field(this, "ID", "ID", SqlDbType.BigInt, true);
                SiteID = new MSSQL.Field(this, "SiteID", "SiteID", SqlDbType.BigInt, false);
                UserID = new MSSQL.Field(this, "UserID", "UserID", SqlDbType.BigInt, false);
                DateTime = new MSSQL.Field(this, "DateTime", "DateTime", SqlDbType.DateTime, false);
                PlayTypeID = new MSSQL.Field(this, "PlayTypeID", "PlayTypeID", SqlDbType.Int, false);
                Description = new MSSQL.Field(this, "Description", "Description", SqlDbType.VarChar, false);
                MaxNumberOf = new MSSQL.Field(this, "MaxNumberOf", "MaxNumberOf", SqlDbType.Int, false);
            }
        }

        public class T_UserToCpsUId : MSSQL.TableBase
        {
            public MSSQL.Field ID;
            public MSSQL.Field Uid;
            public MSSQL.Field CpsID;
            public MSSQL.Field PID;

            public T_UserToCpsUId()
            {
                TableName = "T_UserToCpsUId";

                ID = new MSSQL.Field(this, "ID", "ID", SqlDbType.Int, false);
                Uid = new MSSQL.Field(this, "Uid", "Uid", SqlDbType.Int, false);
                CpsID = new MSSQL.Field(this, "CpsID", "CpsID", SqlDbType.Int, false);
                PID = new MSSQL.Field(this, "PID", "PID", SqlDbType.Int, false);
            }
        }

        public class T_WinScoreScale : MSSQL.TableBase
        {
            public MSSQL.Field ID;
            public MSSQL.Field PlayTypeID;
            public MSSQL.Field WinMoney;
            public MSSQL.Field ScoreScale;

            public T_WinScoreScale()
            {
                TableName = "T_WinScoreScale";

                ID = new MSSQL.Field(this, "ID", "ID", SqlDbType.Int, true);
                PlayTypeID = new MSSQL.Field(this, "PlayTypeID", "PlayTypeID", SqlDbType.Int, false);
                WinMoney = new MSSQL.Field(this, "WinMoney", "WinMoney", SqlDbType.Money, false);
                ScoreScale = new MSSQL.Field(this, "ScoreScale", "ScoreScale", SqlDbType.Float, false);
            }
        }

        public class T_WinTypes : MSSQL.TableBase
        {
            public MSSQL.Field ID;
            public MSSQL.Field LotteryID;
            public MSSQL.Field Name;
            public MSSQL.Field Order;
            public MSSQL.Field DefaultMoney;
            public MSSQL.Field DefaultMoneyNoWithTax;

            public T_WinTypes()
            {
                TableName = "T_WinTypes";

                ID = new MSSQL.Field(this, "ID", "ID", SqlDbType.Int, false);
                LotteryID = new MSSQL.Field(this, "LotteryID", "LotteryID", SqlDbType.Int, false);
                Name = new MSSQL.Field(this, "Name", "Name", SqlDbType.VarChar, false);
                Order = new MSSQL.Field(this, "Order", "Order", SqlDbType.Int, false);
                DefaultMoney = new MSSQL.Field(this, "DefaultMoney", "DefaultMoney", SqlDbType.Money, false);
                DefaultMoneyNoWithTax = new MSSQL.Field(this, "DefaultMoneyNoWithTax", "DefaultMoneyNoWithTax", SqlDbType.Money, false);
            }
        }
    }

    public class Views
    {
        public class V_Advertisements : MSSQL.ViewBase
        {
            public V_Advertisements()
            {
                ViewName = "V_Advertisements";
            }
        }

        public class V_BuyDetails : MSSQL.ViewBase
        {
            public V_BuyDetails()
            {
                ViewName = "V_BuyDetails";
            }
        }

        public class V_BuyDetailsNonce : MSSQL.ViewBase
        {
            public V_BuyDetailsNonce()
            {
                ViewName = "V_BuyDetailsNonce";
            }
        }

        public class V_BuyDetailsToCenter : MSSQL.ViewBase
        {
            public V_BuyDetailsToCenter()
            {
                ViewName = "V_BuyDetailsToCenter";
            }
        }

        public class V_BuyDetailsWithQuashed : MSSQL.ViewBase
        {
            public V_BuyDetailsWithQuashed()
            {
                ViewName = "V_BuyDetailsWithQuashed";
            }
        }

        public class V_BuyDetailsWithQuashedAll : MSSQL.ViewBase
        {
            public V_BuyDetailsWithQuashedAll()
            {
                ViewName = "V_BuyDetailsWithQuashedAll";
            }
        }

        public class V_CardPasswordDetails : MSSQL.ViewBase
        {
            public V_CardPasswordDetails()
            {
                ViewName = "V_CardPasswordDetails";
            }
        }

        public class V_ChaseTaskDetails : MSSQL.ViewBase
        {
            public V_ChaseTaskDetails()
            {
                ViewName = "V_ChaseTaskDetails";
            }
        }

        public class V_ChaseTasks : MSSQL.ViewBase
        {
            public V_ChaseTasks()
            {
                ViewName = "V_ChaseTasks";
            }
        }

        public class V_ChaseTasksTotal : MSSQL.ViewBase
        {
            public V_ChaseTasksTotal()
            {
                ViewName = "V_ChaseTasksTotal";
            }
        }

        public class V_Citys : MSSQL.ViewBase
        {
            public V_Citys()
            {
                ViewName = "V_Citys";
            }
        }

        public class V_Cps : MSSQL.ViewBase
        {
            public V_Cps()
            {
                ViewName = "V_Cps";
            }
        }

        public class V_CpsTrys : MSSQL.ViewBase
        {
            public V_CpsTrys()
            {
                ViewName = "V_CpsTrys";
            }
        }

        public class V_CpsWithTransactionMoney : MSSQL.ViewBase
        {
            public V_CpsWithTransactionMoney()
            {
                ViewName = "V_CpsWithTransactionMoney";
            }
        }

        public class V_CustomFollowSchemes : MSSQL.ViewBase
        {
            public V_CustomFollowSchemes()
            {
                ViewName = "V_CustomFollowSchemes";
            }
        }

        public class V_ElectronTicketAgentSchemes : MSSQL.ViewBase
        {
            public V_ElectronTicketAgentSchemes()
            {
                ViewName = "V_ElectronTicketAgentSchemes";
            }
        }

        public class V_ElectronTicketAgentSchemesElectronTickets : MSSQL.ViewBase
        {
            public V_ElectronTicketAgentSchemesElectronTickets()
            {
                ViewName = "V_ElectronTicketAgentSchemesElectronTickets";
            }
        }

        public class V_ElectronTicketAgentSchemesSendToCenter : MSSQL.ViewBase
        {
            public V_ElectronTicketAgentSchemesSendToCenter()
            {
                ViewName = "V_ElectronTicketAgentSchemesSendToCenter";
            }
        }

        public class V_Experts : MSSQL.ViewBase
        {
            public V_Experts()
            {
                ViewName = "V_Experts";
            }
        }

        public class V_ExpertsCommends : MSSQL.ViewBase
        {
            public V_ExpertsCommends()
            {
                ViewName = "V_ExpertsCommends";
            }
        }

        public class V_ExpertsPredictNews : MSSQL.ViewBase
        {
            public V_ExpertsPredictNews()
            {
                ViewName = "V_ExpertsPredictNews";
            }
        }

        public class V_ExpertsTrys : MSSQL.ViewBase
        {
            public V_ExpertsTrys()
            {
                ViewName = "V_ExpertsTrys";
            }
        }

        public class V_FullSchemesCount : MSSQL.ViewBase
        {
            public V_FullSchemesCount()
            {
                ViewName = "V_FullSchemesCount";
            }
        }

        public class V_GetDate : MSSQL.ViewBase
        {
            public V_GetDate()
            {
                ViewName = "V_GetDate";
            }
        }

        public class V_IsuseForZCDC : MSSQL.ViewBase
        {
            public V_IsuseForZCDC()
            {
                ViewName = "V_IsuseForZCDC";
            }
        }

        public class V_Isuses : MSSQL.ViewBase
        {
            public V_Isuses()
            {
                ViewName = "V_Isuses";
            }
        }

        public class v_Isuses_JXSSC : MSSQL.ViewBase
        {
            public v_Isuses_JXSSC()
            {
                ViewName = "v_Isuses_JXSSC";
            }
        }

        public class v_Isuses_SYYDJ : MSSQL.ViewBase
        {
            public v_Isuses_SYYDJ()
            {
                ViewName = "v_Isuses_SYYDJ";
            }
        }

        public class V_News : MSSQL.ViewBase
        {
            public V_News()
            {
                ViewName = "V_News";
            }
        }

        public class V_NPIsuses : MSSQL.ViewBase
        {
            public V_NPIsuses()
            {
                ViewName = "V_NPIsuses";
            }
        }

        public class V_PlayTypes : MSSQL.ViewBase
        {
            public V_PlayTypes()
            {
                ViewName = "V_PlayTypes";
            }
        }

        public class V_Questions : MSSQL.ViewBase
        {
            public V_Questions()
            {
                ViewName = "V_Questions";
            }
        }

        public class V_SchemeChatContents : MSSQL.ViewBase
        {
            public V_SchemeChatContents()
            {
                ViewName = "V_SchemeChatContents";
            }
        }

        public class V_SchemeCount : MSSQL.ViewBase
        {
            public V_SchemeCount()
            {
                ViewName = "V_SchemeCount";
            }
        }

        public class V_SchemeEndTime : MSSQL.ViewBase
        {
            public V_SchemeEndTime()
            {
                ViewName = "V_SchemeEndTime";
            }
        }

        public class V_SchemeForZCDC : MSSQL.ViewBase
        {
            public V_SchemeForZCDC()
            {
                ViewName = "V_SchemeForZCDC";
            }
        }

        public class V_Schemes : MSSQL.ViewBase
        {
            public V_Schemes()
            {
                ViewName = "V_Schemes";
            }
        }

        public class V_SchemeSchedules : MSSQL.ViewBase
        {
            public V_SchemeSchedules()
            {
                ViewName = "V_SchemeSchedules";
            }
        }

        public class V_SchemeSchedulesWithQuashed : MSSQL.ViewBase
        {
            public V_SchemeSchedulesWithQuashed()
            {
                ViewName = "V_SchemeSchedulesWithQuashed";
            }
        }

        public class V_SchemesSendToCenter : MSSQL.ViewBase
        {
            public V_SchemesSendToCenter()
            {
                ViewName = "V_SchemesSendToCenter";
            }
        }

        public class V_ScoreCommodities : MSSQL.ViewBase
        {
            public V_ScoreCommodities()
            {
                ViewName = "V_ScoreCommodities";
            }
        }

        public class V_SiteSendNotificationTypes : MSSQL.ViewBase
        {
            public V_SiteSendNotificationTypes()
            {
                ViewName = "V_SiteSendNotificationTypes";
            }
        }

        public class V_StationSMS : MSSQL.ViewBase
        {
            public V_StationSMS()
            {
                ViewName = "V_StationSMS";
            }
        }

        public class V_SurrogateTrys : MSSQL.ViewBase
        {
            public V_SurrogateTrys()
            {
                ViewName = "V_SurrogateTrys";
            }
        }

        public class V_SystemLog : MSSQL.ViewBase
        {
            public V_SystemLog()
            {
                ViewName = "V_SystemLog";
            }
        }

        public class V_UserAcceptNotificationTypes : MSSQL.ViewBase
        {
            public V_UserAcceptNotificationTypes()
            {
                ViewName = "V_UserAcceptNotificationTypes";
            }
        }

        public class V_UserActions : MSSQL.ViewBase
        {
            public V_UserActions()
            {
                ViewName = "V_UserActions";
            }
        }

        public class V_UserDetails : MSSQL.ViewBase
        {
            public V_UserDetails()
            {
                ViewName = "V_UserDetails";
            }
        }

        public class V_UserDetailsWithSchemes : MSSQL.ViewBase
        {
            public V_UserDetailsWithSchemes()
            {
                ViewName = "V_UserDetailsWithSchemes";
            }
        }

        public class V_UserDistills : MSSQL.ViewBase
        {
            public V_UserDistills()
            {
                ViewName = "V_UserDistills";
            }
        }

        public class V_UserForInitiateFollowSchemeTrys : MSSQL.ViewBase
        {
            public V_UserForInitiateFollowSchemeTrys()
            {
                ViewName = "V_UserForInitiateFollowSchemeTrys";
            }
        }

        public class V_UserInGroups : MSSQL.ViewBase
        {
            public V_UserInGroups()
            {
                ViewName = "V_UserInGroups";
            }
        }

        public class V_UserPayDetails : MSSQL.ViewBase
        {
            public V_UserPayDetails()
            {
                ViewName = "V_UserPayDetails";
            }
        }

        public class V_Users : MSSQL.ViewBase
        {
            public V_Users()
            {
                ViewName = "V_Users";
            }
        }

        public class V_UserScoringDetails : MSSQL.ViewBase
        {
            public V_UserScoringDetails()
            {
                ViewName = "V_UserScoringDetails";
            }
        }

        public class V_UsersForInitiateFollowScheme : MSSQL.ViewBase
        {
            public V_UsersForInitiateFollowScheme()
            {
                ViewName = "V_UsersForInitiateFollowScheme";
            }
        }

        public class V_UsersWithSumWinMoney : MSSQL.ViewBase
        {
            public V_UsersWithSumWinMoney()
            {
                ViewName = "V_UsersWithSumWinMoney";
            }
        }

        public class V_UsersWithSumWinMoneyThisWeek : MSSQL.ViewBase
        {
            public V_UsersWithSumWinMoneyThisWeek()
            {
                ViewName = "V_UsersWithSumWinMoneyThisWeek";
            }
        }

        public class V_UsersWithSumWinMoneyToday : MSSQL.ViewBase
        {
            public V_UsersWithSumWinMoneyToday()
            {
                ViewName = "V_UsersWithSumWinMoneyToday";
            }
        }
    }

    public class Functions
    {
        public static int F_AccumulateMember(SqlConnection conn, long SiteID, long UserID, DateTime StartTime, DateTime EndTime, int type)
        {
            object Result = MSSQL.ExecuteFunction(conn, "F_AccumulateMember",
                new MSSQL.Parameter("SiteID", SqlDbType.BigInt, 0, ParameterDirection.Input, SiteID),
                new MSSQL.Parameter("UserID", SqlDbType.BigInt, 0, ParameterDirection.Input, UserID),
                new MSSQL.Parameter("StartTime", SqlDbType.DateTime, 0, ParameterDirection.Input, StartTime),
                new MSSQL.Parameter("EndTime", SqlDbType.DateTime, 0, ParameterDirection.Input, EndTime),
                new MSSQL.Parameter("type", SqlDbType.Int, 0, ParameterDirection.Input, type)
                );

            return System.Convert.ToInt32(Result);
        }

        public static double F_CalculationFollowSchemesMoney(SqlConnection conn, double RemainingMoney, int RemainingShare, double FollowSchemesMoney)
        {
            object Result = MSSQL.ExecuteFunction(conn, "F_CalculationFollowSchemesMoney",
                new MSSQL.Parameter("RemainingMoney", SqlDbType.Money, 0, ParameterDirection.Input, RemainingMoney),
                new MSSQL.Parameter("RemainingShare", SqlDbType.Int, 0, ParameterDirection.Input, RemainingShare),
                new MSSQL.Parameter("FollowSchemesMoney", SqlDbType.Money, 0, ParameterDirection.Input, FollowSchemesMoney)
                );

            return System.Convert.ToDouble(Result);
        }

        public static bool F_ComparisonLotteryList(SqlConnection conn, string ParentLotteryList, string SubLotteryList)
        {
            object Result = MSSQL.ExecuteFunction(conn, "F_ComparisonLotteryList",
                new MSSQL.Parameter("ParentLotteryList", SqlDbType.VarChar, 0, ParameterDirection.Input, ParentLotteryList),
                new MSSQL.Parameter("SubLotteryList", SqlDbType.VarChar, 0, ParameterDirection.Input, SubLotteryList)
                );

            return System.Convert.ToBoolean(Result);
        }

        public static double F_CpsGetCommenderPromoteCpsBonusScale(SqlConnection conn, long OwnerUserID, long LotteryID)
        {
            object Result = MSSQL.ExecuteFunction(conn, "F_CpsGetCommenderPromoteCpsBonusScale",
                new MSSQL.Parameter("OwnerUserID", SqlDbType.BigInt, 0, ParameterDirection.Input, OwnerUserID),
                new MSSQL.Parameter("LotteryID", SqlDbType.BigInt, 0, ParameterDirection.Input, LotteryID)
                );

            return System.Convert.ToDouble(Result);
        }

        public static double F_CpsGetCommenderPromoteMemberBonusScale(SqlConnection conn, long OwnerUserID, long LotteryID)
        {
            object Result = MSSQL.ExecuteFunction(conn, "F_CpsGetCommenderPromoteMemberBonusScale",
                new MSSQL.Parameter("OwnerUserID", SqlDbType.BigInt, 0, ParameterDirection.Input, OwnerUserID),
                new MSSQL.Parameter("LotteryID", SqlDbType.BigInt, 0, ParameterDirection.Input, LotteryID)
                );

            return System.Convert.ToDouble(Result);
        }

        public static double F_CpsGetCpsBonusScale(SqlConnection conn, long CpsID, long LotteryID)
        {
            object Result = MSSQL.ExecuteFunction(conn, "F_CpsGetCpsBonusScale",
                new MSSQL.Parameter("CpsID", SqlDbType.BigInt, 0, ParameterDirection.Input, CpsID),
                new MSSQL.Parameter("LotteryID", SqlDbType.BigInt, 0, ParameterDirection.Input, LotteryID)
                );

            return System.Convert.ToDouble(Result);
        }

        public static int F_CpsMemberAccumulateBuyerMember(SqlConnection conn, long SiteID, long UserID)
        {
            object Result = MSSQL.ExecuteFunction(conn, "F_CpsMemberAccumulateBuyerMember",
                new MSSQL.Parameter("SiteID", SqlDbType.BigInt, 0, ParameterDirection.Input, SiteID),
                new MSSQL.Parameter("UserID", SqlDbType.BigInt, 0, ParameterDirection.Input, UserID)
                );

            return System.Convert.ToInt32(Result);
        }

        public static int F_CpsMemberAccumulateWebSite(SqlConnection conn, long SiteID, long UserID)
        {
            object Result = MSSQL.ExecuteFunction(conn, "F_CpsMemberAccumulateWebSite",
                new MSSQL.Parameter("SiteID", SqlDbType.BigInt, 0, ParameterDirection.Input, SiteID),
                new MSSQL.Parameter("UserID", SqlDbType.BigInt, 0, ParameterDirection.Input, UserID)
                );

            return System.Convert.ToInt32(Result);
        }

        public static int F_CurrentDateRegMember(SqlConnection conn, long SiteID, long UserID, DateTime StartTime, DateTime EndTime, int type)
        {
            object Result = MSSQL.ExecuteFunction(conn, "F_CurrentDateRegMember",
                new MSSQL.Parameter("SiteID", SqlDbType.BigInt, 0, ParameterDirection.Input, SiteID),
                new MSSQL.Parameter("UserID", SqlDbType.BigInt, 0, ParameterDirection.Input, UserID),
                new MSSQL.Parameter("StartTime", SqlDbType.DateTime, 0, ParameterDirection.Input, StartTime),
                new MSSQL.Parameter("EndTime", SqlDbType.DateTime, 0, ParameterDirection.Input, EndTime),
                new MSSQL.Parameter("type", SqlDbType.Int, 0, ParameterDirection.Input, type)
                );

            return System.Convert.ToInt32(Result);
        }

        public static double F_CurrentDateRegMemberPayMoney(SqlConnection conn, long SiteID, long UserID, DateTime CurrentDate, int type)
        {
            object Result = MSSQL.ExecuteFunction(conn, "F_CurrentDateRegMemberPayMoney",
                new MSSQL.Parameter("SiteID", SqlDbType.BigInt, 0, ParameterDirection.Input, SiteID),
                new MSSQL.Parameter("UserID", SqlDbType.BigInt, 0, ParameterDirection.Input, UserID),
                new MSSQL.Parameter("CurrentDate", SqlDbType.DateTime, 0, ParameterDirection.Input, CurrentDate),
                new MSSQL.Parameter("type", SqlDbType.Int, 0, ParameterDirection.Input, type)
                );

            return System.Convert.ToDouble(Result);
        }

        public static double F_CurrentDateRegMemberPayMoneyBonusScale(SqlConnection conn, long SiteID, long UserID, DateTime CurrentDate, int type)
        {
            object Result = MSSQL.ExecuteFunction(conn, "F_CurrentDateRegMemberPayMoneyBonusScale",
                new MSSQL.Parameter("SiteID", SqlDbType.BigInt, 0, ParameterDirection.Input, SiteID),
                new MSSQL.Parameter("UserID", SqlDbType.BigInt, 0, ParameterDirection.Input, UserID),
                new MSSQL.Parameter("CurrentDate", SqlDbType.DateTime, 0, ParameterDirection.Input, CurrentDate),
                new MSSQL.Parameter("type", SqlDbType.Int, 0, ParameterDirection.Input, type)
                );

            return System.Convert.ToDouble(Result);
        }

        public static double F_CurrentDateRegMemberPayMoneyBonusScale_all(SqlConnection conn, long SiteID, long UserID, DateTime CurrentDate)
        {
            object Result = MSSQL.ExecuteFunction(conn, "F_CurrentDateRegMemberPayMoneyBonusScale_all",
                new MSSQL.Parameter("SiteID", SqlDbType.BigInt, 0, ParameterDirection.Input, SiteID),
                new MSSQL.Parameter("UserID", SqlDbType.BigInt, 0, ParameterDirection.Input, UserID),
                new MSSQL.Parameter("CurrentDate", SqlDbType.DateTime, 0, ParameterDirection.Input, CurrentDate)
                );

            return System.Convert.ToDouble(Result);
        }

        public static double F_CurrentDateRegMemberPayMoneyBonusScale_today(SqlConnection conn, long SiteID, long UserID, DateTime CurrentDate)
        {
            object Result = MSSQL.ExecuteFunction(conn, "F_CurrentDateRegMemberPayMoneyBonusScale_today",
                new MSSQL.Parameter("SiteID", SqlDbType.BigInt, 0, ParameterDirection.Input, SiteID),
                new MSSQL.Parameter("UserID", SqlDbType.BigInt, 0, ParameterDirection.Input, UserID),
                new MSSQL.Parameter("CurrentDate", SqlDbType.DateTime, 0, ParameterDirection.Input, CurrentDate)
                );

            return System.Convert.ToDouble(Result);
        }

        public static double F_CurrentDateRegMemberPayMoneyBonusScaleSite(SqlConnection conn, long SiteID, long UserID, DateTime CurrentDate)
        {
            object Result = MSSQL.ExecuteFunction(conn, "F_CurrentDateRegMemberPayMoneyBonusScaleSite",
                new MSSQL.Parameter("SiteID", SqlDbType.BigInt, 0, ParameterDirection.Input, SiteID),
                new MSSQL.Parameter("UserID", SqlDbType.BigInt, 0, ParameterDirection.Input, UserID),
                new MSSQL.Parameter("CurrentDate", SqlDbType.DateTime, 0, ParameterDirection.Input, CurrentDate)
                );

            return System.Convert.ToDouble(Result);
        }

        public static double F_CurrentDateRegMemberPayMoneyBonusScaleSite_all(SqlConnection conn, long SiteID, long UserID, DateTime CurrentDate)
        {
            object Result = MSSQL.ExecuteFunction(conn, "F_CurrentDateRegMemberPayMoneyBonusScaleSite_all",
                new MSSQL.Parameter("SiteID", SqlDbType.BigInt, 0, ParameterDirection.Input, SiteID),
                new MSSQL.Parameter("UserID", SqlDbType.BigInt, 0, ParameterDirection.Input, UserID),
                new MSSQL.Parameter("CurrentDate", SqlDbType.DateTime, 0, ParameterDirection.Input, CurrentDate)
                );

            return System.Convert.ToDouble(Result);
        }

        public static double F_CurrentDateRegMemberPayMoneyBonusScaleSite_today(SqlConnection conn, long SiteID, long UserID, DateTime CurrentDate)
        {
            object Result = MSSQL.ExecuteFunction(conn, "F_CurrentDateRegMemberPayMoneyBonusScaleSite_today",
                new MSSQL.Parameter("SiteID", SqlDbType.BigInt, 0, ParameterDirection.Input, SiteID),
                new MSSQL.Parameter("UserID", SqlDbType.BigInt, 0, ParameterDirection.Input, UserID),
                new MSSQL.Parameter("CurrentDate", SqlDbType.DateTime, 0, ParameterDirection.Input, CurrentDate)
                );

            return System.Convert.ToDouble(Result);
        }

        public static int F_CurrentDateRegPayMember(SqlConnection conn, long SiteID, long UserID, DateTime CurrentDate, int type)
        {
            object Result = MSSQL.ExecuteFunction(conn, "F_CurrentDateRegPayMember",
                new MSSQL.Parameter("SiteID", SqlDbType.BigInt, 0, ParameterDirection.Input, SiteID),
                new MSSQL.Parameter("UserID", SqlDbType.BigInt, 0, ParameterDirection.Input, UserID),
                new MSSQL.Parameter("CurrentDate", SqlDbType.DateTime, 0, ParameterDirection.Input, CurrentDate),
                new MSSQL.Parameter("type", SqlDbType.Int, 0, ParameterDirection.Input, type)
                );

            return System.Convert.ToInt32(Result);
        }

        public static double F_CurrentMonthMemberRecWebSitePayMoney(SqlConnection conn, long SiteID, long UserID, DateTime CurrentDate, int type)
        {
            object Result = MSSQL.ExecuteFunction(conn, "F_CurrentMonthMemberRecWebSitePayMoney",
                new MSSQL.Parameter("SiteID", SqlDbType.BigInt, 0, ParameterDirection.Input, SiteID),
                new MSSQL.Parameter("UserID", SqlDbType.BigInt, 0, ParameterDirection.Input, UserID),
                new MSSQL.Parameter("CurrentDate", SqlDbType.DateTime, 0, ParameterDirection.Input, CurrentDate),
                new MSSQL.Parameter("type", SqlDbType.Int, 0, ParameterDirection.Input, type)
                );

            return System.Convert.ToDouble(Result);
        }

        public static string F_DateTimeToYYMMDD(SqlConnection conn, DateTime Dt)
        {
            object Result = MSSQL.ExecuteFunction(conn, "F_DateTimeToYYMMDD",
                new MSSQL.Parameter("Dt", SqlDbType.DateTime, 0, ParameterDirection.Input, Dt)
                );

            return System.Convert.ToString(Result);
        }

        public static string F_DateTimeToYYMMDDHHMMSS(SqlConnection conn, DateTime Dt)
        {
            object Result = MSSQL.ExecuteFunction(conn, "F_DateTimeToYYMMDDHHMMSS",
                new MSSQL.Parameter("Dt", SqlDbType.DateTime, 0, ParameterDirection.Input, Dt)
                );

            return System.Convert.ToString(Result);
        }

        public static string F_GetBankTypeName(SqlConnection conn, short BankTypeID)
        {
            object Result = MSSQL.ExecuteFunction(conn, "F_GetBankTypeName",
                new MSSQL.Parameter("BankTypeID", SqlDbType.SmallInt, 0, ParameterDirection.Input, BankTypeID)
                );

            return System.Convert.ToString(Result);
        }

        public static double F_GetBonusScaleByCommenderID(SqlConnection conn, long CommenderID)
        {
            object Result = MSSQL.ExecuteFunction(conn, "F_GetBonusScaleByCommenderID",
                new MSSQL.Parameter("CommenderID", SqlDbType.BigInt, 0, ParameterDirection.Input, CommenderID)
                );

            return System.Convert.ToDouble(Result);
        }

        public static short F_GetDetailsOperatorType(SqlConnection conn, string OperatorType)
        {
            object Result = MSSQL.ExecuteFunction(conn, "F_GetDetailsOperatorType",
                new MSSQL.Parameter("OperatorType", SqlDbType.VarChar, 0, ParameterDirection.Input, OperatorType)
                );

            return System.Convert.ToInt16(Result);
        }

        public static string F_GetExpertsLotteryList(SqlConnection conn, long SiteID, long UserID)
        {
            object Result = MSSQL.ExecuteFunction(conn, "F_GetExpertsLotteryList",
                new MSSQL.Parameter("SiteID", SqlDbType.BigInt, 0, ParameterDirection.Input, SiteID),
                new MSSQL.Parameter("UserID", SqlDbType.BigInt, 0, ParameterDirection.Input, UserID)
                );

            return System.Convert.ToString(Result);
        }

        public static bool F_GetIsAdministrator(SqlConnection conn, long SiteID, long UserID)
        {
            object Result = MSSQL.ExecuteFunction(conn, "F_GetIsAdministrator",
                new MSSQL.Parameter("SiteID", SqlDbType.BigInt, 0, ParameterDirection.Input, SiteID),
                new MSSQL.Parameter("UserID", SqlDbType.BigInt, 0, ParameterDirection.Input, UserID)
                );

            return System.Convert.ToBoolean(Result);
        }

        public static bool F_GetIsSendNotification(SqlConnection conn, long SiteID, short Manner, string NotificationCode, long UserID)
        {
            object Result = MSSQL.ExecuteFunction(conn, "F_GetIsSendNotification",
                new MSSQL.Parameter("SiteID", SqlDbType.BigInt, 0, ParameterDirection.Input, SiteID),
                new MSSQL.Parameter("Manner", SqlDbType.SmallInt, 0, ParameterDirection.Input, Manner),
                new MSSQL.Parameter("NotificationCode", SqlDbType.VarChar, 0, ParameterDirection.Input, NotificationCode),
                new MSSQL.Parameter("UserID", SqlDbType.BigInt, 0, ParameterDirection.Input, UserID)
                );

            return System.Convert.ToBoolean(Result);
        }

        public static DateTime F_GetIsuseChaseExecuteTime(SqlConnection conn, long IsuseID)
        {
            object Result = MSSQL.ExecuteFunction(conn, "F_GetIsuseChaseExecuteTime",
                new MSSQL.Parameter("IsuseID", SqlDbType.BigInt, 0, ParameterDirection.Input, IsuseID)
                );

            return System.Convert.ToDateTime(Result);
        }

        public static string F_GetIsuseCount(SqlConnection conn, int LotteryID)
        {
            object Result = MSSQL.ExecuteFunction(conn, "F_GetIsuseCount",
                new MSSQL.Parameter("LotteryID", SqlDbType.Int, 0, ParameterDirection.Input, LotteryID)
                );

            return System.Convert.ToString(Result);
        }

        public static DateTime F_GetIsuseEndTime(SqlConnection conn, long IsuseID)
        {
            object Result = MSSQL.ExecuteFunction(conn, "F_GetIsuseEndTime",
                new MSSQL.Parameter("IsuseID", SqlDbType.BigInt, 0, ParameterDirection.Input, IsuseID)
                );

            return System.Convert.ToDateTime(Result);
        }

        public static DateTime F_GetIsuseStartTime(SqlConnection conn, long IsuseID)
        {
            object Result = MSSQL.ExecuteFunction(conn, "F_GetIsuseStartTime",
                new MSSQL.Parameter("IsuseID", SqlDbType.BigInt, 0, ParameterDirection.Input, IsuseID)
                );

            return System.Convert.ToDateTime(Result);
        }

        public static DateTime F_GetIsuseSystemEndTime(SqlConnection conn, long IsuseID, int PlayTypeID)
        {
            object Result = MSSQL.ExecuteFunction(conn, "F_GetIsuseSystemEndTime",
                new MSSQL.Parameter("IsuseID", SqlDbType.BigInt, 0, ParameterDirection.Input, IsuseID),
                new MSSQL.Parameter("PlayTypeID", SqlDbType.Int, 0, ParameterDirection.Input, PlayTypeID)
                );

            return System.Convert.ToDateTime(Result);
        }

        public static DataTable F_GetLotteryCanChaseIsuses(SqlConnection conn, int LotteryID, int PlayType)
        {
            return MSSQL.Select(conn, "select * from F_GetLotteryCanChaseIsuses(@LotteryID, @PlayType)",
                new MSSQL.Parameter("LotteryID", SqlDbType.Int, 0, ParameterDirection.Input, LotteryID),
                new MSSQL.Parameter("PlayType", SqlDbType.Int, 0, ParameterDirection.Input, PlayType)
                );
        }

        public static string F_GetLotteryCode(SqlConnection conn, int LotteryID)
        {
            object Result = MSSQL.ExecuteFunction(conn, "F_GetLotteryCode",
                new MSSQL.Parameter("LotteryID", SqlDbType.Int, 0, ParameterDirection.Input, LotteryID)
                );

            return System.Convert.ToString(Result);
        }

        public static string F_GetLotteryIntervalType(SqlConnection conn, int LotteryID)
        {
            object Result = MSSQL.ExecuteFunction(conn, "F_GetLotteryIntervalType",
                new MSSQL.Parameter("LotteryID", SqlDbType.Int, 0, ParameterDirection.Input, LotteryID)
                );

            return System.Convert.ToString(Result);
        }

        public static bool F_GetLotteryIsUsed(SqlConnection conn, long SiteID, int LotteryID)
        {
            object Result = MSSQL.ExecuteFunction(conn, "F_GetLotteryIsUsed",
                new MSSQL.Parameter("SiteID", SqlDbType.BigInt, 0, ParameterDirection.Input, SiteID),
                new MSSQL.Parameter("LotteryID", SqlDbType.Int, 0, ParameterDirection.Input, LotteryID)
                );

            return System.Convert.ToBoolean(Result);
        }

        public static string F_GetLotteryName(SqlConnection conn, int LotteryID)
        {
            object Result = MSSQL.ExecuteFunction(conn, "F_GetLotteryName",
                new MSSQL.Parameter("LotteryID", SqlDbType.Int, 0, ParameterDirection.Input, LotteryID)
                );

            return System.Convert.ToString(Result);
        }

        public static short F_GetLotteryPrintOutType(SqlConnection conn, int LotteryID)
        {
            object Result = MSSQL.ExecuteFunction(conn, "F_GetLotteryPrintOutType",
                new MSSQL.Parameter("LotteryID", SqlDbType.Int, 0, ParameterDirection.Input, LotteryID)
                );

            return System.Convert.ToInt16(Result);
        }

        public static string F_GetLotteryType2Name(SqlConnection conn, short Type2)
        {
            object Result = MSSQL.ExecuteFunction(conn, "F_GetLotteryType2Name",
                new MSSQL.Parameter("Type2", SqlDbType.SmallInt, 0, ParameterDirection.Input, Type2)
                );

            return System.Convert.ToString(Result);
        }

        public static string F_GetLotteryWinNumberExemple(SqlConnection conn, int LotteryID)
        {
            object Result = MSSQL.ExecuteFunction(conn, "F_GetLotteryWinNumberExemple",
                new MSSQL.Parameter("LotteryID", SqlDbType.Int, 0, ParameterDirection.Input, LotteryID)
                );

            return System.Convert.ToString(Result);
        }

        public static DataTable F_GetManagers(SqlConnection conn, long SiteID)
        {
            return MSSQL.Select(conn, "select * from F_GetManagers(@SiteID)",
                new MSSQL.Parameter("SiteID", SqlDbType.BigInt, 0, ParameterDirection.Input, SiteID)
                );
        }

        public static long F_GetMasterSiteID(SqlConnection conn)
        {
            object Result = MSSQL.ExecuteFunction(conn, "F_GetMasterSiteID");

            return System.Convert.ToInt64(Result);
        }

        public static int F_GetMaxMultiple(SqlConnection conn, long IsuseID, int PlayTypeID)
        {
            object Result = MSSQL.ExecuteFunction(conn, "F_GetMaxMultiple",
                new MSSQL.Parameter("IsuseID", SqlDbType.BigInt, 0, ParameterDirection.Input, IsuseID),
                new MSSQL.Parameter("PlayTypeID", SqlDbType.Int, 0, ParameterDirection.Input, PlayTypeID)
                );

            return System.Convert.ToInt32(Result);
        }

        public static string F_GetOptions(SqlConnection conn, string Key)
        {
            object Result = MSSQL.ExecuteFunction(conn, "F_GetOptions",
                new MSSQL.Parameter("Key", SqlDbType.VarChar, 0, ParameterDirection.Input, Key)
                );

            return System.Convert.ToString(Result);
        }

        public static string F_GetPlaceFromIPAddress(SqlConnection conn, string IPAddress)
        {
            object Result = MSSQL.ExecuteFunction(conn, "F_GetPlaceFromIPAddress",
                new MSSQL.Parameter("IPAddress", SqlDbType.VarChar, 0, ParameterDirection.Input, IPAddress)
                );

            return System.Convert.ToString(Result);
        }

        public static string F_GetPlayTypeName(SqlConnection conn, int PlayTypeID)
        {
            object Result = MSSQL.ExecuteFunction(conn, "F_GetPlayTypeName",
                new MSSQL.Parameter("PlayTypeID", SqlDbType.Int, 0, ParameterDirection.Input, PlayTypeID)
                );

            return System.Convert.ToString(Result);
        }

        public static string F_GetProvinceCity(SqlConnection conn, int CityID)
        {
            object Result = MSSQL.ExecuteFunction(conn, "F_GetProvinceCity",
                new MSSQL.Parameter("CityID", SqlDbType.Int, 0, ParameterDirection.Input, CityID)
                );

            return System.Convert.ToString(Result);
        }

        public static long F_GetSchemeInitiateUserID(SqlConnection conn, long SiteID, long SchemeID)
        {
            object Result = MSSQL.ExecuteFunction(conn, "F_GetSchemeInitiateUserID",
                new MSSQL.Parameter("SiteID", SqlDbType.BigInt, 0, ParameterDirection.Input, SiteID),
                new MSSQL.Parameter("SchemeID", SqlDbType.BigInt, 0, ParameterDirection.Input, SchemeID)
                );

            return System.Convert.ToInt64(Result);
        }

        public static string F_GetSchemeOpenUsers(SqlConnection conn, long SchemeID)
        {
            object Result = MSSQL.ExecuteFunction(conn, "F_GetSchemeOpenUsers",
                new MSSQL.Parameter("SchemeID", SqlDbType.BigInt, 0, ParameterDirection.Input, SchemeID)
                );

            return System.Convert.ToString(Result);
        }

        public static short F_GetScoringDetailsOperatorType(SqlConnection conn, string OperatorType)
        {
            object Result = MSSQL.ExecuteFunction(conn, "F_GetScoringDetailsOperatorType",
                new MSSQL.Parameter("OperatorType", SqlDbType.VarChar, 0, ParameterDirection.Input, OperatorType)
                );

            return System.Convert.ToInt16(Result);
        }

        public static DataTable F_GetSiteAdministrator(SqlConnection conn, long SiteID)
        {
            return MSSQL.Select(conn, "select * from F_GetSiteAdministrator(@SiteID)",
                new MSSQL.Parameter("SiteID", SqlDbType.BigInt, 0, ParameterDirection.Input, SiteID)
                );
        }

        public static DataTable F_GetSiteAdministrators(SqlConnection conn, long SiteID)
        {
            return MSSQL.Select(conn, "select * from F_GetSiteAdministrators(@SiteID)",
                new MSSQL.Parameter("SiteID", SqlDbType.BigInt, 0, ParameterDirection.Input, SiteID)
                );
        }

        public static long F_GetSiteOwnerUserID(SqlConnection conn, long SiteID)
        {
            object Result = MSSQL.ExecuteFunction(conn, "F_GetSiteOwnerUserID",
                new MSSQL.Parameter("SiteID", SqlDbType.BigInt, 0, ParameterDirection.Input, SiteID)
                );

            return System.Convert.ToInt64(Result);
        }

        public static long F_GetSiteParentID(SqlConnection conn, long SiteID)
        {
            object Result = MSSQL.ExecuteFunction(conn, "F_GetSiteParentID",
                new MSSQL.Parameter("SiteID", SqlDbType.BigInt, 0, ParameterDirection.Input, SiteID)
                );

            return System.Convert.ToInt64(Result);
        }

        public static string F_GetSiteSendNotificationTypes(SqlConnection conn, long SiteID, short Manner)
        {
            object Result = MSSQL.ExecuteFunction(conn, "F_GetSiteSendNotificationTypes",
                new MSSQL.Parameter("SiteID", SqlDbType.BigInt, 0, ParameterDirection.Input, SiteID),
                new MSSQL.Parameter("Manner", SqlDbType.SmallInt, 0, ParameterDirection.Input, Manner)
                );

            return System.Convert.ToString(Result);
        }

        public static string F_GetSiteUrls(SqlConnection conn, long SiteID)
        {
            object Result = MSSQL.ExecuteFunction(conn, "F_GetSiteUrls",
                new MSSQL.Parameter("SiteID", SqlDbType.BigInt, 0, ParameterDirection.Input, SiteID)
                );

            return System.Convert.ToString(Result);
        }

        public static int F_GetSystemEndAheadMinute(SqlConnection conn, int PlayTypeID)
        {
            object Result = MSSQL.ExecuteFunction(conn, "F_GetSystemEndAheadMinute",
                new MSSQL.Parameter("PlayTypeID", SqlDbType.Int, 0, ParameterDirection.Input, PlayTypeID)
                );

            return System.Convert.ToInt32(Result);
        }

        public static string F_GetUsedLotteryList(SqlConnection conn, long SiteID)
        {
            object Result = MSSQL.ExecuteFunction(conn, "F_GetUsedLotteryList",
                new MSSQL.Parameter("SiteID", SqlDbType.BigInt, 0, ParameterDirection.Input, SiteID)
                );

            return System.Convert.ToString(Result);
        }

        public static string F_GetUsedLotteryListQuickBuy(SqlConnection conn, long SiteID)
        {
            object Result = MSSQL.ExecuteFunction(conn, "F_GetUsedLotteryListQuickBuy",
                new MSSQL.Parameter("SiteID", SqlDbType.BigInt, 0, ParameterDirection.Input, SiteID)
                );

            return System.Convert.ToString(Result);
        }

        public static string F_GetUsedLotteryListRestrictions(SqlConnection conn, long SiteID)
        {
            object Result = MSSQL.ExecuteFunction(conn, "F_GetUsedLotteryListRestrictions",
                new MSSQL.Parameter("SiteID", SqlDbType.BigInt, 0, ParameterDirection.Input, SiteID)
                );

            return System.Convert.ToString(Result);
        }

        public static string F_GetUsedLotteryNameList(SqlConnection conn, long SiteID)
        {
            object Result = MSSQL.ExecuteFunction(conn, "F_GetUsedLotteryNameList",
                new MSSQL.Parameter("SiteID", SqlDbType.BigInt, 0, ParameterDirection.Input, SiteID)
                );

            return System.Convert.ToString(Result);
        }

        public static string F_GetUsedLotteryNameListQuickBuy(SqlConnection conn, long SiteID)
        {
            object Result = MSSQL.ExecuteFunction(conn, "F_GetUsedLotteryNameListQuickBuy",
                new MSSQL.Parameter("SiteID", SqlDbType.BigInt, 0, ParameterDirection.Input, SiteID)
                );

            return System.Convert.ToString(Result);
        }

        public static string F_GetUsedLotteryNameListRestrictions(SqlConnection conn, long SiteID)
        {
            object Result = MSSQL.ExecuteFunction(conn, "F_GetUsedLotteryNameListRestrictions",
                new MSSQL.Parameter("SiteID", SqlDbType.BigInt, 0, ParameterDirection.Input, SiteID)
                );

            return System.Convert.ToString(Result);
        }

        public static string F_GetUserAcceptNotificationTypes(SqlConnection conn, long UserID, short Manner)
        {
            object Result = MSSQL.ExecuteFunction(conn, "F_GetUserAcceptNotificationTypes",
                new MSSQL.Parameter("UserID", SqlDbType.BigInt, 0, ParameterDirection.Input, UserID),
                new MSSQL.Parameter("Manner", SqlDbType.SmallInt, 0, ParameterDirection.Input, Manner)
                );

            return System.Convert.ToString(Result);
        }

        public static long F_GetUserCommenderID(SqlConnection conn, long UserID)
        {
            object Result = MSSQL.ExecuteFunction(conn, "F_GetUserCommenderID",
                new MSSQL.Parameter("UserID", SqlDbType.BigInt, 0, ParameterDirection.Input, UserID)
                );

            return System.Convert.ToInt64(Result);
        }

        public static string F_GetUserCompetencesList(SqlConnection conn, long UserID)
        {
            object Result = MSSQL.ExecuteFunction(conn, "F_GetUserCompetencesList",
                new MSSQL.Parameter("UserID", SqlDbType.BigInt, 0, ParameterDirection.Input, UserID)
                );

            return System.Convert.ToString(Result);
        }

        public static long F_GetUserIDByName(SqlConnection conn, long SiteID, string Name)
        {
            object Result = MSSQL.ExecuteFunction(conn, "F_GetUserIDByName",
                new MSSQL.Parameter("SiteID", SqlDbType.BigInt, 0, ParameterDirection.Input, SiteID),
                new MSSQL.Parameter("Name", SqlDbType.VarChar, 0, ParameterDirection.Input, Name)
                );

            return System.Convert.ToInt64(Result);
        }

        public static string F_GetUserNameByID(SqlConnection conn, long SiteID, long ID)
        {
            object Result = MSSQL.ExecuteFunction(conn, "F_GetUserNameByID",
                new MSSQL.Parameter("SiteID", SqlDbType.BigInt, 0, ParameterDirection.Input, SiteID),
                new MSSQL.Parameter("ID", SqlDbType.BigInt, 0, ParameterDirection.Input, ID)
                );

            return System.Convert.ToString(Result);
        }

        public static string F_GetUserOwnerSitesList(SqlConnection conn, long UserID)
        {
            object Result = MSSQL.ExecuteFunction(conn, "F_GetUserOwnerSitesList",
                new MSSQL.Parameter("UserID", SqlDbType.BigInt, 0, ParameterDirection.Input, UserID)
                );

            return System.Convert.ToString(Result);
        }

        public static DataTable F_GetWinLotteryNumber(SqlConnection conn, long SiteID, int LotteryID)
        {
            return MSSQL.Select(conn, "select * from F_GetWinLotteryNumber(@SiteID, @LotteryID)",
                new MSSQL.Parameter("SiteID", SqlDbType.BigInt, 0, ParameterDirection.Input, SiteID),
                new MSSQL.Parameter("LotteryID", SqlDbType.Int, 0, ParameterDirection.Input, LotteryID)
                );
        }

        public static long F_IPAddressToInt64(SqlConnection conn, string IPAddress)
        {
            object Result = MSSQL.ExecuteFunction(conn, "F_IPAddressToInt64",
                new MSSQL.Parameter("IPAddress", SqlDbType.VarChar, 0, ParameterDirection.Input, IPAddress)
                );

            return System.Convert.ToInt64(Result);
        }

        public static bool F_IsDivisibility(SqlConnection conn, double Dividend, double Divisor)
        {
            object Result = MSSQL.ExecuteFunction(conn, "F_IsDivisibility",
                new MSSQL.Parameter("Dividend", SqlDbType.Float, 0, ParameterDirection.Input, Dividend),
                new MSSQL.Parameter("Divisor", SqlDbType.Float, 0, ParameterDirection.Input, Divisor)
                );

            return System.Convert.ToBoolean(Result);
        }

        public static double F_MonthPayMoneyShopBonusScale(SqlConnection conn, long SiteID, long UserID, DateTime StartTime, DateTime EndTime, int type)
        {
            object Result = MSSQL.ExecuteFunction(conn, "F_MonthPayMoneyShopBonusScale",
                new MSSQL.Parameter("SiteID", SqlDbType.BigInt, 0, ParameterDirection.Input, SiteID),
                new MSSQL.Parameter("UserID", SqlDbType.BigInt, 0, ParameterDirection.Input, UserID),
                new MSSQL.Parameter("StartTime", SqlDbType.DateTime, 0, ParameterDirection.Input, StartTime),
                new MSSQL.Parameter("EndTime", SqlDbType.DateTime, 0, ParameterDirection.Input, EndTime),
                new MSSQL.Parameter("type", SqlDbType.Int, 0, ParameterDirection.Input, type)
                );

            return System.Convert.ToDouble(Result);
        }

        public static double F_MonthShopPayMoney(SqlConnection conn, long SiteID, long UserID, DateTime StartTime, DateTime EndTime, int type)
        {
            object Result = MSSQL.ExecuteFunction(conn, "F_MonthShopPayMoney",
                new MSSQL.Parameter("SiteID", SqlDbType.BigInt, 0, ParameterDirection.Input, SiteID),
                new MSSQL.Parameter("UserID", SqlDbType.BigInt, 0, ParameterDirection.Input, UserID),
                new MSSQL.Parameter("StartTime", SqlDbType.DateTime, 0, ParameterDirection.Input, StartTime),
                new MSSQL.Parameter("EndTime", SqlDbType.DateTime, 0, ParameterDirection.Input, EndTime),
                new MSSQL.Parameter("type", SqlDbType.Int, 0, ParameterDirection.Input, type)
                );

            return System.Convert.ToDouble(Result);
        }

        public static string F_PadLeft(SqlConnection conn, string str, string FillChar, int Len)
        {
            object Result = MSSQL.ExecuteFunction(conn, "F_PadLeft",
                new MSSQL.Parameter("str", SqlDbType.VarChar, 0, ParameterDirection.Input, str),
                new MSSQL.Parameter("FillChar", SqlDbType.Char, 0, ParameterDirection.Input, FillChar),
                new MSSQL.Parameter("Len", SqlDbType.Int, 0, ParameterDirection.Input, Len)
                );

            return System.Convert.ToString(Result);
        }

        public static DataTable F_SplitString(SqlConnection conn, string SplitString, string Separator)
        {
            return MSSQL.Select(conn, "select * from F_SplitString(@SplitString, @Separator)",
                new MSSQL.Parameter("SplitString", SqlDbType.VarChar, 0, ParameterDirection.Input, SplitString),
                new MSSQL.Parameter("Separator", SqlDbType.VarChar, 0, ParameterDirection.Input, Separator)
                );
        }

        public static double F_UnionSitePayMoney(SqlConnection conn, long SiteID, long UserID, DateTime StartTime, DateTime EndTime)
        {
            object Result = MSSQL.ExecuteFunction(conn, "F_UnionSitePayMoney",
                new MSSQL.Parameter("SiteID", SqlDbType.BigInt, 0, ParameterDirection.Input, SiteID),
                new MSSQL.Parameter("UserID", SqlDbType.BigInt, 0, ParameterDirection.Input, UserID),
                new MSSQL.Parameter("StartTime", SqlDbType.DateTime, 0, ParameterDirection.Input, StartTime),
                new MSSQL.Parameter("EndTime", SqlDbType.DateTime, 0, ParameterDirection.Input, EndTime)
                );

            return System.Convert.ToDouble(Result);
        }

        public static double F_WebSitePayMoney(SqlConnection conn, long SiteID, long UserID, DateTime StartDate, DateTime EndDate, int type)
        {
            object Result = MSSQL.ExecuteFunction(conn, "F_WebSitePayMoney",
                new MSSQL.Parameter("SiteID", SqlDbType.BigInt, 0, ParameterDirection.Input, SiteID),
                new MSSQL.Parameter("UserID", SqlDbType.BigInt, 0, ParameterDirection.Input, UserID),
                new MSSQL.Parameter("StartDate", SqlDbType.DateTime, 0, ParameterDirection.Input, StartDate),
                new MSSQL.Parameter("EndDate", SqlDbType.DateTime, 0, ParameterDirection.Input, EndDate),
                new MSSQL.Parameter("type", SqlDbType.Int, 0, ParameterDirection.Input, type)
                );

            return System.Convert.ToDouble(Result);
        }
    }

    public class Procedures
    {
        public static int P_AcceptUserHongbaoPromotion(SqlConnection conn, long FromUserID, long ToUserID, long UserHongbaoPromotionID, ref int ReturnValue, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_AcceptUserHongbaoPromotion(conn, ref ds, FromUserID, ToUserID, UserHongbaoPromotionID, ref ReturnValue, ref ReturnDescription);
        }

        public static int P_AcceptUserHongbaoPromotion(SqlConnection conn, ref DataSet ds, long FromUserID, long ToUserID, long UserHongbaoPromotionID, ref int ReturnValue, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(conn, "P_AcceptUserHongbaoPromotion", ref ds, ref Outputs,
                new MSSQL.Parameter("FromUserID", SqlDbType.BigInt, 0, ParameterDirection.Input, FromUserID),
                new MSSQL.Parameter("ToUserID", SqlDbType.BigInt, 0, ParameterDirection.Input, ToUserID),
                new MSSQL.Parameter("UserHongbaoPromotionID", SqlDbType.BigInt, 0, ParameterDirection.Input, UserHongbaoPromotionID),
                new MSSQL.Parameter("ReturnValue", SqlDbType.Int, 4, ParameterDirection.Output, ReturnValue),
                new MSSQL.Parameter("ReturnDescription", SqlDbType.VarChar, 100, ParameterDirection.Output, ReturnDescription)
                );

            try
            {
                ReturnValue = System.Convert.ToInt32(Outputs["ReturnValue"]);
            }
            catch { }

            try
            {
                ReturnDescription = System.Convert.ToString(Outputs["ReturnDescription"]);
            }
            catch { }

            return CallResult;
        }

        public static int P_AccountTransfer(SqlConnection conn, long SiteID, long UserID, double Money, long RelatedUserID, string SecurityAnswer, ref int ReturnValue, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_AccountTransfer(conn, ref ds, SiteID, UserID, Money, RelatedUserID, SecurityAnswer, ref ReturnValue, ref ReturnDescription);
        }

        public static int P_AccountTransfer(SqlConnection conn, ref DataSet ds, long SiteID, long UserID, double Money, long RelatedUserID, string SecurityAnswer, ref int ReturnValue, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(conn, "P_AccountTransfer", ref ds, ref Outputs,
                new MSSQL.Parameter("SiteID", SqlDbType.BigInt, 0, ParameterDirection.Input, SiteID),
                new MSSQL.Parameter("UserID", SqlDbType.BigInt, 0, ParameterDirection.Input, UserID),
                new MSSQL.Parameter("Money", SqlDbType.Money, 0, ParameterDirection.Input, Money),
                new MSSQL.Parameter("RelatedUserID", SqlDbType.BigInt, 0, ParameterDirection.Input, RelatedUserID),
                new MSSQL.Parameter("SecurityAnswer", SqlDbType.VarChar, 0, ParameterDirection.Input, SecurityAnswer),
                new MSSQL.Parameter("ReturnValue", SqlDbType.Int, 4, ParameterDirection.Output, ReturnValue),
                new MSSQL.Parameter("ReturnDescription", SqlDbType.VarChar, 100, ParameterDirection.Output, ReturnDescription)
                );

            try
            {
                ReturnValue = System.Convert.ToInt32(Outputs["ReturnValue"]);
            }
            catch { }

            try
            {
                ReturnDescription = System.Convert.ToString(Outputs["ReturnDescription"]);
            }
            catch { }

            return CallResult;
        }

        public static int P_AccountTransferAccept(SqlConnection conn, long SiteID, long UserID, long TransferID, long RelatedUserID, string SecurityAnswer, ref int ReturnValue, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_AccountTransferAccept(conn, ref ds, SiteID, UserID, TransferID, RelatedUserID, SecurityAnswer, ref ReturnValue, ref ReturnDescription);
        }

        public static int P_AccountTransferAccept(SqlConnection conn, ref DataSet ds, long SiteID, long UserID, long TransferID, long RelatedUserID, string SecurityAnswer, ref int ReturnValue, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(conn, "P_AccountTransferAccept", ref ds, ref Outputs,
                new MSSQL.Parameter("SiteID", SqlDbType.BigInt, 0, ParameterDirection.Input, SiteID),
                new MSSQL.Parameter("UserID", SqlDbType.BigInt, 0, ParameterDirection.Input, UserID),
                new MSSQL.Parameter("TransferID", SqlDbType.BigInt, 0, ParameterDirection.Input, TransferID),
                new MSSQL.Parameter("RelatedUserID", SqlDbType.BigInt, 0, ParameterDirection.Input, RelatedUserID),
                new MSSQL.Parameter("SecurityAnswer", SqlDbType.VarChar, 0, ParameterDirection.Input, SecurityAnswer),
                new MSSQL.Parameter("ReturnValue", SqlDbType.Int, 4, ParameterDirection.Output, ReturnValue),
                new MSSQL.Parameter("ReturnDescription", SqlDbType.VarChar, 100, ParameterDirection.Output, ReturnDescription)
                );

            try
            {
                ReturnValue = System.Convert.ToInt32(Outputs["ReturnValue"]);
            }
            catch { }

            try
            {
                ReturnDescription = System.Convert.ToString(Outputs["ReturnDescription"]);
            }
            catch { }

            return CallResult;
        }

        public static int P_AccountTransferQuash(SqlConnection conn, long SiteID, long UserID, long TransferID, long RelatedUserID, ref int ReturnValue, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_AccountTransferQuash(conn, ref ds, SiteID, UserID, TransferID, RelatedUserID, ref ReturnValue, ref ReturnDescription);
        }

        public static int P_AccountTransferQuash(SqlConnection conn, ref DataSet ds, long SiteID, long UserID, long TransferID, long RelatedUserID, ref int ReturnValue, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(conn, "P_AccountTransferQuash", ref ds, ref Outputs,
                new MSSQL.Parameter("SiteID", SqlDbType.BigInt, 0, ParameterDirection.Input, SiteID),
                new MSSQL.Parameter("UserID", SqlDbType.BigInt, 0, ParameterDirection.Input, UserID),
                new MSSQL.Parameter("TransferID", SqlDbType.BigInt, 0, ParameterDirection.Input, TransferID),
                new MSSQL.Parameter("RelatedUserID", SqlDbType.BigInt, 0, ParameterDirection.Input, RelatedUserID),
                new MSSQL.Parameter("ReturnValue", SqlDbType.Int, 4, ParameterDirection.Output, ReturnValue),
                new MSSQL.Parameter("ReturnDescription", SqlDbType.VarChar, 100, ParameterDirection.Output, ReturnDescription)
                );

            try
            {
                ReturnValue = System.Convert.ToInt32(Outputs["ReturnValue"]);
            }
            catch { }

            try
            {
                ReturnDescription = System.Convert.ToString(Outputs["ReturnDescription"]);
            }
            catch { }

            return CallResult;
        }

        public static int P_AddUserToCpsUId(SqlConnection conn, int ID, int Uid, int CpsID, int PID, ref long ReturnValue, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_AddUserToCpsUId(conn, ref ds, ID, Uid, CpsID, PID, ref ReturnValue, ref ReturnDescription);
        }

        public static int P_AddUserToCpsUId(SqlConnection conn, ref DataSet ds, int ID, int Uid, int CpsID, int PID, ref long ReturnValue, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(conn, "P_AddUserToCpsUId", ref ds, ref Outputs,
                new MSSQL.Parameter("ID", SqlDbType.Int, 0, ParameterDirection.Input, ID),
                new MSSQL.Parameter("Uid", SqlDbType.Int, 0, ParameterDirection.Input, Uid),
                new MSSQL.Parameter("CpsID", SqlDbType.Int, 0, ParameterDirection.Input, CpsID),
                new MSSQL.Parameter("PID", SqlDbType.Int, 0, ParameterDirection.Input, PID),
                new MSSQL.Parameter("ReturnValue", SqlDbType.BigInt, 8, ParameterDirection.Output, ReturnValue),
                new MSSQL.Parameter("ReturnDescription", SqlDbType.VarChar, 100, ParameterDirection.Output, ReturnDescription)
                );

            try
            {
                ReturnValue = System.Convert.ToInt64(Outputs["ReturnValue"]);
            }
            catch { }

            try
            {
                ReturnDescription = System.Convert.ToString(Outputs["ReturnDescription"]);
            }
            catch { }

            return CallResult;
        }

        public static int P_AdvertisementsEdit(SqlConnection conn, int ID, string Title, string Url, int Order, bool isShow, ref int ReturnValue, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_AdvertisementsEdit(conn, ref ds, ID, Title, Url, Order, isShow, ref ReturnValue, ref ReturnDescription);
        }

        public static int P_AdvertisementsEdit(SqlConnection conn, ref DataSet ds, int ID, string Title, string Url, int Order, bool isShow, ref int ReturnValue, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(conn, "P_AdvertisementsEdit", ref ds, ref Outputs,
                new MSSQL.Parameter("ID", SqlDbType.Int, 0, ParameterDirection.Input, ID),
                new MSSQL.Parameter("Title", SqlDbType.VarChar, 0, ParameterDirection.Input, Title),
                new MSSQL.Parameter("Url", SqlDbType.VarChar, 0, ParameterDirection.Input, Url),
                new MSSQL.Parameter("Order", SqlDbType.Int, 0, ParameterDirection.Input, Order),
                new MSSQL.Parameter("isShow", SqlDbType.Bit, 0, ParameterDirection.Input, isShow),
                new MSSQL.Parameter("ReturnValue", SqlDbType.Int, 4, ParameterDirection.Output, ReturnValue),
                new MSSQL.Parameter("ReturnDescription", SqlDbType.VarChar, 100, ParameterDirection.Output, ReturnDescription)
                );

            try
            {
                ReturnValue = System.Convert.ToInt32(Outputs["ReturnValue"]);
            }
            catch { }

            try
            {
                ReturnDescription = System.Convert.ToString(Outputs["ReturnDescription"]);
            }
            catch { }

            return CallResult;
        }

        public static int P_Analysis_3D_Miss(SqlConnection conn, ref int ReturnValue, ref string ReturnDescptrion)
        {
            DataSet ds = null;

            return P_Analysis_3D_Miss(conn, ref ds, ref ReturnValue, ref ReturnDescptrion);
        }

        public static int P_Analysis_3D_Miss(SqlConnection conn, ref DataSet ds, ref int ReturnValue, ref string ReturnDescptrion)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(conn, "P_Analysis_3D_Miss", ref ds, ref Outputs,
                new MSSQL.Parameter("ReturnValue", SqlDbType.Int, 4, ParameterDirection.Output, ReturnValue),
                new MSSQL.Parameter("ReturnDescptrion", SqlDbType.NVarChar, 200, ParameterDirection.Output, ReturnDescptrion)
                );

            try
            {
                ReturnValue = System.Convert.ToInt32(Outputs["ReturnValue"]);
            }
            catch { }

            try
            {
                ReturnDescptrion = System.Convert.ToString(Outputs["ReturnDescptrion"]);
            }
            catch { }

            return CallResult;
        }

        public static int P_Analysis_SHSSL_HotAndCoolAndMiss(SqlConnection conn, ref int ReturnValue, ref string ReturnDescptrion)
        {
            DataSet ds = null;

            return P_Analysis_SHSSL_HotAndCoolAndMiss(conn, ref ds, ref ReturnValue, ref ReturnDescptrion);
        }

        public static int P_Analysis_SHSSL_HotAndCoolAndMiss(SqlConnection conn, ref DataSet ds, ref int ReturnValue, ref string ReturnDescptrion)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(conn, "P_Analysis_SHSSL_HotAndCoolAndMiss", ref ds, ref Outputs,
                new MSSQL.Parameter("ReturnValue", SqlDbType.Int, 4, ParameterDirection.Output, ReturnValue),
                new MSSQL.Parameter("ReturnDescptrion", SqlDbType.NVarChar, 200, ParameterDirection.Output, ReturnDescptrion)
                );

            try
            {
                ReturnValue = System.Convert.ToInt32(Outputs["ReturnValue"]);
            }
            catch { }

            try
            {
                ReturnDescptrion = System.Convert.ToString(Outputs["ReturnDescptrion"]);
            }
            catch { }

            return CallResult;
        }

        public static int P_Analysis_SHSSL_WinUsersList(SqlConnection conn, int LotteryID, ref int ReturnValue, ref string ReturnDescptrion)
        {
            DataSet ds = null;

            return P_Analysis_SHSSL_WinUsersList(conn, ref ds, LotteryID, ref ReturnValue, ref ReturnDescptrion);
        }

        public static int P_Analysis_SHSSL_WinUsersList(SqlConnection conn, ref DataSet ds, int LotteryID, ref int ReturnValue, ref string ReturnDescptrion)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(conn, "P_Analysis_SHSSL_WinUsersList", ref ds, ref Outputs,
                new MSSQL.Parameter("LotteryID", SqlDbType.Int, 0, ParameterDirection.Input, LotteryID),
                new MSSQL.Parameter("ReturnValue", SqlDbType.Int, 4, ParameterDirection.Output, ReturnValue),
                new MSSQL.Parameter("ReturnDescptrion", SqlDbType.NVarChar, 200, ParameterDirection.Output, ReturnDescptrion)
                );

            try
            {
                ReturnValue = System.Convert.ToInt32(Outputs["ReturnValue"]);
            }
            catch { }

            try
            {
                ReturnDescptrion = System.Convert.ToString(Outputs["ReturnDescptrion"]);
            }
            catch { }

            return CallResult;
        }

        public static int P_CalculateScore(SqlConnection conn, ref int ReturnValue, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_CalculateScore(conn, ref ds, ref ReturnValue, ref ReturnDescription);
        }

        public static int P_CalculateScore(SqlConnection conn, ref DataSet ds, ref int ReturnValue, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(conn, "P_CalculateScore", ref ds, ref Outputs,
                new MSSQL.Parameter("ReturnValue", SqlDbType.Int, 4, ParameterDirection.Output, ReturnValue),
                new MSSQL.Parameter("ReturnDescription", SqlDbType.VarChar, 100, ParameterDirection.Output, ReturnDescription)
                );

            try
            {
                ReturnValue = System.Convert.ToInt32(Outputs["ReturnValue"]);
            }
            catch { }

            try
            {
                ReturnDescription = System.Convert.ToString(Outputs["ReturnDescription"]);
            }
            catch { }

            return CallResult;
        }

        public static int P_CalculateUserLevel(SqlConnection conn)
        {
            DataSet ds = null;

            return P_CalculateUserLevel(conn, ref ds);
        }

        public static int P_CalculateUserLevel(SqlConnection conn, ref DataSet ds)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(conn, "P_CalculateUserLevel", ref ds, ref Outputs);

            return CallResult;
        }

        public static int P_CanExpenseBonus(SqlConnection conn, ref int ReturnValue, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_CanExpenseBonus(conn, ref ds, ref ReturnValue, ref ReturnDescription);
        }

        public static int P_CanExpenseBonus(SqlConnection conn, ref DataSet ds, ref int ReturnValue, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(conn, "P_CanExpenseBonus", ref ds, ref Outputs,
                new MSSQL.Parameter("ReturnValue", SqlDbType.Int, 4, ParameterDirection.Output, ReturnValue),
                new MSSQL.Parameter("ReturnDescription", SqlDbType.VarChar, 100, ParameterDirection.Output, ReturnDescription)
                );

            try
            {
                ReturnValue = System.Convert.ToInt32(Outputs["ReturnValue"]);
            }
            catch { }

            try
            {
                ReturnDescription = System.Convert.ToString(Outputs["ReturnDescription"]);
            }
            catch { }

            return CallResult;
        }

        public static int P_CardPasswordAdd(SqlConnection conn, int AgentID, int Period, double Money, int Count, ref int ReturnValue, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_CardPasswordAdd(conn, ref ds, AgentID, Period, Money, Count, ref ReturnValue, ref ReturnDescription);
        }

        public static int P_CardPasswordAdd(SqlConnection conn, ref DataSet ds, int AgentID, int Period, double Money, int Count, ref int ReturnValue, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(conn, "P_CardPasswordAdd", ref ds, ref Outputs,
                new MSSQL.Parameter("AgentID", SqlDbType.Int, 0, ParameterDirection.Input, AgentID),
                new MSSQL.Parameter("Period", SqlDbType.Int, 0, ParameterDirection.Input, Period),
                new MSSQL.Parameter("Money", SqlDbType.Money, 0, ParameterDirection.Input, Money),
                new MSSQL.Parameter("Count", SqlDbType.Int, 0, ParameterDirection.Input, Count),
                new MSSQL.Parameter("ReturnValue", SqlDbType.Int, 4, ParameterDirection.Output, ReturnValue),
                new MSSQL.Parameter("ReturnDescription", SqlDbType.VarChar, 100, ParameterDirection.Output, ReturnDescription)
                );

            try
            {
                ReturnValue = System.Convert.ToInt32(Outputs["ReturnValue"]);
            }
            catch { }

            try
            {
                ReturnDescription = System.Convert.ToString(Outputs["ReturnDescription"]);
            }
            catch { }

            return CallResult;
        }

        public static int P_CardPasswordAgentAddMoney(SqlConnection conn, long AgentID, double Amount, string Memo, ref int ReturnValue, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_CardPasswordAgentAddMoney(conn, ref ds, AgentID, Amount, Memo, ref ReturnValue, ref ReturnDescription);
        }

        public static int P_CardPasswordAgentAddMoney(SqlConnection conn, ref DataSet ds, long AgentID, double Amount, string Memo, ref int ReturnValue, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(conn, "P_CardPasswordAgentAddMoney", ref ds, ref Outputs,
                new MSSQL.Parameter("AgentID", SqlDbType.BigInt, 0, ParameterDirection.Input, AgentID),
                new MSSQL.Parameter("Amount", SqlDbType.Money, 0, ParameterDirection.Input, Amount),
                new MSSQL.Parameter("Memo", SqlDbType.VarChar, 0, ParameterDirection.Input, Memo),
                new MSSQL.Parameter("ReturnValue", SqlDbType.Int, 4, ParameterDirection.Output, ReturnValue),
                new MSSQL.Parameter("ReturnDescription", SqlDbType.VarChar, 100, ParameterDirection.Output, ReturnDescription)
                );

            try
            {
                ReturnValue = System.Convert.ToInt32(Outputs["ReturnValue"]);
            }
            catch { }

            try
            {
                ReturnDescription = System.Convert.ToString(Outputs["ReturnDescription"]);
            }
            catch { }

            return CallResult;
        }

        public static int P_CardPasswordExchange(SqlConnection conn, int AgentID, string CardsXml, ref int ReturnValue, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_CardPasswordExchange(conn, ref ds, AgentID, CardsXml, ref ReturnValue, ref ReturnDescription);
        }

        public static int P_CardPasswordExchange(SqlConnection conn, ref DataSet ds, int AgentID, string CardsXml, ref int ReturnValue, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(conn, "P_CardPasswordExchange", ref ds, ref Outputs,
                new MSSQL.Parameter("AgentID", SqlDbType.Int, 0, ParameterDirection.Input, AgentID),
                new MSSQL.Parameter("CardsXml", SqlDbType.NText, 0, ParameterDirection.Input, CardsXml),
                new MSSQL.Parameter("ReturnValue", SqlDbType.Int, 4, ParameterDirection.Output, ReturnValue),
                new MSSQL.Parameter("ReturnDescription", SqlDbType.VarChar, 100, ParameterDirection.Output, ReturnDescription)
                );

            try
            {
                ReturnValue = System.Convert.ToInt32(Outputs["ReturnValue"]);
            }
            catch { }

            try
            {
                ReturnDescription = System.Convert.ToString(Outputs["ReturnDescription"]);
            }
            catch { }

            return CallResult;
        }

        public static int P_CardPasswordGet(SqlConnection conn, int AgentID, long CardPasswordID, ref DateTime DateTime, ref DateTime Period, ref double Money, ref short State, ref long UserID, ref DateTime UseDateTime, ref int ReturnValue, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_CardPasswordGet(conn, ref ds, AgentID, CardPasswordID, ref DateTime, ref Period, ref Money, ref State, ref UserID, ref UseDateTime, ref ReturnValue, ref ReturnDescription);
        }

        public static int P_CardPasswordGet(SqlConnection conn, ref DataSet ds, int AgentID, long CardPasswordID, ref DateTime DateTime, ref DateTime Period, ref double Money, ref short State, ref long UserID, ref DateTime UseDateTime, ref int ReturnValue, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(conn, "P_CardPasswordGet", ref ds, ref Outputs,
                new MSSQL.Parameter("AgentID", SqlDbType.Int, 0, ParameterDirection.Input, AgentID),
                new MSSQL.Parameter("CardPasswordID", SqlDbType.BigInt, 0, ParameterDirection.Input, CardPasswordID),
                new MSSQL.Parameter("DateTime", SqlDbType.DateTime, 8, ParameterDirection.Output, DateTime),
                new MSSQL.Parameter("Period", SqlDbType.DateTime, 8, ParameterDirection.Output, Period),
                new MSSQL.Parameter("Money", SqlDbType.Money, 8, ParameterDirection.Output, Money),
                new MSSQL.Parameter("State", SqlDbType.SmallInt, 2, ParameterDirection.Output, State),
                new MSSQL.Parameter("UserID", SqlDbType.BigInt, 8, ParameterDirection.Output, UserID),
                new MSSQL.Parameter("UseDateTime", SqlDbType.DateTime, 8, ParameterDirection.Output, UseDateTime),
                new MSSQL.Parameter("ReturnValue", SqlDbType.Int, 4, ParameterDirection.Output, ReturnValue),
                new MSSQL.Parameter("ReturnDescription", SqlDbType.VarChar, 100, ParameterDirection.Output, ReturnDescription)
                );

            try
            {
                DateTime = System.Convert.ToDateTime(Outputs["DateTime"]);
            }
            catch { }

            try
            {
                Period = System.Convert.ToDateTime(Outputs["Period"]);
            }
            catch { }

            try
            {
                Money = System.Convert.ToDouble(Outputs["Money"]);
            }
            catch { }

            try
            {
                State = System.Convert.ToInt16(Outputs["State"]);
            }
            catch { }

            try
            {
                UserID = System.Convert.ToInt64(Outputs["UserID"]);
            }
            catch { }

            try
            {
                UseDateTime = System.Convert.ToDateTime(Outputs["UseDateTime"]);
            }
            catch { }

            try
            {
                ReturnValue = System.Convert.ToInt32(Outputs["ReturnValue"]);
            }
            catch { }

            try
            {
                ReturnDescription = System.Convert.ToString(Outputs["ReturnDescription"]);
            }
            catch { }

            return CallResult;
        }

        public static int P_CardPasswordSetPeriod(SqlConnection conn, int AgentID, long CardPasswordID, DateTime Period, ref int ReturnValue, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_CardPasswordSetPeriod(conn, ref ds, AgentID, CardPasswordID, Period, ref ReturnValue, ref ReturnDescription);
        }

        public static int P_CardPasswordSetPeriod(SqlConnection conn, ref DataSet ds, int AgentID, long CardPasswordID, DateTime Period, ref int ReturnValue, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(conn, "P_CardPasswordSetPeriod", ref ds, ref Outputs,
                new MSSQL.Parameter("AgentID", SqlDbType.Int, 0, ParameterDirection.Input, AgentID),
                new MSSQL.Parameter("CardPasswordID", SqlDbType.BigInt, 0, ParameterDirection.Input, CardPasswordID),
                new MSSQL.Parameter("Period", SqlDbType.DateTime, 0, ParameterDirection.Input, Period),
                new MSSQL.Parameter("ReturnValue", SqlDbType.Int, 4, ParameterDirection.Output, ReturnValue),
                new MSSQL.Parameter("ReturnDescription", SqlDbType.VarChar, 100, ParameterDirection.Output, ReturnDescription)
                );

            try
            {
                ReturnValue = System.Convert.ToInt32(Outputs["ReturnValue"]);
            }
            catch { }

            try
            {
                ReturnDescription = System.Convert.ToString(Outputs["ReturnDescription"]);
            }
            catch { }

            return CallResult;
        }

        public static int P_CardPasswordTryErrorAdd(SqlConnection conn, long UserID, string Number, ref int ReturnValue, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_CardPasswordTryErrorAdd(conn, ref ds, UserID, Number, ref ReturnValue, ref ReturnDescription);
        }

        public static int P_CardPasswordTryErrorAdd(SqlConnection conn, ref DataSet ds, long UserID, string Number, ref int ReturnValue, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(conn, "P_CardPasswordTryErrorAdd", ref ds, ref Outputs,
                new MSSQL.Parameter("UserID", SqlDbType.BigInt, 0, ParameterDirection.Input, UserID),
                new MSSQL.Parameter("Number", SqlDbType.VarChar, 0, ParameterDirection.Input, Number),
                new MSSQL.Parameter("ReturnValue", SqlDbType.Int, 4, ParameterDirection.Output, ReturnValue),
                new MSSQL.Parameter("ReturnDescription", SqlDbType.VarChar, 100, ParameterDirection.Output, ReturnDescription)
                );

            try
            {
                ReturnValue = System.Convert.ToInt32(Outputs["ReturnValue"]);
            }
            catch { }

            try
            {
                ReturnDescription = System.Convert.ToString(Outputs["ReturnDescription"]);
            }
            catch { }

            return CallResult;
        }

        public static int P_CardPasswordTryErrorFreeze(SqlConnection conn, long SiteID, long UserID, ref int Freeze, ref int ReturnValue, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_CardPasswordTryErrorFreeze(conn, ref ds, SiteID, UserID, ref Freeze, ref ReturnValue, ref ReturnDescription);
        }

        public static int P_CardPasswordTryErrorFreeze(SqlConnection conn, ref DataSet ds, long SiteID, long UserID, ref int Freeze, ref int ReturnValue, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(conn, "P_CardPasswordTryErrorFreeze", ref ds, ref Outputs,
                new MSSQL.Parameter("SiteID", SqlDbType.BigInt, 0, ParameterDirection.Input, SiteID),
                new MSSQL.Parameter("UserID", SqlDbType.BigInt, 0, ParameterDirection.Input, UserID),
                new MSSQL.Parameter("Freeze", SqlDbType.Int, 4, ParameterDirection.Output, Freeze),
                new MSSQL.Parameter("ReturnValue", SqlDbType.Int, 4, ParameterDirection.Output, ReturnValue),
                new MSSQL.Parameter("ReturnDescription", SqlDbType.VarChar, 100, ParameterDirection.Output, ReturnDescription)
                );

            try
            {
                Freeze = System.Convert.ToInt32(Outputs["Freeze"]);
            }
            catch { }

            try
            {
                ReturnValue = System.Convert.ToInt32(Outputs["ReturnValue"]);
            }
            catch { }

            try
            {
                ReturnDescription = System.Convert.ToString(Outputs["ReturnDescription"]);
            }
            catch { }

            return CallResult;
        }

        public static int P_CardPasswordUse(SqlConnection conn, int AgentID, long CardPasswordID, string Number, long SiteID, long UserID, ref int ReturnValue, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_CardPasswordUse(conn, ref ds, AgentID, CardPasswordID, Number, SiteID, UserID, ref ReturnValue, ref ReturnDescription);
        }

        public static int P_CardPasswordUse(SqlConnection conn, ref DataSet ds, int AgentID, long CardPasswordID, string Number, long SiteID, long UserID, ref int ReturnValue, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(conn, "P_CardPasswordUse", ref ds, ref Outputs,
                new MSSQL.Parameter("AgentID", SqlDbType.Int, 0, ParameterDirection.Input, AgentID),
                new MSSQL.Parameter("CardPasswordID", SqlDbType.BigInt, 0, ParameterDirection.Input, CardPasswordID),
                new MSSQL.Parameter("Number", SqlDbType.VarChar, 0, ParameterDirection.Input, Number),
                new MSSQL.Parameter("SiteID", SqlDbType.BigInt, 0, ParameterDirection.Input, SiteID),
                new MSSQL.Parameter("UserID", SqlDbType.BigInt, 0, ParameterDirection.Input, UserID),
                new MSSQL.Parameter("ReturnValue", SqlDbType.Int, 4, ParameterDirection.Output, ReturnValue),
                new MSSQL.Parameter("ReturnDescription", SqlDbType.VarChar, 100, ParameterDirection.Output, ReturnDescription)
                );

            try
            {
                ReturnValue = System.Convert.ToInt32(Outputs["ReturnValue"]);
            }
            catch { }

            try
            {
                ReturnDescription = System.Convert.ToString(Outputs["ReturnDescription"]);
            }
            catch { }

            return CallResult;
        }

        public static int P_CelebDelete(SqlConnection conn, long SiteID, long ID, ref int ReturnValue, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_CelebDelete(conn, ref ds, SiteID, ID, ref ReturnValue, ref ReturnDescription);
        }

        public static int P_CelebDelete(SqlConnection conn, ref DataSet ds, long SiteID, long ID, ref int ReturnValue, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(conn, "P_CelebDelete", ref ds, ref Outputs,
                new MSSQL.Parameter("SiteID", SqlDbType.BigInt, 0, ParameterDirection.Input, SiteID),
                new MSSQL.Parameter("ID", SqlDbType.BigInt, 0, ParameterDirection.Input, ID),
                new MSSQL.Parameter("ReturnValue", SqlDbType.Int, 4, ParameterDirection.Output, ReturnValue),
                new MSSQL.Parameter("ReturnDescription", SqlDbType.VarChar, 100, ParameterDirection.Output, ReturnDescription)
                );

            try
            {
                ReturnValue = System.Convert.ToInt32(Outputs["ReturnValue"]);
            }
            catch { }

            try
            {
                ReturnDescription = System.Convert.ToString(Outputs["ReturnDescription"]);
            }
            catch { }

            return CallResult;
        }

        public static int P_CelebEdit(SqlConnection conn, long SiteID, long ID, string Title, string Intro, string Say, string Comment, string Score, long Order, bool isRecommended, ref int ReturnValue, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_CelebEdit(conn, ref ds, SiteID, ID, Title, Intro, Say, Comment, Score, Order, isRecommended, ref ReturnValue, ref ReturnDescription);
        }

        public static int P_CelebEdit(SqlConnection conn, ref DataSet ds, long SiteID, long ID, string Title, string Intro, string Say, string Comment, string Score, long Order, bool isRecommended, ref int ReturnValue, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(conn, "P_CelebEdit", ref ds, ref Outputs,
                new MSSQL.Parameter("SiteID", SqlDbType.BigInt, 0, ParameterDirection.Input, SiteID),
                new MSSQL.Parameter("ID", SqlDbType.BigInt, 0, ParameterDirection.Input, ID),
                new MSSQL.Parameter("Title", SqlDbType.VarChar, 0, ParameterDirection.Input, Title),
                new MSSQL.Parameter("Intro", SqlDbType.VarChar, 0, ParameterDirection.Input, Intro),
                new MSSQL.Parameter("Say", SqlDbType.VarChar, 0, ParameterDirection.Input, Say),
                new MSSQL.Parameter("Comment", SqlDbType.VarChar, 0, ParameterDirection.Input, Comment),
                new MSSQL.Parameter("Score", SqlDbType.VarChar, 0, ParameterDirection.Input, Score),
                new MSSQL.Parameter("Order", SqlDbType.BigInt, 0, ParameterDirection.Input, Order),
                new MSSQL.Parameter("isRecommended", SqlDbType.Bit, 0, ParameterDirection.Input, isRecommended),
                new MSSQL.Parameter("ReturnValue", SqlDbType.Int, 4, ParameterDirection.Output, ReturnValue),
                new MSSQL.Parameter("ReturnDescription", SqlDbType.VarChar, 100, ParameterDirection.Output, ReturnDescription)
                );

            try
            {
                ReturnValue = System.Convert.ToInt32(Outputs["ReturnValue"]);
            }
            catch { }

            try
            {
                ReturnDescription = System.Convert.ToString(Outputs["ReturnDescription"]);
            }
            catch { }

            return CallResult;
        }

        public static int P_ChasesAdd(SqlConnection conn, long UserID, int LotteryID, int PlayTypeID, int Price, short Type, DateTime StartTime, DateTime EndTime, int IsuseCount, int Multiple, int Nums, short BetType, string LotteryNumber, short StopTypeWhenWin, double StopTypeWhenMoney, double Money, string Title, string ChaseXML, ref int ChaseID, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_ChasesAdd(conn, ref ds, UserID, LotteryID, PlayTypeID, Price, Type, StartTime, EndTime, IsuseCount, Multiple, Nums, BetType, LotteryNumber, StopTypeWhenWin, StopTypeWhenMoney, Money, Title, ChaseXML, ref ChaseID, ref ReturnDescription);
        }

        public static int P_ChasesAdd(SqlConnection conn, ref DataSet ds, long UserID, int LotteryID, int PlayTypeID, int Price, short Type, DateTime StartTime, DateTime EndTime, int IsuseCount, int Multiple, int Nums, short BetType, string LotteryNumber, short StopTypeWhenWin, double StopTypeWhenMoney, double Money, string Title, string ChaseXML, ref int ChaseID, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(conn, "P_ChasesAdd", ref ds, ref Outputs,
                new MSSQL.Parameter("UserID", SqlDbType.BigInt, 0, ParameterDirection.Input, UserID),
                new MSSQL.Parameter("LotteryID", SqlDbType.Int, 0, ParameterDirection.Input, LotteryID),
                new MSSQL.Parameter("PlayTypeID", SqlDbType.Int, 0, ParameterDirection.Input, PlayTypeID),
                new MSSQL.Parameter("Price", SqlDbType.Int, 0, ParameterDirection.Input, Price),
                new MSSQL.Parameter("Type", SqlDbType.SmallInt, 0, ParameterDirection.Input, Type),
                new MSSQL.Parameter("StartTime", SqlDbType.DateTime, 0, ParameterDirection.Input, StartTime),
                new MSSQL.Parameter("EndTime", SqlDbType.DateTime, 0, ParameterDirection.Input, EndTime),
                new MSSQL.Parameter("IsuseCount", SqlDbType.Int, 0, ParameterDirection.Input, IsuseCount),
                new MSSQL.Parameter("Multiple", SqlDbType.Int, 0, ParameterDirection.Input, Multiple),
                new MSSQL.Parameter("Nums", SqlDbType.Int, 0, ParameterDirection.Input, Nums),
                new MSSQL.Parameter("BetType", SqlDbType.SmallInt, 0, ParameterDirection.Input, BetType),
                new MSSQL.Parameter("LotteryNumber", SqlDbType.VarChar, 0, ParameterDirection.Input, LotteryNumber),
                new MSSQL.Parameter("StopTypeWhenWin", SqlDbType.SmallInt, 0, ParameterDirection.Input, StopTypeWhenWin),
                new MSSQL.Parameter("StopTypeWhenMoney", SqlDbType.Money, 0, ParameterDirection.Input, StopTypeWhenMoney),
                new MSSQL.Parameter("Money", SqlDbType.Money, 0, ParameterDirection.Input, Money),
                new MSSQL.Parameter("Title", SqlDbType.VarChar, 0, ParameterDirection.Input, Title),
                new MSSQL.Parameter("ChaseXML", SqlDbType.VarChar, 0, ParameterDirection.Input, ChaseXML),
                new MSSQL.Parameter("ChaseID", SqlDbType.Int, 4, ParameterDirection.Output, ChaseID),
                new MSSQL.Parameter("ReturnDescription", SqlDbType.VarChar, 50, ParameterDirection.Output, ReturnDescription)
                );

            try
            {
                ChaseID = System.Convert.ToInt32(Outputs["ChaseID"]);
            }
            catch { }

            try
            {
                ReturnDescription = System.Convert.ToString(Outputs["ReturnDescription"]);
            }
            catch { }

            return CallResult;
        }

        public static int P_ChaseStopWhenWin(SqlConnection conn, long SchemeID, double WinMoney, ref int ReturnValue, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_ChaseStopWhenWin(conn, ref ds, SchemeID, WinMoney, ref ReturnValue, ref ReturnDescription);
        }

        public static int P_ChaseStopWhenWin(SqlConnection conn, ref DataSet ds, long SchemeID, double WinMoney, ref int ReturnValue, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(conn, "P_ChaseStopWhenWin", ref ds, ref Outputs,
                new MSSQL.Parameter("SchemeID", SqlDbType.BigInt, 0, ParameterDirection.Input, SchemeID),
                new MSSQL.Parameter("WinMoney", SqlDbType.Money, 0, ParameterDirection.Input, WinMoney),
                new MSSQL.Parameter("ReturnValue", SqlDbType.Int, 4, ParameterDirection.Output, ReturnValue),
                new MSSQL.Parameter("ReturnDescription", SqlDbType.VarChar, 100, ParameterDirection.Output, ReturnDescription)
                );

            try
            {
                ReturnValue = System.Convert.ToInt32(Outputs["ReturnValue"]);
            }
            catch { }

            try
            {
                ReturnDescription = System.Convert.ToString(Outputs["ReturnDescription"]);
            }
            catch { }

            return CallResult;
        }

        public static int P_ChaseTaskStopWhenWin(SqlConnection conn, long SiteID, long SchemeID, double WinMoney, ref int ReturnValue, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_ChaseTaskStopWhenWin(conn, ref ds, SiteID, SchemeID, WinMoney, ref ReturnValue, ref ReturnDescription);
        }

        public static int P_ChaseTaskStopWhenWin(SqlConnection conn, ref DataSet ds, long SiteID, long SchemeID, double WinMoney, ref int ReturnValue, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(conn, "P_ChaseTaskStopWhenWin", ref ds, ref Outputs,
                new MSSQL.Parameter("SiteID", SqlDbType.BigInt, 0, ParameterDirection.Input, SiteID),
                new MSSQL.Parameter("SchemeID", SqlDbType.BigInt, 0, ParameterDirection.Input, SchemeID),
                new MSSQL.Parameter("WinMoney", SqlDbType.Money, 0, ParameterDirection.Input, WinMoney),
                new MSSQL.Parameter("ReturnValue", SqlDbType.Int, 4, ParameterDirection.Output, ReturnValue),
                new MSSQL.Parameter("ReturnDescription", SqlDbType.VarChar, 100, ParameterDirection.Output, ReturnDescription)
                );

            try
            {
                ReturnValue = System.Convert.ToInt32(Outputs["ReturnValue"]);
            }
            catch { }

            try
            {
                ReturnDescription = System.Convert.ToString(Outputs["ReturnDescription"]);
            }
            catch { }

            return CallResult;
        }

        public static int P_CheckChaseTasks(SqlConnection conn, ref int ReturnValue, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_CheckChaseTasks(conn, ref ds, ref ReturnValue, ref ReturnDescription);
        }

        public static int P_CheckChaseTasks(SqlConnection conn, ref DataSet ds, ref int ReturnValue, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(conn, "P_CheckChaseTasks", ref ds, ref Outputs,
                new MSSQL.Parameter("ReturnValue", SqlDbType.Int, 4, ParameterDirection.Output, ReturnValue),
                new MSSQL.Parameter("ReturnDescription", SqlDbType.VarChar, 100, ParameterDirection.Output, ReturnDescription)
                );

            try
            {
                ReturnValue = System.Convert.ToInt32(Outputs["ReturnValue"]);
            }
            catch { }

            try
            {
                ReturnDescription = System.Convert.ToString(Outputs["ReturnDescription"]);
            }
            catch { }

            return CallResult;
        }

        public static int P_ClearSystemLog(SqlConnection conn, long SiteID, long UserID, string IPAddress, ref int ReturnValue, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_ClearSystemLog(conn, ref ds, SiteID, UserID, IPAddress, ref ReturnValue, ref ReturnDescription);
        }

        public static int P_ClearSystemLog(SqlConnection conn, ref DataSet ds, long SiteID, long UserID, string IPAddress, ref int ReturnValue, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(conn, "P_ClearSystemLog", ref ds, ref Outputs,
                new MSSQL.Parameter("SiteID", SqlDbType.BigInt, 0, ParameterDirection.Input, SiteID),
                new MSSQL.Parameter("UserID", SqlDbType.BigInt, 0, ParameterDirection.Input, UserID),
                new MSSQL.Parameter("IPAddress", SqlDbType.VarChar, 0, ParameterDirection.Input, IPAddress),
                new MSSQL.Parameter("ReturnValue", SqlDbType.Int, 4, ParameterDirection.Output, ReturnValue),
                new MSSQL.Parameter("ReturnDescription", SqlDbType.VarChar, 100, ParameterDirection.Output, ReturnDescription)
                );

            try
            {
                ReturnValue = System.Convert.ToInt32(Outputs["ReturnValue"]);
            }
            catch { }

            try
            {
                ReturnDescription = System.Convert.ToString(Outputs["ReturnDescription"]);
            }
            catch { }

            return CallResult;
        }

        public static int P_CpsAdd(SqlConnection conn, long SiteID, long OwnerUserID, string Name, string Url, string LogoUrl, double BonusScale, bool ON, string Company, string Address, string PostCode, string ResponsiblePerson, string ContactPerson, string Telephone, string Fax, string Mobile, string Email, string QQ, string ServiceTelephone, string MD5Key, short Type, long ParentID, string DomainName, long OperatorID, long CommendID, ref long ID, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_CpsAdd(conn, ref ds, SiteID, OwnerUserID, Name, Url, LogoUrl, BonusScale, ON, Company, Address, PostCode, ResponsiblePerson, ContactPerson, Telephone, Fax, Mobile, Email, QQ, ServiceTelephone, MD5Key, Type, ParentID, DomainName, OperatorID, CommendID, ref ID, ref ReturnDescription);
        }

        public static int P_CpsAdd(SqlConnection conn, ref DataSet ds, long SiteID, long OwnerUserID, string Name, string Url, string LogoUrl, double BonusScale, bool ON, string Company, string Address, string PostCode, string ResponsiblePerson, string ContactPerson, string Telephone, string Fax, string Mobile, string Email, string QQ, string ServiceTelephone, string MD5Key, short Type, long ParentID, string DomainName, long OperatorID, long CommendID, ref long ID, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(conn, "P_CpsAdd", ref ds, ref Outputs,
                new MSSQL.Parameter("SiteID", SqlDbType.BigInt, 0, ParameterDirection.Input, SiteID),
                new MSSQL.Parameter("OwnerUserID", SqlDbType.BigInt, 0, ParameterDirection.Input, OwnerUserID),
                new MSSQL.Parameter("Name", SqlDbType.VarChar, 0, ParameterDirection.Input, Name),
                new MSSQL.Parameter("Url", SqlDbType.VarChar, 0, ParameterDirection.Input, Url),
                new MSSQL.Parameter("LogoUrl", SqlDbType.VarChar, 0, ParameterDirection.Input, LogoUrl),
                new MSSQL.Parameter("BonusScale", SqlDbType.Float, 0, ParameterDirection.Input, BonusScale),
                new MSSQL.Parameter("ON", SqlDbType.Bit, 0, ParameterDirection.Input, ON),
                new MSSQL.Parameter("Company", SqlDbType.VarChar, 0, ParameterDirection.Input, Company),
                new MSSQL.Parameter("Address", SqlDbType.VarChar, 0, ParameterDirection.Input, Address),
                new MSSQL.Parameter("PostCode", SqlDbType.VarChar, 0, ParameterDirection.Input, PostCode),
                new MSSQL.Parameter("ResponsiblePerson", SqlDbType.VarChar, 0, ParameterDirection.Input, ResponsiblePerson),
                new MSSQL.Parameter("ContactPerson", SqlDbType.VarChar, 0, ParameterDirection.Input, ContactPerson),
                new MSSQL.Parameter("Telephone", SqlDbType.VarChar, 0, ParameterDirection.Input, Telephone),
                new MSSQL.Parameter("Fax", SqlDbType.VarChar, 0, ParameterDirection.Input, Fax),
                new MSSQL.Parameter("Mobile", SqlDbType.VarChar, 0, ParameterDirection.Input, Mobile),
                new MSSQL.Parameter("Email", SqlDbType.VarChar, 0, ParameterDirection.Input, Email),
                new MSSQL.Parameter("QQ", SqlDbType.VarChar, 0, ParameterDirection.Input, QQ),
                new MSSQL.Parameter("ServiceTelephone", SqlDbType.VarChar, 0, ParameterDirection.Input, ServiceTelephone),
                new MSSQL.Parameter("MD5Key", SqlDbType.VarChar, 0, ParameterDirection.Input, MD5Key),
                new MSSQL.Parameter("Type", SqlDbType.SmallInt, 0, ParameterDirection.Input, Type),
                new MSSQL.Parameter("ParentID", SqlDbType.BigInt, 0, ParameterDirection.Input, ParentID),
                new MSSQL.Parameter("DomainName", SqlDbType.VarChar, 0, ParameterDirection.Input, DomainName),
                new MSSQL.Parameter("OperatorID", SqlDbType.BigInt, 0, ParameterDirection.Input, OperatorID),
                new MSSQL.Parameter("CommendID", SqlDbType.BigInt, 0, ParameterDirection.Input, CommendID),
                new MSSQL.Parameter("ID", SqlDbType.BigInt, 8, ParameterDirection.Output, ID),
                new MSSQL.Parameter("ReturnDescription", SqlDbType.VarChar, 100, ParameterDirection.Output, ReturnDescription)
                );

            try
            {
                ID = System.Convert.ToInt64(Outputs["ID"]);
            }
            catch { }

            try
            {
                ReturnDescription = System.Convert.ToString(Outputs["ReturnDescription"]);
            }
            catch { }

            return CallResult;
        }

        public static int P_CpsAdminAccountByMonth(SqlConnection conn, long SiteID, ref int ReturnValue, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_CpsAdminAccountByMonth(conn, ref ds, SiteID, ref ReturnValue, ref ReturnDescription);
        }

        public static int P_CpsAdminAccountByMonth(SqlConnection conn, ref DataSet ds, long SiteID, ref int ReturnValue, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(conn, "P_CpsAdminAccountByMonth", ref ds, ref Outputs,
                new MSSQL.Parameter("SiteID", SqlDbType.BigInt, 0, ParameterDirection.Input, SiteID),
                new MSSQL.Parameter("ReturnValue", SqlDbType.Int, 4, ParameterDirection.Output, ReturnValue),
                new MSSQL.Parameter("ReturnDescription", SqlDbType.VarChar, 100, ParameterDirection.Output, ReturnDescription)
                );

            try
            {
                ReturnValue = System.Convert.ToInt32(Outputs["ReturnValue"]);
            }
            catch { }

            try
            {
                ReturnDescription = System.Convert.ToString(Outputs["ReturnDescription"]);
            }
            catch { }

            return CallResult;
        }

        public static int P_CpsCalculateAllowBonus(SqlConnection conn, int Year, int Month, ref int ReturnValue, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_CpsCalculateAllowBonus(conn, ref ds, Year, Month, ref ReturnValue, ref ReturnDescription);
        }

        public static int P_CpsCalculateAllowBonus(SqlConnection conn, ref DataSet ds, int Year, int Month, ref int ReturnValue, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(conn, "P_CpsCalculateAllowBonus", ref ds, ref Outputs,
                new MSSQL.Parameter("Year", SqlDbType.Int, 0, ParameterDirection.Input, Year),
                new MSSQL.Parameter("Month", SqlDbType.Int, 0, ParameterDirection.Input, Month),
                new MSSQL.Parameter("ReturnValue", SqlDbType.Int, 4, ParameterDirection.Output, ReturnValue),
                new MSSQL.Parameter("ReturnDescription", SqlDbType.VarChar, 100, ParameterDirection.Output, ReturnDescription)
                );

            try
            {
                ReturnValue = System.Convert.ToInt32(Outputs["ReturnValue"]);
            }
            catch { }

            try
            {
                ReturnDescription = System.Convert.ToString(Outputs["ReturnDescription"]);
            }
            catch { }

            return CallResult;
        }

        public static int P_CpsCalculateBonus(SqlConnection conn, ref int ReturnValue, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_CpsCalculateBonus(conn, ref ds, ref ReturnValue, ref ReturnDescription);
        }

        public static int P_CpsCalculateBonus(SqlConnection conn, ref DataSet ds, ref int ReturnValue, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(conn, "P_CpsCalculateBonus", ref ds, ref Outputs,
                new MSSQL.Parameter("ReturnValue", SqlDbType.Int, 4, ParameterDirection.Output, ReturnValue),
                new MSSQL.Parameter("ReturnDescription", SqlDbType.VarChar, 100, ParameterDirection.Output, ReturnDescription)
                );

            try
            {
                ReturnValue = System.Convert.ToInt32(Outputs["ReturnValue"]);
            }
            catch { }

            try
            {
                ReturnDescription = System.Convert.ToString(Outputs["ReturnDescription"]);
            }
            catch { }

            return CallResult;
        }

        public static int P_CpsDistill(SqlConnection conn, long SiteID, long UserID, double Money, double FormalitiesFees, string BankUserName, string BankName, string BankCardNumber, string Memo, ref int ReturnValue, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_CpsDistill(conn, ref ds, SiteID, UserID, Money, FormalitiesFees, BankUserName, BankName, BankCardNumber, Memo, ref ReturnValue, ref ReturnDescription);
        }

        public static int P_CpsDistill(SqlConnection conn, ref DataSet ds, long SiteID, long UserID, double Money, double FormalitiesFees, string BankUserName, string BankName, string BankCardNumber, string Memo, ref int ReturnValue, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(conn, "P_CpsDistill", ref ds, ref Outputs,
                new MSSQL.Parameter("SiteID", SqlDbType.BigInt, 0, ParameterDirection.Input, SiteID),
                new MSSQL.Parameter("UserID", SqlDbType.BigInt, 0, ParameterDirection.Input, UserID),
                new MSSQL.Parameter("Money", SqlDbType.Money, 0, ParameterDirection.Input, Money),
                new MSSQL.Parameter("FormalitiesFees", SqlDbType.Money, 0, ParameterDirection.Input, FormalitiesFees),
                new MSSQL.Parameter("BankUserName", SqlDbType.VarChar, 0, ParameterDirection.Input, BankUserName),
                new MSSQL.Parameter("BankName", SqlDbType.VarChar, 0, ParameterDirection.Input, BankName),
                new MSSQL.Parameter("BankCardNumber", SqlDbType.VarChar, 0, ParameterDirection.Input, BankCardNumber),
                new MSSQL.Parameter("Memo", SqlDbType.VarChar, 0, ParameterDirection.Input, Memo),
                new MSSQL.Parameter("ReturnValue", SqlDbType.Int, 4, ParameterDirection.Output, ReturnValue),
                new MSSQL.Parameter("ReturnDescription", SqlDbType.VarChar, 100, ParameterDirection.Output, ReturnDescription)
                );

            try
            {
                ReturnValue = System.Convert.ToInt32(Outputs["ReturnValue"]);
            }
            catch { }

            try
            {
                ReturnDescription = System.Convert.ToString(Outputs["ReturnDescription"]);
            }
            catch { }

            return CallResult;
        }

        public static int P_CpsDistillAccept(SqlConnection conn, long SiteID, long UserID, long DistillID, string PayName, string PayBank, string PayCardNumber, string Memo, long HandleOperatorID, ref int ReturnValue, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_CpsDistillAccept(conn, ref ds, SiteID, UserID, DistillID, PayName, PayBank, PayCardNumber, Memo, HandleOperatorID, ref ReturnValue, ref ReturnDescription);
        }

        public static int P_CpsDistillAccept(SqlConnection conn, ref DataSet ds, long SiteID, long UserID, long DistillID, string PayName, string PayBank, string PayCardNumber, string Memo, long HandleOperatorID, ref int ReturnValue, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(conn, "P_CpsDistillAccept", ref ds, ref Outputs,
                new MSSQL.Parameter("SiteID", SqlDbType.BigInt, 0, ParameterDirection.Input, SiteID),
                new MSSQL.Parameter("UserID", SqlDbType.BigInt, 0, ParameterDirection.Input, UserID),
                new MSSQL.Parameter("DistillID", SqlDbType.BigInt, 0, ParameterDirection.Input, DistillID),
                new MSSQL.Parameter("PayName", SqlDbType.VarChar, 0, ParameterDirection.Input, PayName),
                new MSSQL.Parameter("PayBank", SqlDbType.VarChar, 0, ParameterDirection.Input, PayBank),
                new MSSQL.Parameter("PayCardNumber", SqlDbType.VarChar, 0, ParameterDirection.Input, PayCardNumber),
                new MSSQL.Parameter("Memo", SqlDbType.VarChar, 0, ParameterDirection.Input, Memo),
                new MSSQL.Parameter("HandleOperatorID", SqlDbType.BigInt, 0, ParameterDirection.Input, HandleOperatorID),
                new MSSQL.Parameter("ReturnValue", SqlDbType.Int, 4, ParameterDirection.Output, ReturnValue),
                new MSSQL.Parameter("ReturnDescription", SqlDbType.VarChar, 100, ParameterDirection.Output, ReturnDescription)
                );

            try
            {
                ReturnValue = System.Convert.ToInt32(Outputs["ReturnValue"]);
            }
            catch { }

            try
            {
                ReturnDescription = System.Convert.ToString(Outputs["ReturnDescription"]);
            }
            catch { }

            return CallResult;
        }

        public static int P_CpsDistillNoAccept(SqlConnection conn, long SiteID, long UserID, long DistillID, string Memo, long HandleOperatorID, ref int ReturnValue, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_CpsDistillNoAccept(conn, ref ds, SiteID, UserID, DistillID, Memo, HandleOperatorID, ref ReturnValue, ref ReturnDescription);
        }

        public static int P_CpsDistillNoAccept(SqlConnection conn, ref DataSet ds, long SiteID, long UserID, long DistillID, string Memo, long HandleOperatorID, ref int ReturnValue, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(conn, "P_CpsDistillNoAccept", ref ds, ref Outputs,
                new MSSQL.Parameter("SiteID", SqlDbType.BigInt, 0, ParameterDirection.Input, SiteID),
                new MSSQL.Parameter("UserID", SqlDbType.BigInt, 0, ParameterDirection.Input, UserID),
                new MSSQL.Parameter("DistillID", SqlDbType.BigInt, 0, ParameterDirection.Input, DistillID),
                new MSSQL.Parameter("Memo", SqlDbType.VarChar, 0, ParameterDirection.Input, Memo),
                new MSSQL.Parameter("HandleOperatorID", SqlDbType.BigInt, 0, ParameterDirection.Input, HandleOperatorID),
                new MSSQL.Parameter("ReturnValue", SqlDbType.Int, 4, ParameterDirection.Output, ReturnValue),
                new MSSQL.Parameter("ReturnDescription", SqlDbType.VarChar, 100, ParameterDirection.Output, ReturnDescription)
                );

            try
            {
                ReturnValue = System.Convert.ToInt32(Outputs["ReturnValue"]);
            }
            catch { }

            try
            {
                ReturnDescription = System.Convert.ToString(Outputs["ReturnDescription"]);
            }
            catch { }

            return CallResult;
        }

        public static int P_CpsDistillQuash(SqlConnection conn, long SiteID, long UserID, long DistillID, ref int ReturnValue, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_CpsDistillQuash(conn, ref ds, SiteID, UserID, DistillID, ref ReturnValue, ref ReturnDescription);
        }

        public static int P_CpsDistillQuash(SqlConnection conn, ref DataSet ds, long SiteID, long UserID, long DistillID, ref int ReturnValue, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(conn, "P_CpsDistillQuash", ref ds, ref Outputs,
                new MSSQL.Parameter("SiteID", SqlDbType.BigInt, 0, ParameterDirection.Input, SiteID),
                new MSSQL.Parameter("UserID", SqlDbType.BigInt, 0, ParameterDirection.Input, UserID),
                new MSSQL.Parameter("DistillID", SqlDbType.BigInt, 0, ParameterDirection.Input, DistillID),
                new MSSQL.Parameter("ReturnValue", SqlDbType.Int, 4, ParameterDirection.Output, ReturnValue),
                new MSSQL.Parameter("ReturnDescription", SqlDbType.VarChar, 100, ParameterDirection.Output, ReturnDescription)
                );

            try
            {
                ReturnValue = System.Convert.ToInt32(Outputs["ReturnValue"]);
            }
            catch { }

            try
            {
                ReturnDescription = System.Convert.ToString(Outputs["ReturnDescription"]);
            }
            catch { }

            return CallResult;
        }

        public static int P_CpsEdit(SqlConnection conn, long SiteID, long CpsID, string UrlName, string Url, string LogoUrl, double BonusScale, bool ON, string Company, string Address, string PostCode, string ResponsiblePerson, string ContactPerson, string Telephone, string Fax, string Mobile, string Email, string QQ, string ServiceTelephone, string MD5Key, short Type, string DomainName, bool IsShow, ref int ReturnValue, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_CpsEdit(conn, ref ds, SiteID, CpsID, UrlName, Url, LogoUrl, BonusScale, ON, Company, Address, PostCode, ResponsiblePerson, ContactPerson, Telephone, Fax, Mobile, Email, QQ, ServiceTelephone, MD5Key, Type, DomainName, IsShow, ref ReturnValue, ref ReturnDescription);
        }

        public static int P_CpsEdit(SqlConnection conn, ref DataSet ds, long SiteID, long CpsID, string UrlName, string Url, string LogoUrl, double BonusScale, bool ON, string Company, string Address, string PostCode, string ResponsiblePerson, string ContactPerson, string Telephone, string Fax, string Mobile, string Email, string QQ, string ServiceTelephone, string MD5Key, short Type, string DomainName, bool IsShow, ref int ReturnValue, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(conn, "P_CpsEdit", ref ds, ref Outputs,
                new MSSQL.Parameter("SiteID", SqlDbType.BigInt, 0, ParameterDirection.Input, SiteID),
                new MSSQL.Parameter("CpsID", SqlDbType.BigInt, 0, ParameterDirection.Input, CpsID),
                new MSSQL.Parameter("UrlName", SqlDbType.VarChar, 0, ParameterDirection.Input, UrlName),
                new MSSQL.Parameter("Url", SqlDbType.VarChar, 0, ParameterDirection.Input, Url),
                new MSSQL.Parameter("LogoUrl", SqlDbType.VarChar, 0, ParameterDirection.Input, LogoUrl),
                new MSSQL.Parameter("BonusScale", SqlDbType.Float, 0, ParameterDirection.Input, BonusScale),
                new MSSQL.Parameter("ON", SqlDbType.Bit, 0, ParameterDirection.Input, ON),
                new MSSQL.Parameter("Company", SqlDbType.VarChar, 0, ParameterDirection.Input, Company),
                new MSSQL.Parameter("Address", SqlDbType.VarChar, 0, ParameterDirection.Input, Address),
                new MSSQL.Parameter("PostCode", SqlDbType.VarChar, 0, ParameterDirection.Input, PostCode),
                new MSSQL.Parameter("ResponsiblePerson", SqlDbType.VarChar, 0, ParameterDirection.Input, ResponsiblePerson),
                new MSSQL.Parameter("ContactPerson", SqlDbType.VarChar, 0, ParameterDirection.Input, ContactPerson),
                new MSSQL.Parameter("Telephone", SqlDbType.VarChar, 0, ParameterDirection.Input, Telephone),
                new MSSQL.Parameter("Fax", SqlDbType.VarChar, 0, ParameterDirection.Input, Fax),
                new MSSQL.Parameter("Mobile", SqlDbType.VarChar, 0, ParameterDirection.Input, Mobile),
                new MSSQL.Parameter("Email", SqlDbType.VarChar, 0, ParameterDirection.Input, Email),
                new MSSQL.Parameter("QQ", SqlDbType.VarChar, 0, ParameterDirection.Input, QQ),
                new MSSQL.Parameter("ServiceTelephone", SqlDbType.VarChar, 0, ParameterDirection.Input, ServiceTelephone),
                new MSSQL.Parameter("MD5Key", SqlDbType.VarChar, 0, ParameterDirection.Input, MD5Key),
                new MSSQL.Parameter("Type", SqlDbType.SmallInt, 0, ParameterDirection.Input, Type),
                new MSSQL.Parameter("DomainName", SqlDbType.VarChar, 0, ParameterDirection.Input, DomainName),
                new MSSQL.Parameter("IsShow", SqlDbType.Bit, 0, ParameterDirection.Input, IsShow),
                new MSSQL.Parameter("ReturnValue", SqlDbType.Int, 4, ParameterDirection.Output, ReturnValue),
                new MSSQL.Parameter("ReturnDescription", SqlDbType.VarChar, 100, ParameterDirection.Output, ReturnDescription)
                );

            try
            {
                ReturnValue = System.Convert.ToInt32(Outputs["ReturnValue"]);
            }
            catch { }

            try
            {
                ReturnDescription = System.Convert.ToString(Outputs["ReturnDescription"]);
            }
            catch { }

            return CallResult;
        }

        public static int P_CpsGetCommenderBuyDetailByDate(SqlConnection conn, long SiteID, long CommenderID, DateTime FromTime, DateTime ToTime, ref int ReturnValue, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_CpsGetCommenderBuyDetailByDate(conn, ref ds, SiteID, CommenderID, FromTime, ToTime, ref ReturnValue, ref ReturnDescription);
        }

        public static int P_CpsGetCommenderBuyDetailByDate(SqlConnection conn, ref DataSet ds, long SiteID, long CommenderID, DateTime FromTime, DateTime ToTime, ref int ReturnValue, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(conn, "P_CpsGetCommenderBuyDetailByDate", ref ds, ref Outputs,
                new MSSQL.Parameter("SiteID", SqlDbType.BigInt, 0, ParameterDirection.Input, SiteID),
                new MSSQL.Parameter("CommenderID", SqlDbType.BigInt, 0, ParameterDirection.Input, CommenderID),
                new MSSQL.Parameter("FromTime", SqlDbType.DateTime, 0, ParameterDirection.Input, FromTime),
                new MSSQL.Parameter("ToTime", SqlDbType.DateTime, 0, ParameterDirection.Input, ToTime),
                new MSSQL.Parameter("ReturnValue", SqlDbType.Int, 4, ParameterDirection.Output, ReturnValue),
                new MSSQL.Parameter("ReturnDescription", SqlDbType.VarChar, 100, ParameterDirection.Output, ReturnDescription)
                );

            try
            {
                ReturnValue = System.Convert.ToInt32(Outputs["ReturnValue"]);
            }
            catch { }

            try
            {
                ReturnDescription = System.Convert.ToString(Outputs["ReturnDescription"]);
            }
            catch { }

            return CallResult;
        }

        public static int P_CpsGetCommendMemberBuyDetail(SqlConnection conn, long SiteID, long CommenderID, long MemberID, DateTime FromTime, DateTime ToTime, ref int ReturnValue, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_CpsGetCommendMemberBuyDetail(conn, ref ds, SiteID, CommenderID, MemberID, FromTime, ToTime, ref ReturnValue, ref ReturnDescription);
        }

        public static int P_CpsGetCommendMemberBuyDetail(SqlConnection conn, ref DataSet ds, long SiteID, long CommenderID, long MemberID, DateTime FromTime, DateTime ToTime, ref int ReturnValue, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(conn, "P_CpsGetCommendMemberBuyDetail", ref ds, ref Outputs,
                new MSSQL.Parameter("SiteID", SqlDbType.BigInt, 0, ParameterDirection.Input, SiteID),
                new MSSQL.Parameter("CommenderID", SqlDbType.BigInt, 0, ParameterDirection.Input, CommenderID),
                new MSSQL.Parameter("MemberID", SqlDbType.BigInt, 0, ParameterDirection.Input, MemberID),
                new MSSQL.Parameter("FromTime", SqlDbType.DateTime, 0, ParameterDirection.Input, FromTime),
                new MSSQL.Parameter("ToTime", SqlDbType.DateTime, 0, ParameterDirection.Input, ToTime),
                new MSSQL.Parameter("ReturnValue", SqlDbType.Int, 4, ParameterDirection.Output, ReturnValue),
                new MSSQL.Parameter("ReturnDescription", SqlDbType.VarChar, 100, ParameterDirection.Output, ReturnDescription)
                );

            try
            {
                ReturnValue = System.Convert.ToInt32(Outputs["ReturnValue"]);
            }
            catch { }

            try
            {
                ReturnDescription = System.Convert.ToString(Outputs["ReturnDescription"]);
            }
            catch { }

            return CallResult;
        }

        public static int P_CpsGetCommendMemberList(SqlConnection conn, long CommmenderID, ref int ReturnValue, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_CpsGetCommendMemberList(conn, ref ds, CommmenderID, ref ReturnValue, ref ReturnDescription);
        }

        public static int P_CpsGetCommendMemberList(SqlConnection conn, ref DataSet ds, long CommmenderID, ref int ReturnValue, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(conn, "P_CpsGetCommendMemberList", ref ds, ref Outputs,
                new MSSQL.Parameter("CommmenderID", SqlDbType.BigInt, 0, ParameterDirection.Input, CommmenderID),
                new MSSQL.Parameter("ReturnValue", SqlDbType.Int, 4, ParameterDirection.Output, ReturnValue),
                new MSSQL.Parameter("ReturnDescription", SqlDbType.VarChar, 100, ParameterDirection.Output, ReturnDescription)
                );

            try
            {
                ReturnValue = System.Convert.ToInt32(Outputs["ReturnValue"]);
            }
            catch { }

            try
            {
                ReturnDescription = System.Convert.ToString(Outputs["ReturnDescription"]);
            }
            catch { }

            return CallResult;
        }

        public static int P_CpsGetCommendSiteBuyDetail(SqlConnection conn, long SiteID, long CommenderID, long CpsID, long MemberID, DateTime FromTime, DateTime ToTime, ref int ReturnValue, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_CpsGetCommendSiteBuyDetail(conn, ref ds, SiteID, CommenderID, CpsID, MemberID, FromTime, ToTime, ref ReturnValue, ref ReturnDescription);
        }

        public static int P_CpsGetCommendSiteBuyDetail(SqlConnection conn, ref DataSet ds, long SiteID, long CommenderID, long CpsID, long MemberID, DateTime FromTime, DateTime ToTime, ref int ReturnValue, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(conn, "P_CpsGetCommendSiteBuyDetail", ref ds, ref Outputs,
                new MSSQL.Parameter("SiteID", SqlDbType.BigInt, 0, ParameterDirection.Input, SiteID),
                new MSSQL.Parameter("CommenderID", SqlDbType.BigInt, 0, ParameterDirection.Input, CommenderID),
                new MSSQL.Parameter("CpsID", SqlDbType.BigInt, 0, ParameterDirection.Input, CpsID),
                new MSSQL.Parameter("MemberID", SqlDbType.BigInt, 0, ParameterDirection.Input, MemberID),
                new MSSQL.Parameter("FromTime", SqlDbType.DateTime, 0, ParameterDirection.Input, FromTime),
                new MSSQL.Parameter("ToTime", SqlDbType.DateTime, 0, ParameterDirection.Input, ToTime),
                new MSSQL.Parameter("ReturnValue", SqlDbType.Int, 4, ParameterDirection.Output, ReturnValue),
                new MSSQL.Parameter("ReturnDescription", SqlDbType.VarChar, 100, ParameterDirection.Output, ReturnDescription)
                );

            try
            {
                ReturnValue = System.Convert.ToInt32(Outputs["ReturnValue"]);
            }
            catch { }

            try
            {
                ReturnDescription = System.Convert.ToString(Outputs["ReturnDescription"]);
            }
            catch { }

            return CallResult;
        }

        public static int P_CpsGetDayBuyDetailByType(SqlConnection conn, DateTime DayDate, ref int ReturnValue, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_CpsGetDayBuyDetailByType(conn, ref ds, DayDate, ref ReturnValue, ref ReturnDescription);
        }

        public static int P_CpsGetDayBuyDetailByType(SqlConnection conn, ref DataSet ds, DateTime DayDate, ref int ReturnValue, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(conn, "P_CpsGetDayBuyDetailByType", ref ds, ref Outputs,
                new MSSQL.Parameter("DayDate", SqlDbType.DateTime, 0, ParameterDirection.Input, DayDate),
                new MSSQL.Parameter("ReturnValue", SqlDbType.Int, 4, ParameterDirection.Output, ReturnValue),
                new MSSQL.Parameter("ReturnDescription", SqlDbType.VarChar, 100, ParameterDirection.Output, ReturnDescription)
                );

            try
            {
                ReturnValue = System.Convert.ToInt32(Outputs["ReturnValue"]);
            }
            catch { }

            try
            {
                ReturnDescription = System.Convert.ToString(Outputs["ReturnDescription"]);
            }
            catch { }

            return CallResult;
        }

        public static int P_CpsGetIncomeListByMonth(SqlConnection conn, long SiteID, long CpsID, ref int ReturnValue, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_CpsGetIncomeListByMonth(conn, ref ds, SiteID, CpsID, ref ReturnValue, ref ReturnDescription);
        }

        public static int P_CpsGetIncomeListByMonth(SqlConnection conn, ref DataSet ds, long SiteID, long CpsID, ref int ReturnValue, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(conn, "P_CpsGetIncomeListByMonth", ref ds, ref Outputs,
                new MSSQL.Parameter("SiteID", SqlDbType.BigInt, 0, ParameterDirection.Input, SiteID),
                new MSSQL.Parameter("CpsID", SqlDbType.BigInt, 0, ParameterDirection.Input, CpsID),
                new MSSQL.Parameter("ReturnValue", SqlDbType.Int, 4, ParameterDirection.Output, ReturnValue),
                new MSSQL.Parameter("ReturnDescription", SqlDbType.VarChar, 100, ParameterDirection.Output, ReturnDescription)
                );

            try
            {
                ReturnValue = System.Convert.ToInt32(Outputs["ReturnValue"]);
            }
            catch { }

            try
            {
                ReturnDescription = System.Convert.ToString(Outputs["ReturnDescription"]);
            }
            catch { }

            return CallResult;
        }

        public static int P_CpsGetPerDayTradeSumOfMonthByType(SqlConnection conn, DateTime MonthDate, ref int ReturnValue, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_CpsGetPerDayTradeSumOfMonthByType(conn, ref ds, MonthDate, ref ReturnValue, ref ReturnDescription);
        }

        public static int P_CpsGetPerDayTradeSumOfMonthByType(SqlConnection conn, ref DataSet ds, DateTime MonthDate, ref int ReturnValue, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(conn, "P_CpsGetPerDayTradeSumOfMonthByType", ref ds, ref Outputs,
                new MSSQL.Parameter("MonthDate", SqlDbType.DateTime, 0, ParameterDirection.Input, MonthDate),
                new MSSQL.Parameter("ReturnValue", SqlDbType.Int, 4, ParameterDirection.Output, ReturnValue),
                new MSSQL.Parameter("ReturnDescription", SqlDbType.VarChar, 100, ParameterDirection.Output, ReturnDescription)
                );

            try
            {
                ReturnValue = System.Convert.ToInt32(Outputs["ReturnValue"]);
            }
            catch { }

            try
            {
                ReturnDescription = System.Convert.ToString(Outputs["ReturnDescription"]);
            }
            catch { }

            return CallResult;
        }

        public static int P_CpsGetUnionPidTradeSum(SqlConnection conn, long SiteID, long CpsID, string PID, ref int ReturnValue, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_CpsGetUnionPidTradeSum(conn, ref ds, SiteID, CpsID, PID, ref ReturnValue, ref ReturnDescription);
        }

        public static int P_CpsGetUnionPidTradeSum(SqlConnection conn, ref DataSet ds, long SiteID, long CpsID, string PID, ref int ReturnValue, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(conn, "P_CpsGetUnionPidTradeSum", ref ds, ref Outputs,
                new MSSQL.Parameter("SiteID", SqlDbType.BigInt, 0, ParameterDirection.Input, SiteID),
                new MSSQL.Parameter("CpsID", SqlDbType.BigInt, 0, ParameterDirection.Input, CpsID),
                new MSSQL.Parameter("PID", SqlDbType.VarChar, 0, ParameterDirection.Input, PID),
                new MSSQL.Parameter("ReturnValue", SqlDbType.Int, 4, ParameterDirection.Output, ReturnValue),
                new MSSQL.Parameter("ReturnDescription", SqlDbType.VarChar, 100, ParameterDirection.Output, ReturnDescription)
                );

            try
            {
                ReturnValue = System.Convert.ToInt32(Outputs["ReturnValue"]);
            }
            catch { }

            try
            {
                ReturnDescription = System.Convert.ToString(Outputs["ReturnDescription"]);
            }
            catch { }

            return CallResult;
        }

        public static int P_CpsGetUserBonusScaleList(SqlConnection conn, long OwnerUserID, ref int ReturnValue, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_CpsGetUserBonusScaleList(conn, ref ds, OwnerUserID, ref ReturnValue, ref ReturnDescription);
        }

        public static int P_CpsGetUserBonusScaleList(SqlConnection conn, ref DataSet ds, long OwnerUserID, ref int ReturnValue, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(conn, "P_CpsGetUserBonusScaleList", ref ds, ref Outputs,
                new MSSQL.Parameter("OwnerUserID", SqlDbType.BigInt, 0, ParameterDirection.Input, OwnerUserID),
                new MSSQL.Parameter("ReturnValue", SqlDbType.Int, 4, ParameterDirection.Output, ReturnValue),
                new MSSQL.Parameter("ReturnDescription", SqlDbType.VarChar, 100, ParameterDirection.Output, ReturnDescription)
                );

            try
            {
                ReturnValue = System.Convert.ToInt32(Outputs["ReturnValue"]);
            }
            catch { }

            try
            {
                ReturnDescription = System.Convert.ToString(Outputs["ReturnDescription"]);
            }
            catch { }

            return CallResult;
        }

        public static int P_CpsMemRecommendWebsiteList(SqlConnection conn, long userid, long siteid, ref int ReturnValue, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_CpsMemRecommendWebsiteList(conn, ref ds, userid, siteid, ref ReturnValue, ref ReturnDescription);
        }

        public static int P_CpsMemRecommendWebsiteList(SqlConnection conn, ref DataSet ds, long userid, long siteid, ref int ReturnValue, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(conn, "P_CpsMemRecommendWebsiteList", ref ds, ref Outputs,
                new MSSQL.Parameter("userid", SqlDbType.BigInt, 0, ParameterDirection.Input, userid),
                new MSSQL.Parameter("siteid", SqlDbType.BigInt, 0, ParameterDirection.Input, siteid),
                new MSSQL.Parameter("ReturnValue", SqlDbType.Int, 4, ParameterDirection.Output, ReturnValue),
                new MSSQL.Parameter("ReturnDescription", SqlDbType.VarChar, 100, ParameterDirection.Output, ReturnDescription)
                );

            try
            {
                ReturnValue = System.Convert.ToInt32(Outputs["ReturnValue"]);
            }
            catch { }

            try
            {
                ReturnDescription = System.Convert.ToString(Outputs["ReturnDescription"]);
            }
            catch { }

            return CallResult;
        }

        public static int P_CpsPromoterList(SqlConnection conn, long SiteID, ref int ReturnValue, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_CpsPromoterList(conn, ref ds, SiteID, ref ReturnValue, ref ReturnDescription);
        }

        public static int P_CpsPromoterList(SqlConnection conn, ref DataSet ds, long SiteID, ref int ReturnValue, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(conn, "P_CpsPromoterList", ref ds, ref Outputs,
                new MSSQL.Parameter("SiteID", SqlDbType.BigInt, 0, ParameterDirection.Input, SiteID),
                new MSSQL.Parameter("ReturnValue", SqlDbType.Int, 4, ParameterDirection.Output, ReturnValue),
                new MSSQL.Parameter("ReturnDescription", SqlDbType.VarChar, 100, ParameterDirection.Output, ReturnDescription)
                );

            try
            {
                ReturnValue = System.Convert.ToInt32(Outputs["ReturnValue"]);
            }
            catch { }

            try
            {
                ReturnDescription = System.Convert.ToString(Outputs["ReturnDescription"]);
            }
            catch { }

            return CallResult;
        }

        public static int P_CpsSetBonusScaleSetting(SqlConnection conn, long LotteryID, double UnionBonusScale, double SiteBonusScale, ref int ReturnValue, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_CpsSetBonusScaleSetting(conn, ref ds, LotteryID, UnionBonusScale, SiteBonusScale, ref ReturnValue, ref ReturnDescription);
        }

        public static int P_CpsSetBonusScaleSetting(SqlConnection conn, ref DataSet ds, long LotteryID, double UnionBonusScale, double SiteBonusScale, ref int ReturnValue, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(conn, "P_CpsSetBonusScaleSetting", ref ds, ref Outputs,
                new MSSQL.Parameter("LotteryID", SqlDbType.BigInt, 0, ParameterDirection.Input, LotteryID),
                new MSSQL.Parameter("UnionBonusScale", SqlDbType.Float, 0, ParameterDirection.Input, UnionBonusScale),
                new MSSQL.Parameter("SiteBonusScale", SqlDbType.Float, 0, ParameterDirection.Input, SiteBonusScale),
                new MSSQL.Parameter("ReturnValue", SqlDbType.Int, 4, ParameterDirection.Output, ReturnValue),
                new MSSQL.Parameter("ReturnDescription", SqlDbType.VarChar, 100, ParameterDirection.Output, ReturnDescription)
                );

            try
            {
                ReturnValue = System.Convert.ToInt32(Outputs["ReturnValue"]);
            }
            catch { }

            try
            {
                ReturnDescription = System.Convert.ToString(Outputs["ReturnDescription"]);
            }
            catch { }

            return CallResult;
        }

        public static int P_CpsSetBonusScaleType(SqlConnection conn, int OperateType, long TypeID, string Name, long ParentTypeID, ref int ReturnValue, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_CpsSetBonusScaleType(conn, ref ds, OperateType, TypeID, Name, ParentTypeID, ref ReturnValue, ref ReturnDescription);
        }

        public static int P_CpsSetBonusScaleType(SqlConnection conn, ref DataSet ds, int OperateType, long TypeID, string Name, long ParentTypeID, ref int ReturnValue, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(conn, "P_CpsSetBonusScaleType", ref ds, ref Outputs,
                new MSSQL.Parameter("OperateType", SqlDbType.Int, 0, ParameterDirection.Input, OperateType),
                new MSSQL.Parameter("TypeID", SqlDbType.BigInt, 0, ParameterDirection.Input, TypeID),
                new MSSQL.Parameter("Name", SqlDbType.NVarChar, 0, ParameterDirection.Input, Name),
                new MSSQL.Parameter("ParentTypeID", SqlDbType.BigInt, 0, ParameterDirection.Input, ParentTypeID),
                new MSSQL.Parameter("ReturnValue", SqlDbType.Int, 4, ParameterDirection.Output, ReturnValue),
                new MSSQL.Parameter("ReturnDescription", SqlDbType.VarChar, 100, ParameterDirection.Output, ReturnDescription)
                );

            try
            {
                ReturnValue = System.Convert.ToInt32(Outputs["ReturnValue"]);
            }
            catch { }

            try
            {
                ReturnDescription = System.Convert.ToString(Outputs["ReturnDescription"]);
            }
            catch { }

            return CallResult;
        }

        public static int P_CpsSetUserBonusScale(SqlConnection conn, long OwnerUserID, long LotteryID, double BonusScale, ref int ReturnValue, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_CpsSetUserBonusScale(conn, ref ds, OwnerUserID, LotteryID, BonusScale, ref ReturnValue, ref ReturnDescription);
        }

        public static int P_CpsSetUserBonusScale(SqlConnection conn, ref DataSet ds, long OwnerUserID, long LotteryID, double BonusScale, ref int ReturnValue, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(conn, "P_CpsSetUserBonusScale", ref ds, ref Outputs,
                new MSSQL.Parameter("OwnerUserID", SqlDbType.BigInt, 0, ParameterDirection.Input, OwnerUserID),
                new MSSQL.Parameter("LotteryID", SqlDbType.BigInt, 0, ParameterDirection.Input, LotteryID),
                new MSSQL.Parameter("BonusScale", SqlDbType.Float, 0, ParameterDirection.Input, BonusScale),
                new MSSQL.Parameter("ReturnValue", SqlDbType.Int, 4, ParameterDirection.Output, ReturnValue),
                new MSSQL.Parameter("ReturnDescription", SqlDbType.VarChar, 100, ParameterDirection.Output, ReturnDescription)
                );

            try
            {
                ReturnValue = System.Convert.ToInt32(Outputs["ReturnValue"]);
            }
            catch { }

            try
            {
                ReturnDescription = System.Convert.ToString(Outputs["ReturnDescription"]);
            }
            catch { }

            return CallResult;
        }

        public static int P_CpsTry(SqlConnection conn, long SiteID, long UserID, string Content, string Name, string Url, string LogoUrl, string Company, string Address, string PostCode, string ResponsiblePerson, string ContactPerson, string Telephone, string Fax, string Mobile, string Email, string QQ, string ServiceTelephone, string MD5Key, short Type, string DomainName, long ParentID, double BonusScale, long CommendID, ref long ReturnValue, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_CpsTry(conn, ref ds, SiteID, UserID, Content, Name, Url, LogoUrl, Company, Address, PostCode, ResponsiblePerson, ContactPerson, Telephone, Fax, Mobile, Email, QQ, ServiceTelephone, MD5Key, Type, DomainName, ParentID, BonusScale, CommendID, ref ReturnValue, ref ReturnDescription);
        }

        public static int P_CpsTry(SqlConnection conn, ref DataSet ds, long SiteID, long UserID, string Content, string Name, string Url, string LogoUrl, string Company, string Address, string PostCode, string ResponsiblePerson, string ContactPerson, string Telephone, string Fax, string Mobile, string Email, string QQ, string ServiceTelephone, string MD5Key, short Type, string DomainName, long ParentID, double BonusScale, long CommendID, ref long ReturnValue, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(conn, "P_CpsTry", ref ds, ref Outputs,
                new MSSQL.Parameter("SiteID", SqlDbType.BigInt, 0, ParameterDirection.Input, SiteID),
                new MSSQL.Parameter("UserID", SqlDbType.BigInt, 0, ParameterDirection.Input, UserID),
                new MSSQL.Parameter("Content", SqlDbType.VarChar, 0, ParameterDirection.Input, Content),
                new MSSQL.Parameter("Name", SqlDbType.VarChar, 0, ParameterDirection.Input, Name),
                new MSSQL.Parameter("Url", SqlDbType.VarChar, 0, ParameterDirection.Input, Url),
                new MSSQL.Parameter("LogoUrl", SqlDbType.VarChar, 0, ParameterDirection.Input, LogoUrl),
                new MSSQL.Parameter("Company", SqlDbType.VarChar, 0, ParameterDirection.Input, Company),
                new MSSQL.Parameter("Address", SqlDbType.VarChar, 0, ParameterDirection.Input, Address),
                new MSSQL.Parameter("PostCode", SqlDbType.VarChar, 0, ParameterDirection.Input, PostCode),
                new MSSQL.Parameter("ResponsiblePerson", SqlDbType.VarChar, 0, ParameterDirection.Input, ResponsiblePerson),
                new MSSQL.Parameter("ContactPerson", SqlDbType.VarChar, 0, ParameterDirection.Input, ContactPerson),
                new MSSQL.Parameter("Telephone", SqlDbType.VarChar, 0, ParameterDirection.Input, Telephone),
                new MSSQL.Parameter("Fax", SqlDbType.VarChar, 0, ParameterDirection.Input, Fax),
                new MSSQL.Parameter("Mobile", SqlDbType.VarChar, 0, ParameterDirection.Input, Mobile),
                new MSSQL.Parameter("Email", SqlDbType.VarChar, 0, ParameterDirection.Input, Email),
                new MSSQL.Parameter("QQ", SqlDbType.VarChar, 0, ParameterDirection.Input, QQ),
                new MSSQL.Parameter("ServiceTelephone", SqlDbType.VarChar, 0, ParameterDirection.Input, ServiceTelephone),
                new MSSQL.Parameter("MD5Key", SqlDbType.VarChar, 0, ParameterDirection.Input, MD5Key),
                new MSSQL.Parameter("Type", SqlDbType.SmallInt, 0, ParameterDirection.Input, Type),
                new MSSQL.Parameter("DomainName", SqlDbType.VarChar, 0, ParameterDirection.Input, DomainName),
                new MSSQL.Parameter("ParentID", SqlDbType.BigInt, 0, ParameterDirection.Input, ParentID),
                new MSSQL.Parameter("BonusScale", SqlDbType.Float, 0, ParameterDirection.Input, BonusScale),
                new MSSQL.Parameter("CommendID", SqlDbType.BigInt, 0, ParameterDirection.Input, CommendID),
                new MSSQL.Parameter("ReturnValue", SqlDbType.BigInt, 8, ParameterDirection.Output, ReturnValue),
                new MSSQL.Parameter("ReturnDescription", SqlDbType.VarChar, 100, ParameterDirection.Output, ReturnDescription)
                );

            try
            {
                ReturnValue = System.Convert.ToInt64(Outputs["ReturnValue"]);
            }
            catch { }

            try
            {
                ReturnDescription = System.Convert.ToString(Outputs["ReturnDescription"]);
            }
            catch { }

            return CallResult;
        }

        public static int P_CpsTryHandle(SqlConnection conn, long SiteID, long TryID, long OperatorID, short HandleResult, double BonusScale, bool ON, ref long CpsID, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_CpsTryHandle(conn, ref ds, SiteID, TryID, OperatorID, HandleResult, BonusScale, ON, ref CpsID, ref ReturnDescription);
        }

        public static int P_CpsTryHandle(SqlConnection conn, ref DataSet ds, long SiteID, long TryID, long OperatorID, short HandleResult, double BonusScale, bool ON, ref long CpsID, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(conn, "P_CpsTryHandle", ref ds, ref Outputs,
                new MSSQL.Parameter("SiteID", SqlDbType.BigInt, 0, ParameterDirection.Input, SiteID),
                new MSSQL.Parameter("TryID", SqlDbType.BigInt, 0, ParameterDirection.Input, TryID),
                new MSSQL.Parameter("OperatorID", SqlDbType.BigInt, 0, ParameterDirection.Input, OperatorID),
                new MSSQL.Parameter("HandleResult", SqlDbType.SmallInt, 0, ParameterDirection.Input, HandleResult),
                new MSSQL.Parameter("BonusScale", SqlDbType.Float, 0, ParameterDirection.Input, BonusScale),
                new MSSQL.Parameter("ON", SqlDbType.Bit, 0, ParameterDirection.Input, ON),
                new MSSQL.Parameter("CpsID", SqlDbType.BigInt, 8, ParameterDirection.Output, CpsID),
                new MSSQL.Parameter("ReturnDescription", SqlDbType.VarChar, 100, ParameterDirection.Output, ReturnDescription)
                );

            try
            {
                CpsID = System.Convert.ToInt64(Outputs["CpsID"]);
            }
            catch { }

            try
            {
                ReturnDescription = System.Convert.ToString(Outputs["ReturnDescription"]);
            }
            catch { }

            return CallResult;
        }

        public static int P_CustomFollowSchemesAdd(SqlConnection conn, long SiteID, long UserID, long FollowSchemeID, double MoneyStart, double MoneyEnd, int BuyShareStart, int BuyShareEnd, short Type, ref int ReturnValue, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_CustomFollowSchemesAdd(conn, ref ds, SiteID, UserID, FollowSchemeID, MoneyStart, MoneyEnd, BuyShareStart, BuyShareEnd, Type, ref ReturnValue, ref ReturnDescription);
        }

        public static int P_CustomFollowSchemesAdd(SqlConnection conn, ref DataSet ds, long SiteID, long UserID, long FollowSchemeID, double MoneyStart, double MoneyEnd, int BuyShareStart, int BuyShareEnd, short Type, ref int ReturnValue, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(conn, "P_CustomFollowSchemesAdd", ref ds, ref Outputs,
                new MSSQL.Parameter("SiteID", SqlDbType.BigInt, 0, ParameterDirection.Input, SiteID),
                new MSSQL.Parameter("UserID", SqlDbType.BigInt, 0, ParameterDirection.Input, UserID),
                new MSSQL.Parameter("FollowSchemeID", SqlDbType.BigInt, 0, ParameterDirection.Input, FollowSchemeID),
                new MSSQL.Parameter("MoneyStart", SqlDbType.Money, 0, ParameterDirection.Input, MoneyStart),
                new MSSQL.Parameter("MoneyEnd", SqlDbType.Money, 0, ParameterDirection.Input, MoneyEnd),
                new MSSQL.Parameter("BuyShareStart", SqlDbType.Int, 0, ParameterDirection.Input, BuyShareStart),
                new MSSQL.Parameter("BuyShareEnd", SqlDbType.Int, 0, ParameterDirection.Input, BuyShareEnd),
                new MSSQL.Parameter("Type", SqlDbType.SmallInt, 0, ParameterDirection.Input, Type),
                new MSSQL.Parameter("ReturnValue", SqlDbType.Int, 4, ParameterDirection.Output, ReturnValue),
                new MSSQL.Parameter("ReturnDescription", SqlDbType.VarChar, 100, ParameterDirection.Output, ReturnDescription)
                );

            try
            {
                ReturnValue = System.Convert.ToInt32(Outputs["ReturnValue"]);
            }
            catch { }

            try
            {
                ReturnDescription = System.Convert.ToString(Outputs["ReturnDescription"]);
            }
            catch { }

            return CallResult;
        }

        public static int P_CustomFollowSchemesDelete(SqlConnection conn, long SiteID, long UserID, long FollowSchemeID, ref int ReturnValue, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_CustomFollowSchemesDelete(conn, ref ds, SiteID, UserID, FollowSchemeID, ref ReturnValue, ref ReturnDescription);
        }

        public static int P_CustomFollowSchemesDelete(SqlConnection conn, ref DataSet ds, long SiteID, long UserID, long FollowSchemeID, ref int ReturnValue, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(conn, "P_CustomFollowSchemesDelete", ref ds, ref Outputs,
                new MSSQL.Parameter("SiteID", SqlDbType.BigInt, 0, ParameterDirection.Input, SiteID),
                new MSSQL.Parameter("UserID", SqlDbType.BigInt, 0, ParameterDirection.Input, UserID),
                new MSSQL.Parameter("FollowSchemeID", SqlDbType.BigInt, 0, ParameterDirection.Input, FollowSchemeID),
                new MSSQL.Parameter("ReturnValue", SqlDbType.Int, 4, ParameterDirection.Output, ReturnValue),
                new MSSQL.Parameter("ReturnDescription", SqlDbType.VarChar, 100, ParameterDirection.Output, ReturnDescription)
                );

            try
            {
                ReturnValue = System.Convert.ToInt32(Outputs["ReturnValue"]);
            }
            catch { }

            try
            {
                ReturnDescription = System.Convert.ToString(Outputs["ReturnDescription"]);
            }
            catch { }

            return CallResult;
        }

        public static int P_Distill(SqlConnection conn, long SiteID, long UserID, int DistillType, double Money, double FormalitiesFees, string BankUserName, string BankName, string BankCardNumber, string AlipayID, string AlipayName, string Memo, bool IsCps, ref int ReturnValue, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_Distill(conn, ref ds, SiteID, UserID, DistillType, Money, FormalitiesFees, BankUserName, BankName, BankCardNumber, AlipayID, AlipayName, Memo, IsCps, ref ReturnValue, ref ReturnDescription);
        }

        public static int P_Distill(SqlConnection conn, ref DataSet ds, long SiteID, long UserID, int DistillType, double Money, double FormalitiesFees, string BankUserName, string BankName, string BankCardNumber, string AlipayID, string AlipayName, string Memo, bool IsCps, ref int ReturnValue, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(conn, "P_Distill", ref ds, ref Outputs,
                new MSSQL.Parameter("SiteID", SqlDbType.BigInt, 0, ParameterDirection.Input, SiteID),
                new MSSQL.Parameter("UserID", SqlDbType.BigInt, 0, ParameterDirection.Input, UserID),
                new MSSQL.Parameter("DistillType", SqlDbType.Int, 0, ParameterDirection.Input, DistillType),
                new MSSQL.Parameter("Money", SqlDbType.Money, 0, ParameterDirection.Input, Money),
                new MSSQL.Parameter("FormalitiesFees", SqlDbType.Money, 0, ParameterDirection.Input, FormalitiesFees),
                new MSSQL.Parameter("BankUserName", SqlDbType.VarChar, 0, ParameterDirection.Input, BankUserName),
                new MSSQL.Parameter("BankName", SqlDbType.VarChar, 0, ParameterDirection.Input, BankName),
                new MSSQL.Parameter("BankCardNumber", SqlDbType.VarChar, 0, ParameterDirection.Input, BankCardNumber),
                new MSSQL.Parameter("AlipayID", SqlDbType.VarChar, 0, ParameterDirection.Input, AlipayID),
                new MSSQL.Parameter("AlipayName", SqlDbType.VarChar, 0, ParameterDirection.Input, AlipayName),
                new MSSQL.Parameter("Memo", SqlDbType.VarChar, 0, ParameterDirection.Input, Memo),
                new MSSQL.Parameter("IsCps", SqlDbType.Bit, 0, ParameterDirection.Input, IsCps),
                new MSSQL.Parameter("ReturnValue", SqlDbType.Int, 4, ParameterDirection.Output, ReturnValue),
                new MSSQL.Parameter("ReturnDescription", SqlDbType.VarChar, 100, ParameterDirection.Output, ReturnDescription)
                );

            try
            {
                ReturnValue = System.Convert.ToInt32(Outputs["ReturnValue"]);
            }
            catch { }

            try
            {
                ReturnDescription = System.Convert.ToString(Outputs["ReturnDescription"]);
            }
            catch { }

            return CallResult;
        }

        public static int P_DistillAccept(SqlConnection conn, long SiteID, long UserID, long DistillID, string PayName, string PayBank, string PayCardNumber, string AlipayID, string AlipayName, string Memo, long HandleOperatorID, ref int ReturnValue, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_DistillAccept(conn, ref ds, SiteID, UserID, DistillID, PayName, PayBank, PayCardNumber, AlipayID, AlipayName, Memo, HandleOperatorID, ref ReturnValue, ref ReturnDescription);
        }

        public static int P_DistillAccept(SqlConnection conn, ref DataSet ds, long SiteID, long UserID, long DistillID, string PayName, string PayBank, string PayCardNumber, string AlipayID, string AlipayName, string Memo, long HandleOperatorID, ref int ReturnValue, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(conn, "P_DistillAccept", ref ds, ref Outputs,
                new MSSQL.Parameter("SiteID", SqlDbType.BigInt, 0, ParameterDirection.Input, SiteID),
                new MSSQL.Parameter("UserID", SqlDbType.BigInt, 0, ParameterDirection.Input, UserID),
                new MSSQL.Parameter("DistillID", SqlDbType.BigInt, 0, ParameterDirection.Input, DistillID),
                new MSSQL.Parameter("PayName", SqlDbType.VarChar, 0, ParameterDirection.Input, PayName),
                new MSSQL.Parameter("PayBank", SqlDbType.VarChar, 0, ParameterDirection.Input, PayBank),
                new MSSQL.Parameter("PayCardNumber", SqlDbType.VarChar, 0, ParameterDirection.Input, PayCardNumber),
                new MSSQL.Parameter("AlipayID", SqlDbType.VarChar, 0, ParameterDirection.Input, AlipayID),
                new MSSQL.Parameter("AlipayName", SqlDbType.VarChar, 0, ParameterDirection.Input, AlipayName),
                new MSSQL.Parameter("Memo", SqlDbType.VarChar, 0, ParameterDirection.Input, Memo),
                new MSSQL.Parameter("HandleOperatorID", SqlDbType.BigInt, 0, ParameterDirection.Input, HandleOperatorID),
                new MSSQL.Parameter("ReturnValue", SqlDbType.Int, 4, ParameterDirection.Output, ReturnValue),
                new MSSQL.Parameter("ReturnDescription", SqlDbType.VarChar, 100, ParameterDirection.Output, ReturnDescription)
                );

            try
            {
                ReturnValue = System.Convert.ToInt32(Outputs["ReturnValue"]);
            }
            catch { }

            try
            {
                ReturnDescription = System.Convert.ToString(Outputs["ReturnDescription"]);
            }
            catch { }

            return CallResult;
        }

        public static int P_DistillNoAccept(SqlConnection conn, long SiteID, long UserID, long DistillID, string Memo, long HandleOperatorID, ref int ReturnValue, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_DistillNoAccept(conn, ref ds, SiteID, UserID, DistillID, Memo, HandleOperatorID, ref ReturnValue, ref ReturnDescription);
        }

        public static int P_DistillNoAccept(SqlConnection conn, ref DataSet ds, long SiteID, long UserID, long DistillID, string Memo, long HandleOperatorID, ref int ReturnValue, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(conn, "P_DistillNoAccept", ref ds, ref Outputs,
                new MSSQL.Parameter("SiteID", SqlDbType.BigInt, 0, ParameterDirection.Input, SiteID),
                new MSSQL.Parameter("UserID", SqlDbType.BigInt, 0, ParameterDirection.Input, UserID),
                new MSSQL.Parameter("DistillID", SqlDbType.BigInt, 0, ParameterDirection.Input, DistillID),
                new MSSQL.Parameter("Memo", SqlDbType.VarChar, 0, ParameterDirection.Input, Memo),
                new MSSQL.Parameter("HandleOperatorID", SqlDbType.BigInt, 0, ParameterDirection.Input, HandleOperatorID),
                new MSSQL.Parameter("ReturnValue", SqlDbType.Int, 4, ParameterDirection.Output, ReturnValue),
                new MSSQL.Parameter("ReturnDescription", SqlDbType.VarChar, 100, ParameterDirection.Output, ReturnDescription)
                );

            try
            {
                ReturnValue = System.Convert.ToInt32(Outputs["ReturnValue"]);
            }
            catch { }

            try
            {
                ReturnDescription = System.Convert.ToString(Outputs["ReturnDescription"]);
            }
            catch { }

            return CallResult;
        }

        public static int P_DistillPaySuccess(SqlConnection conn, long SiteID, long UserID, long DistillID, string Memo, long HandleOperatorID, ref int ReturnValue, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_DistillPaySuccess(conn, ref ds, SiteID, UserID, DistillID, Memo, HandleOperatorID, ref ReturnValue, ref ReturnDescription);
        }

        public static int P_DistillPaySuccess(SqlConnection conn, ref DataSet ds, long SiteID, long UserID, long DistillID, string Memo, long HandleOperatorID, ref int ReturnValue, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(conn, "P_DistillPaySuccess", ref ds, ref Outputs,
                new MSSQL.Parameter("SiteID", SqlDbType.BigInt, 0, ParameterDirection.Input, SiteID),
                new MSSQL.Parameter("UserID", SqlDbType.BigInt, 0, ParameterDirection.Input, UserID),
                new MSSQL.Parameter("DistillID", SqlDbType.BigInt, 0, ParameterDirection.Input, DistillID),
                new MSSQL.Parameter("Memo", SqlDbType.VarChar, 0, ParameterDirection.Input, Memo),
                new MSSQL.Parameter("HandleOperatorID", SqlDbType.BigInt, 0, ParameterDirection.Input, HandleOperatorID),
                new MSSQL.Parameter("ReturnValue", SqlDbType.Int, 4, ParameterDirection.Output, ReturnValue),
                new MSSQL.Parameter("ReturnDescription", SqlDbType.VarChar, 100, ParameterDirection.Output, ReturnDescription)
                );

            try
            {
                ReturnValue = System.Convert.ToInt32(Outputs["ReturnValue"]);
            }
            catch { }

            try
            {
                ReturnDescription = System.Convert.ToString(Outputs["ReturnDescription"]);
            }
            catch { }

            return CallResult;
        }

        public static int P_DistillQuash(SqlConnection conn, long SiteID, long UserID, long DistillID, ref int ReturnValue, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_DistillQuash(conn, ref ds, SiteID, UserID, DistillID, ref ReturnValue, ref ReturnDescription);
        }

        public static int P_DistillQuash(SqlConnection conn, ref DataSet ds, long SiteID, long UserID, long DistillID, ref int ReturnValue, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(conn, "P_DistillQuash", ref ds, ref Outputs,
                new MSSQL.Parameter("SiteID", SqlDbType.BigInt, 0, ParameterDirection.Input, SiteID),
                new MSSQL.Parameter("UserID", SqlDbType.BigInt, 0, ParameterDirection.Input, UserID),
                new MSSQL.Parameter("DistillID", SqlDbType.BigInt, 0, ParameterDirection.Input, DistillID),
                new MSSQL.Parameter("ReturnValue", SqlDbType.Int, 4, ParameterDirection.Output, ReturnValue),
                new MSSQL.Parameter("ReturnDescription", SqlDbType.VarChar, 100, ParameterDirection.Output, ReturnDescription)
                );

            try
            {
                ReturnValue = System.Convert.ToInt32(Outputs["ReturnValue"]);
            }
            catch { }

            try
            {
                ReturnDescription = System.Convert.ToString(Outputs["ReturnDescription"]);
            }
            catch { }

            return CallResult;
        }

        public static int P_DownloadAdd(SqlConnection conn, long SiteID, DateTime DateTime, string Title, string FileUrl, bool isShow, ref long NewDownloadID, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_DownloadAdd(conn, ref ds, SiteID, DateTime, Title, FileUrl, isShow, ref NewDownloadID, ref ReturnDescription);
        }

        public static int P_DownloadAdd(SqlConnection conn, ref DataSet ds, long SiteID, DateTime DateTime, string Title, string FileUrl, bool isShow, ref long NewDownloadID, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(conn, "P_DownloadAdd", ref ds, ref Outputs,
                new MSSQL.Parameter("SiteID", SqlDbType.BigInt, 0, ParameterDirection.Input, SiteID),
                new MSSQL.Parameter("DateTime", SqlDbType.DateTime, 0, ParameterDirection.Input, DateTime),
                new MSSQL.Parameter("Title", SqlDbType.VarChar, 0, ParameterDirection.Input, Title),
                new MSSQL.Parameter("FileUrl", SqlDbType.VarChar, 0, ParameterDirection.Input, FileUrl),
                new MSSQL.Parameter("isShow", SqlDbType.Bit, 0, ParameterDirection.Input, isShow),
                new MSSQL.Parameter("NewDownloadID", SqlDbType.BigInt, 8, ParameterDirection.Output, NewDownloadID),
                new MSSQL.Parameter("ReturnDescription", SqlDbType.VarChar, 100, ParameterDirection.Output, ReturnDescription)
                );

            try
            {
                NewDownloadID = System.Convert.ToInt64(Outputs["NewDownloadID"]);
            }
            catch { }

            try
            {
                ReturnDescription = System.Convert.ToString(Outputs["ReturnDescription"]);
            }
            catch { }

            return CallResult;
        }

        public static int P_DownloadDelete(SqlConnection conn, long SiteID, long ID, ref int ReturnValue, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_DownloadDelete(conn, ref ds, SiteID, ID, ref ReturnValue, ref ReturnDescription);
        }

        public static int P_DownloadDelete(SqlConnection conn, ref DataSet ds, long SiteID, long ID, ref int ReturnValue, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(conn, "P_DownloadDelete", ref ds, ref Outputs,
                new MSSQL.Parameter("SiteID", SqlDbType.BigInt, 0, ParameterDirection.Input, SiteID),
                new MSSQL.Parameter("ID", SqlDbType.BigInt, 0, ParameterDirection.Input, ID),
                new MSSQL.Parameter("ReturnValue", SqlDbType.Int, 4, ParameterDirection.Output, ReturnValue),
                new MSSQL.Parameter("ReturnDescription", SqlDbType.VarChar, 100, ParameterDirection.Output, ReturnDescription)
                );

            try
            {
                ReturnValue = System.Convert.ToInt32(Outputs["ReturnValue"]);
            }
            catch { }

            try
            {
                ReturnDescription = System.Convert.ToString(Outputs["ReturnDescription"]);
            }
            catch { }

            return CallResult;
        }

        public static int P_DownloadEdit(SqlConnection conn, long SiteID, long ID, DateTime DateTime, string Title, string FileUrl, bool isShow, ref int ReturnValue, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_DownloadEdit(conn, ref ds, SiteID, ID, DateTime, Title, FileUrl, isShow, ref ReturnValue, ref ReturnDescription);
        }

        public static int P_DownloadEdit(SqlConnection conn, ref DataSet ds, long SiteID, long ID, DateTime DateTime, string Title, string FileUrl, bool isShow, ref int ReturnValue, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(conn, "P_DownloadEdit", ref ds, ref Outputs,
                new MSSQL.Parameter("SiteID", SqlDbType.BigInt, 0, ParameterDirection.Input, SiteID),
                new MSSQL.Parameter("ID", SqlDbType.BigInt, 0, ParameterDirection.Input, ID),
                new MSSQL.Parameter("DateTime", SqlDbType.DateTime, 0, ParameterDirection.Input, DateTime),
                new MSSQL.Parameter("Title", SqlDbType.VarChar, 0, ParameterDirection.Input, Title),
                new MSSQL.Parameter("FileUrl", SqlDbType.VarChar, 0, ParameterDirection.Input, FileUrl),
                new MSSQL.Parameter("isShow", SqlDbType.Bit, 0, ParameterDirection.Input, isShow),
                new MSSQL.Parameter("ReturnValue", SqlDbType.Int, 4, ParameterDirection.Output, ReturnValue),
                new MSSQL.Parameter("ReturnDescription", SqlDbType.VarChar, 100, ParameterDirection.Output, ReturnDescription)
                );

            try
            {
                ReturnValue = System.Convert.ToInt32(Outputs["ReturnValue"]);
            }
            catch { }

            try
            {
                ReturnDescription = System.Convert.ToString(Outputs["ReturnDescription"]);
            }
            catch { }

            return CallResult;
        }

        public static int P_ElectronTicketAgentAddMoney(SqlConnection conn, long AgentID, double Amount, string Memo, ref int ReturnValue, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_ElectronTicketAgentAddMoney(conn, ref ds, AgentID, Amount, Memo, ref ReturnValue, ref ReturnDescription);
        }

        public static int P_ElectronTicketAgentAddMoney(SqlConnection conn, ref DataSet ds, long AgentID, double Amount, string Memo, ref int ReturnValue, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(conn, "P_ElectronTicketAgentAddMoney", ref ds, ref Outputs,
                new MSSQL.Parameter("AgentID", SqlDbType.BigInt, 0, ParameterDirection.Input, AgentID),
                new MSSQL.Parameter("Amount", SqlDbType.Money, 0, ParameterDirection.Input, Amount),
                new MSSQL.Parameter("Memo", SqlDbType.VarChar, 0, ParameterDirection.Input, Memo),
                new MSSQL.Parameter("ReturnValue", SqlDbType.Int, 4, ParameterDirection.Output, ReturnValue),
                new MSSQL.Parameter("ReturnDescription", SqlDbType.VarChar, 100, ParameterDirection.Output, ReturnDescription)
                );

            try
            {
                ReturnValue = System.Convert.ToInt32(Outputs["ReturnValue"]);
            }
            catch { }

            try
            {
                ReturnDescription = System.Convert.ToString(Outputs["ReturnDescription"]);
            }
            catch { }

            return CallResult;
        }

        public static int P_ElectronTicketAgentSchemeAdd(SqlConnection conn, long AgentID, string SchemeNumber, int LotteryID, int PlayTypeID, long IsuseID, string LotteryNumber, double Amount, int Multiple, int Share, string InitiateName, string InitiateAlipayName, string InitiateAlipayID, string InitiateRealityName, string InitiateIDCard, string InitiateTelephone, string InitiateMobile, string InitiateEmail, double InitiateBonusScale, double InitiateBonusLimitLower, double InitiateBonusLimitUpper, string DetailXML, ref long SchemeID, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_ElectronTicketAgentSchemeAdd(conn, ref ds, AgentID, SchemeNumber, LotteryID, PlayTypeID, IsuseID, LotteryNumber, Amount, Multiple, Share, InitiateName, InitiateAlipayName, InitiateAlipayID, InitiateRealityName, InitiateIDCard, InitiateTelephone, InitiateMobile, InitiateEmail, InitiateBonusScale, InitiateBonusLimitLower, InitiateBonusLimitUpper, DetailXML, ref SchemeID, ref ReturnDescription);
        }

        public static int P_ElectronTicketAgentSchemeAdd(SqlConnection conn, ref DataSet ds, long AgentID, string SchemeNumber, int LotteryID, int PlayTypeID, long IsuseID, string LotteryNumber, double Amount, int Multiple, int Share, string InitiateName, string InitiateAlipayName, string InitiateAlipayID, string InitiateRealityName, string InitiateIDCard, string InitiateTelephone, string InitiateMobile, string InitiateEmail, double InitiateBonusScale, double InitiateBonusLimitLower, double InitiateBonusLimitUpper, string DetailXML, ref long SchemeID, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(conn, "P_ElectronTicketAgentSchemeAdd", ref ds, ref Outputs,
                new MSSQL.Parameter("AgentID", SqlDbType.BigInt, 0, ParameterDirection.Input, AgentID),
                new MSSQL.Parameter("SchemeNumber", SqlDbType.VarChar, 0, ParameterDirection.Input, SchemeNumber),
                new MSSQL.Parameter("LotteryID", SqlDbType.Int, 0, ParameterDirection.Input, LotteryID),
                new MSSQL.Parameter("PlayTypeID", SqlDbType.Int, 0, ParameterDirection.Input, PlayTypeID),
                new MSSQL.Parameter("IsuseID", SqlDbType.BigInt, 0, ParameterDirection.Input, IsuseID),
                new MSSQL.Parameter("LotteryNumber", SqlDbType.VarChar, 0, ParameterDirection.Input, LotteryNumber),
                new MSSQL.Parameter("Amount", SqlDbType.Money, 0, ParameterDirection.Input, Amount),
                new MSSQL.Parameter("Multiple", SqlDbType.Int, 0, ParameterDirection.Input, Multiple),
                new MSSQL.Parameter("Share", SqlDbType.Int, 0, ParameterDirection.Input, Share),
                new MSSQL.Parameter("InitiateName", SqlDbType.VarChar, 0, ParameterDirection.Input, InitiateName),
                new MSSQL.Parameter("InitiateAlipayName", SqlDbType.VarChar, 0, ParameterDirection.Input, InitiateAlipayName),
                new MSSQL.Parameter("InitiateAlipayID", SqlDbType.VarChar, 0, ParameterDirection.Input, InitiateAlipayID),
                new MSSQL.Parameter("InitiateRealityName", SqlDbType.VarChar, 0, ParameterDirection.Input, InitiateRealityName),
                new MSSQL.Parameter("InitiateIDCard", SqlDbType.VarChar, 0, ParameterDirection.Input, InitiateIDCard),
                new MSSQL.Parameter("InitiateTelephone", SqlDbType.VarChar, 0, ParameterDirection.Input, InitiateTelephone),
                new MSSQL.Parameter("InitiateMobile", SqlDbType.VarChar, 0, ParameterDirection.Input, InitiateMobile),
                new MSSQL.Parameter("InitiateEmail", SqlDbType.VarChar, 0, ParameterDirection.Input, InitiateEmail),
                new MSSQL.Parameter("InitiateBonusScale", SqlDbType.Float, 0, ParameterDirection.Input, InitiateBonusScale),
                new MSSQL.Parameter("InitiateBonusLimitLower", SqlDbType.Money, 0, ParameterDirection.Input, InitiateBonusLimitLower),
                new MSSQL.Parameter("InitiateBonusLimitUpper", SqlDbType.Money, 0, ParameterDirection.Input, InitiateBonusLimitUpper),
                new MSSQL.Parameter("DetailXML", SqlDbType.NText, 0, ParameterDirection.Input, DetailXML),
                new MSSQL.Parameter("SchemeID", SqlDbType.BigInt, 8, ParameterDirection.Output, SchemeID),
                new MSSQL.Parameter("ReturnDescription", SqlDbType.VarChar, 100, ParameterDirection.Output, ReturnDescription)
                );

            try
            {
                SchemeID = System.Convert.ToInt64(Outputs["SchemeID"]);
            }
            catch { }

            try
            {
                ReturnDescription = System.Convert.ToString(Outputs["ReturnDescription"]);
            }
            catch { }

            return CallResult;
        }

        public static int P_ElectronTicketAgentSchemeQuash(SqlConnection conn, long IsuseID, ref int ReturnValue, ref string ReturnDescptrion)
        {
            DataSet ds = null;

            return P_ElectronTicketAgentSchemeQuash(conn, ref ds, IsuseID, ref ReturnValue, ref ReturnDescptrion);
        }

        public static int P_ElectronTicketAgentSchemeQuash(SqlConnection conn, ref DataSet ds, long IsuseID, ref int ReturnValue, ref string ReturnDescptrion)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(conn, "P_ElectronTicketAgentSchemeQuash", ref ds, ref Outputs,
                new MSSQL.Parameter("IsuseID", SqlDbType.BigInt, 0, ParameterDirection.Input, IsuseID),
                new MSSQL.Parameter("ReturnValue", SqlDbType.Int, 4, ParameterDirection.Output, ReturnValue),
                new MSSQL.Parameter("ReturnDescptrion", SqlDbType.NVarChar, 200, ParameterDirection.Output, ReturnDescptrion)
                );

            try
            {
                ReturnValue = System.Convert.ToInt32(Outputs["ReturnValue"]);
            }
            catch { }

            try
            {
                ReturnDescptrion = System.Convert.ToString(Outputs["ReturnDescptrion"]);
            }
            catch { }

            return CallResult;
        }

        public static int P_ElectronTicketAgentSchemeSendToCenterAdd(SqlConnection conn, long SchemeID, int PlayTypeID, string AgrentNo, string TicketXML, ref int ReturnValue, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_ElectronTicketAgentSchemeSendToCenterAdd(conn, ref ds, SchemeID, PlayTypeID, AgrentNo, TicketXML, ref ReturnValue, ref ReturnDescription);
        }

        public static int P_ElectronTicketAgentSchemeSendToCenterAdd(SqlConnection conn, ref DataSet ds, long SchemeID, int PlayTypeID, string AgrentNo, string TicketXML, ref int ReturnValue, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(conn, "P_ElectronTicketAgentSchemeSendToCenterAdd", ref ds, ref Outputs,
                new MSSQL.Parameter("SchemeID", SqlDbType.BigInt, 0, ParameterDirection.Input, SchemeID),
                new MSSQL.Parameter("PlayTypeID", SqlDbType.Int, 0, ParameterDirection.Input, PlayTypeID),
                new MSSQL.Parameter("AgrentNo", SqlDbType.NVarChar, 0, ParameterDirection.Input, AgrentNo),
                new MSSQL.Parameter("TicketXML", SqlDbType.NText, 0, ParameterDirection.Input, TicketXML),
                new MSSQL.Parameter("ReturnValue", SqlDbType.Int, 4, ParameterDirection.Output, ReturnValue),
                new MSSQL.Parameter("ReturnDescription", SqlDbType.VarChar, 100, ParameterDirection.Output, ReturnDescription)
                );

            try
            {
                ReturnValue = System.Convert.ToInt32(Outputs["ReturnValue"]);
            }
            catch { }

            try
            {
                ReturnDescription = System.Convert.ToString(Outputs["ReturnDescription"]);
            }
            catch { }

            return CallResult;
        }

        public static int P_ElectronTicketAgentSchemeSendToCenterAdd_Single(SqlConnection conn, long SchemeID, int PlayTypeID, double Money, int Multiple, string Ticket, bool isFirstWrite, string AgrentNo, ref long ID, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_ElectronTicketAgentSchemeSendToCenterAdd_Single(conn, ref ds, SchemeID, PlayTypeID, Money, Multiple, Ticket, isFirstWrite, AgrentNo, ref ID, ref ReturnDescription);
        }

        public static int P_ElectronTicketAgentSchemeSendToCenterAdd_Single(SqlConnection conn, ref DataSet ds, long SchemeID, int PlayTypeID, double Money, int Multiple, string Ticket, bool isFirstWrite, string AgrentNo, ref long ID, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(conn, "P_ElectronTicketAgentSchemeSendToCenterAdd_Single", ref ds, ref Outputs,
                new MSSQL.Parameter("SchemeID", SqlDbType.BigInt, 0, ParameterDirection.Input, SchemeID),
                new MSSQL.Parameter("PlayTypeID", SqlDbType.Int, 0, ParameterDirection.Input, PlayTypeID),
                new MSSQL.Parameter("Money", SqlDbType.Money, 0, ParameterDirection.Input, Money),
                new MSSQL.Parameter("Multiple", SqlDbType.Int, 0, ParameterDirection.Input, Multiple),
                new MSSQL.Parameter("Ticket", SqlDbType.VarChar, 0, ParameterDirection.Input, Ticket),
                new MSSQL.Parameter("isFirstWrite", SqlDbType.Bit, 0, ParameterDirection.Input, isFirstWrite),
                new MSSQL.Parameter("AgrentNo", SqlDbType.VarChar, 0, ParameterDirection.Input, AgrentNo),
                new MSSQL.Parameter("ID", SqlDbType.BigInt, 8, ParameterDirection.Output, ID),
                new MSSQL.Parameter("ReturnDescription", SqlDbType.VarChar, 100, ParameterDirection.Output, ReturnDescription)
                );

            try
            {
                ID = System.Convert.ToInt64(Outputs["ID"]);
            }
            catch { }

            try
            {
                ReturnDescription = System.Convert.ToString(Outputs["ReturnDescription"]);
            }
            catch { }

            return CallResult;
        }

        public static int P_ElectronTicketAgentSchemesSendToCenterHandleUniteAnte(SqlConnection conn, long SchemeID, DateTime DealTime, ref int ReturnValue, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_ElectronTicketAgentSchemesSendToCenterHandleUniteAnte(conn, ref ds, SchemeID, DealTime, ref ReturnValue, ref ReturnDescription);
        }

        public static int P_ElectronTicketAgentSchemesSendToCenterHandleUniteAnte(SqlConnection conn, ref DataSet ds, long SchemeID, DateTime DealTime, ref int ReturnValue, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(conn, "P_ElectronTicketAgentSchemesSendToCenterHandleUniteAnte", ref ds, ref Outputs,
                new MSSQL.Parameter("SchemeID", SqlDbType.BigInt, 0, ParameterDirection.Input, SchemeID),
                new MSSQL.Parameter("DealTime", SqlDbType.DateTime, 0, ParameterDirection.Input, DealTime),
                new MSSQL.Parameter("ReturnValue", SqlDbType.Int, 4, ParameterDirection.Output, ReturnValue),
                new MSSQL.Parameter("ReturnDescription", SqlDbType.VarChar, 100, ParameterDirection.Output, ReturnDescription)
                );

            try
            {
                ReturnValue = System.Convert.ToInt32(Outputs["ReturnValue"]);
            }
            catch { }

            try
            {
                ReturnDescription = System.Convert.ToString(Outputs["ReturnDescription"]);
            }
            catch { }

            return CallResult;
        }

        public static int P_ElectronTicketWin(SqlConnection conn, long IsuseID, string BonusXML, string AgentBonusXML, ref int ReturnValue, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_ElectronTicketWin(conn, ref ds, IsuseID, BonusXML, AgentBonusXML, ref ReturnValue, ref ReturnDescription);
        }

        public static int P_ElectronTicketWin(SqlConnection conn, ref DataSet ds, long IsuseID, string BonusXML, string AgentBonusXML, ref int ReturnValue, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(conn, "P_ElectronTicketWin", ref ds, ref Outputs,
                new MSSQL.Parameter("IsuseID", SqlDbType.BigInt, 0, ParameterDirection.Input, IsuseID),
                new MSSQL.Parameter("BonusXML", SqlDbType.NText, 0, ParameterDirection.Input, BonusXML),
                new MSSQL.Parameter("AgentBonusXML", SqlDbType.NText, 0, ParameterDirection.Input, AgentBonusXML),
                new MSSQL.Parameter("ReturnValue", SqlDbType.Int, 4, ParameterDirection.Output, ReturnValue),
                new MSSQL.Parameter("ReturnDescription", SqlDbType.VarChar, 100, ParameterDirection.Output, ReturnDescription)
                );

            try
            {
                ReturnValue = System.Convert.ToInt32(Outputs["ReturnValue"]);
            }
            catch { }

            try
            {
                ReturnDescription = System.Convert.ToString(Outputs["ReturnDescription"]);
            }
            catch { }

            return CallResult;
        }

        public static int P_EnterSchemeChatRoom(SqlConnection conn, long SiteID, long UserID, long SchemeID, ref int ReturnValue, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_EnterSchemeChatRoom(conn, ref ds, SiteID, UserID, SchemeID, ref ReturnValue, ref ReturnDescription);
        }

        public static int P_EnterSchemeChatRoom(SqlConnection conn, ref DataSet ds, long SiteID, long UserID, long SchemeID, ref int ReturnValue, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(conn, "P_EnterSchemeChatRoom", ref ds, ref Outputs,
                new MSSQL.Parameter("SiteID", SqlDbType.BigInt, 0, ParameterDirection.Input, SiteID),
                new MSSQL.Parameter("UserID", SqlDbType.BigInt, 0, ParameterDirection.Input, UserID),
                new MSSQL.Parameter("SchemeID", SqlDbType.BigInt, 0, ParameterDirection.Input, SchemeID),
                new MSSQL.Parameter("ReturnValue", SqlDbType.Int, 4, ParameterDirection.Output, ReturnValue),
                new MSSQL.Parameter("ReturnDescription", SqlDbType.VarChar, 100, ParameterDirection.Output, ReturnDescription)
                );

            try
            {
                ReturnValue = System.Convert.ToInt32(Outputs["ReturnValue"]);
            }
            catch { }

            try
            {
                ReturnDescription = System.Convert.ToString(Outputs["ReturnDescription"]);
            }
            catch { }

            return CallResult;
        }

        public static int P_ExecChases(SqlConnection conn, ref int ReturnValue, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_ExecChases(conn, ref ds, ref ReturnValue, ref ReturnDescription);
        }

        public static int P_ExecChases(SqlConnection conn, ref DataSet ds, ref int ReturnValue, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(conn, "P_ExecChases", ref ds, ref Outputs,
                new MSSQL.Parameter("ReturnValue", SqlDbType.Int, 4, ParameterDirection.Output, ReturnValue),
                new MSSQL.Parameter("ReturnDescription", SqlDbType.VarChar, 100, ParameterDirection.Output, ReturnDescription)
                );

            try
            {
                ReturnValue = System.Convert.ToInt32(Outputs["ReturnValue"]);
            }
            catch { }

            try
            {
                ReturnDescription = System.Convert.ToString(Outputs["ReturnDescription"]);
            }
            catch { }

            return CallResult;
        }

        public static int P_ExecChaseTaskDetail(SqlConnection conn, long SiteID, long ChaseTaskDetailID, ref int ReturnValue, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_ExecChaseTaskDetail(conn, ref ds, SiteID, ChaseTaskDetailID, ref ReturnValue, ref ReturnDescription);
        }

        public static int P_ExecChaseTaskDetail(SqlConnection conn, ref DataSet ds, long SiteID, long ChaseTaskDetailID, ref int ReturnValue, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(conn, "P_ExecChaseTaskDetail", ref ds, ref Outputs,
                new MSSQL.Parameter("SiteID", SqlDbType.BigInt, 0, ParameterDirection.Input, SiteID),
                new MSSQL.Parameter("ChaseTaskDetailID", SqlDbType.BigInt, 0, ParameterDirection.Input, ChaseTaskDetailID),
                new MSSQL.Parameter("ReturnValue", SqlDbType.Int, 4, ParameterDirection.Output, ReturnValue),
                new MSSQL.Parameter("ReturnDescription", SqlDbType.VarChar, 100, ParameterDirection.Output, ReturnDescription)
                );

            try
            {
                ReturnValue = System.Convert.ToInt32(Outputs["ReturnValue"]);
            }
            catch { }

            try
            {
                ReturnDescription = System.Convert.ToString(Outputs["ReturnDescription"]);
            }
            catch { }

            return CallResult;
        }

        public static int P_ExecChaseTasks(SqlConnection conn, ref int ReturnValue, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_ExecChaseTasks(conn, ref ds, ref ReturnValue, ref ReturnDescription);
        }

        public static int P_ExecChaseTasks(SqlConnection conn, ref DataSet ds, ref int ReturnValue, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(conn, "P_ExecChaseTasks", ref ds, ref Outputs,
                new MSSQL.Parameter("ReturnValue", SqlDbType.Int, 4, ParameterDirection.Output, ReturnValue),
                new MSSQL.Parameter("ReturnDescription", SqlDbType.VarChar, 100, ParameterDirection.Output, ReturnDescription)
                );

            try
            {
                ReturnValue = System.Convert.ToInt32(Outputs["ReturnValue"]);
            }
            catch { }

            try
            {
                ReturnDescription = System.Convert.ToString(Outputs["ReturnDescription"]);
            }
            catch { }

            return CallResult;
        }

        public static int P_ExpertsCommendAdd(SqlConnection conn, long SiteID, long ExpertsID, DateTime DateTime, long IsuseID, string Title, string Content, string Number, double Price, ref long NewExpertsCommendID, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_ExpertsCommendAdd(conn, ref ds, SiteID, ExpertsID, DateTime, IsuseID, Title, Content, Number, Price, ref NewExpertsCommendID, ref ReturnDescription);
        }

        public static int P_ExpertsCommendAdd(SqlConnection conn, ref DataSet ds, long SiteID, long ExpertsID, DateTime DateTime, long IsuseID, string Title, string Content, string Number, double Price, ref long NewExpertsCommendID, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(conn, "P_ExpertsCommendAdd", ref ds, ref Outputs,
                new MSSQL.Parameter("SiteID", SqlDbType.BigInt, 0, ParameterDirection.Input, SiteID),
                new MSSQL.Parameter("ExpertsID", SqlDbType.BigInt, 0, ParameterDirection.Input, ExpertsID),
                new MSSQL.Parameter("DateTime", SqlDbType.DateTime, 0, ParameterDirection.Input, DateTime),
                new MSSQL.Parameter("IsuseID", SqlDbType.BigInt, 0, ParameterDirection.Input, IsuseID),
                new MSSQL.Parameter("Title", SqlDbType.VarChar, 0, ParameterDirection.Input, Title),
                new MSSQL.Parameter("Content", SqlDbType.VarChar, 0, ParameterDirection.Input, Content),
                new MSSQL.Parameter("Number", SqlDbType.VarChar, 0, ParameterDirection.Input, Number),
                new MSSQL.Parameter("Price", SqlDbType.Money, 0, ParameterDirection.Input, Price),
                new MSSQL.Parameter("NewExpertsCommendID", SqlDbType.BigInt, 8, ParameterDirection.Output, NewExpertsCommendID),
                new MSSQL.Parameter("ReturnDescription", SqlDbType.VarChar, 100, ParameterDirection.Output, ReturnDescription)
                );

            try
            {
                NewExpertsCommendID = System.Convert.ToInt64(Outputs["NewExpertsCommendID"]);
            }
            catch { }

            try
            {
                ReturnDescription = System.Convert.ToString(Outputs["ReturnDescription"]);
            }
            catch { }

            return CallResult;
        }

        public static int P_ExpertsCommendDelete(SqlConnection conn, long SiteID, long ExpertsCommendID, ref int ReturnValue, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_ExpertsCommendDelete(conn, ref ds, SiteID, ExpertsCommendID, ref ReturnValue, ref ReturnDescription);
        }

        public static int P_ExpertsCommendDelete(SqlConnection conn, ref DataSet ds, long SiteID, long ExpertsCommendID, ref int ReturnValue, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(conn, "P_ExpertsCommendDelete", ref ds, ref Outputs,
                new MSSQL.Parameter("SiteID", SqlDbType.BigInt, 0, ParameterDirection.Input, SiteID),
                new MSSQL.Parameter("ExpertsCommendID", SqlDbType.BigInt, 0, ParameterDirection.Input, ExpertsCommendID),
                new MSSQL.Parameter("ReturnValue", SqlDbType.Int, 4, ParameterDirection.Output, ReturnValue),
                new MSSQL.Parameter("ReturnDescription", SqlDbType.VarChar, 100, ParameterDirection.Output, ReturnDescription)
                );

            try
            {
                ReturnValue = System.Convert.ToInt32(Outputs["ReturnValue"]);
            }
            catch { }

            try
            {
                ReturnDescription = System.Convert.ToString(Outputs["ReturnDescription"]);
            }
            catch { }

            return CallResult;
        }

        public static int P_ExpertsCommendEdit(SqlConnection conn, long SiteID, long ExpertsCommendID, DateTime DateTime, long IsuseID, string Title, string Content, string Number, double Price, double WinMoney, bool isCommend, ref int ReturnValue, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_ExpertsCommendEdit(conn, ref ds, SiteID, ExpertsCommendID, DateTime, IsuseID, Title, Content, Number, Price, WinMoney, isCommend, ref ReturnValue, ref ReturnDescription);
        }

        public static int P_ExpertsCommendEdit(SqlConnection conn, ref DataSet ds, long SiteID, long ExpertsCommendID, DateTime DateTime, long IsuseID, string Title, string Content, string Number, double Price, double WinMoney, bool isCommend, ref int ReturnValue, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(conn, "P_ExpertsCommendEdit", ref ds, ref Outputs,
                new MSSQL.Parameter("SiteID", SqlDbType.BigInt, 0, ParameterDirection.Input, SiteID),
                new MSSQL.Parameter("ExpertsCommendID", SqlDbType.BigInt, 0, ParameterDirection.Input, ExpertsCommendID),
                new MSSQL.Parameter("DateTime", SqlDbType.DateTime, 0, ParameterDirection.Input, DateTime),
                new MSSQL.Parameter("IsuseID", SqlDbType.BigInt, 0, ParameterDirection.Input, IsuseID),
                new MSSQL.Parameter("Title", SqlDbType.VarChar, 0, ParameterDirection.Input, Title),
                new MSSQL.Parameter("Content", SqlDbType.VarChar, 0, ParameterDirection.Input, Content),
                new MSSQL.Parameter("Number", SqlDbType.VarChar, 0, ParameterDirection.Input, Number),
                new MSSQL.Parameter("Price", SqlDbType.Money, 0, ParameterDirection.Input, Price),
                new MSSQL.Parameter("WinMoney", SqlDbType.Money, 0, ParameterDirection.Input, WinMoney),
                new MSSQL.Parameter("isCommend", SqlDbType.Bit, 0, ParameterDirection.Input, isCommend),
                new MSSQL.Parameter("ReturnValue", SqlDbType.Int, 4, ParameterDirection.Output, ReturnValue),
                new MSSQL.Parameter("ReturnDescription", SqlDbType.VarChar, 100, ParameterDirection.Output, ReturnDescription)
                );

            try
            {
                ReturnValue = System.Convert.ToInt32(Outputs["ReturnValue"]);
            }
            catch { }

            try
            {
                ReturnDescription = System.Convert.ToString(Outputs["ReturnDescription"]);
            }
            catch { }

            return CallResult;
        }

        public static int P_ExpertsCommendRead(SqlConnection conn, long SiteID, long ExpertsCommendID, long UserID, ref int ReturnValue, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_ExpertsCommendRead(conn, ref ds, SiteID, ExpertsCommendID, UserID, ref ReturnValue, ref ReturnDescription);
        }

        public static int P_ExpertsCommendRead(SqlConnection conn, ref DataSet ds, long SiteID, long ExpertsCommendID, long UserID, ref int ReturnValue, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(conn, "P_ExpertsCommendRead", ref ds, ref Outputs,
                new MSSQL.Parameter("SiteID", SqlDbType.BigInt, 0, ParameterDirection.Input, SiteID),
                new MSSQL.Parameter("ExpertsCommendID", SqlDbType.BigInt, 0, ParameterDirection.Input, ExpertsCommendID),
                new MSSQL.Parameter("UserID", SqlDbType.BigInt, 0, ParameterDirection.Input, UserID),
                new MSSQL.Parameter("ReturnValue", SqlDbType.Int, 4, ParameterDirection.Output, ReturnValue),
                new MSSQL.Parameter("ReturnDescription", SqlDbType.VarChar, 100, ParameterDirection.Output, ReturnDescription)
                );

            try
            {
                ReturnValue = System.Convert.ToInt32(Outputs["ReturnValue"]);
            }
            catch { }

            try
            {
                ReturnDescription = System.Convert.ToString(Outputs["ReturnDescription"]);
            }
            catch { }

            return CallResult;
        }

        public static int P_ExpertsDelete(SqlConnection conn, long SiteID, long ExpertsID, ref int ReturnValue, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_ExpertsDelete(conn, ref ds, SiteID, ExpertsID, ref ReturnValue, ref ReturnDescription);
        }

        public static int P_ExpertsDelete(SqlConnection conn, ref DataSet ds, long SiteID, long ExpertsID, ref int ReturnValue, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(conn, "P_ExpertsDelete", ref ds, ref Outputs,
                new MSSQL.Parameter("SiteID", SqlDbType.BigInt, 0, ParameterDirection.Input, SiteID),
                new MSSQL.Parameter("ExpertsID", SqlDbType.BigInt, 0, ParameterDirection.Input, ExpertsID),
                new MSSQL.Parameter("ReturnValue", SqlDbType.Int, 4, ParameterDirection.Output, ReturnValue),
                new MSSQL.Parameter("ReturnDescription", SqlDbType.VarChar, 100, ParameterDirection.Output, ReturnDescription)
                );

            try
            {
                ReturnValue = System.Convert.ToInt32(Outputs["ReturnValue"]);
            }
            catch { }

            try
            {
                ReturnDescription = System.Convert.ToString(Outputs["ReturnDescription"]);
            }
            catch { }

            return CallResult;
        }

        public static int P_ExpertsEdit(SqlConnection conn, long SiteID, long ExpertsID, string Description, double MaxPrice, double BonusScale, bool ON, bool isCommend, int ReadCount, ref int ReturnValue, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_ExpertsEdit(conn, ref ds, SiteID, ExpertsID, Description, MaxPrice, BonusScale, ON, isCommend, ReadCount, ref ReturnValue, ref ReturnDescription);
        }

        public static int P_ExpertsEdit(SqlConnection conn, ref DataSet ds, long SiteID, long ExpertsID, string Description, double MaxPrice, double BonusScale, bool ON, bool isCommend, int ReadCount, ref int ReturnValue, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(conn, "P_ExpertsEdit", ref ds, ref Outputs,
                new MSSQL.Parameter("SiteID", SqlDbType.BigInt, 0, ParameterDirection.Input, SiteID),
                new MSSQL.Parameter("ExpertsID", SqlDbType.BigInt, 0, ParameterDirection.Input, ExpertsID),
                new MSSQL.Parameter("Description", SqlDbType.VarChar, 0, ParameterDirection.Input, Description),
                new MSSQL.Parameter("MaxPrice", SqlDbType.Money, 0, ParameterDirection.Input, MaxPrice),
                new MSSQL.Parameter("BonusScale", SqlDbType.Float, 0, ParameterDirection.Input, BonusScale),
                new MSSQL.Parameter("ON", SqlDbType.Bit, 0, ParameterDirection.Input, ON),
                new MSSQL.Parameter("isCommend", SqlDbType.Bit, 0, ParameterDirection.Input, isCommend),
                new MSSQL.Parameter("ReadCount", SqlDbType.Int, 0, ParameterDirection.Input, ReadCount),
                new MSSQL.Parameter("ReturnValue", SqlDbType.Int, 4, ParameterDirection.Output, ReturnValue),
                new MSSQL.Parameter("ReturnDescription", SqlDbType.VarChar, 100, ParameterDirection.Output, ReturnDescription)
                );

            try
            {
                ReturnValue = System.Convert.ToInt32(Outputs["ReturnValue"]);
            }
            catch { }

            try
            {
                ReturnDescription = System.Convert.ToString(Outputs["ReturnDescription"]);
            }
            catch { }

            return CallResult;
        }

        public static int P_ExpertsTry(SqlConnection conn, long SiteID, long UserID, int LotteryID, string Description, double MaxPrice, double BonusScale, ref long NewExpertsTryID, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_ExpertsTry(conn, ref ds, SiteID, UserID, LotteryID, Description, MaxPrice, BonusScale, ref NewExpertsTryID, ref ReturnDescription);
        }

        public static int P_ExpertsTry(SqlConnection conn, ref DataSet ds, long SiteID, long UserID, int LotteryID, string Description, double MaxPrice, double BonusScale, ref long NewExpertsTryID, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(conn, "P_ExpertsTry", ref ds, ref Outputs,
                new MSSQL.Parameter("SiteID", SqlDbType.BigInt, 0, ParameterDirection.Input, SiteID),
                new MSSQL.Parameter("UserID", SqlDbType.BigInt, 0, ParameterDirection.Input, UserID),
                new MSSQL.Parameter("LotteryID", SqlDbType.Int, 0, ParameterDirection.Input, LotteryID),
                new MSSQL.Parameter("Description", SqlDbType.VarChar, 0, ParameterDirection.Input, Description),
                new MSSQL.Parameter("MaxPrice", SqlDbType.Money, 0, ParameterDirection.Input, MaxPrice),
                new MSSQL.Parameter("BonusScale", SqlDbType.Float, 0, ParameterDirection.Input, BonusScale),
                new MSSQL.Parameter("NewExpertsTryID", SqlDbType.BigInt, 8, ParameterDirection.Output, NewExpertsTryID),
                new MSSQL.Parameter("ReturnDescription", SqlDbType.VarChar, 100, ParameterDirection.Output, ReturnDescription)
                );

            try
            {
                NewExpertsTryID = System.Convert.ToInt64(Outputs["NewExpertsTryID"]);
            }
            catch { }

            try
            {
                ReturnDescription = System.Convert.ToString(Outputs["ReturnDescription"]);
            }
            catch { }

            return CallResult;
        }

        public static int P_ExpertsTryHandle(SqlConnection conn, long SiteID, long ExpertsTryID, short HandleResult, string Description, double MaxPrice, double BonusScale, bool isCommend, ref int ReturnValue, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_ExpertsTryHandle(conn, ref ds, SiteID, ExpertsTryID, HandleResult, Description, MaxPrice, BonusScale, isCommend, ref ReturnValue, ref ReturnDescription);
        }

        public static int P_ExpertsTryHandle(SqlConnection conn, ref DataSet ds, long SiteID, long ExpertsTryID, short HandleResult, string Description, double MaxPrice, double BonusScale, bool isCommend, ref int ReturnValue, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(conn, "P_ExpertsTryHandle", ref ds, ref Outputs,
                new MSSQL.Parameter("SiteID", SqlDbType.BigInt, 0, ParameterDirection.Input, SiteID),
                new MSSQL.Parameter("ExpertsTryID", SqlDbType.BigInt, 0, ParameterDirection.Input, ExpertsTryID),
                new MSSQL.Parameter("HandleResult", SqlDbType.SmallInt, 0, ParameterDirection.Input, HandleResult),
                new MSSQL.Parameter("Description", SqlDbType.VarChar, 0, ParameterDirection.Input, Description),
                new MSSQL.Parameter("MaxPrice", SqlDbType.Money, 0, ParameterDirection.Input, MaxPrice),
                new MSSQL.Parameter("BonusScale", SqlDbType.Float, 0, ParameterDirection.Input, BonusScale),
                new MSSQL.Parameter("isCommend", SqlDbType.Bit, 0, ParameterDirection.Input, isCommend),
                new MSSQL.Parameter("ReturnValue", SqlDbType.Int, 4, ParameterDirection.Output, ReturnValue),
                new MSSQL.Parameter("ReturnDescription", SqlDbType.VarChar, 100, ParameterDirection.Output, ReturnDescription)
                );

            try
            {
                ReturnValue = System.Convert.ToInt32(Outputs["ReturnValue"]);
            }
            catch { }

            try
            {
                ReturnDescription = System.Convert.ToString(Outputs["ReturnDescription"]);
            }
            catch { }

            return CallResult;
        }

        public static int P_ExpertsWinCommendAdd(SqlConnection conn, long SiteID, long UserID, DateTime DateTime, long IsuseID, string Title, string Content, bool isShow, ref long NewExpertsWinCommendID, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_ExpertsWinCommendAdd(conn, ref ds, SiteID, UserID, DateTime, IsuseID, Title, Content, isShow, ref NewExpertsWinCommendID, ref ReturnDescription);
        }

        public static int P_ExpertsWinCommendAdd(SqlConnection conn, ref DataSet ds, long SiteID, long UserID, DateTime DateTime, long IsuseID, string Title, string Content, bool isShow, ref long NewExpertsWinCommendID, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(conn, "P_ExpertsWinCommendAdd", ref ds, ref Outputs,
                new MSSQL.Parameter("SiteID", SqlDbType.BigInt, 0, ParameterDirection.Input, SiteID),
                new MSSQL.Parameter("UserID", SqlDbType.BigInt, 0, ParameterDirection.Input, UserID),
                new MSSQL.Parameter("DateTime", SqlDbType.DateTime, 0, ParameterDirection.Input, DateTime),
                new MSSQL.Parameter("IsuseID", SqlDbType.BigInt, 0, ParameterDirection.Input, IsuseID),
                new MSSQL.Parameter("Title", SqlDbType.VarChar, 0, ParameterDirection.Input, Title),
                new MSSQL.Parameter("Content", SqlDbType.VarChar, 0, ParameterDirection.Input, Content),
                new MSSQL.Parameter("isShow", SqlDbType.Bit, 0, ParameterDirection.Input, isShow),
                new MSSQL.Parameter("NewExpertsWinCommendID", SqlDbType.BigInt, 8, ParameterDirection.Output, NewExpertsWinCommendID),
                new MSSQL.Parameter("ReturnDescription", SqlDbType.VarChar, 100, ParameterDirection.Output, ReturnDescription)
                );

            try
            {
                NewExpertsWinCommendID = System.Convert.ToInt64(Outputs["NewExpertsWinCommendID"]);
            }
            catch { }

            try
            {
                ReturnDescription = System.Convert.ToString(Outputs["ReturnDescription"]);
            }
            catch { }

            return CallResult;
        }

        public static int P_ExpertsWinCommendDelete(SqlConnection conn, long SiteID, long ExpertsWinCommendsID, ref int ReturnValue, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_ExpertsWinCommendDelete(conn, ref ds, SiteID, ExpertsWinCommendsID, ref ReturnValue, ref ReturnDescription);
        }

        public static int P_ExpertsWinCommendDelete(SqlConnection conn, ref DataSet ds, long SiteID, long ExpertsWinCommendsID, ref int ReturnValue, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(conn, "P_ExpertsWinCommendDelete", ref ds, ref Outputs,
                new MSSQL.Parameter("SiteID", SqlDbType.BigInt, 0, ParameterDirection.Input, SiteID),
                new MSSQL.Parameter("ExpertsWinCommendsID", SqlDbType.BigInt, 0, ParameterDirection.Input, ExpertsWinCommendsID),
                new MSSQL.Parameter("ReturnValue", SqlDbType.Int, 4, ParameterDirection.Output, ReturnValue),
                new MSSQL.Parameter("ReturnDescription", SqlDbType.VarChar, 100, ParameterDirection.Output, ReturnDescription)
                );

            try
            {
                ReturnValue = System.Convert.ToInt32(Outputs["ReturnValue"]);
            }
            catch { }

            try
            {
                ReturnDescription = System.Convert.ToString(Outputs["ReturnDescription"]);
            }
            catch { }

            return CallResult;
        }

        public static int P_ExpertsWinCommendEdit(SqlConnection conn, long SiteID, long ExpertsWinCommendsID, DateTime DateTime, long IsuseID, string Title, string Content, bool isShow, bool ON, bool isCommend, bool isAdmin, ref int ReturnValue, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_ExpertsWinCommendEdit(conn, ref ds, SiteID, ExpertsWinCommendsID, DateTime, IsuseID, Title, Content, isShow, ON, isCommend, isAdmin, ref ReturnValue, ref ReturnDescription);
        }

        public static int P_ExpertsWinCommendEdit(SqlConnection conn, ref DataSet ds, long SiteID, long ExpertsWinCommendsID, DateTime DateTime, long IsuseID, string Title, string Content, bool isShow, bool ON, bool isCommend, bool isAdmin, ref int ReturnValue, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(conn, "P_ExpertsWinCommendEdit", ref ds, ref Outputs,
                new MSSQL.Parameter("SiteID", SqlDbType.BigInt, 0, ParameterDirection.Input, SiteID),
                new MSSQL.Parameter("ExpertsWinCommendsID", SqlDbType.BigInt, 0, ParameterDirection.Input, ExpertsWinCommendsID),
                new MSSQL.Parameter("DateTime", SqlDbType.DateTime, 0, ParameterDirection.Input, DateTime),
                new MSSQL.Parameter("IsuseID", SqlDbType.BigInt, 0, ParameterDirection.Input, IsuseID),
                new MSSQL.Parameter("Title", SqlDbType.VarChar, 0, ParameterDirection.Input, Title),
                new MSSQL.Parameter("Content", SqlDbType.VarChar, 0, ParameterDirection.Input, Content),
                new MSSQL.Parameter("isShow", SqlDbType.Bit, 0, ParameterDirection.Input, isShow),
                new MSSQL.Parameter("ON", SqlDbType.Bit, 0, ParameterDirection.Input, ON),
                new MSSQL.Parameter("isCommend", SqlDbType.Bit, 0, ParameterDirection.Input, isCommend),
                new MSSQL.Parameter("isAdmin", SqlDbType.Bit, 0, ParameterDirection.Input, isAdmin),
                new MSSQL.Parameter("ReturnValue", SqlDbType.Int, 4, ParameterDirection.Output, ReturnValue),
                new MSSQL.Parameter("ReturnDescription", SqlDbType.VarChar, 100, ParameterDirection.Output, ReturnDescription)
                );

            try
            {
                ReturnValue = System.Convert.ToInt32(Outputs["ReturnValue"]);
            }
            catch { }

            try
            {
                ReturnDescription = System.Convert.ToString(Outputs["ReturnDescription"]);
            }
            catch { }

            return CallResult;
        }

        public static int P_FootballLeagueTypeAdd(SqlConnection conn, string Name, string Code, string MarkersColor, string Description, int Order, bool isUse, ref int FootballLeagueTypeID, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_FootballLeagueTypeAdd(conn, ref ds, Name, Code, MarkersColor, Description, Order, isUse, ref FootballLeagueTypeID, ref ReturnDescription);
        }

        public static int P_FootballLeagueTypeAdd(SqlConnection conn, ref DataSet ds, string Name, string Code, string MarkersColor, string Description, int Order, bool isUse, ref int FootballLeagueTypeID, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(conn, "P_FootballLeagueTypeAdd", ref ds, ref Outputs,
                new MSSQL.Parameter("Name", SqlDbType.VarChar, 0, ParameterDirection.Input, Name),
                new MSSQL.Parameter("Code", SqlDbType.VarChar, 0, ParameterDirection.Input, Code),
                new MSSQL.Parameter("MarkersColor", SqlDbType.VarChar, 0, ParameterDirection.Input, MarkersColor),
                new MSSQL.Parameter("Description", SqlDbType.VarChar, 0, ParameterDirection.Input, Description),
                new MSSQL.Parameter("Order", SqlDbType.Int, 0, ParameterDirection.Input, Order),
                new MSSQL.Parameter("isUse", SqlDbType.Bit, 0, ParameterDirection.Input, isUse),
                new MSSQL.Parameter("FootballLeagueTypeID", SqlDbType.Int, 4, ParameterDirection.Output, FootballLeagueTypeID),
                new MSSQL.Parameter("ReturnDescription", SqlDbType.VarChar, 100, ParameterDirection.Output, ReturnDescription)
                );

            try
            {
                FootballLeagueTypeID = System.Convert.ToInt32(Outputs["FootballLeagueTypeID"]);
            }
            catch { }

            try
            {
                ReturnDescription = System.Convert.ToString(Outputs["ReturnDescription"]);
            }
            catch { }

            return CallResult;
        }

        public static int P_FootballLeagueTypeEdit(SqlConnection conn, int ID, string Name, string Code, string MarkersColor, string Description, int Order, bool isUse, ref int ReturnValue, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_FootballLeagueTypeEdit(conn, ref ds, ID, Name, Code, MarkersColor, Description, Order, isUse, ref ReturnValue, ref ReturnDescription);
        }

        public static int P_FootballLeagueTypeEdit(SqlConnection conn, ref DataSet ds, int ID, string Name, string Code, string MarkersColor, string Description, int Order, bool isUse, ref int ReturnValue, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(conn, "P_FootballLeagueTypeEdit", ref ds, ref Outputs,
                new MSSQL.Parameter("ID", SqlDbType.Int, 0, ParameterDirection.Input, ID),
                new MSSQL.Parameter("Name", SqlDbType.VarChar, 0, ParameterDirection.Input, Name),
                new MSSQL.Parameter("Code", SqlDbType.VarChar, 0, ParameterDirection.Input, Code),
                new MSSQL.Parameter("MarkersColor", SqlDbType.VarChar, 0, ParameterDirection.Input, MarkersColor),
                new MSSQL.Parameter("Description", SqlDbType.VarChar, 0, ParameterDirection.Input, Description),
                new MSSQL.Parameter("Order", SqlDbType.Int, 0, ParameterDirection.Input, Order),
                new MSSQL.Parameter("isUse", SqlDbType.Bit, 0, ParameterDirection.Input, isUse),
                new MSSQL.Parameter("ReturnValue", SqlDbType.Int, 4, ParameterDirection.Output, ReturnValue),
                new MSSQL.Parameter("ReturnDescription", SqlDbType.VarChar, 100, ParameterDirection.Output, ReturnDescription)
                );

            try
            {
                ReturnValue = System.Convert.ToInt32(Outputs["ReturnValue"]);
            }
            catch { }

            try
            {
                ReturnDescription = System.Convert.ToString(Outputs["ReturnDescription"]);
            }
            catch { }

            return CallResult;
        }

        public static int P_FriendshipLinkAdd(SqlConnection conn, long SiteID, string LinkName, string LogoUrl, string Url, int Order, bool isShow, ref long NewFriendshipLinkID, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_FriendshipLinkAdd(conn, ref ds, SiteID, LinkName, LogoUrl, Url, Order, isShow, ref NewFriendshipLinkID, ref ReturnDescription);
        }

        public static int P_FriendshipLinkAdd(SqlConnection conn, ref DataSet ds, long SiteID, string LinkName, string LogoUrl, string Url, int Order, bool isShow, ref long NewFriendshipLinkID, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(conn, "P_FriendshipLinkAdd", ref ds, ref Outputs,
                new MSSQL.Parameter("SiteID", SqlDbType.BigInt, 0, ParameterDirection.Input, SiteID),
                new MSSQL.Parameter("LinkName", SqlDbType.VarChar, 0, ParameterDirection.Input, LinkName),
                new MSSQL.Parameter("LogoUrl", SqlDbType.VarChar, 0, ParameterDirection.Input, LogoUrl),
                new MSSQL.Parameter("Url", SqlDbType.VarChar, 0, ParameterDirection.Input, Url),
                new MSSQL.Parameter("Order", SqlDbType.Int, 0, ParameterDirection.Input, Order),
                new MSSQL.Parameter("isShow", SqlDbType.Bit, 0, ParameterDirection.Input, isShow),
                new MSSQL.Parameter("NewFriendshipLinkID", SqlDbType.BigInt, 8, ParameterDirection.Output, NewFriendshipLinkID),
                new MSSQL.Parameter("ReturnDescription", SqlDbType.VarChar, 100, ParameterDirection.Output, ReturnDescription)
                );

            try
            {
                NewFriendshipLinkID = System.Convert.ToInt64(Outputs["NewFriendshipLinkID"]);
            }
            catch { }

            try
            {
                ReturnDescription = System.Convert.ToString(Outputs["ReturnDescription"]);
            }
            catch { }

            return CallResult;
        }

        public static int P_FriendshipLinkDelete(SqlConnection conn, long SiteID, long ID, ref int ReturnValue, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_FriendshipLinkDelete(conn, ref ds, SiteID, ID, ref ReturnValue, ref ReturnDescription);
        }

        public static int P_FriendshipLinkDelete(SqlConnection conn, ref DataSet ds, long SiteID, long ID, ref int ReturnValue, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(conn, "P_FriendshipLinkDelete", ref ds, ref Outputs,
                new MSSQL.Parameter("SiteID", SqlDbType.BigInt, 0, ParameterDirection.Input, SiteID),
                new MSSQL.Parameter("ID", SqlDbType.BigInt, 0, ParameterDirection.Input, ID),
                new MSSQL.Parameter("ReturnValue", SqlDbType.Int, 4, ParameterDirection.Output, ReturnValue),
                new MSSQL.Parameter("ReturnDescription", SqlDbType.VarChar, 100, ParameterDirection.Output, ReturnDescription)
                );

            try
            {
                ReturnValue = System.Convert.ToInt32(Outputs["ReturnValue"]);
            }
            catch { }

            try
            {
                ReturnDescription = System.Convert.ToString(Outputs["ReturnDescription"]);
            }
            catch { }

            return CallResult;
        }

        public static int P_FriendshipLinkEdit(SqlConnection conn, long SiteID, long ID, string LinkName, string LogoUrl, string Url, int Order, bool isShow, ref int ReturnValue, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_FriendshipLinkEdit(conn, ref ds, SiteID, ID, LinkName, LogoUrl, Url, Order, isShow, ref ReturnValue, ref ReturnDescription);
        }

        public static int P_FriendshipLinkEdit(SqlConnection conn, ref DataSet ds, long SiteID, long ID, string LinkName, string LogoUrl, string Url, int Order, bool isShow, ref int ReturnValue, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(conn, "P_FriendshipLinkEdit", ref ds, ref Outputs,
                new MSSQL.Parameter("SiteID", SqlDbType.BigInt, 0, ParameterDirection.Input, SiteID),
                new MSSQL.Parameter("ID", SqlDbType.BigInt, 0, ParameterDirection.Input, ID),
                new MSSQL.Parameter("LinkName", SqlDbType.VarChar, 0, ParameterDirection.Input, LinkName),
                new MSSQL.Parameter("LogoUrl", SqlDbType.VarChar, 0, ParameterDirection.Input, LogoUrl),
                new MSSQL.Parameter("Url", SqlDbType.VarChar, 0, ParameterDirection.Input, Url),
                new MSSQL.Parameter("Order", SqlDbType.Int, 0, ParameterDirection.Input, Order),
                new MSSQL.Parameter("isShow", SqlDbType.Bit, 0, ParameterDirection.Input, isShow),
                new MSSQL.Parameter("ReturnValue", SqlDbType.Int, 4, ParameterDirection.Output, ReturnValue),
                new MSSQL.Parameter("ReturnDescription", SqlDbType.VarChar, 100, ParameterDirection.Output, ReturnDescription)
                );

            try
            {
                ReturnValue = System.Convert.ToInt32(Outputs["ReturnValue"]);
            }
            catch { }

            try
            {
                ReturnDescription = System.Convert.ToString(Outputs["ReturnDescription"]);
            }
            catch { }

            return CallResult;
        }

        public static int P_GetAccount(SqlConnection conn, long SiteID, int Year, int Month, ref int ReturnValue, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_GetAccount(conn, ref ds, SiteID, Year, Month, ref ReturnValue, ref ReturnDescription);
        }

        public static int P_GetAccount(SqlConnection conn, ref DataSet ds, long SiteID, int Year, int Month, ref int ReturnValue, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(conn, "P_GetAccount", ref ds, ref Outputs,
                new MSSQL.Parameter("SiteID", SqlDbType.BigInt, 0, ParameterDirection.Input, SiteID),
                new MSSQL.Parameter("Year", SqlDbType.Int, 0, ParameterDirection.Input, Year),
                new MSSQL.Parameter("Month", SqlDbType.Int, 0, ParameterDirection.Input, Month),
                new MSSQL.Parameter("ReturnValue", SqlDbType.Int, 4, ParameterDirection.Output, ReturnValue),
                new MSSQL.Parameter("ReturnDescription", SqlDbType.VarChar, 100, ParameterDirection.Output, ReturnDescription)
                );

            try
            {
                ReturnValue = System.Convert.ToInt32(Outputs["ReturnValue"]);
            }
            catch { }

            try
            {
                ReturnDescription = System.Convert.ToString(Outputs["ReturnDescription"]);
            }
            catch { }

            return CallResult;
        }

        public static int P_GetCpsAccount(SqlConnection conn, int Year, int Month, long CpsID, ref int ReturnValue, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_GetCpsAccount(conn, ref ds, Year, Month, CpsID, ref ReturnValue, ref ReturnDescription);
        }

        public static int P_GetCpsAccount(SqlConnection conn, ref DataSet ds, int Year, int Month, long CpsID, ref int ReturnValue, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(conn, "P_GetCpsAccount", ref ds, ref Outputs,
                new MSSQL.Parameter("Year", SqlDbType.Int, 0, ParameterDirection.Input, Year),
                new MSSQL.Parameter("Month", SqlDbType.Int, 0, ParameterDirection.Input, Month),
                new MSSQL.Parameter("CpsID", SqlDbType.BigInt, 0, ParameterDirection.Input, CpsID),
                new MSSQL.Parameter("ReturnValue", SqlDbType.Int, 4, ParameterDirection.Output, ReturnValue),
                new MSSQL.Parameter("ReturnDescription", SqlDbType.VarChar, 100, ParameterDirection.Output, ReturnDescription)
                );

            try
            {
                ReturnValue = System.Convert.ToInt32(Outputs["ReturnValue"]);
            }
            catch { }

            try
            {
                ReturnDescription = System.Convert.ToString(Outputs["ReturnDescription"]);
            }
            catch { }

            return CallResult;
        }

        public static int P_GetCpsAccountByPid(SqlConnection conn, long SiteID, long UserID, DateTime StartTime, DateTime EndTime, string pid, ref int ReturnValue, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_GetCpsAccountByPid(conn, ref ds, SiteID, UserID, StartTime, EndTime, pid, ref ReturnValue, ref ReturnDescription);
        }

        public static int P_GetCpsAccountByPid(SqlConnection conn, ref DataSet ds, long SiteID, long UserID, DateTime StartTime, DateTime EndTime, string pid, ref int ReturnValue, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(conn, "P_GetCpsAccountByPid", ref ds, ref Outputs,
                new MSSQL.Parameter("SiteID", SqlDbType.BigInt, 0, ParameterDirection.Input, SiteID),
                new MSSQL.Parameter("UserID", SqlDbType.BigInt, 0, ParameterDirection.Input, UserID),
                new MSSQL.Parameter("StartTime", SqlDbType.DateTime, 0, ParameterDirection.Input, StartTime),
                new MSSQL.Parameter("EndTime", SqlDbType.DateTime, 0, ParameterDirection.Input, EndTime),
                new MSSQL.Parameter("pid", SqlDbType.VarChar, 0, ParameterDirection.Input, pid),
                new MSSQL.Parameter("ReturnValue", SqlDbType.Int, 4, ParameterDirection.Output, ReturnValue),
                new MSSQL.Parameter("ReturnDescription", SqlDbType.VarChar, 100, ParameterDirection.Output, ReturnDescription)
                );

            try
            {
                ReturnValue = System.Convert.ToInt32(Outputs["ReturnValue"]);
            }
            catch { }

            try
            {
                ReturnDescription = System.Convert.ToString(Outputs["ReturnDescription"]);
            }
            catch { }

            return CallResult;
        }

        public static int P_GetCpsAccountDetail(SqlConnection conn, long SiteID, long UserID, int Year, int Month, ref int ReturnValue, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_GetCpsAccountDetail(conn, ref ds, SiteID, UserID, Year, Month, ref ReturnValue, ref ReturnDescription);
        }

        public static int P_GetCpsAccountDetail(SqlConnection conn, ref DataSet ds, long SiteID, long UserID, int Year, int Month, ref int ReturnValue, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(conn, "P_GetCpsAccountDetail", ref ds, ref Outputs,
                new MSSQL.Parameter("SiteID", SqlDbType.BigInt, 0, ParameterDirection.Input, SiteID),
                new MSSQL.Parameter("UserID", SqlDbType.BigInt, 0, ParameterDirection.Input, UserID),
                new MSSQL.Parameter("Year", SqlDbType.Int, 0, ParameterDirection.Input, Year),
                new MSSQL.Parameter("Month", SqlDbType.Int, 0, ParameterDirection.Input, Month),
                new MSSQL.Parameter("ReturnValue", SqlDbType.Int, 4, ParameterDirection.Output, ReturnValue),
                new MSSQL.Parameter("ReturnDescription", SqlDbType.VarChar, 100, ParameterDirection.Output, ReturnDescription)
                );

            try
            {
                ReturnValue = System.Convert.ToInt32(Outputs["ReturnValue"]);
            }
            catch { }

            try
            {
                ReturnDescription = System.Convert.ToString(Outputs["ReturnDescription"]);
            }
            catch { }

            return CallResult;
        }

        public static int P_GetCpsAccountDetails(SqlConnection conn, long SiteID, long UserID, DateTime StartTime, DateTime EndTime, ref int ReturnValue, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_GetCpsAccountDetails(conn, ref ds, SiteID, UserID, StartTime, EndTime, ref ReturnValue, ref ReturnDescription);
        }

        public static int P_GetCpsAccountDetails(SqlConnection conn, ref DataSet ds, long SiteID, long UserID, DateTime StartTime, DateTime EndTime, ref int ReturnValue, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(conn, "P_GetCpsAccountDetails", ref ds, ref Outputs,
                new MSSQL.Parameter("SiteID", SqlDbType.BigInt, 0, ParameterDirection.Input, SiteID),
                new MSSQL.Parameter("UserID", SqlDbType.BigInt, 0, ParameterDirection.Input, UserID),
                new MSSQL.Parameter("StartTime", SqlDbType.DateTime, 0, ParameterDirection.Input, StartTime),
                new MSSQL.Parameter("EndTime", SqlDbType.DateTime, 0, ParameterDirection.Input, EndTime),
                new MSSQL.Parameter("ReturnValue", SqlDbType.Int, 4, ParameterDirection.Output, ReturnValue),
                new MSSQL.Parameter("ReturnDescription", SqlDbType.VarChar, 100, ParameterDirection.Output, ReturnDescription)
                );

            try
            {
                ReturnValue = System.Convert.ToInt32(Outputs["ReturnValue"]);
            }
            catch { }

            try
            {
                ReturnDescription = System.Convert.ToString(Outputs["ReturnDescription"]);
            }
            catch { }

            return CallResult;
        }

        public static int P_GetCpsAccountDetailWithUser(SqlConnection conn, long CpsID, DateTime StartTime, DateTime EndTime, string KeyWord, ref int ReturnValue, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_GetCpsAccountDetailWithUser(conn, ref ds, CpsID, StartTime, EndTime, KeyWord, ref ReturnValue, ref ReturnDescription);
        }

        public static int P_GetCpsAccountDetailWithUser(SqlConnection conn, ref DataSet ds, long CpsID, DateTime StartTime, DateTime EndTime, string KeyWord, ref int ReturnValue, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(conn, "P_GetCpsAccountDetailWithUser", ref ds, ref Outputs,
                new MSSQL.Parameter("CpsID", SqlDbType.BigInt, 0, ParameterDirection.Input, CpsID),
                new MSSQL.Parameter("StartTime", SqlDbType.DateTime, 0, ParameterDirection.Input, StartTime),
                new MSSQL.Parameter("EndTime", SqlDbType.DateTime, 0, ParameterDirection.Input, EndTime),
                new MSSQL.Parameter("KeyWord", SqlDbType.VarChar, 0, ParameterDirection.Input, KeyWord),
                new MSSQL.Parameter("ReturnValue", SqlDbType.Int, 4, ParameterDirection.Output, ReturnValue),
                new MSSQL.Parameter("ReturnDescription", SqlDbType.VarChar, 100, ParameterDirection.Output, ReturnDescription)
                );

            try
            {
                ReturnValue = System.Convert.ToInt32(Outputs["ReturnValue"]);
            }
            catch { }

            try
            {
                ReturnDescription = System.Convert.ToString(Outputs["ReturnDescription"]);
            }
            catch { }

            return CallResult;
        }

        public static int P_GetCpsAccountRevenue(SqlConnection conn, long SiteID, long UserID, DateTime StartTime, DateTime EndTime, ref int ReturnValue, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_GetCpsAccountRevenue(conn, ref ds, SiteID, UserID, StartTime, EndTime, ref ReturnValue, ref ReturnDescription);
        }

        public static int P_GetCpsAccountRevenue(SqlConnection conn, ref DataSet ds, long SiteID, long UserID, DateTime StartTime, DateTime EndTime, ref int ReturnValue, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(conn, "P_GetCpsAccountRevenue", ref ds, ref Outputs,
                new MSSQL.Parameter("SiteID", SqlDbType.BigInt, 0, ParameterDirection.Input, SiteID),
                new MSSQL.Parameter("UserID", SqlDbType.BigInt, 0, ParameterDirection.Input, UserID),
                new MSSQL.Parameter("StartTime", SqlDbType.DateTime, 0, ParameterDirection.Input, StartTime),
                new MSSQL.Parameter("EndTime", SqlDbType.DateTime, 0, ParameterDirection.Input, EndTime),
                new MSSQL.Parameter("ReturnValue", SqlDbType.Int, 4, ParameterDirection.Output, ReturnValue),
                new MSSQL.Parameter("ReturnDescription", SqlDbType.VarChar, 100, ParameterDirection.Output, ReturnDescription)
                );

            try
            {
                ReturnValue = System.Convert.ToInt32(Outputs["ReturnValue"]);
            }
            catch { }

            try
            {
                ReturnDescription = System.Convert.ToString(Outputs["ReturnDescription"]);
            }
            catch { }

            return CallResult;
        }

        public static int P_GetCpsAccountRevenue_center(SqlConnection conn, long SiteID, long UserID, DateTime StartTime, DateTime EndTime, ref int ReturnValue, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_GetCpsAccountRevenue_center(conn, ref ds, SiteID, UserID, StartTime, EndTime, ref ReturnValue, ref ReturnDescription);
        }

        public static int P_GetCpsAccountRevenue_center(SqlConnection conn, ref DataSet ds, long SiteID, long UserID, DateTime StartTime, DateTime EndTime, ref int ReturnValue, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(conn, "P_GetCpsAccountRevenue_center", ref ds, ref Outputs,
                new MSSQL.Parameter("SiteID", SqlDbType.BigInt, 0, ParameterDirection.Input, SiteID),
                new MSSQL.Parameter("UserID", SqlDbType.BigInt, 0, ParameterDirection.Input, UserID),
                new MSSQL.Parameter("StartTime", SqlDbType.DateTime, 0, ParameterDirection.Input, StartTime),
                new MSSQL.Parameter("EndTime", SqlDbType.DateTime, 0, ParameterDirection.Input, EndTime),
                new MSSQL.Parameter("ReturnValue", SqlDbType.Int, 4, ParameterDirection.Output, ReturnValue),
                new MSSQL.Parameter("ReturnDescription", SqlDbType.VarChar, 100, ParameterDirection.Output, ReturnDescription)
                );

            try
            {
                ReturnValue = System.Convert.ToInt32(Outputs["ReturnValue"]);
            }
            catch { }

            try
            {
                ReturnDescription = System.Convert.ToString(Outputs["ReturnDescription"]);
            }
            catch { }

            return CallResult;
        }

        public static int P_GetCpsAccountRevenueSite(SqlConnection conn, long SiteID, long UserID, DateTime StartTime, DateTime EndTime, ref int ReturnValue, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_GetCpsAccountRevenueSite(conn, ref ds, SiteID, UserID, StartTime, EndTime, ref ReturnValue, ref ReturnDescription);
        }

        public static int P_GetCpsAccountRevenueSite(SqlConnection conn, ref DataSet ds, long SiteID, long UserID, DateTime StartTime, DateTime EndTime, ref int ReturnValue, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(conn, "P_GetCpsAccountRevenueSite", ref ds, ref Outputs,
                new MSSQL.Parameter("SiteID", SqlDbType.BigInt, 0, ParameterDirection.Input, SiteID),
                new MSSQL.Parameter("UserID", SqlDbType.BigInt, 0, ParameterDirection.Input, UserID),
                new MSSQL.Parameter("StartTime", SqlDbType.DateTime, 0, ParameterDirection.Input, StartTime),
                new MSSQL.Parameter("EndTime", SqlDbType.DateTime, 0, ParameterDirection.Input, EndTime),
                new MSSQL.Parameter("ReturnValue", SqlDbType.Int, 4, ParameterDirection.Output, ReturnValue),
                new MSSQL.Parameter("ReturnDescription", SqlDbType.VarChar, 100, ParameterDirection.Output, ReturnDescription)
                );

            try
            {
                ReturnValue = System.Convert.ToInt32(Outputs["ReturnValue"]);
            }
            catch { }

            try
            {
                ReturnDescription = System.Convert.ToString(Outputs["ReturnDescription"]);
            }
            catch { }

            return CallResult;
        }

        public static int P_GetCpsAccountRevenueSite_center(SqlConnection conn, long SiteID, long UserID, DateTime StartTime, DateTime EndTime, ref int ReturnValue, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_GetCpsAccountRevenueSite_center(conn, ref ds, SiteID, UserID, StartTime, EndTime, ref ReturnValue, ref ReturnDescription);
        }

        public static int P_GetCpsAccountRevenueSite_center(SqlConnection conn, ref DataSet ds, long SiteID, long UserID, DateTime StartTime, DateTime EndTime, ref int ReturnValue, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(conn, "P_GetCpsAccountRevenueSite_center", ref ds, ref Outputs,
                new MSSQL.Parameter("SiteID", SqlDbType.BigInt, 0, ParameterDirection.Input, SiteID),
                new MSSQL.Parameter("UserID", SqlDbType.BigInt, 0, ParameterDirection.Input, UserID),
                new MSSQL.Parameter("StartTime", SqlDbType.DateTime, 0, ParameterDirection.Input, StartTime),
                new MSSQL.Parameter("EndTime", SqlDbType.DateTime, 0, ParameterDirection.Input, EndTime),
                new MSSQL.Parameter("ReturnValue", SqlDbType.Int, 4, ParameterDirection.Output, ReturnValue),
                new MSSQL.Parameter("ReturnDescription", SqlDbType.VarChar, 100, ParameterDirection.Output, ReturnDescription)
                );

            try
            {
                ReturnValue = System.Convert.ToInt32(Outputs["ReturnValue"]);
            }
            catch { }

            try
            {
                ReturnDescription = System.Convert.ToString(Outputs["ReturnDescription"]);
            }
            catch { }

            return CallResult;
        }

        public static int P_GetCpsAccountWithCpsID(SqlConnection conn, DateTime StartTime, DateTime EndTime, long CpsID, ref int ReturnValue, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_GetCpsAccountWithCpsID(conn, ref ds, StartTime, EndTime, CpsID, ref ReturnValue, ref ReturnDescription);
        }

        public static int P_GetCpsAccountWithCpsID(SqlConnection conn, ref DataSet ds, DateTime StartTime, DateTime EndTime, long CpsID, ref int ReturnValue, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(conn, "P_GetCpsAccountWithCpsID", ref ds, ref Outputs,
                new MSSQL.Parameter("StartTime", SqlDbType.DateTime, 0, ParameterDirection.Input, StartTime),
                new MSSQL.Parameter("EndTime", SqlDbType.DateTime, 0, ParameterDirection.Input, EndTime),
                new MSSQL.Parameter("CpsID", SqlDbType.BigInt, 0, ParameterDirection.Input, CpsID),
                new MSSQL.Parameter("ReturnValue", SqlDbType.Int, 4, ParameterDirection.Output, ReturnValue),
                new MSSQL.Parameter("ReturnDescription", SqlDbType.VarChar, 100, ParameterDirection.Output, ReturnDescription)
                );

            try
            {
                ReturnValue = System.Convert.ToInt32(Outputs["ReturnValue"]);
            }
            catch { }

            try
            {
                ReturnDescription = System.Convert.ToString(Outputs["ReturnDescription"]);
            }
            catch { }

            return CallResult;
        }

        public static int P_GetCpsAccountWithCpsUser(SqlConnection conn, long CpsID, DateTime StartTime, DateTime EndTime, string KeyWord, ref int ReturnValue, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_GetCpsAccountWithCpsUser(conn, ref ds, CpsID, StartTime, EndTime, KeyWord, ref ReturnValue, ref ReturnDescription);
        }

        public static int P_GetCpsAccountWithCpsUser(SqlConnection conn, ref DataSet ds, long CpsID, DateTime StartTime, DateTime EndTime, string KeyWord, ref int ReturnValue, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(conn, "P_GetCpsAccountWithCpsUser", ref ds, ref Outputs,
                new MSSQL.Parameter("CpsID", SqlDbType.BigInt, 0, ParameterDirection.Input, CpsID),
                new MSSQL.Parameter("StartTime", SqlDbType.DateTime, 0, ParameterDirection.Input, StartTime),
                new MSSQL.Parameter("EndTime", SqlDbType.DateTime, 0, ParameterDirection.Input, EndTime),
                new MSSQL.Parameter("KeyWord", SqlDbType.VarChar, 0, ParameterDirection.Input, KeyWord),
                new MSSQL.Parameter("ReturnValue", SqlDbType.Int, 4, ParameterDirection.Output, ReturnValue),
                new MSSQL.Parameter("ReturnDescription", SqlDbType.VarChar, 100, ParameterDirection.Output, ReturnDescription)
                );

            try
            {
                ReturnValue = System.Convert.ToInt32(Outputs["ReturnValue"]);
            }
            catch { }

            try
            {
                ReturnDescription = System.Convert.ToString(Outputs["ReturnDescription"]);
            }
            catch { }

            return CallResult;
        }

        public static int P_GetCpsAccountWithMonth(SqlConnection conn, DateTime StartTime, DateTime EndTime, string UserName, long CpsID, ref int ReturnValue, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_GetCpsAccountWithMonth(conn, ref ds, StartTime, EndTime, UserName, CpsID, ref ReturnValue, ref ReturnDescription);
        }

        public static int P_GetCpsAccountWithMonth(SqlConnection conn, ref DataSet ds, DateTime StartTime, DateTime EndTime, string UserName, long CpsID, ref int ReturnValue, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(conn, "P_GetCpsAccountWithMonth", ref ds, ref Outputs,
                new MSSQL.Parameter("StartTime", SqlDbType.DateTime, 0, ParameterDirection.Input, StartTime),
                new MSSQL.Parameter("EndTime", SqlDbType.DateTime, 0, ParameterDirection.Input, EndTime),
                new MSSQL.Parameter("UserName", SqlDbType.VarChar, 0, ParameterDirection.Input, UserName),
                new MSSQL.Parameter("CpsID", SqlDbType.BigInt, 0, ParameterDirection.Input, CpsID),
                new MSSQL.Parameter("ReturnValue", SqlDbType.Int, 4, ParameterDirection.Output, ReturnValue),
                new MSSQL.Parameter("ReturnDescription", SqlDbType.VarChar, 100, ParameterDirection.Output, ReturnDescription)
                );

            try
            {
                ReturnValue = System.Convert.ToInt32(Outputs["ReturnValue"]);
            }
            catch { }

            try
            {
                ReturnDescription = System.Convert.ToString(Outputs["ReturnDescription"]);
            }
            catch { }

            return CallResult;
        }

        public static int P_GetCpsBuyDetailByDate(SqlConnection conn, long SiteID, long CpsID, DateTime FromTime, DateTime ToTime, ref int ReturnValue, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_GetCpsBuyDetailByDate(conn, ref ds, SiteID, CpsID, FromTime, ToTime, ref ReturnValue, ref ReturnDescription);
        }

        public static int P_GetCpsBuyDetailByDate(SqlConnection conn, ref DataSet ds, long SiteID, long CpsID, DateTime FromTime, DateTime ToTime, ref int ReturnValue, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(conn, "P_GetCpsBuyDetailByDate", ref ds, ref Outputs,
                new MSSQL.Parameter("SiteID", SqlDbType.BigInt, 0, ParameterDirection.Input, SiteID),
                new MSSQL.Parameter("CpsID", SqlDbType.BigInt, 0, ParameterDirection.Input, CpsID),
                new MSSQL.Parameter("FromTime", SqlDbType.DateTime, 0, ParameterDirection.Input, FromTime),
                new MSSQL.Parameter("ToTime", SqlDbType.DateTime, 0, ParameterDirection.Input, ToTime),
                new MSSQL.Parameter("ReturnValue", SqlDbType.Int, 4, ParameterDirection.Output, ReturnValue),
                new MSSQL.Parameter("ReturnDescription", SqlDbType.VarChar, 100, ParameterDirection.Output, ReturnDescription)
                );

            try
            {
                ReturnValue = System.Convert.ToInt32(Outputs["ReturnValue"]);
            }
            catch { }

            try
            {
                ReturnDescription = System.Convert.ToString(Outputs["ReturnDescription"]);
            }
            catch { }

            return CallResult;
        }

        public static int P_GetCpsByDay(SqlConnection conn, long CpsID, ref int ReturnValue, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_GetCpsByDay(conn, ref ds, CpsID, ref ReturnValue, ref ReturnDescription);
        }

        public static int P_GetCpsByDay(SqlConnection conn, ref DataSet ds, long CpsID, ref int ReturnValue, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(conn, "P_GetCpsByDay", ref ds, ref Outputs,
                new MSSQL.Parameter("CpsID", SqlDbType.BigInt, 0, ParameterDirection.Input, CpsID),
                new MSSQL.Parameter("ReturnValue", SqlDbType.Int, 4, ParameterDirection.Output, ReturnValue),
                new MSSQL.Parameter("ReturnDescription", SqlDbType.VarChar, 100, ParameterDirection.Output, ReturnDescription)
                );

            try
            {
                ReturnValue = System.Convert.ToInt32(Outputs["ReturnValue"]);
            }
            catch { }

            try
            {
                ReturnDescription = System.Convert.ToString(Outputs["ReturnDescription"]);
            }
            catch { }

            return CallResult;
        }

        public static int P_GetCpsInformationByID(SqlConnection conn, long SiteID, long CpsID, ref long OwnerUserID, ref string Name, ref DateTime Datetime, ref string Url, ref string LogoUrl, ref double BonusScale, ref bool ON, ref string Company, ref string Address, ref string PostCode, ref string ResponsiblePerson, ref string ContactPerson, ref string Telephone, ref string Fax, ref string Mobile, ref string Email, ref string QQ, ref string ServiceTelephone, ref string MD5Key, ref short Type, ref long ParentID, ref string DomainName, ref int ReturnValue, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_GetCpsInformationByID(conn, ref ds, SiteID, CpsID, ref OwnerUserID, ref Name, ref Datetime, ref Url, ref LogoUrl, ref BonusScale, ref ON, ref Company, ref Address, ref PostCode, ref ResponsiblePerson, ref ContactPerson, ref Telephone, ref Fax, ref Mobile, ref Email, ref QQ, ref ServiceTelephone, ref MD5Key, ref Type, ref ParentID, ref DomainName, ref ReturnValue, ref ReturnDescription);
        }

        public static int P_GetCpsInformationByID(SqlConnection conn, ref DataSet ds, long SiteID, long CpsID, ref long OwnerUserID, ref string Name, ref DateTime Datetime, ref string Url, ref string LogoUrl, ref double BonusScale, ref bool ON, ref string Company, ref string Address, ref string PostCode, ref string ResponsiblePerson, ref string ContactPerson, ref string Telephone, ref string Fax, ref string Mobile, ref string Email, ref string QQ, ref string ServiceTelephone, ref string MD5Key, ref short Type, ref long ParentID, ref string DomainName, ref int ReturnValue, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(conn, "P_GetCpsInformationByID", ref ds, ref Outputs,
                new MSSQL.Parameter("SiteID", SqlDbType.BigInt, 0, ParameterDirection.Input, SiteID),
                new MSSQL.Parameter("CpsID", SqlDbType.BigInt, 0, ParameterDirection.Input, CpsID),
                new MSSQL.Parameter("OwnerUserID", SqlDbType.BigInt, 8, ParameterDirection.Output, OwnerUserID),
                new MSSQL.Parameter("Name", SqlDbType.VarChar, 50, ParameterDirection.Output, Name),
                new MSSQL.Parameter("Datetime", SqlDbType.DateTime, 8, ParameterDirection.Output, Datetime),
                new MSSQL.Parameter("Url", SqlDbType.VarChar, 100, ParameterDirection.Output, Url),
                new MSSQL.Parameter("LogoUrl", SqlDbType.VarChar, 200, ParameterDirection.Output, LogoUrl),
                new MSSQL.Parameter("BonusScale", SqlDbType.Float, 8, ParameterDirection.Output, BonusScale),
                new MSSQL.Parameter("ON", SqlDbType.Bit, 1, ParameterDirection.Output, ON),
                new MSSQL.Parameter("Company", SqlDbType.VarChar, 50, ParameterDirection.Output, Company),
                new MSSQL.Parameter("Address", SqlDbType.VarChar, 50, ParameterDirection.Output, Address),
                new MSSQL.Parameter("PostCode", SqlDbType.VarChar, 6, ParameterDirection.Output, PostCode),
                new MSSQL.Parameter("ResponsiblePerson", SqlDbType.VarChar, 50, ParameterDirection.Output, ResponsiblePerson),
                new MSSQL.Parameter("ContactPerson", SqlDbType.VarChar, 50, ParameterDirection.Output, ContactPerson),
                new MSSQL.Parameter("Telephone", SqlDbType.VarChar, 50, ParameterDirection.Output, Telephone),
                new MSSQL.Parameter("Fax", SqlDbType.VarChar, 50, ParameterDirection.Output, Fax),
                new MSSQL.Parameter("Mobile", SqlDbType.VarChar, 50, ParameterDirection.Output, Mobile),
                new MSSQL.Parameter("Email", SqlDbType.VarChar, 50, ParameterDirection.Output, Email),
                new MSSQL.Parameter("QQ", SqlDbType.VarChar, 50, ParameterDirection.Output, QQ),
                new MSSQL.Parameter("ServiceTelephone", SqlDbType.VarChar, 30, ParameterDirection.Output, ServiceTelephone),
                new MSSQL.Parameter("MD5Key", SqlDbType.VarChar, 120, ParameterDirection.Output, MD5Key),
                new MSSQL.Parameter("Type", SqlDbType.SmallInt, 2, ParameterDirection.Output, Type),
                new MSSQL.Parameter("ParentID", SqlDbType.BigInt, 8, ParameterDirection.Output, ParentID),
                new MSSQL.Parameter("DomainName", SqlDbType.VarChar, 200, ParameterDirection.Output, DomainName),
                new MSSQL.Parameter("ReturnValue", SqlDbType.Int, 4, ParameterDirection.Output, ReturnValue),
                new MSSQL.Parameter("ReturnDescription", SqlDbType.VarChar, 100, ParameterDirection.Output, ReturnDescription)
                );

            try
            {
                OwnerUserID = System.Convert.ToInt64(Outputs["OwnerUserID"]);
            }
            catch { }

            try
            {
                Name = System.Convert.ToString(Outputs["Name"]);
            }
            catch { }

            try
            {
                Datetime = System.Convert.ToDateTime(Outputs["Datetime"]);
            }
            catch { }

            try
            {
                Url = System.Convert.ToString(Outputs["Url"]);
            }
            catch { }

            try
            {
                LogoUrl = System.Convert.ToString(Outputs["LogoUrl"]);
            }
            catch { }

            try
            {
                BonusScale = System.Convert.ToDouble(Outputs["BonusScale"]);
            }
            catch { }

            try
            {
                ON = System.Convert.ToBoolean(Outputs["ON"]);
            }
            catch { }

            try
            {
                Company = System.Convert.ToString(Outputs["Company"]);
            }
            catch { }

            try
            {
                Address = System.Convert.ToString(Outputs["Address"]);
            }
            catch { }

            try
            {
                PostCode = System.Convert.ToString(Outputs["PostCode"]);
            }
            catch { }

            try
            {
                ResponsiblePerson = System.Convert.ToString(Outputs["ResponsiblePerson"]);
            }
            catch { }

            try
            {
                ContactPerson = System.Convert.ToString(Outputs["ContactPerson"]);
            }
            catch { }

            try
            {
                Telephone = System.Convert.ToString(Outputs["Telephone"]);
            }
            catch { }

            try
            {
                Fax = System.Convert.ToString(Outputs["Fax"]);
            }
            catch { }

            try
            {
                Mobile = System.Convert.ToString(Outputs["Mobile"]);
            }
            catch { }

            try
            {
                Email = System.Convert.ToString(Outputs["Email"]);
            }
            catch { }

            try
            {
                QQ = System.Convert.ToString(Outputs["QQ"]);
            }
            catch { }

            try
            {
                ServiceTelephone = System.Convert.ToString(Outputs["ServiceTelephone"]);
            }
            catch { }

            try
            {
                MD5Key = System.Convert.ToString(Outputs["MD5Key"]);
            }
            catch { }

            try
            {
                Type = System.Convert.ToInt16(Outputs["Type"]);
            }
            catch { }

            try
            {
                ParentID = System.Convert.ToInt64(Outputs["ParentID"]);
            }
            catch { }

            try
            {
                DomainName = System.Convert.ToString(Outputs["DomainName"]);
            }
            catch { }

            try
            {
                ReturnValue = System.Convert.ToInt32(Outputs["ReturnValue"]);
            }
            catch { }

            try
            {
                ReturnDescription = System.Convert.ToString(Outputs["ReturnDescription"]);
            }
            catch { }

            return CallResult;
        }

        public static int P_GetCpsInformationByOwnerUserID(SqlConnection conn, long SiteID, long OwnerUserID, ref long CpsID, ref string Name, ref DateTime Datetime, ref string Url, ref string LogoUrl, ref double BonusScale, ref bool ON, ref string Company, ref string Address, ref string PostCode, ref string ResponsiblePerson, ref string ContactPerson, ref string Telephone, ref string Fax, ref string Mobile, ref string Email, ref string QQ, ref string ServiceTelephone, ref string MD5Key, ref short Type, ref long ParentID, ref string DomainName, ref int ReturnValue, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_GetCpsInformationByOwnerUserID(conn, ref ds, SiteID, OwnerUserID, ref CpsID, ref Name, ref Datetime, ref Url, ref LogoUrl, ref BonusScale, ref ON, ref Company, ref Address, ref PostCode, ref ResponsiblePerson, ref ContactPerson, ref Telephone, ref Fax, ref Mobile, ref Email, ref QQ, ref ServiceTelephone, ref MD5Key, ref Type, ref ParentID, ref DomainName, ref ReturnValue, ref ReturnDescription);
        }

        public static int P_GetCpsInformationByOwnerUserID(SqlConnection conn, ref DataSet ds, long SiteID, long OwnerUserID, ref long CpsID, ref string Name, ref DateTime Datetime, ref string Url, ref string LogoUrl, ref double BonusScale, ref bool ON, ref string Company, ref string Address, ref string PostCode, ref string ResponsiblePerson, ref string ContactPerson, ref string Telephone, ref string Fax, ref string Mobile, ref string Email, ref string QQ, ref string ServiceTelephone, ref string MD5Key, ref short Type, ref long ParentID, ref string DomainName, ref int ReturnValue, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(conn, "P_GetCpsInformationByOwnerUserID", ref ds, ref Outputs,
                new MSSQL.Parameter("SiteID", SqlDbType.BigInt, 0, ParameterDirection.Input, SiteID),
                new MSSQL.Parameter("OwnerUserID", SqlDbType.BigInt, 0, ParameterDirection.Input, OwnerUserID),
                new MSSQL.Parameter("CpsID", SqlDbType.BigInt, 8, ParameterDirection.Output, CpsID),
                new MSSQL.Parameter("Name", SqlDbType.VarChar, 50, ParameterDirection.Output, Name),
                new MSSQL.Parameter("Datetime", SqlDbType.DateTime, 8, ParameterDirection.Output, Datetime),
                new MSSQL.Parameter("Url", SqlDbType.VarChar, 100, ParameterDirection.Output, Url),
                new MSSQL.Parameter("LogoUrl", SqlDbType.VarChar, 200, ParameterDirection.Output, LogoUrl),
                new MSSQL.Parameter("BonusScale", SqlDbType.Float, 8, ParameterDirection.Output, BonusScale),
                new MSSQL.Parameter("ON", SqlDbType.Bit, 1, ParameterDirection.Output, ON),
                new MSSQL.Parameter("Company", SqlDbType.VarChar, 50, ParameterDirection.Output, Company),
                new MSSQL.Parameter("Address", SqlDbType.VarChar, 50, ParameterDirection.Output, Address),
                new MSSQL.Parameter("PostCode", SqlDbType.VarChar, 6, ParameterDirection.Output, PostCode),
                new MSSQL.Parameter("ResponsiblePerson", SqlDbType.VarChar, 50, ParameterDirection.Output, ResponsiblePerson),
                new MSSQL.Parameter("ContactPerson", SqlDbType.VarChar, 50, ParameterDirection.Output, ContactPerson),
                new MSSQL.Parameter("Telephone", SqlDbType.VarChar, 50, ParameterDirection.Output, Telephone),
                new MSSQL.Parameter("Fax", SqlDbType.VarChar, 50, ParameterDirection.Output, Fax),
                new MSSQL.Parameter("Mobile", SqlDbType.VarChar, 50, ParameterDirection.Output, Mobile),
                new MSSQL.Parameter("Email", SqlDbType.VarChar, 50, ParameterDirection.Output, Email),
                new MSSQL.Parameter("QQ", SqlDbType.VarChar, 50, ParameterDirection.Output, QQ),
                new MSSQL.Parameter("ServiceTelephone", SqlDbType.VarChar, 30, ParameterDirection.Output, ServiceTelephone),
                new MSSQL.Parameter("MD5Key", SqlDbType.VarChar, 120, ParameterDirection.Output, MD5Key),
                new MSSQL.Parameter("Type", SqlDbType.SmallInt, 2, ParameterDirection.Output, Type),
                new MSSQL.Parameter("ParentID", SqlDbType.BigInt, 8, ParameterDirection.Output, ParentID),
                new MSSQL.Parameter("DomainName", SqlDbType.VarChar, 200, ParameterDirection.Output, DomainName),
                new MSSQL.Parameter("ReturnValue", SqlDbType.Int, 4, ParameterDirection.Output, ReturnValue),
                new MSSQL.Parameter("ReturnDescription", SqlDbType.VarChar, 100, ParameterDirection.Output, ReturnDescription)
                );

            try
            {
                CpsID = System.Convert.ToInt64(Outputs["CpsID"]);
            }
            catch { }

            try
            {
                Name = System.Convert.ToString(Outputs["Name"]);
            }
            catch { }

            try
            {
                Datetime = System.Convert.ToDateTime(Outputs["Datetime"]);
            }
            catch { }

            try
            {
                Url = System.Convert.ToString(Outputs["Url"]);
            }
            catch { }

            try
            {
                LogoUrl = System.Convert.ToString(Outputs["LogoUrl"]);
            }
            catch { }

            try
            {
                BonusScale = System.Convert.ToDouble(Outputs["BonusScale"]);
            }
            catch { }

            try
            {
                ON = System.Convert.ToBoolean(Outputs["ON"]);
            }
            catch { }

            try
            {
                Company = System.Convert.ToString(Outputs["Company"]);
            }
            catch { }

            try
            {
                Address = System.Convert.ToString(Outputs["Address"]);
            }
            catch { }

            try
            {
                PostCode = System.Convert.ToString(Outputs["PostCode"]);
            }
            catch { }

            try
            {
                ResponsiblePerson = System.Convert.ToString(Outputs["ResponsiblePerson"]);
            }
            catch { }

            try
            {
                ContactPerson = System.Convert.ToString(Outputs["ContactPerson"]);
            }
            catch { }

            try
            {
                Telephone = System.Convert.ToString(Outputs["Telephone"]);
            }
            catch { }

            try
            {
                Fax = System.Convert.ToString(Outputs["Fax"]);
            }
            catch { }

            try
            {
                Mobile = System.Convert.ToString(Outputs["Mobile"]);
            }
            catch { }

            try
            {
                Email = System.Convert.ToString(Outputs["Email"]);
            }
            catch { }

            try
            {
                QQ = System.Convert.ToString(Outputs["QQ"]);
            }
            catch { }

            try
            {
                ServiceTelephone = System.Convert.ToString(Outputs["ServiceTelephone"]);
            }
            catch { }

            try
            {
                MD5Key = System.Convert.ToString(Outputs["MD5Key"]);
            }
            catch { }

            try
            {
                Type = System.Convert.ToInt16(Outputs["Type"]);
            }
            catch { }

            try
            {
                ParentID = System.Convert.ToInt64(Outputs["ParentID"]);
            }
            catch { }

            try
            {
                DomainName = System.Convert.ToString(Outputs["DomainName"]);
            }
            catch { }

            try
            {
                ReturnValue = System.Convert.ToInt32(Outputs["ReturnValue"]);
            }
            catch { }

            try
            {
                ReturnDescription = System.Convert.ToString(Outputs["ReturnDescription"]);
            }
            catch { }

            return CallResult;
        }

        public static int P_GetCpsLinkList(SqlConnection conn, long CpsID, DateTime StartTime, DateTime EndTime, ref int ReturnValue, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_GetCpsLinkList(conn, ref ds, CpsID, StartTime, EndTime, ref ReturnValue, ref ReturnDescription);
        }

        public static int P_GetCpsLinkList(SqlConnection conn, ref DataSet ds, long CpsID, DateTime StartTime, DateTime EndTime, ref int ReturnValue, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(conn, "P_GetCpsLinkList", ref ds, ref Outputs,
                new MSSQL.Parameter("CpsID", SqlDbType.BigInt, 0, ParameterDirection.Input, CpsID),
                new MSSQL.Parameter("StartTime", SqlDbType.DateTime, 0, ParameterDirection.Input, StartTime),
                new MSSQL.Parameter("EndTime", SqlDbType.DateTime, 0, ParameterDirection.Input, EndTime),
                new MSSQL.Parameter("ReturnValue", SqlDbType.Int, 4, ParameterDirection.Output, ReturnValue),
                new MSSQL.Parameter("ReturnDescription", SqlDbType.VarChar, 100, ParameterDirection.Output, ReturnDescription)
                );

            try
            {
                ReturnValue = System.Convert.ToInt32(Outputs["ReturnValue"]);
            }
            catch { }

            try
            {
                ReturnDescription = System.Convert.ToString(Outputs["ReturnDescription"]);
            }
            catch { }

            return CallResult;
        }

        public static int P_GetCpsList(SqlConnection conn, ref int ReturnValue, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_GetCpsList(conn, ref ds, ref ReturnValue, ref ReturnDescription);
        }

        public static int P_GetCpsList(SqlConnection conn, ref DataSet ds, ref int ReturnValue, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(conn, "P_GetCpsList", ref ds, ref Outputs,
                new MSSQL.Parameter("ReturnValue", SqlDbType.Int, 4, ParameterDirection.Output, ReturnValue),
                new MSSQL.Parameter("ReturnDescription", SqlDbType.VarChar, 100, ParameterDirection.Output, ReturnDescription)
                );

            try
            {
                ReturnValue = System.Convert.ToInt32(Outputs["ReturnValue"]);
            }
            catch { }

            try
            {
                ReturnDescription = System.Convert.ToString(Outputs["ReturnDescription"]);
            }
            catch { }

            return CallResult;
        }

        public static int P_GetCpsMemberBuyDetail(SqlConnection conn, long SiteID, long CpsID, long UserID, DateTime FromTime, DateTime ToTime, ref int ReturnValue, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_GetCpsMemberBuyDetail(conn, ref ds, SiteID, CpsID, UserID, FromTime, ToTime, ref ReturnValue, ref ReturnDescription);
        }

        public static int P_GetCpsMemberBuyDetail(SqlConnection conn, ref DataSet ds, long SiteID, long CpsID, long UserID, DateTime FromTime, DateTime ToTime, ref int ReturnValue, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(conn, "P_GetCpsMemberBuyDetail", ref ds, ref Outputs,
                new MSSQL.Parameter("SiteID", SqlDbType.BigInt, 0, ParameterDirection.Input, SiteID),
                new MSSQL.Parameter("CpsID", SqlDbType.BigInt, 0, ParameterDirection.Input, CpsID),
                new MSSQL.Parameter("UserID", SqlDbType.BigInt, 0, ParameterDirection.Input, UserID),
                new MSSQL.Parameter("FromTime", SqlDbType.DateTime, 0, ParameterDirection.Input, FromTime),
                new MSSQL.Parameter("ToTime", SqlDbType.DateTime, 0, ParameterDirection.Input, ToTime),
                new MSSQL.Parameter("ReturnValue", SqlDbType.Int, 4, ParameterDirection.Output, ReturnValue),
                new MSSQL.Parameter("ReturnDescription", SqlDbType.VarChar, 100, ParameterDirection.Output, ReturnDescription)
                );

            try
            {
                ReturnValue = System.Convert.ToInt32(Outputs["ReturnValue"]);
            }
            catch { }

            try
            {
                ReturnDescription = System.Convert.ToString(Outputs["ReturnDescription"]);
            }
            catch { }

            return CallResult;
        }

        public static int P_GetCpsMemberList(SqlConnection conn, long CpsID, DateTime FromTime, DateTime ToTime, ref int ReturnValue, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_GetCpsMemberList(conn, ref ds, CpsID, FromTime, ToTime, ref ReturnValue, ref ReturnDescription);
        }

        public static int P_GetCpsMemberList(SqlConnection conn, ref DataSet ds, long CpsID, DateTime FromTime, DateTime ToTime, ref int ReturnValue, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(conn, "P_GetCpsMemberList", ref ds, ref Outputs,
                new MSSQL.Parameter("CpsID", SqlDbType.BigInt, 0, ParameterDirection.Input, CpsID),
                new MSSQL.Parameter("FromTime", SqlDbType.DateTime, 0, ParameterDirection.Input, FromTime),
                new MSSQL.Parameter("ToTime", SqlDbType.DateTime, 0, ParameterDirection.Input, ToTime),
                new MSSQL.Parameter("ReturnValue", SqlDbType.Int, 4, ParameterDirection.Output, ReturnValue),
                new MSSQL.Parameter("ReturnDescription", SqlDbType.VarChar, 100, ParameterDirection.Output, ReturnDescription)
                );

            try
            {
                ReturnValue = System.Convert.ToInt32(Outputs["ReturnValue"]);
            }
            catch { }

            try
            {
                ReturnDescription = System.Convert.ToString(Outputs["ReturnDescription"]);
            }
            catch { }

            return CallResult;
        }

        public static int P_GetCpsPIDBuyDetailByDate(SqlConnection conn, long SiteID, long CpsID, string PID, DateTime FromTime, DateTime ToTime, ref int ReturnValue, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_GetCpsPIDBuyDetailByDate(conn, ref ds, SiteID, CpsID, PID, FromTime, ToTime, ref ReturnValue, ref ReturnDescription);
        }

        public static int P_GetCpsPIDBuyDetailByDate(SqlConnection conn, ref DataSet ds, long SiteID, long CpsID, string PID, DateTime FromTime, DateTime ToTime, ref int ReturnValue, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(conn, "P_GetCpsPIDBuyDetailByDate", ref ds, ref Outputs,
                new MSSQL.Parameter("SiteID", SqlDbType.BigInt, 0, ParameterDirection.Input, SiteID),
                new MSSQL.Parameter("CpsID", SqlDbType.BigInt, 0, ParameterDirection.Input, CpsID),
                new MSSQL.Parameter("PID", SqlDbType.VarChar, 0, ParameterDirection.Input, PID),
                new MSSQL.Parameter("FromTime", SqlDbType.DateTime, 0, ParameterDirection.Input, FromTime),
                new MSSQL.Parameter("ToTime", SqlDbType.DateTime, 0, ParameterDirection.Input, ToTime),
                new MSSQL.Parameter("ReturnValue", SqlDbType.Int, 4, ParameterDirection.Output, ReturnValue),
                new MSSQL.Parameter("ReturnDescription", SqlDbType.VarChar, 100, ParameterDirection.Output, ReturnDescription)
                );

            try
            {
                ReturnValue = System.Convert.ToInt32(Outputs["ReturnValue"]);
            }
            catch { }

            try
            {
                ReturnDescription = System.Convert.ToString(Outputs["ReturnDescription"]);
            }
            catch { }

            return CallResult;
        }

        public static int P_GetCpsPopularizeAccount(SqlConnection conn, long SiteID, long UserID, DateTime StartTime, DateTime EndTime, int type, ref int ReturnValue, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_GetCpsPopularizeAccount(conn, ref ds, SiteID, UserID, StartTime, EndTime, type, ref ReturnValue, ref ReturnDescription);
        }

        public static int P_GetCpsPopularizeAccount(SqlConnection conn, ref DataSet ds, long SiteID, long UserID, DateTime StartTime, DateTime EndTime, int type, ref int ReturnValue, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(conn, "P_GetCpsPopularizeAccount", ref ds, ref Outputs,
                new MSSQL.Parameter("SiteID", SqlDbType.BigInt, 0, ParameterDirection.Input, SiteID),
                new MSSQL.Parameter("UserID", SqlDbType.BigInt, 0, ParameterDirection.Input, UserID),
                new MSSQL.Parameter("StartTime", SqlDbType.DateTime, 0, ParameterDirection.Input, StartTime),
                new MSSQL.Parameter("EndTime", SqlDbType.DateTime, 0, ParameterDirection.Input, EndTime),
                new MSSQL.Parameter("type", SqlDbType.Int, 0, ParameterDirection.Input, type),
                new MSSQL.Parameter("ReturnValue", SqlDbType.Int, 4, ParameterDirection.Output, ReturnValue),
                new MSSQL.Parameter("ReturnDescription", SqlDbType.VarChar, 100, ParameterDirection.Output, ReturnDescription)
                );

            try
            {
                ReturnValue = System.Convert.ToInt32(Outputs["ReturnValue"]);
            }
            catch { }

            try
            {
                ReturnDescription = System.Convert.ToString(Outputs["ReturnDescription"]);
            }
            catch { }

            return CallResult;
        }

        public static int P_GetCpsPopularizeAccountRevenue(SqlConnection conn, long SiteID, long UserID, DateTime StartTime, DateTime EndTime, int type, ref int ReturnValue, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_GetCpsPopularizeAccountRevenue(conn, ref ds, SiteID, UserID, StartTime, EndTime, type, ref ReturnValue, ref ReturnDescription);
        }

        public static int P_GetCpsPopularizeAccountRevenue(SqlConnection conn, ref DataSet ds, long SiteID, long UserID, DateTime StartTime, DateTime EndTime, int type, ref int ReturnValue, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(conn, "P_GetCpsPopularizeAccountRevenue", ref ds, ref Outputs,
                new MSSQL.Parameter("SiteID", SqlDbType.BigInt, 0, ParameterDirection.Input, SiteID),
                new MSSQL.Parameter("UserID", SqlDbType.BigInt, 0, ParameterDirection.Input, UserID),
                new MSSQL.Parameter("StartTime", SqlDbType.DateTime, 0, ParameterDirection.Input, StartTime),
                new MSSQL.Parameter("EndTime", SqlDbType.DateTime, 0, ParameterDirection.Input, EndTime),
                new MSSQL.Parameter("type", SqlDbType.Int, 0, ParameterDirection.Input, type),
                new MSSQL.Parameter("ReturnValue", SqlDbType.Int, 4, ParameterDirection.Output, ReturnValue),
                new MSSQL.Parameter("ReturnDescription", SqlDbType.VarChar, 100, ParameterDirection.Output, ReturnDescription)
                );

            try
            {
                ReturnValue = System.Convert.ToInt32(Outputs["ReturnValue"]);
            }
            catch { }

            try
            {
                ReturnDescription = System.Convert.ToString(Outputs["ReturnDescription"]);
            }
            catch { }

            return CallResult;
        }

        public static int P_GetCpsUnionBusinessList(SqlConnection conn, ref int ReturnValue, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_GetCpsUnionBusinessList(conn, ref ds, ref ReturnValue, ref ReturnDescription);
        }

        public static int P_GetCpsUnionBusinessList(SqlConnection conn, ref DataSet ds, ref int ReturnValue, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(conn, "P_GetCpsUnionBusinessList", ref ds, ref Outputs,
                new MSSQL.Parameter("ReturnValue", SqlDbType.Int, 4, ParameterDirection.Output, ReturnValue),
                new MSSQL.Parameter("ReturnDescription", SqlDbType.VarChar, 100, ParameterDirection.Output, ReturnDescription)
                );

            try
            {
                ReturnValue = System.Convert.ToInt32(Outputs["ReturnValue"]);
            }
            catch { }

            try
            {
                ReturnDescription = System.Convert.ToString(Outputs["ReturnDescription"]);
            }
            catch { }

            return CallResult;
        }

        public static int P_GetCpsWebSiteList(SqlConnection conn, ref int ReturnValue, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_GetCpsWebSiteList(conn, ref ds, ref ReturnValue, ref ReturnDescription);
        }

        public static int P_GetCpsWebSiteList(SqlConnection conn, ref DataSet ds, ref int ReturnValue, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(conn, "P_GetCpsWebSiteList", ref ds, ref Outputs,
                new MSSQL.Parameter("ReturnValue", SqlDbType.Int, 4, ParameterDirection.Output, ReturnValue),
                new MSSQL.Parameter("ReturnDescription", SqlDbType.VarChar, 100, ParameterDirection.Output, ReturnDescription)
                );

            try
            {
                ReturnValue = System.Convert.ToInt32(Outputs["ReturnValue"]);
            }
            catch { }

            try
            {
                ReturnDescription = System.Convert.ToString(Outputs["ReturnDescription"]);
            }
            catch { }

            return CallResult;
        }

        public static int P_GetDistillMoneyAndAddMoney(SqlConnection conn, long SiteID, DateTime FromDate, DateTime ToDate, int DistillType, ref int ReturnValue, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_GetDistillMoneyAndAddMoney(conn, ref ds, SiteID, FromDate, ToDate, DistillType, ref ReturnValue, ref ReturnDescription);
        }

        public static int P_GetDistillMoneyAndAddMoney(SqlConnection conn, ref DataSet ds, long SiteID, DateTime FromDate, DateTime ToDate, int DistillType, ref int ReturnValue, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(conn, "P_GetDistillMoneyAndAddMoney", ref ds, ref Outputs,
                new MSSQL.Parameter("SiteID", SqlDbType.BigInt, 0, ParameterDirection.Input, SiteID),
                new MSSQL.Parameter("FromDate", SqlDbType.DateTime, 0, ParameterDirection.Input, FromDate),
                new MSSQL.Parameter("ToDate", SqlDbType.DateTime, 0, ParameterDirection.Input, ToDate),
                new MSSQL.Parameter("DistillType", SqlDbType.Int, 0, ParameterDirection.Input, DistillType),
                new MSSQL.Parameter("ReturnValue", SqlDbType.Int, 4, ParameterDirection.Output, ReturnValue),
                new MSSQL.Parameter("ReturnDescription", SqlDbType.VarChar, 100, ParameterDirection.Output, ReturnDescription)
                );

            try
            {
                ReturnValue = System.Convert.ToInt32(Outputs["ReturnValue"]);
            }
            catch { }

            try
            {
                ReturnDescription = System.Convert.ToString(Outputs["ReturnDescription"]);
            }
            catch { }

            return CallResult;
        }

        public static int P_GetDistillStatisticByAccount(SqlConnection conn, long SiteID, DateTime FromDate, DateTime ToDate, string AccountName, ref int ReturnValue, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_GetDistillStatisticByAccount(conn, ref ds, SiteID, FromDate, ToDate, AccountName, ref ReturnValue, ref ReturnDescription);
        }

        public static int P_GetDistillStatisticByAccount(SqlConnection conn, ref DataSet ds, long SiteID, DateTime FromDate, DateTime ToDate, string AccountName, ref int ReturnValue, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(conn, "P_GetDistillStatisticByAccount", ref ds, ref Outputs,
                new MSSQL.Parameter("SiteID", SqlDbType.BigInt, 0, ParameterDirection.Input, SiteID),
                new MSSQL.Parameter("FromDate", SqlDbType.DateTime, 0, ParameterDirection.Input, FromDate),
                new MSSQL.Parameter("ToDate", SqlDbType.DateTime, 0, ParameterDirection.Input, ToDate),
                new MSSQL.Parameter("AccountName", SqlDbType.NVarChar, 0, ParameterDirection.Input, AccountName),
                new MSSQL.Parameter("ReturnValue", SqlDbType.Int, 4, ParameterDirection.Output, ReturnValue),
                new MSSQL.Parameter("ReturnDescription", SqlDbType.VarChar, 100, ParameterDirection.Output, ReturnDescription)
                );

            try
            {
                ReturnValue = System.Convert.ToInt32(Outputs["ReturnValue"]);
            }
            catch { }

            try
            {
                ReturnDescription = System.Convert.ToString(Outputs["ReturnDescription"]);
            }
            catch { }

            return CallResult;
        }

        public static int P_GetDistillStatisticByDistillType(SqlConnection conn, long SiteID, DateTime FromDate, DateTime ToDate, ref int ReturnValue, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_GetDistillStatisticByDistillType(conn, ref ds, SiteID, FromDate, ToDate, ref ReturnValue, ref ReturnDescription);
        }

        public static int P_GetDistillStatisticByDistillType(SqlConnection conn, ref DataSet ds, long SiteID, DateTime FromDate, DateTime ToDate, ref int ReturnValue, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(conn, "P_GetDistillStatisticByDistillType", ref ds, ref Outputs,
                new MSSQL.Parameter("SiteID", SqlDbType.BigInt, 0, ParameterDirection.Input, SiteID),
                new MSSQL.Parameter("FromDate", SqlDbType.DateTime, 0, ParameterDirection.Input, FromDate),
                new MSSQL.Parameter("ToDate", SqlDbType.DateTime, 0, ParameterDirection.Input, ToDate),
                new MSSQL.Parameter("ReturnValue", SqlDbType.Int, 4, ParameterDirection.Output, ReturnValue),
                new MSSQL.Parameter("ReturnDescription", SqlDbType.VarChar, 100, ParameterDirection.Output, ReturnDescription)
                );

            try
            {
                ReturnValue = System.Convert.ToInt32(Outputs["ReturnValue"]);
            }
            catch { }

            try
            {
                ReturnDescription = System.Convert.ToString(Outputs["ReturnDescription"]);
            }
            catch { }

            return CallResult;
        }

        public static int P_GetExpertsAccount(SqlConnection conn, long SiteID, int Year, int Month, long ExpertsID, ref int ReturnValue, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_GetExpertsAccount(conn, ref ds, SiteID, Year, Month, ExpertsID, ref ReturnValue, ref ReturnDescription);
        }

        public static int P_GetExpertsAccount(SqlConnection conn, ref DataSet ds, long SiteID, int Year, int Month, long ExpertsID, ref int ReturnValue, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(conn, "P_GetExpertsAccount", ref ds, ref Outputs,
                new MSSQL.Parameter("SiteID", SqlDbType.BigInt, 0, ParameterDirection.Input, SiteID),
                new MSSQL.Parameter("Year", SqlDbType.Int, 0, ParameterDirection.Input, Year),
                new MSSQL.Parameter("Month", SqlDbType.Int, 0, ParameterDirection.Input, Month),
                new MSSQL.Parameter("ExpertsID", SqlDbType.BigInt, 0, ParameterDirection.Input, ExpertsID),
                new MSSQL.Parameter("ReturnValue", SqlDbType.Int, 4, ParameterDirection.Output, ReturnValue),
                new MSSQL.Parameter("ReturnDescription", SqlDbType.VarChar, 100, ParameterDirection.Output, ReturnDescription)
                );

            try
            {
                ReturnValue = System.Convert.ToInt32(Outputs["ReturnValue"]);
            }
            catch { }

            try
            {
                ReturnDescription = System.Convert.ToString(Outputs["ReturnDescription"]);
            }
            catch { }

            return CallResult;
        }

        public static int P_GetExpertsAccountDetail(SqlConnection conn, long SiteID, long UserID, int Year, int Month, ref int ReturnValue, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_GetExpertsAccountDetail(conn, ref ds, SiteID, UserID, Year, Month, ref ReturnValue, ref ReturnDescription);
        }

        public static int P_GetExpertsAccountDetail(SqlConnection conn, ref DataSet ds, long SiteID, long UserID, int Year, int Month, ref int ReturnValue, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(conn, "P_GetExpertsAccountDetail", ref ds, ref Outputs,
                new MSSQL.Parameter("SiteID", SqlDbType.BigInt, 0, ParameterDirection.Input, SiteID),
                new MSSQL.Parameter("UserID", SqlDbType.BigInt, 0, ParameterDirection.Input, UserID),
                new MSSQL.Parameter("Year", SqlDbType.Int, 0, ParameterDirection.Input, Year),
                new MSSQL.Parameter("Month", SqlDbType.Int, 0, ParameterDirection.Input, Month),
                new MSSQL.Parameter("ReturnValue", SqlDbType.Int, 4, ParameterDirection.Output, ReturnValue),
                new MSSQL.Parameter("ReturnDescription", SqlDbType.VarChar, 100, ParameterDirection.Output, ReturnDescription)
                );

            try
            {
                ReturnValue = System.Convert.ToInt32(Outputs["ReturnValue"]);
            }
            catch { }

            try
            {
                ReturnDescription = System.Convert.ToString(Outputs["ReturnDescription"]);
            }
            catch { }

            return CallResult;
        }

        public static int P_GetExpertsGroupUserID(SqlConnection conn, int ExpertsCount)
        {
            DataSet ds = null;

            return P_GetExpertsGroupUserID(conn, ref ds, ExpertsCount);
        }

        public static int P_GetExpertsGroupUserID(SqlConnection conn, ref DataSet ds, int ExpertsCount)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(conn, "P_GetExpertsGroupUserID", ref ds, ref Outputs,
                new MSSQL.Parameter("ExpertsCount", SqlDbType.Int, 0, ParameterDirection.Input, ExpertsCount)
                );

            return CallResult;
        }

        public static int P_GetFinanceAddMoney(SqlConnection conn, long SiteID, long UserID, int Year, int Month, ref int ReturnValue, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_GetFinanceAddMoney(conn, ref ds, SiteID, UserID, Year, Month, ref ReturnValue, ref ReturnDescription);
        }

        public static int P_GetFinanceAddMoney(SqlConnection conn, ref DataSet ds, long SiteID, long UserID, int Year, int Month, ref int ReturnValue, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(conn, "P_GetFinanceAddMoney", ref ds, ref Outputs,
                new MSSQL.Parameter("SiteID", SqlDbType.BigInt, 0, ParameterDirection.Input, SiteID),
                new MSSQL.Parameter("UserID", SqlDbType.BigInt, 0, ParameterDirection.Input, UserID),
                new MSSQL.Parameter("Year", SqlDbType.Int, 0, ParameterDirection.Input, Year),
                new MSSQL.Parameter("Month", SqlDbType.Int, 0, ParameterDirection.Input, Month),
                new MSSQL.Parameter("ReturnValue", SqlDbType.Int, 4, ParameterDirection.Output, ReturnValue),
                new MSSQL.Parameter("ReturnDescription", SqlDbType.VarChar, 100, ParameterDirection.Output, ReturnDescription)
                );

            try
            {
                ReturnValue = System.Convert.ToInt32(Outputs["ReturnValue"]);
            }
            catch { }

            try
            {
                ReturnDescription = System.Convert.ToString(Outputs["ReturnDescription"]);
            }
            catch { }

            return CallResult;
        }

        public static int P_GetFriendsInitiateSchemes(SqlConnection conn, long SiteID, long UserID, long LotterID, long IsuseID, ref int ReturnValue, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_GetFriendsInitiateSchemes(conn, ref ds, SiteID, UserID, LotterID, IsuseID, ref ReturnValue, ref ReturnDescription);
        }

        public static int P_GetFriendsInitiateSchemes(SqlConnection conn, ref DataSet ds, long SiteID, long UserID, long LotterID, long IsuseID, ref int ReturnValue, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(conn, "P_GetFriendsInitiateSchemes", ref ds, ref Outputs,
                new MSSQL.Parameter("SiteID", SqlDbType.BigInt, 0, ParameterDirection.Input, SiteID),
                new MSSQL.Parameter("UserID", SqlDbType.BigInt, 0, ParameterDirection.Input, UserID),
                new MSSQL.Parameter("LotterID", SqlDbType.BigInt, 0, ParameterDirection.Input, LotterID),
                new MSSQL.Parameter("IsuseID", SqlDbType.BigInt, 0, ParameterDirection.Input, IsuseID),
                new MSSQL.Parameter("ReturnValue", SqlDbType.Int, 4, ParameterDirection.Output, ReturnValue),
                new MSSQL.Parameter("ReturnDescription", SqlDbType.VarChar, 100, ParameterDirection.Output, ReturnDescription)
                );

            try
            {
                ReturnValue = System.Convert.ToInt32(Outputs["ReturnValue"]);
            }
            catch { }

            try
            {
                ReturnDescription = System.Convert.ToString(Outputs["ReturnDescription"]);
            }
            catch { }

            return CallResult;
        }

        public static int P_GetFriendsWinInfo(SqlConnection conn, long UserID, string SnsName, ref int ReturnValue, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_GetFriendsWinInfo(conn, ref ds, UserID, SnsName, ref ReturnValue, ref ReturnDescription);
        }

        public static int P_GetFriendsWinInfo(SqlConnection conn, ref DataSet ds, long UserID, string SnsName, ref int ReturnValue, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(conn, "P_GetFriendsWinInfo", ref ds, ref Outputs,
                new MSSQL.Parameter("UserID", SqlDbType.BigInt, 0, ParameterDirection.Input, UserID),
                new MSSQL.Parameter("SnsName", SqlDbType.VarChar, 0, ParameterDirection.Input, SnsName),
                new MSSQL.Parameter("ReturnValue", SqlDbType.Int, 4, ParameterDirection.Output, ReturnValue),
                new MSSQL.Parameter("ReturnDescription", SqlDbType.VarChar, 100, ParameterDirection.Output, ReturnDescription)
                );

            try
            {
                ReturnValue = System.Convert.ToInt32(Outputs["ReturnValue"]);
            }
            catch { }

            try
            {
                ReturnDescription = System.Convert.ToString(Outputs["ReturnDescription"]);
            }
            catch { }

            return CallResult;
        }

        public static int P_GetIsuseSalesAmount(SqlConnection conn, long SiteID)
        {
            DataSet ds = null;

            return P_GetIsuseSalesAmount(conn, ref ds, SiteID);
        }

        public static int P_GetIsuseSalesAmount(SqlConnection conn, ref DataSet ds, long SiteID)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(conn, "P_GetIsuseSalesAmount", ref ds, ref Outputs,
                new MSSQL.Parameter("SiteID", SqlDbType.BigInt, 0, ParameterDirection.Input, SiteID)
                );

            return CallResult;
        }

        public static int P_GetLoginCount(SqlConnection conn, int Year, int Month)
        {
            DataSet ds = null;

            return P_GetLoginCount(conn, ref ds, Year, Month);
        }

        public static int P_GetLoginCount(SqlConnection conn, ref DataSet ds, int Year, int Month)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(conn, "P_GetLoginCount", ref ds, ref Outputs,
                new MSSQL.Parameter("Year", SqlDbType.Int, 0, ParameterDirection.Input, Year),
                new MSSQL.Parameter("Month", SqlDbType.Int, 0, ParameterDirection.Input, Month)
                );

            return CallResult;
        }

        public static int P_GetNewPayNumber(SqlConnection conn, long SiteID, long UserID, string PayType, double Money, double FormalitiesFees, ref long NewPayNumber, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_GetNewPayNumber(conn, ref ds, SiteID, UserID, PayType, Money, FormalitiesFees, ref NewPayNumber, ref ReturnDescription);
        }

        public static int P_GetNewPayNumber(SqlConnection conn, ref DataSet ds, long SiteID, long UserID, string PayType, double Money, double FormalitiesFees, ref long NewPayNumber, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(conn, "P_GetNewPayNumber", ref ds, ref Outputs,
                new MSSQL.Parameter("SiteID", SqlDbType.BigInt, 0, ParameterDirection.Input, SiteID),
                new MSSQL.Parameter("UserID", SqlDbType.BigInt, 0, ParameterDirection.Input, UserID),
                new MSSQL.Parameter("PayType", SqlDbType.VarChar, 0, ParameterDirection.Input, PayType),
                new MSSQL.Parameter("Money", SqlDbType.Money, 0, ParameterDirection.Input, Money),
                new MSSQL.Parameter("FormalitiesFees", SqlDbType.Money, 0, ParameterDirection.Input, FormalitiesFees),
                new MSSQL.Parameter("NewPayNumber", SqlDbType.BigInt, 8, ParameterDirection.Output, NewPayNumber),
                new MSSQL.Parameter("ReturnDescription", SqlDbType.VarChar, 100, ParameterDirection.Output, ReturnDescription)
                );

            try
            {
                NewPayNumber = System.Convert.ToInt64(Outputs["NewPayNumber"]);
            }
            catch { }

            try
            {
                ReturnDescription = System.Convert.ToString(Outputs["ReturnDescription"]);
            }
            catch { }

            return CallResult;
        }

        public static int P_GetPromoterInfoByIDList(SqlConnection conn, long SiteID, string UserIDList, ref int ReturnValue, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_GetPromoterInfoByIDList(conn, ref ds, SiteID, UserIDList, ref ReturnValue, ref ReturnDescription);
        }

        public static int P_GetPromoterInfoByIDList(SqlConnection conn, ref DataSet ds, long SiteID, string UserIDList, ref int ReturnValue, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(conn, "P_GetPromoterInfoByIDList", ref ds, ref Outputs,
                new MSSQL.Parameter("SiteID", SqlDbType.BigInt, 0, ParameterDirection.Input, SiteID),
                new MSSQL.Parameter("UserIDList", SqlDbType.VarChar, 0, ParameterDirection.Input, UserIDList),
                new MSSQL.Parameter("ReturnValue", SqlDbType.Int, 4, ParameterDirection.Output, ReturnValue),
                new MSSQL.Parameter("ReturnDescription", SqlDbType.VarChar, 100, ParameterDirection.Output, ReturnDescription)
                );

            try
            {
                ReturnValue = System.Convert.ToInt32(Outputs["ReturnValue"]);
            }
            catch { }

            try
            {
                ReturnDescription = System.Convert.ToString(Outputs["ReturnDescription"]);
            }
            catch { }

            return CallResult;
        }

        public static int P_GetSchemeChatContents(SqlConnection conn, long SiteID, long UserID, long SchemeID, ref int ReturnValue, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_GetSchemeChatContents(conn, ref ds, SiteID, UserID, SchemeID, ref ReturnValue, ref ReturnDescription);
        }

        public static int P_GetSchemeChatContents(SqlConnection conn, ref DataSet ds, long SiteID, long UserID, long SchemeID, ref int ReturnValue, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(conn, "P_GetSchemeChatContents", ref ds, ref Outputs,
                new MSSQL.Parameter("SiteID", SqlDbType.BigInt, 0, ParameterDirection.Input, SiteID),
                new MSSQL.Parameter("UserID", SqlDbType.BigInt, 0, ParameterDirection.Input, UserID),
                new MSSQL.Parameter("SchemeID", SqlDbType.BigInt, 0, ParameterDirection.Input, SchemeID),
                new MSSQL.Parameter("ReturnValue", SqlDbType.Int, 4, ParameterDirection.Output, ReturnValue),
                new MSSQL.Parameter("ReturnDescription", SqlDbType.VarChar, 100, ParameterDirection.Output, ReturnDescription)
                );

            try
            {
                ReturnValue = System.Convert.ToInt32(Outputs["ReturnValue"]);
            }
            catch { }

            try
            {
                ReturnDescription = System.Convert.ToString(Outputs["ReturnDescription"]);
            }
            catch { }

            return CallResult;
        }

        public static int P_GetSedimentBalance(SqlConnection conn, ref double SedimentBalance)
        {
            DataSet ds = null;

            return P_GetSedimentBalance(conn, ref ds, ref SedimentBalance);
        }

        public static int P_GetSedimentBalance(SqlConnection conn, ref DataSet ds, ref double SedimentBalance)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(conn, "P_GetSedimentBalance", ref ds, ref Outputs,
                new MSSQL.Parameter("SedimentBalance", SqlDbType.Money, 8, ParameterDirection.Output, SedimentBalance)
                );

            try
            {
                SedimentBalance = System.Convert.ToDouble(Outputs["SedimentBalance"]);
            }
            catch { }

            return CallResult;
        }

        public static int P_GetSiteInformationByID(SqlConnection conn, long SiteID, ref long ParentID, ref long OwnerUserID, ref string Name, ref string LogoUrl, ref string Company, ref string Address, ref string PostCode, ref string ResponsiblePerson, ref string ContactPerson, ref string Telephone, ref string Fax, ref string Mobile, ref string Email, ref string QQ, ref string ServiceTelephone, ref string ICPCert, ref short Level, ref bool ON, ref double BonusScale, ref int MaxSubSites, ref string UseLotteryListRestrictions, ref string UseLotteryList, ref string UseLotteryListQuickBuy, ref string Urls, ref long AdministratorID, ref string PageTitle, ref string PageKeywords, ref int ReturnValue, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_GetSiteInformationByID(conn, ref ds, SiteID, ref ParentID, ref OwnerUserID, ref Name, ref LogoUrl, ref Company, ref Address, ref PostCode, ref ResponsiblePerson, ref ContactPerson, ref Telephone, ref Fax, ref Mobile, ref Email, ref QQ, ref ServiceTelephone, ref ICPCert, ref Level, ref ON, ref BonusScale, ref MaxSubSites, ref UseLotteryListRestrictions, ref UseLotteryList, ref UseLotteryListQuickBuy, ref Urls, ref AdministratorID, ref PageTitle, ref PageKeywords, ref ReturnValue, ref ReturnDescription);
        }

        public static int P_GetSiteInformationByID(SqlConnection conn, ref DataSet ds, long SiteID, ref long ParentID, ref long OwnerUserID, ref string Name, ref string LogoUrl, ref string Company, ref string Address, ref string PostCode, ref string ResponsiblePerson, ref string ContactPerson, ref string Telephone, ref string Fax, ref string Mobile, ref string Email, ref string QQ, ref string ServiceTelephone, ref string ICPCert, ref short Level, ref bool ON, ref double BonusScale, ref int MaxSubSites, ref string UseLotteryListRestrictions, ref string UseLotteryList, ref string UseLotteryListQuickBuy, ref string Urls, ref long AdministratorID, ref string PageTitle, ref string PageKeywords, ref int ReturnValue, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(conn, "P_GetSiteInformationByID", ref ds, ref Outputs,
                new MSSQL.Parameter("SiteID", SqlDbType.BigInt, 0, ParameterDirection.Input, SiteID),
                new MSSQL.Parameter("ParentID", SqlDbType.BigInt, 8, ParameterDirection.Output, ParentID),
                new MSSQL.Parameter("OwnerUserID", SqlDbType.BigInt, 8, ParameterDirection.Output, OwnerUserID),
                new MSSQL.Parameter("Name", SqlDbType.VarChar, 50, ParameterDirection.Output, Name),
                new MSSQL.Parameter("LogoUrl", SqlDbType.VarChar, 200, ParameterDirection.Output, LogoUrl),
                new MSSQL.Parameter("Company", SqlDbType.VarChar, 50, ParameterDirection.Output, Company),
                new MSSQL.Parameter("Address", SqlDbType.VarChar, 50, ParameterDirection.Output, Address),
                new MSSQL.Parameter("PostCode", SqlDbType.VarChar, 6, ParameterDirection.Output, PostCode),
                new MSSQL.Parameter("ResponsiblePerson", SqlDbType.VarChar, 50, ParameterDirection.Output, ResponsiblePerson),
                new MSSQL.Parameter("ContactPerson", SqlDbType.VarChar, 50, ParameterDirection.Output, ContactPerson),
                new MSSQL.Parameter("Telephone", SqlDbType.VarChar, 50, ParameterDirection.Output, Telephone),
                new MSSQL.Parameter("Fax", SqlDbType.VarChar, 50, ParameterDirection.Output, Fax),
                new MSSQL.Parameter("Mobile", SqlDbType.VarChar, 50, ParameterDirection.Output, Mobile),
                new MSSQL.Parameter("Email", SqlDbType.VarChar, 50, ParameterDirection.Output, Email),
                new MSSQL.Parameter("QQ", SqlDbType.VarChar, 50, ParameterDirection.Output, QQ),
                new MSSQL.Parameter("ServiceTelephone", SqlDbType.VarChar, 50, ParameterDirection.Output, ServiceTelephone),
                new MSSQL.Parameter("ICPCert", SqlDbType.VarChar, 20, ParameterDirection.Output, ICPCert),
                new MSSQL.Parameter("Level", SqlDbType.SmallInt, 2, ParameterDirection.Output, Level),
                new MSSQL.Parameter("ON", SqlDbType.Bit, 1, ParameterDirection.Output, ON),
                new MSSQL.Parameter("BonusScale", SqlDbType.Float, 8, ParameterDirection.Output, BonusScale),
                new MSSQL.Parameter("MaxSubSites", SqlDbType.Int, 4, ParameterDirection.Output, MaxSubSites),
                new MSSQL.Parameter("UseLotteryListRestrictions", SqlDbType.VarChar, 1000, ParameterDirection.Output, UseLotteryListRestrictions),
                new MSSQL.Parameter("UseLotteryList", SqlDbType.VarChar, 1000, ParameterDirection.Output, UseLotteryList),
                new MSSQL.Parameter("UseLotteryListQuickBuy", SqlDbType.VarChar, 100, ParameterDirection.Output, UseLotteryListQuickBuy),
                new MSSQL.Parameter("Urls", SqlDbType.VarChar, 8000, ParameterDirection.Output, Urls),
                new MSSQL.Parameter("AdministratorID", SqlDbType.BigInt, 8, ParameterDirection.Output, AdministratorID),
                new MSSQL.Parameter("PageTitle", SqlDbType.VarChar, 1000, ParameterDirection.Output, PageTitle),
                new MSSQL.Parameter("PageKeywords", SqlDbType.VarChar, 1000, ParameterDirection.Output, PageKeywords),
                new MSSQL.Parameter("ReturnValue", SqlDbType.Int, 4, ParameterDirection.Output, ReturnValue),
                new MSSQL.Parameter("ReturnDescription", SqlDbType.VarChar, 100, ParameterDirection.Output, ReturnDescription)
                );

            try
            {
                ParentID = System.Convert.ToInt64(Outputs["ParentID"]);
            }
            catch { }

            try
            {
                OwnerUserID = System.Convert.ToInt64(Outputs["OwnerUserID"]);
            }
            catch { }

            try
            {
                Name = System.Convert.ToString(Outputs["Name"]);
            }
            catch { }

            try
            {
                LogoUrl = System.Convert.ToString(Outputs["LogoUrl"]);
            }
            catch { }

            try
            {
                Company = System.Convert.ToString(Outputs["Company"]);
            }
            catch { }

            try
            {
                Address = System.Convert.ToString(Outputs["Address"]);
            }
            catch { }

            try
            {
                PostCode = System.Convert.ToString(Outputs["PostCode"]);
            }
            catch { }

            try
            {
                ResponsiblePerson = System.Convert.ToString(Outputs["ResponsiblePerson"]);
            }
            catch { }

            try
            {
                ContactPerson = System.Convert.ToString(Outputs["ContactPerson"]);
            }
            catch { }

            try
            {
                Telephone = System.Convert.ToString(Outputs["Telephone"]);
            }
            catch { }

            try
            {
                Fax = System.Convert.ToString(Outputs["Fax"]);
            }
            catch { }

            try
            {
                Mobile = System.Convert.ToString(Outputs["Mobile"]);
            }
            catch { }

            try
            {
                Email = System.Convert.ToString(Outputs["Email"]);
            }
            catch { }

            try
            {
                QQ = System.Convert.ToString(Outputs["QQ"]);
            }
            catch { }

            try
            {
                ServiceTelephone = System.Convert.ToString(Outputs["ServiceTelephone"]);
            }
            catch { }

            try
            {
                ICPCert = System.Convert.ToString(Outputs["ICPCert"]);
            }
            catch { }

            try
            {
                Level = System.Convert.ToInt16(Outputs["Level"]);
            }
            catch { }

            try
            {
                ON = System.Convert.ToBoolean(Outputs["ON"]);
            }
            catch { }

            try
            {
                BonusScale = System.Convert.ToDouble(Outputs["BonusScale"]);
            }
            catch { }

            try
            {
                MaxSubSites = System.Convert.ToInt32(Outputs["MaxSubSites"]);
            }
            catch { }

            try
            {
                UseLotteryListRestrictions = System.Convert.ToString(Outputs["UseLotteryListRestrictions"]);
            }
            catch { }

            try
            {
                UseLotteryList = System.Convert.ToString(Outputs["UseLotteryList"]);
            }
            catch { }

            try
            {
                UseLotteryListQuickBuy = System.Convert.ToString(Outputs["UseLotteryListQuickBuy"]);
            }
            catch { }

            try
            {
                Urls = System.Convert.ToString(Outputs["Urls"]);
            }
            catch { }

            try
            {
                AdministratorID = System.Convert.ToInt64(Outputs["AdministratorID"]);
            }
            catch { }

            try
            {
                PageTitle = System.Convert.ToString(Outputs["PageTitle"]);
            }
            catch { }

            try
            {
                PageKeywords = System.Convert.ToString(Outputs["PageKeywords"]);
            }
            catch { }

            try
            {
                ReturnValue = System.Convert.ToInt32(Outputs["ReturnValue"]);
            }
            catch { }

            try
            {
                ReturnDescription = System.Convert.ToString(Outputs["ReturnDescription"]);
            }
            catch { }

            return CallResult;
        }

        public static int P_GetSiteInformationByUrl(SqlConnection conn, string Url, ref long SiteID, ref long ParentID, ref long OwnerUserID, ref string Name, ref string LogoUrl, ref string Company, ref string Address, ref string PostCode, ref string ResponsiblePerson, ref string ContactPerson, ref string Telephone, ref string Fax, ref string Mobile, ref string Email, ref string QQ, ref string ServiceTelephone, ref string ICPCert, ref short Level, ref bool ON, ref double BonusScale, ref int MaxSubSites, ref string UseLotteryListRestrictions, ref string UseLotteryList, ref string UseLotteryListQuickBuy, ref string Urls, ref long AdministratorID, ref string PageTitle, ref string PageKeywords, ref int ReturnValue, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_GetSiteInformationByUrl(conn, ref ds, Url, ref SiteID, ref ParentID, ref OwnerUserID, ref Name, ref LogoUrl, ref Company, ref Address, ref PostCode, ref ResponsiblePerson, ref ContactPerson, ref Telephone, ref Fax, ref Mobile, ref Email, ref QQ, ref ServiceTelephone, ref ICPCert, ref Level, ref ON, ref BonusScale, ref MaxSubSites, ref UseLotteryListRestrictions, ref UseLotteryList, ref UseLotteryListQuickBuy, ref Urls, ref AdministratorID, ref PageTitle, ref PageKeywords, ref ReturnValue, ref ReturnDescription);
        }

        public static int P_GetSiteInformationByUrl(SqlConnection conn, ref DataSet ds, string Url, ref long SiteID, ref long ParentID, ref long OwnerUserID, ref string Name, ref string LogoUrl, ref string Company, ref string Address, ref string PostCode, ref string ResponsiblePerson, ref string ContactPerson, ref string Telephone, ref string Fax, ref string Mobile, ref string Email, ref string QQ, ref string ServiceTelephone, ref string ICPCert, ref short Level, ref bool ON, ref double BonusScale, ref int MaxSubSites, ref string UseLotteryListRestrictions, ref string UseLotteryList, ref string UseLotteryListQuickBuy, ref string Urls, ref long AdministratorID, ref string PageTitle, ref string PageKeywords, ref int ReturnValue, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(conn, "P_GetSiteInformationByUrl", ref ds, ref Outputs,
                new MSSQL.Parameter("Url", SqlDbType.VarChar, 0, ParameterDirection.Input, Url),
                new MSSQL.Parameter("SiteID", SqlDbType.BigInt, 8, ParameterDirection.Output, SiteID),
                new MSSQL.Parameter("ParentID", SqlDbType.BigInt, 8, ParameterDirection.Output, ParentID),
                new MSSQL.Parameter("OwnerUserID", SqlDbType.BigInt, 8, ParameterDirection.Output, OwnerUserID),
                new MSSQL.Parameter("Name", SqlDbType.VarChar, 50, ParameterDirection.Output, Name),
                new MSSQL.Parameter("LogoUrl", SqlDbType.VarChar, 200, ParameterDirection.Output, LogoUrl),
                new MSSQL.Parameter("Company", SqlDbType.VarChar, 50, ParameterDirection.Output, Company),
                new MSSQL.Parameter("Address", SqlDbType.VarChar, 50, ParameterDirection.Output, Address),
                new MSSQL.Parameter("PostCode", SqlDbType.VarChar, 6, ParameterDirection.Output, PostCode),
                new MSSQL.Parameter("ResponsiblePerson", SqlDbType.VarChar, 50, ParameterDirection.Output, ResponsiblePerson),
                new MSSQL.Parameter("ContactPerson", SqlDbType.VarChar, 50, ParameterDirection.Output, ContactPerson),
                new MSSQL.Parameter("Telephone", SqlDbType.VarChar, 50, ParameterDirection.Output, Telephone),
                new MSSQL.Parameter("Fax", SqlDbType.VarChar, 50, ParameterDirection.Output, Fax),
                new MSSQL.Parameter("Mobile", SqlDbType.VarChar, 50, ParameterDirection.Output, Mobile),
                new MSSQL.Parameter("Email", SqlDbType.VarChar, 50, ParameterDirection.Output, Email),
                new MSSQL.Parameter("QQ", SqlDbType.VarChar, 50, ParameterDirection.Output, QQ),
                new MSSQL.Parameter("ServiceTelephone", SqlDbType.VarChar, 50, ParameterDirection.Output, ServiceTelephone),
                new MSSQL.Parameter("ICPCert", SqlDbType.VarChar, 20, ParameterDirection.Output, ICPCert),
                new MSSQL.Parameter("Level", SqlDbType.SmallInt, 2, ParameterDirection.Output, Level),
                new MSSQL.Parameter("ON", SqlDbType.Bit, 1, ParameterDirection.Output, ON),
                new MSSQL.Parameter("BonusScale", SqlDbType.Float, 8, ParameterDirection.Output, BonusScale),
                new MSSQL.Parameter("MaxSubSites", SqlDbType.Int, 4, ParameterDirection.Output, MaxSubSites),
                new MSSQL.Parameter("UseLotteryListRestrictions", SqlDbType.VarChar, 1000, ParameterDirection.Output, UseLotteryListRestrictions),
                new MSSQL.Parameter("UseLotteryList", SqlDbType.VarChar, 1000, ParameterDirection.Output, UseLotteryList),
                new MSSQL.Parameter("UseLotteryListQuickBuy", SqlDbType.VarChar, 100, ParameterDirection.Output, UseLotteryListQuickBuy),
                new MSSQL.Parameter("Urls", SqlDbType.VarChar, 8000, ParameterDirection.Output, Urls),
                new MSSQL.Parameter("AdministratorID", SqlDbType.BigInt, 8, ParameterDirection.Output, AdministratorID),
                new MSSQL.Parameter("PageTitle", SqlDbType.VarChar, 1000, ParameterDirection.Output, PageTitle),
                new MSSQL.Parameter("PageKeywords", SqlDbType.VarChar, 1000, ParameterDirection.Output, PageKeywords),
                new MSSQL.Parameter("ReturnValue", SqlDbType.Int, 4, ParameterDirection.Output, ReturnValue),
                new MSSQL.Parameter("ReturnDescription", SqlDbType.VarChar, 100, ParameterDirection.Output, ReturnDescription)
                );

            try
            {
                SiteID = System.Convert.ToInt64(Outputs["SiteID"]);
            }
            catch { }

            try
            {
                ParentID = System.Convert.ToInt64(Outputs["ParentID"]);
            }
            catch { }

            try
            {
                OwnerUserID = System.Convert.ToInt64(Outputs["OwnerUserID"]);
            }
            catch { }

            try
            {
                Name = System.Convert.ToString(Outputs["Name"]);
            }
            catch { }

            try
            {
                LogoUrl = System.Convert.ToString(Outputs["LogoUrl"]);
            }
            catch { }

            try
            {
                Company = System.Convert.ToString(Outputs["Company"]);
            }
            catch { }

            try
            {
                Address = System.Convert.ToString(Outputs["Address"]);
            }
            catch { }

            try
            {
                PostCode = System.Convert.ToString(Outputs["PostCode"]);
            }
            catch { }

            try
            {
                ResponsiblePerson = System.Convert.ToString(Outputs["ResponsiblePerson"]);
            }
            catch { }

            try
            {
                ContactPerson = System.Convert.ToString(Outputs["ContactPerson"]);
            }
            catch { }

            try
            {
                Telephone = System.Convert.ToString(Outputs["Telephone"]);
            }
            catch { }

            try
            {
                Fax = System.Convert.ToString(Outputs["Fax"]);
            }
            catch { }

            try
            {
                Mobile = System.Convert.ToString(Outputs["Mobile"]);
            }
            catch { }

            try
            {
                Email = System.Convert.ToString(Outputs["Email"]);
            }
            catch { }

            try
            {
                QQ = System.Convert.ToString(Outputs["QQ"]);
            }
            catch { }

            try
            {
                ServiceTelephone = System.Convert.ToString(Outputs["ServiceTelephone"]);
            }
            catch { }

            try
            {
                ICPCert = System.Convert.ToString(Outputs["ICPCert"]);
            }
            catch { }

            try
            {
                Level = System.Convert.ToInt16(Outputs["Level"]);
            }
            catch { }

            try
            {
                ON = System.Convert.ToBoolean(Outputs["ON"]);
            }
            catch { }

            try
            {
                BonusScale = System.Convert.ToDouble(Outputs["BonusScale"]);
            }
            catch { }

            try
            {
                MaxSubSites = System.Convert.ToInt32(Outputs["MaxSubSites"]);
            }
            catch { }

            try
            {
                UseLotteryListRestrictions = System.Convert.ToString(Outputs["UseLotteryListRestrictions"]);
            }
            catch { }

            try
            {
                UseLotteryList = System.Convert.ToString(Outputs["UseLotteryList"]);
            }
            catch { }

            try
            {
                UseLotteryListQuickBuy = System.Convert.ToString(Outputs["UseLotteryListQuickBuy"]);
            }
            catch { }

            try
            {
                Urls = System.Convert.ToString(Outputs["Urls"]);
            }
            catch { }

            try
            {
                AdministratorID = System.Convert.ToInt64(Outputs["AdministratorID"]);
            }
            catch { }

            try
            {
                PageTitle = System.Convert.ToString(Outputs["PageTitle"]);
            }
            catch { }

            try
            {
                PageKeywords = System.Convert.ToString(Outputs["PageKeywords"]);
            }
            catch { }

            try
            {
                ReturnValue = System.Convert.ToInt32(Outputs["ReturnValue"]);
            }
            catch { }

            try
            {
                ReturnDescription = System.Convert.ToString(Outputs["ReturnDescription"]);
            }
            catch { }

            return CallResult;
        }

        public static int P_GetSiteNotificationTemplate(SqlConnection conn, long SiteID, short Manner, string NotificationType, ref string Value)
        {
            DataSet ds = null;

            return P_GetSiteNotificationTemplate(conn, ref ds, SiteID, Manner, NotificationType, ref Value);
        }

        public static int P_GetSiteNotificationTemplate(SqlConnection conn, ref DataSet ds, long SiteID, short Manner, string NotificationType, ref string Value)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(conn, "P_GetSiteNotificationTemplate", ref ds, ref Outputs,
                new MSSQL.Parameter("SiteID", SqlDbType.BigInt, 0, ParameterDirection.Input, SiteID),
                new MSSQL.Parameter("Manner", SqlDbType.SmallInt, 0, ParameterDirection.Input, Manner),
                new MSSQL.Parameter("NotificationType", SqlDbType.VarChar, 0, ParameterDirection.Input, NotificationType),
                new MSSQL.Parameter("Value", SqlDbType.VarChar, 1073741823, ParameterDirection.Output, Value)
                );

            try
            {
                Value = System.Convert.ToString(Outputs["Value"]);
            }
            catch { }

            return CallResult;
        }

        public static int P_GetSurrogateAccount(SqlConnection conn, long SiteID, int Year, int Month, long SubSiteID, ref int ReturnValue, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_GetSurrogateAccount(conn, ref ds, SiteID, Year, Month, SubSiteID, ref ReturnValue, ref ReturnDescription);
        }

        public static int P_GetSurrogateAccount(SqlConnection conn, ref DataSet ds, long SiteID, int Year, int Month, long SubSiteID, ref int ReturnValue, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(conn, "P_GetSurrogateAccount", ref ds, ref Outputs,
                new MSSQL.Parameter("SiteID", SqlDbType.BigInt, 0, ParameterDirection.Input, SiteID),
                new MSSQL.Parameter("Year", SqlDbType.Int, 0, ParameterDirection.Input, Year),
                new MSSQL.Parameter("Month", SqlDbType.Int, 0, ParameterDirection.Input, Month),
                new MSSQL.Parameter("SubSiteID", SqlDbType.BigInt, 0, ParameterDirection.Input, SubSiteID),
                new MSSQL.Parameter("ReturnValue", SqlDbType.Int, 4, ParameterDirection.Output, ReturnValue),
                new MSSQL.Parameter("ReturnDescription", SqlDbType.VarChar, 100, ParameterDirection.Output, ReturnDescription)
                );

            try
            {
                ReturnValue = System.Convert.ToInt32(Outputs["ReturnValue"]);
            }
            catch { }

            try
            {
                ReturnDescription = System.Convert.ToString(Outputs["ReturnDescription"]);
            }
            catch { }

            return CallResult;
        }

        public static int P_GetSurrogateSalesRanked(SqlConnection conn, long SiteID, int RanksType, int Year, int Month, int Top, ref int ReturnValue, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_GetSurrogateSalesRanked(conn, ref ds, SiteID, RanksType, Year, Month, Top, ref ReturnValue, ref ReturnDescription);
        }

        public static int P_GetSurrogateSalesRanked(SqlConnection conn, ref DataSet ds, long SiteID, int RanksType, int Year, int Month, int Top, ref int ReturnValue, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(conn, "P_GetSurrogateSalesRanked", ref ds, ref Outputs,
                new MSSQL.Parameter("SiteID", SqlDbType.BigInt, 0, ParameterDirection.Input, SiteID),
                new MSSQL.Parameter("RanksType", SqlDbType.Int, 0, ParameterDirection.Input, RanksType),
                new MSSQL.Parameter("Year", SqlDbType.Int, 0, ParameterDirection.Input, Year),
                new MSSQL.Parameter("Month", SqlDbType.Int, 0, ParameterDirection.Input, Month),
                new MSSQL.Parameter("Top", SqlDbType.Int, 0, ParameterDirection.Input, Top),
                new MSSQL.Parameter("ReturnValue", SqlDbType.Int, 4, ParameterDirection.Output, ReturnValue),
                new MSSQL.Parameter("ReturnDescription", SqlDbType.VarChar, 100, ParameterDirection.Output, ReturnDescription)
                );

            try
            {
                ReturnValue = System.Convert.ToInt32(Outputs["ReturnValue"]);
            }
            catch { }

            try
            {
                ReturnDescription = System.Convert.ToString(Outputs["ReturnDescription"]);
            }
            catch { }

            return CallResult;
        }

        public static int P_GetUserAccountDetail(SqlConnection conn, long SiteID, long UserID, int Year, int Month, int Day, ref int ReturnValue, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_GetUserAccountDetail(conn, ref ds, SiteID, UserID, Year, Month, Day, ref ReturnValue, ref ReturnDescription);
        }

        public static int P_GetUserAccountDetail(SqlConnection conn, ref DataSet ds, long SiteID, long UserID, int Year, int Month, int Day, ref int ReturnValue, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(conn, "P_GetUserAccountDetail", ref ds, ref Outputs,
                new MSSQL.Parameter("SiteID", SqlDbType.BigInt, 0, ParameterDirection.Input, SiteID),
                new MSSQL.Parameter("UserID", SqlDbType.BigInt, 0, ParameterDirection.Input, UserID),
                new MSSQL.Parameter("Year", SqlDbType.Int, 0, ParameterDirection.Input, Year),
                new MSSQL.Parameter("Month", SqlDbType.Int, 0, ParameterDirection.Input, Month),
                new MSSQL.Parameter("Day", SqlDbType.Int, 0, ParameterDirection.Input, Day),
                new MSSQL.Parameter("ReturnValue", SqlDbType.Int, 4, ParameterDirection.Output, ReturnValue),
                new MSSQL.Parameter("ReturnDescription", SqlDbType.VarChar, 100, ParameterDirection.Output, ReturnDescription)
                );

            try
            {
                ReturnValue = System.Convert.ToInt32(Outputs["ReturnValue"]);
            }
            catch { }

            try
            {
                ReturnDescription = System.Convert.ToString(Outputs["ReturnDescription"]);
            }
            catch { }

            return CallResult;
        }

        public static int P_GetUserAccountDetails(SqlConnection conn, long SiteID, long UserID, DateTime StartTime, DateTime EndTime, ref int ReturnValue, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_GetUserAccountDetails(conn, ref ds, SiteID, UserID, StartTime, EndTime, ref ReturnValue, ref ReturnDescription);
        }

        public static int P_GetUserAccountDetails(SqlConnection conn, ref DataSet ds, long SiteID, long UserID, DateTime StartTime, DateTime EndTime, ref int ReturnValue, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(conn, "P_GetUserAccountDetails", ref ds, ref Outputs,
                new MSSQL.Parameter("SiteID", SqlDbType.BigInt, 0, ParameterDirection.Input, SiteID),
                new MSSQL.Parameter("UserID", SqlDbType.BigInt, 0, ParameterDirection.Input, UserID),
                new MSSQL.Parameter("StartTime", SqlDbType.DateTime, 0, ParameterDirection.Input, StartTime),
                new MSSQL.Parameter("EndTime", SqlDbType.DateTime, 0, ParameterDirection.Input, EndTime),
                new MSSQL.Parameter("ReturnValue", SqlDbType.Int, 4, ParameterDirection.Output, ReturnValue),
                new MSSQL.Parameter("ReturnDescription", SqlDbType.VarChar, 100, ParameterDirection.Output, ReturnDescription)
                );

            try
            {
                ReturnValue = System.Convert.ToInt32(Outputs["ReturnValue"]);
            }
            catch { }

            try
            {
                ReturnDescription = System.Convert.ToString(Outputs["ReturnDescription"]);
            }
            catch { }

            return CallResult;
        }

        public static int P_GetUserBankDetail(SqlConnection conn, long SiteID, long UserID, ref string BankTypeName, ref string BankName, ref string BankCardNumber, ref string BankInProvinceName, ref string BankInCityName, ref string BankUserName, ref int ReturnValue, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_GetUserBankDetail(conn, ref ds, SiteID, UserID, ref BankTypeName, ref BankName, ref BankCardNumber, ref BankInProvinceName, ref BankInCityName, ref BankUserName, ref ReturnValue, ref ReturnDescription);
        }

        public static int P_GetUserBankDetail(SqlConnection conn, ref DataSet ds, long SiteID, long UserID, ref string BankTypeName, ref string BankName, ref string BankCardNumber, ref string BankInProvinceName, ref string BankInCityName, ref string BankUserName, ref int ReturnValue, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(conn, "P_GetUserBankDetail", ref ds, ref Outputs,
                new MSSQL.Parameter("SiteID", SqlDbType.BigInt, 0, ParameterDirection.Input, SiteID),
                new MSSQL.Parameter("UserID", SqlDbType.BigInt, 0, ParameterDirection.Input, UserID),
                new MSSQL.Parameter("BankTypeName", SqlDbType.VarChar, 50, ParameterDirection.Output, BankTypeName),
                new MSSQL.Parameter("BankName", SqlDbType.VarChar, 50, ParameterDirection.Output, BankName),
                new MSSQL.Parameter("BankCardNumber", SqlDbType.VarChar, 50, ParameterDirection.Output, BankCardNumber),
                new MSSQL.Parameter("BankInProvinceName", SqlDbType.VarChar, 50, ParameterDirection.Output, BankInProvinceName),
                new MSSQL.Parameter("BankInCityName", SqlDbType.VarChar, 50, ParameterDirection.Output, BankInCityName),
                new MSSQL.Parameter("BankUserName", SqlDbType.VarChar, 20, ParameterDirection.Output, BankUserName),
                new MSSQL.Parameter("ReturnValue", SqlDbType.Int, 4, ParameterDirection.Output, ReturnValue),
                new MSSQL.Parameter("ReturnDescription", SqlDbType.VarChar, 100, ParameterDirection.Output, ReturnDescription)
                );

            try
            {
                BankTypeName = System.Convert.ToString(Outputs["BankTypeName"]);
            }
            catch { }

            try
            {
                BankName = System.Convert.ToString(Outputs["BankName"]);
            }
            catch { }

            try
            {
                BankCardNumber = System.Convert.ToString(Outputs["BankCardNumber"]);
            }
            catch { }

            try
            {
                BankInProvinceName = System.Convert.ToString(Outputs["BankInProvinceName"]);
            }
            catch { }

            try
            {
                BankInCityName = System.Convert.ToString(Outputs["BankInCityName"]);
            }
            catch { }

            try
            {
                BankUserName = System.Convert.ToString(Outputs["BankUserName"]);
            }
            catch { }

            try
            {
                ReturnValue = System.Convert.ToInt32(Outputs["ReturnValue"]);
            }
            catch { }

            try
            {
                ReturnDescription = System.Convert.ToString(Outputs["ReturnDescription"]);
            }
            catch { }

            return CallResult;
        }

        public static int P_GetUserCustomFollowSchemes(SqlConnection conn, long SiteID, long UserID, int PlayTypeID, ref int ReturnValue, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_GetUserCustomFollowSchemes(conn, ref ds, SiteID, UserID, PlayTypeID, ref ReturnValue, ref ReturnDescription);
        }

        public static int P_GetUserCustomFollowSchemes(SqlConnection conn, ref DataSet ds, long SiteID, long UserID, int PlayTypeID, ref int ReturnValue, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(conn, "P_GetUserCustomFollowSchemes", ref ds, ref Outputs,
                new MSSQL.Parameter("SiteID", SqlDbType.BigInt, 0, ParameterDirection.Input, SiteID),
                new MSSQL.Parameter("UserID", SqlDbType.BigInt, 0, ParameterDirection.Input, UserID),
                new MSSQL.Parameter("PlayTypeID", SqlDbType.Int, 0, ParameterDirection.Input, PlayTypeID),
                new MSSQL.Parameter("ReturnValue", SqlDbType.Int, 4, ParameterDirection.Output, ReturnValue),
                new MSSQL.Parameter("ReturnDescription", SqlDbType.VarChar, 100, ParameterDirection.Output, ReturnDescription)
                );

            try
            {
                ReturnValue = System.Convert.ToInt32(Outputs["ReturnValue"]);
            }
            catch { }

            try
            {
                ReturnDescription = System.Convert.ToString(Outputs["ReturnDescription"]);
            }
            catch { }

            return CallResult;
        }

        public static int P_GetUserFreezeDetail(SqlConnection conn, long SiteID, long UserID, ref int ReturnValue, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_GetUserFreezeDetail(conn, ref ds, SiteID, UserID, ref ReturnValue, ref ReturnDescription);
        }

        public static int P_GetUserFreezeDetail(SqlConnection conn, ref DataSet ds, long SiteID, long UserID, ref int ReturnValue, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(conn, "P_GetUserFreezeDetail", ref ds, ref Outputs,
                new MSSQL.Parameter("SiteID", SqlDbType.BigInt, 0, ParameterDirection.Input, SiteID),
                new MSSQL.Parameter("UserID", SqlDbType.BigInt, 0, ParameterDirection.Input, UserID),
                new MSSQL.Parameter("ReturnValue", SqlDbType.Int, 4, ParameterDirection.Output, ReturnValue),
                new MSSQL.Parameter("ReturnDescription", SqlDbType.VarChar, 100, ParameterDirection.Output, ReturnDescription)
                );

            try
            {
                ReturnValue = System.Convert.ToInt32(Outputs["ReturnValue"]);
            }
            catch { }

            try
            {
                ReturnDescription = System.Convert.ToString(Outputs["ReturnDescription"]);
            }
            catch { }

            return CallResult;
        }

        public static int P_GetUserInformationByID(SqlConnection conn, long UserID, long SiteID, ref string Name, ref string NickName, ref string RealityName, ref string Password, ref string PasswordAdv, ref int CityID, ref string Sex, ref DateTime BirthDay, ref string IDCardNumber, ref string Address, ref string Email, ref bool isEmailValided, ref string QQ, ref bool isQQValided, ref string Telephone, ref string Mobile, ref bool isMobileValided, ref bool isPrivacy, ref bool isCanLogin, ref bool isAllowWinScore, ref DateTime RegisterTime, ref DateTime LastLoginTime, ref string LastLoginIP, ref int LoginCount, ref short UserType, ref short BankType, ref string BankName, ref string BankCardNumber, ref double Balance, ref double Freeze, ref double ScoringOfSelfBuy, ref double ScoringOfCommendBuy, ref double Scoring, ref short Level, ref long CommenderID, ref long CpsID, ref string OwnerSites, ref string AlipayID, ref double Bonus, ref double Reward, ref string AlipayName, ref bool isAlipayNameValided, ref int ComeFrom, ref bool IsCrossLogin, ref string Memo, ref double BonusThisMonth, ref double BonusAllow, ref double BonusUse, ref double PromotionMemberBonusScale, ref double PromotionSiteBonusScale, ref string VisitSource, ref string HeadUrl, ref string SecurityQuestion, ref string SecurityAnswer, ref string FriendList, ref int ReturnValue, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_GetUserInformationByID(conn, ref ds, UserID, SiteID, ref Name, ref NickName, ref RealityName, ref Password, ref PasswordAdv, ref CityID, ref Sex, ref BirthDay, ref IDCardNumber, ref Address, ref Email, ref isEmailValided, ref QQ, ref isQQValided, ref Telephone, ref Mobile, ref isMobileValided, ref isPrivacy, ref isCanLogin, ref isAllowWinScore, ref RegisterTime, ref LastLoginTime, ref LastLoginIP, ref LoginCount, ref UserType, ref BankType, ref BankName, ref BankCardNumber, ref Balance, ref Freeze, ref ScoringOfSelfBuy, ref ScoringOfCommendBuy, ref Scoring, ref Level, ref CommenderID, ref CpsID, ref OwnerSites, ref AlipayID, ref Bonus, ref Reward, ref AlipayName, ref isAlipayNameValided, ref ComeFrom, ref IsCrossLogin, ref Memo, ref BonusThisMonth, ref BonusAllow, ref BonusUse, ref PromotionMemberBonusScale, ref PromotionSiteBonusScale, ref VisitSource, ref HeadUrl, ref SecurityQuestion, ref SecurityAnswer, ref FriendList, ref ReturnValue, ref ReturnDescription);
        }

        public static int P_GetUserInformationByID(SqlConnection conn, ref DataSet ds, long UserID, long SiteID, ref string Name, ref string NickName, ref string RealityName, ref string Password, ref string PasswordAdv, ref int CityID, ref string Sex, ref DateTime BirthDay, ref string IDCardNumber, ref string Address, ref string Email, ref bool isEmailValided, ref string QQ, ref bool isQQValided, ref string Telephone, ref string Mobile, ref bool isMobileValided, ref bool isPrivacy, ref bool isCanLogin, ref bool isAllowWinScore, ref DateTime RegisterTime, ref DateTime LastLoginTime, ref string LastLoginIP, ref int LoginCount, ref short UserType, ref short BankType, ref string BankName, ref string BankCardNumber, ref double Balance, ref double Freeze, ref double ScoringOfSelfBuy, ref double ScoringOfCommendBuy, ref double Scoring, ref short Level, ref long CommenderID, ref long CpsID, ref string OwnerSites, ref string AlipayID, ref double Bonus, ref double Reward, ref string AlipayName, ref bool isAlipayNameValided, ref int ComeFrom, ref bool IsCrossLogin, ref string Memo, ref double BonusThisMonth, ref double BonusAllow, ref double BonusUse, ref double PromotionMemberBonusScale, ref double PromotionSiteBonusScale, ref string VisitSource, ref string HeadUrl, ref string SecurityQuestion, ref string SecurityAnswer, ref string FriendList, ref int ReturnValue, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(conn, "P_GetUserInformationByID", ref ds, ref Outputs,
                new MSSQL.Parameter("UserID", SqlDbType.BigInt, 0, ParameterDirection.Input, UserID),
                new MSSQL.Parameter("SiteID", SqlDbType.BigInt, 0, ParameterDirection.Input, SiteID),
                new MSSQL.Parameter("Name", SqlDbType.VarChar, 50, ParameterDirection.Output, Name),
                new MSSQL.Parameter("NickName", SqlDbType.VarChar, 50, ParameterDirection.Output, NickName),
                new MSSQL.Parameter("RealityName", SqlDbType.VarChar, 50, ParameterDirection.Output, RealityName),
                new MSSQL.Parameter("Password", SqlDbType.VarChar, 32, ParameterDirection.Output, Password),
                new MSSQL.Parameter("PasswordAdv", SqlDbType.VarChar, 32, ParameterDirection.Output, PasswordAdv),
                new MSSQL.Parameter("CityID", SqlDbType.Int, 4, ParameterDirection.Output, CityID),
                new MSSQL.Parameter("Sex", SqlDbType.VarChar, 2, ParameterDirection.Output, Sex),
                new MSSQL.Parameter("BirthDay", SqlDbType.DateTime, 8, ParameterDirection.Output, BirthDay),
                new MSSQL.Parameter("IDCardNumber", SqlDbType.VarChar, 50, ParameterDirection.Output, IDCardNumber),
                new MSSQL.Parameter("Address", SqlDbType.VarChar, 50, ParameterDirection.Output, Address),
                new MSSQL.Parameter("Email", SqlDbType.VarChar, 50, ParameterDirection.Output, Email),
                new MSSQL.Parameter("isEmailValided", SqlDbType.Bit, 1, ParameterDirection.Output, isEmailValided),
                new MSSQL.Parameter("QQ", SqlDbType.VarChar, 50, ParameterDirection.Output, QQ),
                new MSSQL.Parameter("isQQValided", SqlDbType.Bit, 1, ParameterDirection.Output, isQQValided),
                new MSSQL.Parameter("Telephone", SqlDbType.VarChar, 50, ParameterDirection.Output, Telephone),
                new MSSQL.Parameter("Mobile", SqlDbType.VarChar, 50, ParameterDirection.Output, Mobile),
                new MSSQL.Parameter("isMobileValided", SqlDbType.Bit, 1, ParameterDirection.Output, isMobileValided),
                new MSSQL.Parameter("isPrivacy", SqlDbType.Bit, 1, ParameterDirection.Output, isPrivacy),
                new MSSQL.Parameter("isCanLogin", SqlDbType.Bit, 1, ParameterDirection.Output, isCanLogin),
                new MSSQL.Parameter("isAllowWinScore", SqlDbType.Bit, 1, ParameterDirection.Output, isAllowWinScore),
                new MSSQL.Parameter("RegisterTime", SqlDbType.DateTime, 8, ParameterDirection.Output, RegisterTime),
                new MSSQL.Parameter("LastLoginTime", SqlDbType.DateTime, 8, ParameterDirection.Output, LastLoginTime),
                new MSSQL.Parameter("LastLoginIP", SqlDbType.VarChar, 50, ParameterDirection.Output, LastLoginIP),
                new MSSQL.Parameter("LoginCount", SqlDbType.Int, 4, ParameterDirection.Output, LoginCount),
                new MSSQL.Parameter("UserType", SqlDbType.SmallInt, 2, ParameterDirection.Output, UserType),
                new MSSQL.Parameter("BankType", SqlDbType.SmallInt, 2, ParameterDirection.Output, BankType),
                new MSSQL.Parameter("BankName", SqlDbType.VarChar, 50, ParameterDirection.Output, BankName),
                new MSSQL.Parameter("BankCardNumber", SqlDbType.VarChar, 50, ParameterDirection.Output, BankCardNumber),
                new MSSQL.Parameter("Balance", SqlDbType.Money, 8, ParameterDirection.Output, Balance),
                new MSSQL.Parameter("Freeze", SqlDbType.Money, 8, ParameterDirection.Output, Freeze),
                new MSSQL.Parameter("ScoringOfSelfBuy", SqlDbType.Float, 8, ParameterDirection.Output, ScoringOfSelfBuy),
                new MSSQL.Parameter("ScoringOfCommendBuy", SqlDbType.Float, 8, ParameterDirection.Output, ScoringOfCommendBuy),
                new MSSQL.Parameter("Scoring", SqlDbType.Float, 8, ParameterDirection.Output, Scoring),
                new MSSQL.Parameter("Level", SqlDbType.SmallInt, 2, ParameterDirection.Output, Level),
                new MSSQL.Parameter("CommenderID", SqlDbType.BigInt, 8, ParameterDirection.Output, CommenderID),
                new MSSQL.Parameter("CpsID", SqlDbType.BigInt, 8, ParameterDirection.Output, CpsID),
                new MSSQL.Parameter("OwnerSites", SqlDbType.VarChar, 1000, ParameterDirection.Output, OwnerSites),
                new MSSQL.Parameter("AlipayID", SqlDbType.VarChar, 32, ParameterDirection.Output, AlipayID),
                new MSSQL.Parameter("Bonus", SqlDbType.Money, 8, ParameterDirection.Output, Bonus),
                new MSSQL.Parameter("Reward", SqlDbType.Money, 8, ParameterDirection.Output, Reward),
                new MSSQL.Parameter("AlipayName", SqlDbType.VarChar, 50, ParameterDirection.Output, AlipayName),
                new MSSQL.Parameter("isAlipayNameValided", SqlDbType.Bit, 1, ParameterDirection.Output, isAlipayNameValided),
                new MSSQL.Parameter("ComeFrom", SqlDbType.Int, 4, ParameterDirection.Output, ComeFrom),
                new MSSQL.Parameter("IsCrossLogin", SqlDbType.Bit, 1, ParameterDirection.Output, IsCrossLogin),
                new MSSQL.Parameter("Memo", SqlDbType.VarChar, 50, ParameterDirection.Output, Memo),
                new MSSQL.Parameter("BonusThisMonth", SqlDbType.Money, 8, ParameterDirection.Output, BonusThisMonth),
                new MSSQL.Parameter("BonusAllow", SqlDbType.Money, 8, ParameterDirection.Output, BonusAllow),
                new MSSQL.Parameter("BonusUse", SqlDbType.Money, 8, ParameterDirection.Output, BonusUse),
                new MSSQL.Parameter("PromotionMemberBonusScale", SqlDbType.Float, 8, ParameterDirection.Output, PromotionMemberBonusScale),
                new MSSQL.Parameter("PromotionSiteBonusScale", SqlDbType.Float, 8, ParameterDirection.Output, PromotionSiteBonusScale),
                new MSSQL.Parameter("VisitSource", SqlDbType.VarChar, 255, ParameterDirection.Output, VisitSource),
                new MSSQL.Parameter("HeadUrl", SqlDbType.VarChar, 500, ParameterDirection.Output, HeadUrl),
                new MSSQL.Parameter("SecurityQuestion", SqlDbType.VarChar, 100, ParameterDirection.Output, SecurityQuestion),
                new MSSQL.Parameter("SecurityAnswer", SqlDbType.VarChar, 50, ParameterDirection.Output, SecurityAnswer),
                new MSSQL.Parameter("FriendList", SqlDbType.VarChar, 1073741823, ParameterDirection.Output, FriendList),
                new MSSQL.Parameter("ReturnValue", SqlDbType.Int, 4, ParameterDirection.Output, ReturnValue),
                new MSSQL.Parameter("ReturnDescription", SqlDbType.VarChar, 100, ParameterDirection.Output, ReturnDescription)
                );

            try
            {
                Name = System.Convert.ToString(Outputs["Name"]);
            }
            catch { }

            try
            {
                NickName = System.Convert.ToString(Outputs["NickName"]);
            }
            catch { }

            try
            {
                RealityName = System.Convert.ToString(Outputs["RealityName"]);
            }
            catch { }

            try
            {
                Password = System.Convert.ToString(Outputs["Password"]);
            }
            catch { }

            try
            {
                PasswordAdv = System.Convert.ToString(Outputs["PasswordAdv"]);
            }
            catch { }

            try
            {
                CityID = System.Convert.ToInt32(Outputs["CityID"]);
            }
            catch { }

            try
            {
                Sex = System.Convert.ToString(Outputs["Sex"]);
            }
            catch { }

            try
            {
                BirthDay = System.Convert.ToDateTime(Outputs["BirthDay"]);
            }
            catch { }

            try
            {
                IDCardNumber = System.Convert.ToString(Outputs["IDCardNumber"]);
            }
            catch { }

            try
            {
                Address = System.Convert.ToString(Outputs["Address"]);
            }
            catch { }

            try
            {
                Email = System.Convert.ToString(Outputs["Email"]);
            }
            catch { }

            try
            {
                isEmailValided = System.Convert.ToBoolean(Outputs["isEmailValided"]);
            }
            catch { }

            try
            {
                QQ = System.Convert.ToString(Outputs["QQ"]);
            }
            catch { }

            try
            {
                isQQValided = System.Convert.ToBoolean(Outputs["isQQValided"]);
            }
            catch { }

            try
            {
                Telephone = System.Convert.ToString(Outputs["Telephone"]);
            }
            catch { }

            try
            {
                Mobile = System.Convert.ToString(Outputs["Mobile"]);
            }
            catch { }

            try
            {
                isMobileValided = System.Convert.ToBoolean(Outputs["isMobileValided"]);
            }
            catch { }

            try
            {
                isPrivacy = System.Convert.ToBoolean(Outputs["isPrivacy"]);
            }
            catch { }

            try
            {
                isCanLogin = System.Convert.ToBoolean(Outputs["isCanLogin"]);
            }
            catch { }

            try
            {
                isAllowWinScore = System.Convert.ToBoolean(Outputs["isAllowWinScore"]);
            }
            catch { }

            try
            {
                RegisterTime = System.Convert.ToDateTime(Outputs["RegisterTime"]);
            }
            catch { }

            try
            {
                LastLoginTime = System.Convert.ToDateTime(Outputs["LastLoginTime"]);
            }
            catch { }

            try
            {
                LastLoginIP = System.Convert.ToString(Outputs["LastLoginIP"]);
            }
            catch { }

            try
            {
                LoginCount = System.Convert.ToInt32(Outputs["LoginCount"]);
            }
            catch { }

            try
            {
                UserType = System.Convert.ToInt16(Outputs["UserType"]);
            }
            catch { }

            try
            {
                BankType = System.Convert.ToInt16(Outputs["BankType"]);
            }
            catch { }

            try
            {
                BankName = System.Convert.ToString(Outputs["BankName"]);
            }
            catch { }

            try
            {
                BankCardNumber = System.Convert.ToString(Outputs["BankCardNumber"]);
            }
            catch { }

            try
            {
                Balance = System.Convert.ToDouble(Outputs["Balance"]);
            }
            catch { }

            try
            {
                Freeze = System.Convert.ToDouble(Outputs["Freeze"]);
            }
            catch { }

            try
            {
                ScoringOfSelfBuy = System.Convert.ToDouble(Outputs["ScoringOfSelfBuy"]);
            }
            catch { }

            try
            {
                ScoringOfCommendBuy = System.Convert.ToDouble(Outputs["ScoringOfCommendBuy"]);
            }
            catch { }

            try
            {
                Scoring = System.Convert.ToDouble(Outputs["Scoring"]);
            }
            catch { }

            try
            {
                Level = System.Convert.ToInt16(Outputs["Level"]);
            }
            catch { }

            try
            {
                CommenderID = System.Convert.ToInt64(Outputs["CommenderID"]);
            }
            catch { }

            try
            {
                CpsID = System.Convert.ToInt64(Outputs["CpsID"]);
            }
            catch { }

            try
            {
                OwnerSites = System.Convert.ToString(Outputs["OwnerSites"]);
            }
            catch { }

            try
            {
                AlipayID = System.Convert.ToString(Outputs["AlipayID"]);
            }
            catch { }

            try
            {
                Bonus = System.Convert.ToDouble(Outputs["Bonus"]);
            }
            catch { }

            try
            {
                Reward = System.Convert.ToDouble(Outputs["Reward"]);
            }
            catch { }

            try
            {
                AlipayName = System.Convert.ToString(Outputs["AlipayName"]);
            }
            catch { }

            try
            {
                isAlipayNameValided = System.Convert.ToBoolean(Outputs["isAlipayNameValided"]);
            }
            catch { }

            try
            {
                ComeFrom = System.Convert.ToInt32(Outputs["ComeFrom"]);
            }
            catch { }

            try
            {
                IsCrossLogin = System.Convert.ToBoolean(Outputs["IsCrossLogin"]);
            }
            catch { }

            try
            {
                Memo = System.Convert.ToString(Outputs["Memo"]);
            }
            catch { }

            try
            {
                BonusThisMonth = System.Convert.ToDouble(Outputs["BonusThisMonth"]);
            }
            catch { }

            try
            {
                BonusAllow = System.Convert.ToDouble(Outputs["BonusAllow"]);
            }
            catch { }

            try
            {
                BonusUse = System.Convert.ToDouble(Outputs["BonusUse"]);
            }
            catch { }

            try
            {
                PromotionMemberBonusScale = System.Convert.ToDouble(Outputs["PromotionMemberBonusScale"]);
            }
            catch { }

            try
            {
                PromotionSiteBonusScale = System.Convert.ToDouble(Outputs["PromotionSiteBonusScale"]);
            }
            catch { }

            try
            {
                VisitSource = System.Convert.ToString(Outputs["VisitSource"]);
            }
            catch { }

            try
            {
                HeadUrl = System.Convert.ToString(Outputs["HeadUrl"]);
            }
            catch { }

            try
            {
                SecurityQuestion = System.Convert.ToString(Outputs["SecurityQuestion"]);
            }
            catch { }

            try
            {
                SecurityAnswer = System.Convert.ToString(Outputs["SecurityAnswer"]);
            }
            catch { }

            try
            {
                FriendList = System.Convert.ToString(Outputs["FriendList"]);
            }
            catch { }

            try
            {
                ReturnValue = System.Convert.ToInt32(Outputs["ReturnValue"]);
            }
            catch { }

            try
            {
                ReturnDescription = System.Convert.ToString(Outputs["ReturnDescription"]);
            }
            catch { }

            return CallResult;
        }

        public static int P_GetUserInformationByName(SqlConnection conn, string Name, long SiteID, ref long UserID, ref string NickName, ref string RealityName, ref string Password, ref string PasswordAdv, ref int CityID, ref string Sex, ref DateTime BirthDay, ref string IDCardNumber, ref string Address, ref string Email, ref bool isEmailValided, ref string QQ, ref bool isQQValided, ref string Telephone, ref string Mobile, ref bool isMobileValided, ref bool isPrivacy, ref bool isCanLogin, ref bool isAllowWinScore, ref DateTime RegisterTime, ref DateTime LastLoginTime, ref string LastLoginIP, ref int LoginCount, ref short UserType, ref short BankType, ref string BankName, ref string BankCardNumber, ref double Balance, ref double Freeze, ref double ScoringOfSelfBuy, ref double ScoringOfCommendBuy, ref double Scoring, ref short Level, ref long CommenderID, ref long CpsID, ref string OwnerSites, ref string AlipayID, ref double Bonus, ref double Reward, ref string AlipayName, ref bool isAlipayNameValided, ref int ComeFrom, ref bool IsCrossLogin, ref string Memo, ref double BonusThisMonth, ref double BonusAllow, ref double BonusUse, ref double PromotionMemberBonusScale, ref double PromotionSiteBonusScale, ref string VisitSource, ref string HeadUrl, ref string SecurityQuestion, ref string SecurityAnswer, ref string FriendList, ref int ReturnValue, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_GetUserInformationByName(conn, ref ds, Name, SiteID, ref UserID, ref NickName, ref RealityName, ref Password, ref PasswordAdv, ref CityID, ref Sex, ref BirthDay, ref IDCardNumber, ref Address, ref Email, ref isEmailValided, ref QQ, ref isQQValided, ref Telephone, ref Mobile, ref isMobileValided, ref isPrivacy, ref isCanLogin, ref isAllowWinScore, ref RegisterTime, ref LastLoginTime, ref LastLoginIP, ref LoginCount, ref UserType, ref BankType, ref BankName, ref BankCardNumber, ref Balance, ref Freeze, ref ScoringOfSelfBuy, ref ScoringOfCommendBuy, ref Scoring, ref Level, ref CommenderID, ref CpsID, ref OwnerSites, ref AlipayID, ref Bonus, ref Reward, ref AlipayName, ref isAlipayNameValided, ref ComeFrom, ref IsCrossLogin, ref Memo, ref BonusThisMonth, ref BonusAllow, ref BonusUse, ref PromotionMemberBonusScale, ref PromotionSiteBonusScale, ref VisitSource, ref HeadUrl, ref SecurityQuestion, ref SecurityAnswer, ref FriendList, ref ReturnValue, ref ReturnDescription);
        }

        public static int P_GetUserInformationByName(SqlConnection conn, ref DataSet ds, string Name, long SiteID, ref long UserID, ref string NickName, ref string RealityName, ref string Password, ref string PasswordAdv, ref int CityID, ref string Sex, ref DateTime BirthDay, ref string IDCardNumber, ref string Address, ref string Email, ref bool isEmailValided, ref string QQ, ref bool isQQValided, ref string Telephone, ref string Mobile, ref bool isMobileValided, ref bool isPrivacy, ref bool isCanLogin, ref bool isAllowWinScore, ref DateTime RegisterTime, ref DateTime LastLoginTime, ref string LastLoginIP, ref int LoginCount, ref short UserType, ref short BankType, ref string BankName, ref string BankCardNumber, ref double Balance, ref double Freeze, ref double ScoringOfSelfBuy, ref double ScoringOfCommendBuy, ref double Scoring, ref short Level, ref long CommenderID, ref long CpsID, ref string OwnerSites, ref string AlipayID, ref double Bonus, ref double Reward, ref string AlipayName, ref bool isAlipayNameValided, ref int ComeFrom, ref bool IsCrossLogin, ref string Memo, ref double BonusThisMonth, ref double BonusAllow, ref double BonusUse, ref double PromotionMemberBonusScale, ref double PromotionSiteBonusScale, ref string VisitSource, ref string HeadUrl, ref string SecurityQuestion, ref string SecurityAnswer, ref string FriendList, ref int ReturnValue, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(conn, "P_GetUserInformationByName", ref ds, ref Outputs,
                new MSSQL.Parameter("Name", SqlDbType.VarChar, 0, ParameterDirection.Input, Name),
                new MSSQL.Parameter("SiteID", SqlDbType.BigInt, 0, ParameterDirection.Input, SiteID),
                new MSSQL.Parameter("UserID", SqlDbType.BigInt, 8, ParameterDirection.Output, UserID),
                new MSSQL.Parameter("NickName", SqlDbType.VarChar, 50, ParameterDirection.Output, NickName),
                new MSSQL.Parameter("RealityName", SqlDbType.VarChar, 50, ParameterDirection.Output, RealityName),
                new MSSQL.Parameter("Password", SqlDbType.VarChar, 32, ParameterDirection.Output, Password),
                new MSSQL.Parameter("PasswordAdv", SqlDbType.VarChar, 32, ParameterDirection.Output, PasswordAdv),
                new MSSQL.Parameter("CityID", SqlDbType.Int, 4, ParameterDirection.Output, CityID),
                new MSSQL.Parameter("Sex", SqlDbType.VarChar, 2, ParameterDirection.Output, Sex),
                new MSSQL.Parameter("BirthDay", SqlDbType.DateTime, 8, ParameterDirection.Output, BirthDay),
                new MSSQL.Parameter("IDCardNumber", SqlDbType.VarChar, 50, ParameterDirection.Output, IDCardNumber),
                new MSSQL.Parameter("Address", SqlDbType.VarChar, 50, ParameterDirection.Output, Address),
                new MSSQL.Parameter("Email", SqlDbType.VarChar, 50, ParameterDirection.Output, Email),
                new MSSQL.Parameter("isEmailValided", SqlDbType.Bit, 1, ParameterDirection.Output, isEmailValided),
                new MSSQL.Parameter("QQ", SqlDbType.VarChar, 50, ParameterDirection.Output, QQ),
                new MSSQL.Parameter("isQQValided", SqlDbType.Bit, 1, ParameterDirection.Output, isQQValided),
                new MSSQL.Parameter("Telephone", SqlDbType.VarChar, 50, ParameterDirection.Output, Telephone),
                new MSSQL.Parameter("Mobile", SqlDbType.VarChar, 50, ParameterDirection.Output, Mobile),
                new MSSQL.Parameter("isMobileValided", SqlDbType.Bit, 1, ParameterDirection.Output, isMobileValided),
                new MSSQL.Parameter("isPrivacy", SqlDbType.Bit, 1, ParameterDirection.Output, isPrivacy),
                new MSSQL.Parameter("isCanLogin", SqlDbType.Bit, 1, ParameterDirection.Output, isCanLogin),
                new MSSQL.Parameter("isAllowWinScore", SqlDbType.Bit, 1, ParameterDirection.Output, isAllowWinScore),
                new MSSQL.Parameter("RegisterTime", SqlDbType.DateTime, 8, ParameterDirection.Output, RegisterTime),
                new MSSQL.Parameter("LastLoginTime", SqlDbType.DateTime, 8, ParameterDirection.Output, LastLoginTime),
                new MSSQL.Parameter("LastLoginIP", SqlDbType.VarChar, 50, ParameterDirection.Output, LastLoginIP),
                new MSSQL.Parameter("LoginCount", SqlDbType.Int, 4, ParameterDirection.Output, LoginCount),
                new MSSQL.Parameter("UserType", SqlDbType.SmallInt, 2, ParameterDirection.Output, UserType),
                new MSSQL.Parameter("BankType", SqlDbType.SmallInt, 2, ParameterDirection.Output, BankType),
                new MSSQL.Parameter("BankName", SqlDbType.VarChar, 50, ParameterDirection.Output, BankName),
                new MSSQL.Parameter("BankCardNumber", SqlDbType.VarChar, 50, ParameterDirection.Output, BankCardNumber),
                new MSSQL.Parameter("Balance", SqlDbType.Money, 8, ParameterDirection.Output, Balance),
                new MSSQL.Parameter("Freeze", SqlDbType.Money, 8, ParameterDirection.Output, Freeze),
                new MSSQL.Parameter("ScoringOfSelfBuy", SqlDbType.Float, 8, ParameterDirection.Output, ScoringOfSelfBuy),
                new MSSQL.Parameter("ScoringOfCommendBuy", SqlDbType.Float, 8, ParameterDirection.Output, ScoringOfCommendBuy),
                new MSSQL.Parameter("Scoring", SqlDbType.Float, 8, ParameterDirection.Output, Scoring),
                new MSSQL.Parameter("Level", SqlDbType.SmallInt, 2, ParameterDirection.Output, Level),
                new MSSQL.Parameter("CommenderID", SqlDbType.BigInt, 8, ParameterDirection.Output, CommenderID),
                new MSSQL.Parameter("CpsID", SqlDbType.BigInt, 8, ParameterDirection.Output, CpsID),
                new MSSQL.Parameter("OwnerSites", SqlDbType.VarChar, 1000, ParameterDirection.Output, OwnerSites),
                new MSSQL.Parameter("AlipayID", SqlDbType.VarChar, 32, ParameterDirection.Output, AlipayID),
                new MSSQL.Parameter("Bonus", SqlDbType.Money, 8, ParameterDirection.Output, Bonus),
                new MSSQL.Parameter("Reward", SqlDbType.Money, 8, ParameterDirection.Output, Reward),
                new MSSQL.Parameter("AlipayName", SqlDbType.VarChar, 50, ParameterDirection.Output, AlipayName),
                new MSSQL.Parameter("isAlipayNameValided", SqlDbType.Bit, 1, ParameterDirection.Output, isAlipayNameValided),
                new MSSQL.Parameter("ComeFrom", SqlDbType.Int, 4, ParameterDirection.Output, ComeFrom),
                new MSSQL.Parameter("IsCrossLogin", SqlDbType.Bit, 1, ParameterDirection.Output, IsCrossLogin),
                new MSSQL.Parameter("Memo", SqlDbType.VarChar, 50, ParameterDirection.Output, Memo),
                new MSSQL.Parameter("BonusThisMonth", SqlDbType.Money, 8, ParameterDirection.Output, BonusThisMonth),
                new MSSQL.Parameter("BonusAllow", SqlDbType.Money, 8, ParameterDirection.Output, BonusAllow),
                new MSSQL.Parameter("BonusUse", SqlDbType.Money, 8, ParameterDirection.Output, BonusUse),
                new MSSQL.Parameter("PromotionMemberBonusScale", SqlDbType.Float, 8, ParameterDirection.Output, PromotionMemberBonusScale),
                new MSSQL.Parameter("PromotionSiteBonusScale", SqlDbType.Float, 8, ParameterDirection.Output, PromotionSiteBonusScale),
                new MSSQL.Parameter("VisitSource", SqlDbType.VarChar, 255, ParameterDirection.Output, VisitSource),
                new MSSQL.Parameter("HeadUrl", SqlDbType.VarChar, 500, ParameterDirection.Output, HeadUrl),
                new MSSQL.Parameter("SecurityQuestion", SqlDbType.VarChar, 100, ParameterDirection.Output, SecurityQuestion),
                new MSSQL.Parameter("SecurityAnswer", SqlDbType.VarChar, 50, ParameterDirection.Output, SecurityAnswer),
                new MSSQL.Parameter("FriendList", SqlDbType.VarChar, 1073741823, ParameterDirection.Output, FriendList),
                new MSSQL.Parameter("ReturnValue", SqlDbType.Int, 4, ParameterDirection.Output, ReturnValue),
                new MSSQL.Parameter("ReturnDescription", SqlDbType.VarChar, 100, ParameterDirection.Output, ReturnDescription)
                );

            try
            {
                UserID = System.Convert.ToInt64(Outputs["UserID"]);
            }
            catch { }

            try
            {
                NickName = System.Convert.ToString(Outputs["NickName"]);
            }
            catch { }

            try
            {
                RealityName = System.Convert.ToString(Outputs["RealityName"]);
            }
            catch { }

            try
            {
                Password = System.Convert.ToString(Outputs["Password"]);
            }
            catch { }

            try
            {
                PasswordAdv = System.Convert.ToString(Outputs["PasswordAdv"]);
            }
            catch { }

            try
            {
                CityID = System.Convert.ToInt32(Outputs["CityID"]);
            }
            catch { }

            try
            {
                Sex = System.Convert.ToString(Outputs["Sex"]);
            }
            catch { }

            try
            {
                BirthDay = System.Convert.ToDateTime(Outputs["BirthDay"]);
            }
            catch { }

            try
            {
                IDCardNumber = System.Convert.ToString(Outputs["IDCardNumber"]);
            }
            catch { }

            try
            {
                Address = System.Convert.ToString(Outputs["Address"]);
            }
            catch { }

            try
            {
                Email = System.Convert.ToString(Outputs["Email"]);
            }
            catch { }

            try
            {
                isEmailValided = System.Convert.ToBoolean(Outputs["isEmailValided"]);
            }
            catch { }

            try
            {
                QQ = System.Convert.ToString(Outputs["QQ"]);
            }
            catch { }

            try
            {
                isQQValided = System.Convert.ToBoolean(Outputs["isQQValided"]);
            }
            catch { }

            try
            {
                Telephone = System.Convert.ToString(Outputs["Telephone"]);
            }
            catch { }

            try
            {
                Mobile = System.Convert.ToString(Outputs["Mobile"]);
            }
            catch { }

            try
            {
                isMobileValided = System.Convert.ToBoolean(Outputs["isMobileValided"]);
            }
            catch { }

            try
            {
                isPrivacy = System.Convert.ToBoolean(Outputs["isPrivacy"]);
            }
            catch { }

            try
            {
                isCanLogin = System.Convert.ToBoolean(Outputs["isCanLogin"]);
            }
            catch { }

            try
            {
                isAllowWinScore = System.Convert.ToBoolean(Outputs["isAllowWinScore"]);
            }
            catch { }

            try
            {
                RegisterTime = System.Convert.ToDateTime(Outputs["RegisterTime"]);
            }
            catch { }

            try
            {
                LastLoginTime = System.Convert.ToDateTime(Outputs["LastLoginTime"]);
            }
            catch { }

            try
            {
                LastLoginIP = System.Convert.ToString(Outputs["LastLoginIP"]);
            }
            catch { }

            try
            {
                LoginCount = System.Convert.ToInt32(Outputs["LoginCount"]);
            }
            catch { }

            try
            {
                UserType = System.Convert.ToInt16(Outputs["UserType"]);
            }
            catch { }

            try
            {
                BankType = System.Convert.ToInt16(Outputs["BankType"]);
            }
            catch { }

            try
            {
                BankName = System.Convert.ToString(Outputs["BankName"]);
            }
            catch { }

            try
            {
                BankCardNumber = System.Convert.ToString(Outputs["BankCardNumber"]);
            }
            catch { }

            try
            {
                Balance = System.Convert.ToDouble(Outputs["Balance"]);
            }
            catch { }

            try
            {
                Freeze = System.Convert.ToDouble(Outputs["Freeze"]);
            }
            catch { }

            try
            {
                ScoringOfSelfBuy = System.Convert.ToDouble(Outputs["ScoringOfSelfBuy"]);
            }
            catch { }

            try
            {
                ScoringOfCommendBuy = System.Convert.ToDouble(Outputs["ScoringOfCommendBuy"]);
            }
            catch { }

            try
            {
                Scoring = System.Convert.ToDouble(Outputs["Scoring"]);
            }
            catch { }

            try
            {
                Level = System.Convert.ToInt16(Outputs["Level"]);
            }
            catch { }

            try
            {
                CommenderID = System.Convert.ToInt64(Outputs["CommenderID"]);
            }
            catch { }

            try
            {
                CpsID = System.Convert.ToInt64(Outputs["CpsID"]);
            }
            catch { }

            try
            {
                OwnerSites = System.Convert.ToString(Outputs["OwnerSites"]);
            }
            catch { }

            try
            {
                AlipayID = System.Convert.ToString(Outputs["AlipayID"]);
            }
            catch { }

            try
            {
                Bonus = System.Convert.ToDouble(Outputs["Bonus"]);
            }
            catch { }

            try
            {
                Reward = System.Convert.ToDouble(Outputs["Reward"]);
            }
            catch { }

            try
            {
                AlipayName = System.Convert.ToString(Outputs["AlipayName"]);
            }
            catch { }

            try
            {
                isAlipayNameValided = System.Convert.ToBoolean(Outputs["isAlipayNameValided"]);
            }
            catch { }

            try
            {
                ComeFrom = System.Convert.ToInt32(Outputs["ComeFrom"]);
            }
            catch { }

            try
            {
                IsCrossLogin = System.Convert.ToBoolean(Outputs["IsCrossLogin"]);
            }
            catch { }

            try
            {
                Memo = System.Convert.ToString(Outputs["Memo"]);
            }
            catch { }

            try
            {
                BonusThisMonth = System.Convert.ToDouble(Outputs["BonusThisMonth"]);
            }
            catch { }

            try
            {
                BonusAllow = System.Convert.ToDouble(Outputs["BonusAllow"]);
            }
            catch { }

            try
            {
                BonusUse = System.Convert.ToDouble(Outputs["BonusUse"]);
            }
            catch { }

            try
            {
                PromotionMemberBonusScale = System.Convert.ToDouble(Outputs["PromotionMemberBonusScale"]);
            }
            catch { }

            try
            {
                PromotionSiteBonusScale = System.Convert.ToDouble(Outputs["PromotionSiteBonusScale"]);
            }
            catch { }

            try
            {
                VisitSource = System.Convert.ToString(Outputs["VisitSource"]);
            }
            catch { }

            try
            {
                HeadUrl = System.Convert.ToString(Outputs["HeadUrl"]);
            }
            catch { }

            try
            {
                SecurityQuestion = System.Convert.ToString(Outputs["SecurityQuestion"]);
            }
            catch { }

            try
            {
                SecurityAnswer = System.Convert.ToString(Outputs["SecurityAnswer"]);
            }
            catch { }

            try
            {
                FriendList = System.Convert.ToString(Outputs["FriendList"]);
            }
            catch { }

            try
            {
                ReturnValue = System.Convert.ToInt32(Outputs["ReturnValue"]);
            }
            catch { }

            try
            {
                ReturnDescription = System.Convert.ToString(Outputs["ReturnDescription"]);
            }
            catch { }

            return CallResult;
        }

        public static int P_GetUserIsAwardwinning(SqlConnection conn, string AlipayName, ref int ReturnValue, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_GetUserIsAwardwinning(conn, ref ds, AlipayName, ref ReturnValue, ref ReturnDescription);
        }

        public static int P_GetUserIsAwardwinning(SqlConnection conn, ref DataSet ds, string AlipayName, ref int ReturnValue, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(conn, "P_GetUserIsAwardwinning", ref ds, ref Outputs,
                new MSSQL.Parameter("AlipayName", SqlDbType.VarChar, 0, ParameterDirection.Input, AlipayName),
                new MSSQL.Parameter("ReturnValue", SqlDbType.Int, 4, ParameterDirection.Output, ReturnValue),
                new MSSQL.Parameter("ReturnDescription", SqlDbType.VarChar, 100, ParameterDirection.Output, ReturnDescription)
                );

            try
            {
                ReturnValue = System.Convert.ToInt32(Outputs["ReturnValue"]);
            }
            catch { }

            try
            {
                ReturnDescription = System.Convert.ToString(Outputs["ReturnDescription"]);
            }
            catch { }

            return CallResult;
        }

        public static int P_GetUserScoringDetail(SqlConnection conn, long SiteID, long UserID, int Year, int Month, ref int ReturnValue, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_GetUserScoringDetail(conn, ref ds, SiteID, UserID, Year, Month, ref ReturnValue, ref ReturnDescription);
        }

        public static int P_GetUserScoringDetail(SqlConnection conn, ref DataSet ds, long SiteID, long UserID, int Year, int Month, ref int ReturnValue, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(conn, "P_GetUserScoringDetail", ref ds, ref Outputs,
                new MSSQL.Parameter("SiteID", SqlDbType.BigInt, 0, ParameterDirection.Input, SiteID),
                new MSSQL.Parameter("UserID", SqlDbType.BigInt, 0, ParameterDirection.Input, UserID),
                new MSSQL.Parameter("Year", SqlDbType.Int, 0, ParameterDirection.Input, Year),
                new MSSQL.Parameter("Month", SqlDbType.Int, 0, ParameterDirection.Input, Month),
                new MSSQL.Parameter("ReturnValue", SqlDbType.Int, 4, ParameterDirection.Output, ReturnValue),
                new MSSQL.Parameter("ReturnDescription", SqlDbType.VarChar, 100, ParameterDirection.Output, ReturnDescription)
                );

            try
            {
                ReturnValue = System.Convert.ToInt32(Outputs["ReturnValue"]);
            }
            catch { }

            try
            {
                ReturnDescription = System.Convert.ToString(Outputs["ReturnDescription"]);
            }
            catch { }

            return CallResult;
        }

        public static int P_GetUserSMSDetail(SqlConnection conn, long SiteID, long UserID, int Year, int Month, ref int ReturnValue, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_GetUserSMSDetail(conn, ref ds, SiteID, UserID, Year, Month, ref ReturnValue, ref ReturnDescription);
        }

        public static int P_GetUserSMSDetail(SqlConnection conn, ref DataSet ds, long SiteID, long UserID, int Year, int Month, ref int ReturnValue, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(conn, "P_GetUserSMSDetail", ref ds, ref Outputs,
                new MSSQL.Parameter("SiteID", SqlDbType.BigInt, 0, ParameterDirection.Input, SiteID),
                new MSSQL.Parameter("UserID", SqlDbType.BigInt, 0, ParameterDirection.Input, UserID),
                new MSSQL.Parameter("Year", SqlDbType.Int, 0, ParameterDirection.Input, Year),
                new MSSQL.Parameter("Month", SqlDbType.Int, 0, ParameterDirection.Input, Month),
                new MSSQL.Parameter("ReturnValue", SqlDbType.Int, 4, ParameterDirection.Output, ReturnValue),
                new MSSQL.Parameter("ReturnDescription", SqlDbType.VarChar, 100, ParameterDirection.Output, ReturnDescription)
                );

            try
            {
                ReturnValue = System.Convert.ToInt32(Outputs["ReturnValue"]);
            }
            catch { }

            try
            {
                ReturnDescription = System.Convert.ToString(Outputs["ReturnDescription"]);
            }
            catch { }

            return CallResult;
        }

        public static int P_GetWinLotteryNumber(SqlConnection conn, long SiteID, int LotteryID, int IsuseCount, ref int ReturnValue, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_GetWinLotteryNumber(conn, ref ds, SiteID, LotteryID, IsuseCount, ref ReturnValue, ref ReturnDescription);
        }

        public static int P_GetWinLotteryNumber(SqlConnection conn, ref DataSet ds, long SiteID, int LotteryID, int IsuseCount, ref int ReturnValue, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(conn, "P_GetWinLotteryNumber", ref ds, ref Outputs,
                new MSSQL.Parameter("SiteID", SqlDbType.BigInt, 0, ParameterDirection.Input, SiteID),
                new MSSQL.Parameter("LotteryID", SqlDbType.Int, 0, ParameterDirection.Input, LotteryID),
                new MSSQL.Parameter("IsuseCount", SqlDbType.Int, 0, ParameterDirection.Input, IsuseCount),
                new MSSQL.Parameter("ReturnValue", SqlDbType.Int, 4, ParameterDirection.Output, ReturnValue),
                new MSSQL.Parameter("ReturnDescription", SqlDbType.VarChar, 100, ParameterDirection.Output, ReturnDescription)
                );

            try
            {
                ReturnValue = System.Convert.ToInt32(Outputs["ReturnValue"]);
            }
            catch { }

            try
            {
                ReturnDescription = System.Convert.ToString(Outputs["ReturnDescription"]);
            }
            catch { }

            return CallResult;
        }

        public static int P_GetZCDCSPFMessage(SqlConnection conn, string IsuseName, ref int ReturnValue, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_GetZCDCSPFMessage(conn, ref ds, IsuseName, ref ReturnValue, ref ReturnDescription);
        }

        public static int P_GetZCDCSPFMessage(SqlConnection conn, ref DataSet ds, string IsuseName, ref int ReturnValue, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(conn, "P_GetZCDCSPFMessage", ref ds, ref Outputs,
                new MSSQL.Parameter("IsuseName", SqlDbType.VarChar, 0, ParameterDirection.Input, IsuseName),
                new MSSQL.Parameter("ReturnValue", SqlDbType.Int, 4, ParameterDirection.Output, ReturnValue),
                new MSSQL.Parameter("ReturnDescription", SqlDbType.VarChar, 100, ParameterDirection.Output, ReturnDescription)
                );

            try
            {
                ReturnValue = System.Convert.ToInt32(Outputs["ReturnValue"]);
            }
            catch { }

            try
            {
                ReturnDescription = System.Convert.ToString(Outputs["ReturnDescription"]);
            }
            catch { }

            return CallResult;
        }

        public static int P_InitializationData(SqlConnection conn, string CallPassword, ref int ReturnValue, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_InitializationData(conn, ref ds, CallPassword, ref ReturnValue, ref ReturnDescription);
        }

        public static int P_InitializationData(SqlConnection conn, ref DataSet ds, string CallPassword, ref int ReturnValue, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(conn, "P_InitializationData", ref ds, ref Outputs,
                new MSSQL.Parameter("CallPassword", SqlDbType.VarChar, 0, ParameterDirection.Input, CallPassword),
                new MSSQL.Parameter("ReturnValue", SqlDbType.Int, 4, ParameterDirection.Output, ReturnValue),
                new MSSQL.Parameter("ReturnDescription", SqlDbType.VarChar, 100, ParameterDirection.Output, ReturnDescription)
                );

            try
            {
                ReturnValue = System.Convert.ToInt32(Outputs["ReturnValue"]);
            }
            catch { }

            try
            {
                ReturnDescription = System.Convert.ToString(Outputs["ReturnDescription"]);
            }
            catch { }

            return CallResult;
        }

        public static int P_InitializationSiteTemplates(SqlConnection conn, long SiteID, ref int ReturnValue, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_InitializationSiteTemplates(conn, ref ds, SiteID, ref ReturnValue, ref ReturnDescription);
        }

        public static int P_InitializationSiteTemplates(SqlConnection conn, ref DataSet ds, long SiteID, ref int ReturnValue, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(conn, "P_InitializationSiteTemplates", ref ds, ref Outputs,
                new MSSQL.Parameter("SiteID", SqlDbType.BigInt, 0, ParameterDirection.Input, SiteID),
                new MSSQL.Parameter("ReturnValue", SqlDbType.Int, 4, ParameterDirection.Output, ReturnValue),
                new MSSQL.Parameter("ReturnDescription", SqlDbType.VarChar, 100, ParameterDirection.Output, ReturnDescription)
                );

            try
            {
                ReturnValue = System.Convert.ToInt32(Outputs["ReturnValue"]);
            }
            catch { }

            try
            {
                ReturnDescription = System.Convert.ToString(Outputs["ReturnDescription"]);
            }
            catch { }

            return CallResult;
        }

        public static int P_InitiateChaseTask(SqlConnection conn, long SiteID, long UserID, string Title, string Description, int LotteryID, double StopWhenWinMoney, string DetailXML, string LotteryNumber, double SchemeBonusScale, ref long ChaseTaskID, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_InitiateChaseTask(conn, ref ds, SiteID, UserID, Title, Description, LotteryID, StopWhenWinMoney, DetailXML, LotteryNumber, SchemeBonusScale, ref ChaseTaskID, ref ReturnDescription);
        }

        public static int P_InitiateChaseTask(SqlConnection conn, ref DataSet ds, long SiteID, long UserID, string Title, string Description, int LotteryID, double StopWhenWinMoney, string DetailXML, string LotteryNumber, double SchemeBonusScale, ref long ChaseTaskID, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(conn, "P_InitiateChaseTask", ref ds, ref Outputs,
                new MSSQL.Parameter("SiteID", SqlDbType.BigInt, 0, ParameterDirection.Input, SiteID),
                new MSSQL.Parameter("UserID", SqlDbType.BigInt, 0, ParameterDirection.Input, UserID),
                new MSSQL.Parameter("Title", SqlDbType.VarChar, 0, ParameterDirection.Input, Title),
                new MSSQL.Parameter("Description", SqlDbType.VarChar, 0, ParameterDirection.Input, Description),
                new MSSQL.Parameter("LotteryID", SqlDbType.Int, 0, ParameterDirection.Input, LotteryID),
                new MSSQL.Parameter("StopWhenWinMoney", SqlDbType.Money, 0, ParameterDirection.Input, StopWhenWinMoney),
                new MSSQL.Parameter("DetailXML", SqlDbType.NText, 0, ParameterDirection.Input, DetailXML),
                new MSSQL.Parameter("LotteryNumber", SqlDbType.VarChar, 0, ParameterDirection.Input, LotteryNumber),
                new MSSQL.Parameter("SchemeBonusScale", SqlDbType.Float, 0, ParameterDirection.Input, SchemeBonusScale),
                new MSSQL.Parameter("ChaseTaskID", SqlDbType.BigInt, 8, ParameterDirection.Output, ChaseTaskID),
                new MSSQL.Parameter("ReturnDescription", SqlDbType.VarChar, 100, ParameterDirection.Output, ReturnDescription)
                );

            try
            {
                ChaseTaskID = System.Convert.ToInt64(Outputs["ChaseTaskID"]);
            }
            catch { }

            try
            {
                ReturnDescription = System.Convert.ToString(Outputs["ReturnDescription"]);
            }
            catch { }

            return CallResult;
        }

        public static int P_InitiateScheme(SqlConnection conn, long SiteID, long UserID, long IsuseID, int PlayTypeID, string Title, string Description, string LotteryNumber, string UploadFileContent, int Multiple, double Money, double AssureMoney, int Share, int BuyShare, string OpenUsers, short SecrecyLevel, double SchemeBonusScale, ref long SchemeID, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_InitiateScheme(conn, ref ds, SiteID, UserID, IsuseID, PlayTypeID, Title, Description, LotteryNumber, UploadFileContent, Multiple, Money, AssureMoney, Share, BuyShare, OpenUsers, SecrecyLevel, SchemeBonusScale, ref SchemeID, ref ReturnDescription);
        }

        public static int P_InitiateScheme(SqlConnection conn, ref DataSet ds, long SiteID, long UserID, long IsuseID, int PlayTypeID, string Title, string Description, string LotteryNumber, string UploadFileContent, int Multiple, double Money, double AssureMoney, int Share, int BuyShare, string OpenUsers, short SecrecyLevel, double SchemeBonusScale, ref long SchemeID, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(conn, "P_InitiateScheme", ref ds, ref Outputs,
                new MSSQL.Parameter("SiteID", SqlDbType.BigInt, 0, ParameterDirection.Input, SiteID),
                new MSSQL.Parameter("UserID", SqlDbType.BigInt, 0, ParameterDirection.Input, UserID),
                new MSSQL.Parameter("IsuseID", SqlDbType.BigInt, 0, ParameterDirection.Input, IsuseID),
                new MSSQL.Parameter("PlayTypeID", SqlDbType.Int, 0, ParameterDirection.Input, PlayTypeID),
                new MSSQL.Parameter("Title", SqlDbType.VarChar, 0, ParameterDirection.Input, Title),
                new MSSQL.Parameter("Description", SqlDbType.VarChar, 0, ParameterDirection.Input, Description),
                new MSSQL.Parameter("LotteryNumber", SqlDbType.VarChar, 0, ParameterDirection.Input, LotteryNumber),
                new MSSQL.Parameter("UploadFileContent", SqlDbType.VarChar, 0, ParameterDirection.Input, UploadFileContent),
                new MSSQL.Parameter("Multiple", SqlDbType.Int, 0, ParameterDirection.Input, Multiple),
                new MSSQL.Parameter("Money", SqlDbType.Money, 0, ParameterDirection.Input, Money),
                new MSSQL.Parameter("AssureMoney", SqlDbType.Money, 0, ParameterDirection.Input, AssureMoney),
                new MSSQL.Parameter("Share", SqlDbType.Int, 0, ParameterDirection.Input, Share),
                new MSSQL.Parameter("BuyShare", SqlDbType.Int, 0, ParameterDirection.Input, BuyShare),
                new MSSQL.Parameter("OpenUsers", SqlDbType.VarChar, 0, ParameterDirection.Input, OpenUsers),
                new MSSQL.Parameter("SecrecyLevel", SqlDbType.SmallInt, 0, ParameterDirection.Input, SecrecyLevel),
                new MSSQL.Parameter("SchemeBonusScale", SqlDbType.Float, 0, ParameterDirection.Input, SchemeBonusScale),
                new MSSQL.Parameter("SchemeID", SqlDbType.BigInt, 8, ParameterDirection.Output, SchemeID),
                new MSSQL.Parameter("ReturnDescription", SqlDbType.VarChar, 100, ParameterDirection.Output, ReturnDescription)
                );

            try
            {
                SchemeID = System.Convert.ToInt64(Outputs["SchemeID"]);
            }
            catch { }

            try
            {
                ReturnDescription = System.Convert.ToString(Outputs["ReturnDescription"]);
            }
            catch { }

            return CallResult;
        }

        public static int P_InquirySchemesHandle(SqlConnection conn, string CounterAnteId, string DealTime, short HandleResult, string HandleDescription, short PrintOutType, ref int ReturnValue, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_InquirySchemesHandle(conn, ref ds, CounterAnteId, DealTime, HandleResult, HandleDescription, PrintOutType, ref ReturnValue, ref ReturnDescription);
        }

        public static int P_InquirySchemesHandle(SqlConnection conn, ref DataSet ds, string CounterAnteId, string DealTime, short HandleResult, string HandleDescription, short PrintOutType, ref int ReturnValue, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(conn, "P_InquirySchemesHandle", ref ds, ref Outputs,
                new MSSQL.Parameter("CounterAnteId", SqlDbType.VarChar, 0, ParameterDirection.Input, CounterAnteId),
                new MSSQL.Parameter("DealTime", SqlDbType.VarChar, 0, ParameterDirection.Input, DealTime),
                new MSSQL.Parameter("HandleResult", SqlDbType.SmallInt, 0, ParameterDirection.Input, HandleResult),
                new MSSQL.Parameter("HandleDescription", SqlDbType.VarChar, 0, ParameterDirection.Input, HandleDescription),
                new MSSQL.Parameter("PrintOutType", SqlDbType.SmallInt, 0, ParameterDirection.Input, PrintOutType),
                new MSSQL.Parameter("ReturnValue", SqlDbType.Int, 4, ParameterDirection.Output, ReturnValue),
                new MSSQL.Parameter("ReturnDescription", SqlDbType.VarChar, 100, ParameterDirection.Output, ReturnDescription)
                );

            try
            {
                ReturnValue = System.Convert.ToInt32(Outputs["ReturnValue"]);
            }
            catch { }

            try
            {
                ReturnDescription = System.Convert.ToString(Outputs["ReturnDescription"]);
            }
            catch { }

            return CallResult;
        }

        public static int P_IsuseAdd(SqlConnection conn, int LotteryID, string Name, DateTime StartTime, DateTime EndTime, string AdditionalXML, ref long IsuseID, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_IsuseAdd(conn, ref ds, LotteryID, Name, StartTime, EndTime, AdditionalXML, ref IsuseID, ref ReturnDescription);
        }

        public static int P_IsuseAdd(SqlConnection conn, ref DataSet ds, int LotteryID, string Name, DateTime StartTime, DateTime EndTime, string AdditionalXML, ref long IsuseID, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(conn, "P_IsuseAdd", ref ds, ref Outputs,
                new MSSQL.Parameter("LotteryID", SqlDbType.Int, 0, ParameterDirection.Input, LotteryID),
                new MSSQL.Parameter("Name", SqlDbType.VarChar, 0, ParameterDirection.Input, Name),
                new MSSQL.Parameter("StartTime", SqlDbType.DateTime, 0, ParameterDirection.Input, StartTime),
                new MSSQL.Parameter("EndTime", SqlDbType.DateTime, 0, ParameterDirection.Input, EndTime),
                new MSSQL.Parameter("AdditionalXML", SqlDbType.NText, 0, ParameterDirection.Input, AdditionalXML),
                new MSSQL.Parameter("IsuseID", SqlDbType.BigInt, 8, ParameterDirection.Output, IsuseID),
                new MSSQL.Parameter("ReturnDescription", SqlDbType.VarChar, 100, ParameterDirection.Output, ReturnDescription)
                );

            try
            {
                IsuseID = System.Convert.ToInt64(Outputs["IsuseID"]);
            }
            catch { }

            try
            {
                ReturnDescription = System.Convert.ToString(Outputs["ReturnDescription"]);
            }
            catch { }

            return CallResult;
        }

        public static int P_IsuseBonusesAdd(SqlConnection conn, long IsuseId, long UserID, string WinListXML, ref int ReturnValue, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_IsuseBonusesAdd(conn, ref ds, IsuseId, UserID, WinListXML, ref ReturnValue, ref ReturnDescription);
        }

        public static int P_IsuseBonusesAdd(SqlConnection conn, ref DataSet ds, long IsuseId, long UserID, string WinListXML, ref int ReturnValue, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(conn, "P_IsuseBonusesAdd", ref ds, ref Outputs,
                new MSSQL.Parameter("IsuseId", SqlDbType.BigInt, 0, ParameterDirection.Input, IsuseId),
                new MSSQL.Parameter("UserID", SqlDbType.BigInt, 0, ParameterDirection.Input, UserID),
                new MSSQL.Parameter("WinListXML", SqlDbType.NText, 0, ParameterDirection.Input, WinListXML),
                new MSSQL.Parameter("ReturnValue", SqlDbType.Int, 4, ParameterDirection.Output, ReturnValue),
                new MSSQL.Parameter("ReturnDescription", SqlDbType.VarChar, 100, ParameterDirection.Output, ReturnDescription)
                );

            try
            {
                ReturnValue = System.Convert.ToInt32(Outputs["ReturnValue"]);
            }
            catch { }

            try
            {
                ReturnDescription = System.Convert.ToString(Outputs["ReturnDescription"]);
            }
            catch { }

            return CallResult;
        }

        public static int P_IsuseEdit(SqlConnection conn, long IsuseID, string Name, DateTime StartTime, DateTime EndTime, string AdditionalXML, ref int ReturnValue, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_IsuseEdit(conn, ref ds, IsuseID, Name, StartTime, EndTime, AdditionalXML, ref ReturnValue, ref ReturnDescription);
        }

        public static int P_IsuseEdit(SqlConnection conn, ref DataSet ds, long IsuseID, string Name, DateTime StartTime, DateTime EndTime, string AdditionalXML, ref int ReturnValue, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(conn, "P_IsuseEdit", ref ds, ref Outputs,
                new MSSQL.Parameter("IsuseID", SqlDbType.BigInt, 0, ParameterDirection.Input, IsuseID),
                new MSSQL.Parameter("Name", SqlDbType.VarChar, 0, ParameterDirection.Input, Name),
                new MSSQL.Parameter("StartTime", SqlDbType.DateTime, 0, ParameterDirection.Input, StartTime),
                new MSSQL.Parameter("EndTime", SqlDbType.DateTime, 0, ParameterDirection.Input, EndTime),
                new MSSQL.Parameter("AdditionalXML", SqlDbType.NText, 0, ParameterDirection.Input, AdditionalXML),
                new MSSQL.Parameter("ReturnValue", SqlDbType.Int, 4, ParameterDirection.Output, ReturnValue),
                new MSSQL.Parameter("ReturnDescription", SqlDbType.VarChar, 100, ParameterDirection.Output, ReturnDescription)
                );

            try
            {
                ReturnValue = System.Convert.ToInt32(Outputs["ReturnValue"]);
            }
            catch { }

            try
            {
                ReturnDescription = System.Convert.ToString(Outputs["ReturnDescription"]);
            }
            catch { }

            return CallResult;
        }

        public static int P_IsuseEndTime(SqlConnection conn, ref int ReturnValue, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_IsuseEndTime(conn, ref ds, ref ReturnValue, ref ReturnDescription);
        }

        public static int P_IsuseEndTime(SqlConnection conn, ref DataSet ds, ref int ReturnValue, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(conn, "P_IsuseEndTime", ref ds, ref Outputs,
                new MSSQL.Parameter("ReturnValue", SqlDbType.Int, 4, ParameterDirection.Output, ReturnValue),
                new MSSQL.Parameter("ReturnDescription", SqlDbType.VarChar, 100, ParameterDirection.Output, ReturnDescription)
                );

            try
            {
                ReturnValue = System.Convert.ToInt32(Outputs["ReturnValue"]);
            }
            catch { }

            try
            {
                ReturnDescription = System.Convert.ToString(Outputs["ReturnDescription"]);
            }
            catch { }

            return CallResult;
        }

        public static int P_IsuseInsertOneResultForZCDC(SqlConnection conn, long ID, string HalftimeResult, string Result, string LetBall, string SPFResult, double SPF_Sp, string ZJQResult, double ZJQ_Sp, string SXDSResult, double SXDS_Sp, string ZQBFResult, double ZQBF_Sp, string BQCSPFResult, double BQCSPF_Sp, ref int ReturnValue, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_IsuseInsertOneResultForZCDC(conn, ref ds, ID, HalftimeResult, Result, LetBall, SPFResult, SPF_Sp, ZJQResult, ZJQ_Sp, SXDSResult, SXDS_Sp, ZQBFResult, ZQBF_Sp, BQCSPFResult, BQCSPF_Sp, ref ReturnValue, ref ReturnDescription);
        }

        public static int P_IsuseInsertOneResultForZCDC(SqlConnection conn, ref DataSet ds, long ID, string HalftimeResult, string Result, string LetBall, string SPFResult, double SPF_Sp, string ZJQResult, double ZJQ_Sp, string SXDSResult, double SXDS_Sp, string ZQBFResult, double ZQBF_Sp, string BQCSPFResult, double BQCSPF_Sp, ref int ReturnValue, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(conn, "P_IsuseInsertOneResultForZCDC", ref ds, ref Outputs,
                new MSSQL.Parameter("ID", SqlDbType.BigInt, 0, ParameterDirection.Input, ID),
                new MSSQL.Parameter("HalftimeResult", SqlDbType.VarChar, 0, ParameterDirection.Input, HalftimeResult),
                new MSSQL.Parameter("Result", SqlDbType.VarChar, 0, ParameterDirection.Input, Result),
                new MSSQL.Parameter("LetBall", SqlDbType.VarChar, 0, ParameterDirection.Input, LetBall),
                new MSSQL.Parameter("SPFResult", SqlDbType.VarChar, 0, ParameterDirection.Input, SPFResult),
                new MSSQL.Parameter("SPF_Sp", SqlDbType.Float, 0, ParameterDirection.Input, SPF_Sp),
                new MSSQL.Parameter("ZJQResult", SqlDbType.VarChar, 0, ParameterDirection.Input, ZJQResult),
                new MSSQL.Parameter("ZJQ_Sp", SqlDbType.Float, 0, ParameterDirection.Input, ZJQ_Sp),
                new MSSQL.Parameter("SXDSResult", SqlDbType.VarChar, 0, ParameterDirection.Input, SXDSResult),
                new MSSQL.Parameter("SXDS_Sp", SqlDbType.Float, 0, ParameterDirection.Input, SXDS_Sp),
                new MSSQL.Parameter("ZQBFResult", SqlDbType.VarChar, 0, ParameterDirection.Input, ZQBFResult),
                new MSSQL.Parameter("ZQBF_Sp", SqlDbType.Float, 0, ParameterDirection.Input, ZQBF_Sp),
                new MSSQL.Parameter("BQCSPFResult", SqlDbType.VarChar, 0, ParameterDirection.Input, BQCSPFResult),
                new MSSQL.Parameter("BQCSPF_Sp", SqlDbType.Float, 0, ParameterDirection.Input, BQCSPF_Sp),
                new MSSQL.Parameter("ReturnValue", SqlDbType.Int, 4, ParameterDirection.Output, ReturnValue),
                new MSSQL.Parameter("ReturnDescription", SqlDbType.VarChar, 100, ParameterDirection.Output, ReturnDescription)
                );

            try
            {
                ReturnValue = System.Convert.ToInt32(Outputs["ReturnValue"]);
            }
            catch { }

            try
            {
                ReturnDescription = System.Convert.ToString(Outputs["ReturnDescription"]);
            }
            catch { }

            return CallResult;
        }

        public static int P_IsuseUpdate(SqlConnection conn, int LotteryID, string IsuseName, short State, DateTime StartTime, DateTime EndTime, DateTime StateUpdateTime, string WinLotteryNumber, ref int ReturnValue, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_IsuseUpdate(conn, ref ds, LotteryID, IsuseName, State, StartTime, EndTime, StateUpdateTime, WinLotteryNumber, ref ReturnValue, ref ReturnDescription);
        }

        public static int P_IsuseUpdate(SqlConnection conn, ref DataSet ds, int LotteryID, string IsuseName, short State, DateTime StartTime, DateTime EndTime, DateTime StateUpdateTime, string WinLotteryNumber, ref int ReturnValue, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(conn, "P_IsuseUpdate", ref ds, ref Outputs,
                new MSSQL.Parameter("LotteryID", SqlDbType.Int, 0, ParameterDirection.Input, LotteryID),
                new MSSQL.Parameter("IsuseName", SqlDbType.VarChar, 0, ParameterDirection.Input, IsuseName),
                new MSSQL.Parameter("State", SqlDbType.SmallInt, 0, ParameterDirection.Input, State),
                new MSSQL.Parameter("StartTime", SqlDbType.DateTime, 0, ParameterDirection.Input, StartTime),
                new MSSQL.Parameter("EndTime", SqlDbType.DateTime, 0, ParameterDirection.Input, EndTime),
                new MSSQL.Parameter("StateUpdateTime", SqlDbType.DateTime, 0, ParameterDirection.Input, StateUpdateTime),
                new MSSQL.Parameter("WinLotteryNumber", SqlDbType.VarChar, 0, ParameterDirection.Input, WinLotteryNumber),
                new MSSQL.Parameter("ReturnValue", SqlDbType.Int, 4, ParameterDirection.Output, ReturnValue),
                new MSSQL.Parameter("ReturnDescription", SqlDbType.VarChar, 100, ParameterDirection.Output, ReturnDescription)
                );

            try
            {
                ReturnValue = System.Convert.ToInt32(Outputs["ReturnValue"]);
            }
            catch { }

            try
            {
                ReturnDescription = System.Convert.ToString(Outputs["ReturnDescription"]);
            }
            catch { }

            return CallResult;
        }

        public static int P_JoinScheme(SqlConnection conn, long SiteID, long UserID, long SchemeID, int Share, bool isAutoFollowScheme, ref int ReturnValue, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_JoinScheme(conn, ref ds, SiteID, UserID, SchemeID, Share, isAutoFollowScheme, ref ReturnValue, ref ReturnDescription);
        }

        public static int P_JoinScheme(SqlConnection conn, ref DataSet ds, long SiteID, long UserID, long SchemeID, int Share, bool isAutoFollowScheme, ref int ReturnValue, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(conn, "P_JoinScheme", ref ds, ref Outputs,
                new MSSQL.Parameter("SiteID", SqlDbType.BigInt, 0, ParameterDirection.Input, SiteID),
                new MSSQL.Parameter("UserID", SqlDbType.BigInt, 0, ParameterDirection.Input, UserID),
                new MSSQL.Parameter("SchemeID", SqlDbType.BigInt, 0, ParameterDirection.Input, SchemeID),
                new MSSQL.Parameter("Share", SqlDbType.Int, 0, ParameterDirection.Input, Share),
                new MSSQL.Parameter("isAutoFollowScheme", SqlDbType.Bit, 0, ParameterDirection.Input, isAutoFollowScheme),
                new MSSQL.Parameter("ReturnValue", SqlDbType.Int, 4, ParameterDirection.Output, ReturnValue),
                new MSSQL.Parameter("ReturnDescription", SqlDbType.VarChar, 100, ParameterDirection.Output, ReturnDescription)
                );

            try
            {
                ReturnValue = System.Convert.ToInt32(Outputs["ReturnValue"]);
            }
            catch { }

            try
            {
                ReturnDescription = System.Convert.ToString(Outputs["ReturnDescription"]);
            }
            catch { }

            return CallResult;
        }

        public static int P_LeaveSchemeChatRoom(SqlConnection conn, long SiteID, long UserID, long SchemeID, ref int ReturnValue, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_LeaveSchemeChatRoom(conn, ref ds, SiteID, UserID, SchemeID, ref ReturnValue, ref ReturnDescription);
        }

        public static int P_LeaveSchemeChatRoom(SqlConnection conn, ref DataSet ds, long SiteID, long UserID, long SchemeID, ref int ReturnValue, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(conn, "P_LeaveSchemeChatRoom", ref ds, ref Outputs,
                new MSSQL.Parameter("SiteID", SqlDbType.BigInt, 0, ParameterDirection.Input, SiteID),
                new MSSQL.Parameter("UserID", SqlDbType.BigInt, 0, ParameterDirection.Input, UserID),
                new MSSQL.Parameter("SchemeID", SqlDbType.BigInt, 0, ParameterDirection.Input, SchemeID),
                new MSSQL.Parameter("ReturnValue", SqlDbType.Int, 4, ParameterDirection.Output, ReturnValue),
                new MSSQL.Parameter("ReturnDescription", SqlDbType.VarChar, 100, ParameterDirection.Output, ReturnDescription)
                );

            try
            {
                ReturnValue = System.Convert.ToInt32(Outputs["ReturnValue"]);
            }
            catch { }

            try
            {
                ReturnDescription = System.Convert.ToString(Outputs["ReturnDescription"]);
            }
            catch { }

            return CallResult;
        }

        public static int P_Login(SqlConnection conn, long SiteID, string Name, string InputPassword, string LoginIP, ref long UserID, ref string PasswordAdv, ref string RealityName, ref int CityID, ref string Sex, ref DateTime BirthDay, ref string IDCardNumber, ref string Address, ref string Email, ref bool isEmailValided, ref string QQ, ref bool isQQValided, ref string Telephone, ref string Mobile, ref bool isMobileValided, ref bool isPrivacy, ref bool isCanLogin, ref bool isAllowWinScore, ref DateTime RegisterTime, ref DateTime LastLoginTime, ref string LastLoginIP, ref int LoginCount, ref short UserType, ref short BankType, ref string BankName, ref string BankCardNumber, ref double Balance, ref double Freeze, ref double ScoringOfSelfBuy, ref double ScoringOfCommendBuy, ref double Scoring, ref short Level, ref long CommenderID, ref long CpsID, ref string AlipayID, ref string AlipayName, ref bool isAlipayNameValided, ref double Bonus, ref double Reward, ref string Memo, ref double BonusThisMonth, ref double BonusAllow, ref double BonusUse, ref double PromotionMemberBonusScale, ref double PromotionSiteBonusScale, ref int ReturnValue, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_Login(conn, ref ds, SiteID, Name, InputPassword, LoginIP, ref UserID, ref PasswordAdv, ref RealityName, ref CityID, ref Sex, ref BirthDay, ref IDCardNumber, ref Address, ref Email, ref isEmailValided, ref QQ, ref isQQValided, ref Telephone, ref Mobile, ref isMobileValided, ref isPrivacy, ref isCanLogin, ref isAllowWinScore, ref RegisterTime, ref LastLoginTime, ref LastLoginIP, ref LoginCount, ref UserType, ref BankType, ref BankName, ref BankCardNumber, ref Balance, ref Freeze, ref ScoringOfSelfBuy, ref ScoringOfCommendBuy, ref Scoring, ref Level, ref CommenderID, ref CpsID, ref AlipayID, ref AlipayName, ref isAlipayNameValided, ref Bonus, ref Reward, ref Memo, ref BonusThisMonth, ref BonusAllow, ref BonusUse, ref PromotionMemberBonusScale, ref PromotionSiteBonusScale, ref ReturnValue, ref ReturnDescription);
        }

        public static int P_Login(SqlConnection conn, ref DataSet ds, long SiteID, string Name, string InputPassword, string LoginIP, ref long UserID, ref string PasswordAdv, ref string RealityName, ref int CityID, ref string Sex, ref DateTime BirthDay, ref string IDCardNumber, ref string Address, ref string Email, ref bool isEmailValided, ref string QQ, ref bool isQQValided, ref string Telephone, ref string Mobile, ref bool isMobileValided, ref bool isPrivacy, ref bool isCanLogin, ref bool isAllowWinScore, ref DateTime RegisterTime, ref DateTime LastLoginTime, ref string LastLoginIP, ref int LoginCount, ref short UserType, ref short BankType, ref string BankName, ref string BankCardNumber, ref double Balance, ref double Freeze, ref double ScoringOfSelfBuy, ref double ScoringOfCommendBuy, ref double Scoring, ref short Level, ref long CommenderID, ref long CpsID, ref string AlipayID, ref string AlipayName, ref bool isAlipayNameValided, ref double Bonus, ref double Reward, ref string Memo, ref double BonusThisMonth, ref double BonusAllow, ref double BonusUse, ref double PromotionMemberBonusScale, ref double PromotionSiteBonusScale, ref int ReturnValue, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(conn, "P_Login", ref ds, ref Outputs,
                new MSSQL.Parameter("SiteID", SqlDbType.BigInt, 0, ParameterDirection.Input, SiteID),
                new MSSQL.Parameter("Name", SqlDbType.VarChar, 0, ParameterDirection.Input, Name),
                new MSSQL.Parameter("InputPassword", SqlDbType.VarChar, 0, ParameterDirection.Input, InputPassword),
                new MSSQL.Parameter("LoginIP", SqlDbType.VarChar, 0, ParameterDirection.Input, LoginIP),
                new MSSQL.Parameter("UserID", SqlDbType.BigInt, 8, ParameterDirection.Output, UserID),
                new MSSQL.Parameter("PasswordAdv", SqlDbType.VarChar, 32, ParameterDirection.Output, PasswordAdv),
                new MSSQL.Parameter("RealityName", SqlDbType.VarChar, 50, ParameterDirection.Output, RealityName),
                new MSSQL.Parameter("CityID", SqlDbType.Int, 4, ParameterDirection.Output, CityID),
                new MSSQL.Parameter("Sex", SqlDbType.VarChar, 2, ParameterDirection.Output, Sex),
                new MSSQL.Parameter("BirthDay", SqlDbType.DateTime, 8, ParameterDirection.Output, BirthDay),
                new MSSQL.Parameter("IDCardNumber", SqlDbType.VarChar, 50, ParameterDirection.Output, IDCardNumber),
                new MSSQL.Parameter("Address", SqlDbType.VarChar, 50, ParameterDirection.Output, Address),
                new MSSQL.Parameter("Email", SqlDbType.VarChar, 50, ParameterDirection.Output, Email),
                new MSSQL.Parameter("isEmailValided", SqlDbType.Bit, 1, ParameterDirection.Output, isEmailValided),
                new MSSQL.Parameter("QQ", SqlDbType.VarChar, 50, ParameterDirection.Output, QQ),
                new MSSQL.Parameter("isQQValided", SqlDbType.Bit, 1, ParameterDirection.Output, isQQValided),
                new MSSQL.Parameter("Telephone", SqlDbType.VarChar, 50, ParameterDirection.Output, Telephone),
                new MSSQL.Parameter("Mobile", SqlDbType.VarChar, 50, ParameterDirection.Output, Mobile),
                new MSSQL.Parameter("isMobileValided", SqlDbType.Bit, 1, ParameterDirection.Output, isMobileValided),
                new MSSQL.Parameter("isPrivacy", SqlDbType.Bit, 1, ParameterDirection.Output, isPrivacy),
                new MSSQL.Parameter("isCanLogin", SqlDbType.Bit, 1, ParameterDirection.Output, isCanLogin),
                new MSSQL.Parameter("isAllowWinScore", SqlDbType.Bit, 1, ParameterDirection.Output, isAllowWinScore),
                new MSSQL.Parameter("RegisterTime", SqlDbType.DateTime, 8, ParameterDirection.Output, RegisterTime),
                new MSSQL.Parameter("LastLoginTime", SqlDbType.DateTime, 8, ParameterDirection.Output, LastLoginTime),
                new MSSQL.Parameter("LastLoginIP", SqlDbType.VarChar, 50, ParameterDirection.Output, LastLoginIP),
                new MSSQL.Parameter("LoginCount", SqlDbType.Int, 4, ParameterDirection.Output, LoginCount),
                new MSSQL.Parameter("UserType", SqlDbType.SmallInt, 2, ParameterDirection.Output, UserType),
                new MSSQL.Parameter("BankType", SqlDbType.SmallInt, 2, ParameterDirection.Output, BankType),
                new MSSQL.Parameter("BankName", SqlDbType.VarChar, 50, ParameterDirection.Output, BankName),
                new MSSQL.Parameter("BankCardNumber", SqlDbType.VarChar, 50, ParameterDirection.Output, BankCardNumber),
                new MSSQL.Parameter("Balance", SqlDbType.Money, 8, ParameterDirection.Output, Balance),
                new MSSQL.Parameter("Freeze", SqlDbType.Money, 8, ParameterDirection.Output, Freeze),
                new MSSQL.Parameter("ScoringOfSelfBuy", SqlDbType.Float, 8, ParameterDirection.Output, ScoringOfSelfBuy),
                new MSSQL.Parameter("ScoringOfCommendBuy", SqlDbType.Float, 8, ParameterDirection.Output, ScoringOfCommendBuy),
                new MSSQL.Parameter("Scoring", SqlDbType.Float, 8, ParameterDirection.Output, Scoring),
                new MSSQL.Parameter("Level", SqlDbType.SmallInt, 2, ParameterDirection.Output, Level),
                new MSSQL.Parameter("CommenderID", SqlDbType.BigInt, 8, ParameterDirection.Output, CommenderID),
                new MSSQL.Parameter("CpsID", SqlDbType.BigInt, 8, ParameterDirection.Output, CpsID),
                new MSSQL.Parameter("AlipayID", SqlDbType.VarChar, 32, ParameterDirection.Output, AlipayID),
                new MSSQL.Parameter("AlipayName", SqlDbType.VarChar, 50, ParameterDirection.Output, AlipayName),
                new MSSQL.Parameter("isAlipayNameValided", SqlDbType.Bit, 1, ParameterDirection.Output, isAlipayNameValided),
                new MSSQL.Parameter("Bonus", SqlDbType.Money, 8, ParameterDirection.Output, Bonus),
                new MSSQL.Parameter("Reward", SqlDbType.Money, 8, ParameterDirection.Output, Reward),
                new MSSQL.Parameter("Memo", SqlDbType.VarChar, 50, ParameterDirection.Output, Memo),
                new MSSQL.Parameter("BonusThisMonth", SqlDbType.Money, 8, ParameterDirection.Output, BonusThisMonth),
                new MSSQL.Parameter("BonusAllow", SqlDbType.Money, 8, ParameterDirection.Output, BonusAllow),
                new MSSQL.Parameter("BonusUse", SqlDbType.Money, 8, ParameterDirection.Output, BonusUse),
                new MSSQL.Parameter("PromotionMemberBonusScale", SqlDbType.Float, 8, ParameterDirection.Output, PromotionMemberBonusScale),
                new MSSQL.Parameter("PromotionSiteBonusScale", SqlDbType.Float, 8, ParameterDirection.Output, PromotionSiteBonusScale),
                new MSSQL.Parameter("ReturnValue", SqlDbType.Int, 4, ParameterDirection.Output, ReturnValue),
                new MSSQL.Parameter("ReturnDescription", SqlDbType.VarChar, 100, ParameterDirection.Output, ReturnDescription)
                );

            try
            {
                UserID = System.Convert.ToInt64(Outputs["UserID"]);
            }
            catch { }

            try
            {
                PasswordAdv = System.Convert.ToString(Outputs["PasswordAdv"]);
            }
            catch { }

            try
            {
                RealityName = System.Convert.ToString(Outputs["RealityName"]);
            }
            catch { }

            try
            {
                CityID = System.Convert.ToInt32(Outputs["CityID"]);
            }
            catch { }

            try
            {
                Sex = System.Convert.ToString(Outputs["Sex"]);
            }
            catch { }

            try
            {
                BirthDay = System.Convert.ToDateTime(Outputs["BirthDay"]);
            }
            catch { }

            try
            {
                IDCardNumber = System.Convert.ToString(Outputs["IDCardNumber"]);
            }
            catch { }

            try
            {
                Address = System.Convert.ToString(Outputs["Address"]);
            }
            catch { }

            try
            {
                Email = System.Convert.ToString(Outputs["Email"]);
            }
            catch { }

            try
            {
                isEmailValided = System.Convert.ToBoolean(Outputs["isEmailValided"]);
            }
            catch { }

            try
            {
                QQ = System.Convert.ToString(Outputs["QQ"]);
            }
            catch { }

            try
            {
                isQQValided = System.Convert.ToBoolean(Outputs["isQQValided"]);
            }
            catch { }

            try
            {
                Telephone = System.Convert.ToString(Outputs["Telephone"]);
            }
            catch { }

            try
            {
                Mobile = System.Convert.ToString(Outputs["Mobile"]);
            }
            catch { }

            try
            {
                isMobileValided = System.Convert.ToBoolean(Outputs["isMobileValided"]);
            }
            catch { }

            try
            {
                isPrivacy = System.Convert.ToBoolean(Outputs["isPrivacy"]);
            }
            catch { }

            try
            {
                isCanLogin = System.Convert.ToBoolean(Outputs["isCanLogin"]);
            }
            catch { }

            try
            {
                isAllowWinScore = System.Convert.ToBoolean(Outputs["isAllowWinScore"]);
            }
            catch { }

            try
            {
                RegisterTime = System.Convert.ToDateTime(Outputs["RegisterTime"]);
            }
            catch { }

            try
            {
                LastLoginTime = System.Convert.ToDateTime(Outputs["LastLoginTime"]);
            }
            catch { }

            try
            {
                LastLoginIP = System.Convert.ToString(Outputs["LastLoginIP"]);
            }
            catch { }

            try
            {
                LoginCount = System.Convert.ToInt32(Outputs["LoginCount"]);
            }
            catch { }

            try
            {
                UserType = System.Convert.ToInt16(Outputs["UserType"]);
            }
            catch { }

            try
            {
                BankType = System.Convert.ToInt16(Outputs["BankType"]);
            }
            catch { }

            try
            {
                BankName = System.Convert.ToString(Outputs["BankName"]);
            }
            catch { }

            try
            {
                BankCardNumber = System.Convert.ToString(Outputs["BankCardNumber"]);
            }
            catch { }

            try
            {
                Balance = System.Convert.ToDouble(Outputs["Balance"]);
            }
            catch { }

            try
            {
                Freeze = System.Convert.ToDouble(Outputs["Freeze"]);
            }
            catch { }

            try
            {
                ScoringOfSelfBuy = System.Convert.ToDouble(Outputs["ScoringOfSelfBuy"]);
            }
            catch { }

            try
            {
                ScoringOfCommendBuy = System.Convert.ToDouble(Outputs["ScoringOfCommendBuy"]);
            }
            catch { }

            try
            {
                Scoring = System.Convert.ToDouble(Outputs["Scoring"]);
            }
            catch { }

            try
            {
                Level = System.Convert.ToInt16(Outputs["Level"]);
            }
            catch { }

            try
            {
                CommenderID = System.Convert.ToInt64(Outputs["CommenderID"]);
            }
            catch { }

            try
            {
                CpsID = System.Convert.ToInt64(Outputs["CpsID"]);
            }
            catch { }

            try
            {
                AlipayID = System.Convert.ToString(Outputs["AlipayID"]);
            }
            catch { }

            try
            {
                AlipayName = System.Convert.ToString(Outputs["AlipayName"]);
            }
            catch { }

            try
            {
                isAlipayNameValided = System.Convert.ToBoolean(Outputs["isAlipayNameValided"]);
            }
            catch { }

            try
            {
                Bonus = System.Convert.ToDouble(Outputs["Bonus"]);
            }
            catch { }

            try
            {
                Reward = System.Convert.ToDouble(Outputs["Reward"]);
            }
            catch { }

            try
            {
                Memo = System.Convert.ToString(Outputs["Memo"]);
            }
            catch { }

            try
            {
                BonusThisMonth = System.Convert.ToDouble(Outputs["BonusThisMonth"]);
            }
            catch { }

            try
            {
                BonusAllow = System.Convert.ToDouble(Outputs["BonusAllow"]);
            }
            catch { }

            try
            {
                BonusUse = System.Convert.ToDouble(Outputs["BonusUse"]);
            }
            catch { }

            try
            {
                PromotionMemberBonusScale = System.Convert.ToDouble(Outputs["PromotionMemberBonusScale"]);
            }
            catch { }

            try
            {
                PromotionSiteBonusScale = System.Convert.ToDouble(Outputs["PromotionSiteBonusScale"]);
            }
            catch { }

            try
            {
                ReturnValue = System.Convert.ToInt32(Outputs["ReturnValue"]);
            }
            catch { }

            try
            {
                ReturnDescription = System.Convert.ToString(Outputs["ReturnDescription"]);
            }
            catch { }

            return CallResult;
        }

        public static int P_LotteryToolLinkAdd(SqlConnection conn, long SiteID, int LotteryID, string LinkName, string LogoUrl, string Url, int Order, bool isShow, ref long NewLotteryToolLinkID, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_LotteryToolLinkAdd(conn, ref ds, SiteID, LotteryID, LinkName, LogoUrl, Url, Order, isShow, ref NewLotteryToolLinkID, ref ReturnDescription);
        }

        public static int P_LotteryToolLinkAdd(SqlConnection conn, ref DataSet ds, long SiteID, int LotteryID, string LinkName, string LogoUrl, string Url, int Order, bool isShow, ref long NewLotteryToolLinkID, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(conn, "P_LotteryToolLinkAdd", ref ds, ref Outputs,
                new MSSQL.Parameter("SiteID", SqlDbType.BigInt, 0, ParameterDirection.Input, SiteID),
                new MSSQL.Parameter("LotteryID", SqlDbType.Int, 0, ParameterDirection.Input, LotteryID),
                new MSSQL.Parameter("LinkName", SqlDbType.VarChar, 0, ParameterDirection.Input, LinkName),
                new MSSQL.Parameter("LogoUrl", SqlDbType.VarChar, 0, ParameterDirection.Input, LogoUrl),
                new MSSQL.Parameter("Url", SqlDbType.VarChar, 0, ParameterDirection.Input, Url),
                new MSSQL.Parameter("Order", SqlDbType.Int, 0, ParameterDirection.Input, Order),
                new MSSQL.Parameter("isShow", SqlDbType.Bit, 0, ParameterDirection.Input, isShow),
                new MSSQL.Parameter("NewLotteryToolLinkID", SqlDbType.BigInt, 8, ParameterDirection.Output, NewLotteryToolLinkID),
                new MSSQL.Parameter("ReturnDescription", SqlDbType.VarChar, 100, ParameterDirection.Output, ReturnDescription)
                );

            try
            {
                NewLotteryToolLinkID = System.Convert.ToInt64(Outputs["NewLotteryToolLinkID"]);
            }
            catch { }

            try
            {
                ReturnDescription = System.Convert.ToString(Outputs["ReturnDescription"]);
            }
            catch { }

            return CallResult;
        }

        public static int P_LotteryToolLinkDelete(SqlConnection conn, long SiteID, long ID, ref int ReturnValue, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_LotteryToolLinkDelete(conn, ref ds, SiteID, ID, ref ReturnValue, ref ReturnDescription);
        }

        public static int P_LotteryToolLinkDelete(SqlConnection conn, ref DataSet ds, long SiteID, long ID, ref int ReturnValue, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(conn, "P_LotteryToolLinkDelete", ref ds, ref Outputs,
                new MSSQL.Parameter("SiteID", SqlDbType.BigInt, 0, ParameterDirection.Input, SiteID),
                new MSSQL.Parameter("ID", SqlDbType.BigInt, 0, ParameterDirection.Input, ID),
                new MSSQL.Parameter("ReturnValue", SqlDbType.Int, 4, ParameterDirection.Output, ReturnValue),
                new MSSQL.Parameter("ReturnDescription", SqlDbType.VarChar, 100, ParameterDirection.Output, ReturnDescription)
                );

            try
            {
                ReturnValue = System.Convert.ToInt32(Outputs["ReturnValue"]);
            }
            catch { }

            try
            {
                ReturnDescription = System.Convert.ToString(Outputs["ReturnDescription"]);
            }
            catch { }

            return CallResult;
        }

        public static int P_LotteryToolLinkEdit(SqlConnection conn, long SiteID, long ID, int LotteryID, string LinkName, string LogoUrl, string Url, int Order, bool isShow, ref int ReturnValue, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_LotteryToolLinkEdit(conn, ref ds, SiteID, ID, LotteryID, LinkName, LogoUrl, Url, Order, isShow, ref ReturnValue, ref ReturnDescription);
        }

        public static int P_LotteryToolLinkEdit(SqlConnection conn, ref DataSet ds, long SiteID, long ID, int LotteryID, string LinkName, string LogoUrl, string Url, int Order, bool isShow, ref int ReturnValue, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(conn, "P_LotteryToolLinkEdit", ref ds, ref Outputs,
                new MSSQL.Parameter("SiteID", SqlDbType.BigInt, 0, ParameterDirection.Input, SiteID),
                new MSSQL.Parameter("ID", SqlDbType.BigInt, 0, ParameterDirection.Input, ID),
                new MSSQL.Parameter("LotteryID", SqlDbType.Int, 0, ParameterDirection.Input, LotteryID),
                new MSSQL.Parameter("LinkName", SqlDbType.VarChar, 0, ParameterDirection.Input, LinkName),
                new MSSQL.Parameter("LogoUrl", SqlDbType.VarChar, 0, ParameterDirection.Input, LogoUrl),
                new MSSQL.Parameter("Url", SqlDbType.VarChar, 0, ParameterDirection.Input, Url),
                new MSSQL.Parameter("Order", SqlDbType.Int, 0, ParameterDirection.Input, Order),
                new MSSQL.Parameter("isShow", SqlDbType.Bit, 0, ParameterDirection.Input, isShow),
                new MSSQL.Parameter("ReturnValue", SqlDbType.Int, 4, ParameterDirection.Output, ReturnValue),
                new MSSQL.Parameter("ReturnDescription", SqlDbType.VarChar, 100, ParameterDirection.Output, ReturnDescription)
                );

            try
            {
                ReturnValue = System.Convert.ToInt32(Outputs["ReturnValue"]);
            }
            catch { }

            try
            {
                ReturnDescription = System.Convert.ToString(Outputs["ReturnDescription"]);
            }
            catch { }

            return CallResult;
        }

        public static int P_MarketOutlookAdd(SqlConnection conn, DateTime DateTime, string Title, string Content, bool isShow, ref long NewMarketOutlookID, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_MarketOutlookAdd(conn, ref ds, DateTime, Title, Content, isShow, ref NewMarketOutlookID, ref ReturnDescription);
        }

        public static int P_MarketOutlookAdd(SqlConnection conn, ref DataSet ds, DateTime DateTime, string Title, string Content, bool isShow, ref long NewMarketOutlookID, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(conn, "P_MarketOutlookAdd", ref ds, ref Outputs,
                new MSSQL.Parameter("DateTime", SqlDbType.DateTime, 0, ParameterDirection.Input, DateTime),
                new MSSQL.Parameter("Title", SqlDbType.VarChar, 0, ParameterDirection.Input, Title),
                new MSSQL.Parameter("Content", SqlDbType.VarChar, 0, ParameterDirection.Input, Content),
                new MSSQL.Parameter("isShow", SqlDbType.Bit, 0, ParameterDirection.Input, isShow),
                new MSSQL.Parameter("NewMarketOutlookID", SqlDbType.BigInt, 8, ParameterDirection.Output, NewMarketOutlookID),
                new MSSQL.Parameter("ReturnDescription", SqlDbType.VarChar, 100, ParameterDirection.Output, ReturnDescription)
                );

            try
            {
                NewMarketOutlookID = System.Convert.ToInt64(Outputs["NewMarketOutlookID"]);
            }
            catch { }

            try
            {
                ReturnDescription = System.Convert.ToString(Outputs["ReturnDescription"]);
            }
            catch { }

            return CallResult;
        }

        public static int P_MarketOutlookDelete(SqlConnection conn, long ID, ref int ReturnValue, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_MarketOutlookDelete(conn, ref ds, ID, ref ReturnValue, ref ReturnDescription);
        }

        public static int P_MarketOutlookDelete(SqlConnection conn, ref DataSet ds, long ID, ref int ReturnValue, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(conn, "P_MarketOutlookDelete", ref ds, ref Outputs,
                new MSSQL.Parameter("ID", SqlDbType.BigInt, 0, ParameterDirection.Input, ID),
                new MSSQL.Parameter("ReturnValue", SqlDbType.Int, 4, ParameterDirection.Output, ReturnValue),
                new MSSQL.Parameter("ReturnDescription", SqlDbType.VarChar, 100, ParameterDirection.Output, ReturnDescription)
                );

            try
            {
                ReturnValue = System.Convert.ToInt32(Outputs["ReturnValue"]);
            }
            catch { }

            try
            {
                ReturnDescription = System.Convert.ToString(Outputs["ReturnDescription"]);
            }
            catch { }

            return CallResult;
        }

        public static int P_MarketOutlookEdit(SqlConnection conn, long ID, DateTime DateTime, string Title, string Content, bool isShow, ref int ReturnValue, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_MarketOutlookEdit(conn, ref ds, ID, DateTime, Title, Content, isShow, ref ReturnValue, ref ReturnDescription);
        }

        public static int P_MarketOutlookEdit(SqlConnection conn, ref DataSet ds, long ID, DateTime DateTime, string Title, string Content, bool isShow, ref int ReturnValue, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(conn, "P_MarketOutlookEdit", ref ds, ref Outputs,
                new MSSQL.Parameter("ID", SqlDbType.BigInt, 0, ParameterDirection.Input, ID),
                new MSSQL.Parameter("DateTime", SqlDbType.DateTime, 0, ParameterDirection.Input, DateTime),
                new MSSQL.Parameter("Title", SqlDbType.VarChar, 0, ParameterDirection.Input, Title),
                new MSSQL.Parameter("Content", SqlDbType.VarChar, 0, ParameterDirection.Input, Content),
                new MSSQL.Parameter("isShow", SqlDbType.Bit, 0, ParameterDirection.Input, isShow),
                new MSSQL.Parameter("ReturnValue", SqlDbType.Int, 4, ParameterDirection.Output, ReturnValue),
                new MSSQL.Parameter("ReturnDescription", SqlDbType.VarChar, 100, ParameterDirection.Output, ReturnDescription)
                );

            try
            {
                ReturnValue = System.Convert.ToInt32(Outputs["ReturnValue"]);
            }
            catch { }

            try
            {
                ReturnDescription = System.Convert.ToString(Outputs["ReturnDescription"]);
            }
            catch { }

            return CallResult;
        }

        public static int P_MergeUserDetails(SqlConnection conn, string CallPassword, ref int ReturnValue, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_MergeUserDetails(conn, ref ds, CallPassword, ref ReturnValue, ref ReturnDescription);
        }

        public static int P_MergeUserDetails(SqlConnection conn, ref DataSet ds, string CallPassword, ref int ReturnValue, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(conn, "P_MergeUserDetails", ref ds, ref Outputs,
                new MSSQL.Parameter("CallPassword", SqlDbType.VarChar, 0, ParameterDirection.Input, CallPassword),
                new MSSQL.Parameter("ReturnValue", SqlDbType.Int, 4, ParameterDirection.Output, ReturnValue),
                new MSSQL.Parameter("ReturnDescription", SqlDbType.VarChar, 100, ParameterDirection.Output, ReturnDescription)
                );

            try
            {
                ReturnValue = System.Convert.ToInt32(Outputs["ReturnValue"]);
            }
            catch { }

            try
            {
                ReturnDescription = System.Convert.ToString(Outputs["ReturnDescription"]);
            }
            catch { }

            return CallResult;
        }

        public static int P_NewsAdd(SqlConnection conn, long SiteID, int TypeID, DateTime DateTime, string Title, string Content, string ImageUrl, bool isShow, bool isHasImage, bool isCanComments, bool isCommend, bool isHot, long ReadCount, ref long NewsID, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_NewsAdd(conn, ref ds, SiteID, TypeID, DateTime, Title, Content, ImageUrl, isShow, isHasImage, isCanComments, isCommend, isHot, ReadCount, ref NewsID, ref ReturnDescription);
        }

        public static int P_NewsAdd(SqlConnection conn, ref DataSet ds, long SiteID, int TypeID, DateTime DateTime, string Title, string Content, string ImageUrl, bool isShow, bool isHasImage, bool isCanComments, bool isCommend, bool isHot, long ReadCount, ref long NewsID, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(conn, "P_NewsAdd", ref ds, ref Outputs,
                new MSSQL.Parameter("SiteID", SqlDbType.BigInt, 0, ParameterDirection.Input, SiteID),
                new MSSQL.Parameter("TypeID", SqlDbType.Int, 0, ParameterDirection.Input, TypeID),
                new MSSQL.Parameter("DateTime", SqlDbType.DateTime, 0, ParameterDirection.Input, DateTime),
                new MSSQL.Parameter("Title", SqlDbType.VarChar, 0, ParameterDirection.Input, Title),
                new MSSQL.Parameter("Content", SqlDbType.VarChar, 0, ParameterDirection.Input, Content),
                new MSSQL.Parameter("ImageUrl", SqlDbType.VarChar, 0, ParameterDirection.Input, ImageUrl),
                new MSSQL.Parameter("isShow", SqlDbType.Bit, 0, ParameterDirection.Input, isShow),
                new MSSQL.Parameter("isHasImage", SqlDbType.Bit, 0, ParameterDirection.Input, isHasImage),
                new MSSQL.Parameter("isCanComments", SqlDbType.Bit, 0, ParameterDirection.Input, isCanComments),
                new MSSQL.Parameter("isCommend", SqlDbType.Bit, 0, ParameterDirection.Input, isCommend),
                new MSSQL.Parameter("isHot", SqlDbType.Bit, 0, ParameterDirection.Input, isHot),
                new MSSQL.Parameter("ReadCount", SqlDbType.BigInt, 0, ParameterDirection.Input, ReadCount),
                new MSSQL.Parameter("NewsID", SqlDbType.BigInt, 8, ParameterDirection.Output, NewsID),
                new MSSQL.Parameter("ReturnDescription", SqlDbType.VarChar, 100, ParameterDirection.Output, ReturnDescription)
                );

            try
            {
                NewsID = System.Convert.ToInt64(Outputs["NewsID"]);
            }
            catch { }

            try
            {
                ReturnDescription = System.Convert.ToString(Outputs["ReturnDescription"]);
            }
            catch { }

            return CallResult;
        }

        public static int P_NewsAddComments(SqlConnection conn, long SiteID, long NewsID, DateTime DateTime, long CommentserID, string CommentserName, string Content, bool isShow, ref long NewNewsCommentsID, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_NewsAddComments(conn, ref ds, SiteID, NewsID, DateTime, CommentserID, CommentserName, Content, isShow, ref NewNewsCommentsID, ref ReturnDescription);
        }

        public static int P_NewsAddComments(SqlConnection conn, ref DataSet ds, long SiteID, long NewsID, DateTime DateTime, long CommentserID, string CommentserName, string Content, bool isShow, ref long NewNewsCommentsID, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(conn, "P_NewsAddComments", ref ds, ref Outputs,
                new MSSQL.Parameter("SiteID", SqlDbType.BigInt, 0, ParameterDirection.Input, SiteID),
                new MSSQL.Parameter("NewsID", SqlDbType.BigInt, 0, ParameterDirection.Input, NewsID),
                new MSSQL.Parameter("DateTime", SqlDbType.DateTime, 0, ParameterDirection.Input, DateTime),
                new MSSQL.Parameter("CommentserID", SqlDbType.BigInt, 0, ParameterDirection.Input, CommentserID),
                new MSSQL.Parameter("CommentserName", SqlDbType.VarChar, 0, ParameterDirection.Input, CommentserName),
                new MSSQL.Parameter("Content", SqlDbType.VarChar, 0, ParameterDirection.Input, Content),
                new MSSQL.Parameter("isShow", SqlDbType.Bit, 0, ParameterDirection.Input, isShow),
                new MSSQL.Parameter("NewNewsCommentsID", SqlDbType.BigInt, 8, ParameterDirection.Output, NewNewsCommentsID),
                new MSSQL.Parameter("ReturnDescription", SqlDbType.VarChar, 100, ParameterDirection.Output, ReturnDescription)
                );

            try
            {
                NewNewsCommentsID = System.Convert.ToInt64(Outputs["NewNewsCommentsID"]);
            }
            catch { }

            try
            {
                ReturnDescription = System.Convert.ToString(Outputs["ReturnDescription"]);
            }
            catch { }

            return CallResult;
        }

        public static int P_NewsDelete(SqlConnection conn, long SiteID, long ID, ref int ReturnValue, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_NewsDelete(conn, ref ds, SiteID, ID, ref ReturnValue, ref ReturnDescription);
        }

        public static int P_NewsDelete(SqlConnection conn, ref DataSet ds, long SiteID, long ID, ref int ReturnValue, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(conn, "P_NewsDelete", ref ds, ref Outputs,
                new MSSQL.Parameter("SiteID", SqlDbType.BigInt, 0, ParameterDirection.Input, SiteID),
                new MSSQL.Parameter("ID", SqlDbType.BigInt, 0, ParameterDirection.Input, ID),
                new MSSQL.Parameter("ReturnValue", SqlDbType.Int, 4, ParameterDirection.Output, ReturnValue),
                new MSSQL.Parameter("ReturnDescription", SqlDbType.VarChar, 100, ParameterDirection.Output, ReturnDescription)
                );

            try
            {
                ReturnValue = System.Convert.ToInt32(Outputs["ReturnValue"]);
            }
            catch { }

            try
            {
                ReturnDescription = System.Convert.ToString(Outputs["ReturnDescription"]);
            }
            catch { }

            return CallResult;
        }

        public static int P_NewsDeleteComments(SqlConnection conn, long SiteID, long NewsCommentsID, ref long ReturnValue, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_NewsDeleteComments(conn, ref ds, SiteID, NewsCommentsID, ref ReturnValue, ref ReturnDescription);
        }

        public static int P_NewsDeleteComments(SqlConnection conn, ref DataSet ds, long SiteID, long NewsCommentsID, ref long ReturnValue, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(conn, "P_NewsDeleteComments", ref ds, ref Outputs,
                new MSSQL.Parameter("SiteID", SqlDbType.BigInt, 0, ParameterDirection.Input, SiteID),
                new MSSQL.Parameter("NewsCommentsID", SqlDbType.BigInt, 0, ParameterDirection.Input, NewsCommentsID),
                new MSSQL.Parameter("ReturnValue", SqlDbType.BigInt, 8, ParameterDirection.Output, ReturnValue),
                new MSSQL.Parameter("ReturnDescription", SqlDbType.VarChar, 100, ParameterDirection.Output, ReturnDescription)
                );

            try
            {
                ReturnValue = System.Convert.ToInt64(Outputs["ReturnValue"]);
            }
            catch { }

            try
            {
                ReturnDescription = System.Convert.ToString(Outputs["ReturnDescription"]);
            }
            catch { }

            return CallResult;
        }

        public static int P_NewsEdit(SqlConnection conn, long SiteID, long ID, int TypeID, DateTime DateTime, string Title, string Content, string ImageUrl, bool isShow, bool isHasImage, bool isCanComments, bool isCommend, bool isHot, long ReadCount, ref int ReturnValue, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_NewsEdit(conn, ref ds, SiteID, ID, TypeID, DateTime, Title, Content, ImageUrl, isShow, isHasImage, isCanComments, isCommend, isHot, ReadCount, ref ReturnValue, ref ReturnDescription);
        }

        public static int P_NewsEdit(SqlConnection conn, ref DataSet ds, long SiteID, long ID, int TypeID, DateTime DateTime, string Title, string Content, string ImageUrl, bool isShow, bool isHasImage, bool isCanComments, bool isCommend, bool isHot, long ReadCount, ref int ReturnValue, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(conn, "P_NewsEdit", ref ds, ref Outputs,
                new MSSQL.Parameter("SiteID", SqlDbType.BigInt, 0, ParameterDirection.Input, SiteID),
                new MSSQL.Parameter("ID", SqlDbType.BigInt, 0, ParameterDirection.Input, ID),
                new MSSQL.Parameter("TypeID", SqlDbType.Int, 0, ParameterDirection.Input, TypeID),
                new MSSQL.Parameter("DateTime", SqlDbType.DateTime, 0, ParameterDirection.Input, DateTime),
                new MSSQL.Parameter("Title", SqlDbType.VarChar, 0, ParameterDirection.Input, Title),
                new MSSQL.Parameter("Content", SqlDbType.VarChar, 0, ParameterDirection.Input, Content),
                new MSSQL.Parameter("ImageUrl", SqlDbType.VarChar, 0, ParameterDirection.Input, ImageUrl),
                new MSSQL.Parameter("isShow", SqlDbType.Bit, 0, ParameterDirection.Input, isShow),
                new MSSQL.Parameter("isHasImage", SqlDbType.Bit, 0, ParameterDirection.Input, isHasImage),
                new MSSQL.Parameter("isCanComments", SqlDbType.Bit, 0, ParameterDirection.Input, isCanComments),
                new MSSQL.Parameter("isCommend", SqlDbType.Bit, 0, ParameterDirection.Input, isCommend),
                new MSSQL.Parameter("isHot", SqlDbType.Bit, 0, ParameterDirection.Input, isHot),
                new MSSQL.Parameter("ReadCount", SqlDbType.BigInt, 0, ParameterDirection.Input, ReadCount),
                new MSSQL.Parameter("ReturnValue", SqlDbType.Int, 4, ParameterDirection.Output, ReturnValue),
                new MSSQL.Parameter("ReturnDescription", SqlDbType.VarChar, 100, ParameterDirection.Output, ReturnDescription)
                );

            try
            {
                ReturnValue = System.Convert.ToInt32(Outputs["ReturnValue"]);
            }
            catch { }

            try
            {
                ReturnDescription = System.Convert.ToString(Outputs["ReturnDescription"]);
            }
            catch { }

            return CallResult;
        }

        public static int P_NewsEditComments(SqlConnection conn, long SiteID, long NewsCommentsID, DateTime DateTime, long CommentserID, string CommentserName, string Content, bool isShow, ref long ReturnValue, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_NewsEditComments(conn, ref ds, SiteID, NewsCommentsID, DateTime, CommentserID, CommentserName, Content, isShow, ref ReturnValue, ref ReturnDescription);
        }

        public static int P_NewsEditComments(SqlConnection conn, ref DataSet ds, long SiteID, long NewsCommentsID, DateTime DateTime, long CommentserID, string CommentserName, string Content, bool isShow, ref long ReturnValue, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(conn, "P_NewsEditComments", ref ds, ref Outputs,
                new MSSQL.Parameter("SiteID", SqlDbType.BigInt, 0, ParameterDirection.Input, SiteID),
                new MSSQL.Parameter("NewsCommentsID", SqlDbType.BigInt, 0, ParameterDirection.Input, NewsCommentsID),
                new MSSQL.Parameter("DateTime", SqlDbType.DateTime, 0, ParameterDirection.Input, DateTime),
                new MSSQL.Parameter("CommentserID", SqlDbType.BigInt, 0, ParameterDirection.Input, CommentserID),
                new MSSQL.Parameter("CommentserName", SqlDbType.VarChar, 0, ParameterDirection.Input, CommentserName),
                new MSSQL.Parameter("Content", SqlDbType.VarChar, 0, ParameterDirection.Input, Content),
                new MSSQL.Parameter("isShow", SqlDbType.Bit, 0, ParameterDirection.Input, isShow),
                new MSSQL.Parameter("ReturnValue", SqlDbType.BigInt, 8, ParameterDirection.Output, ReturnValue),
                new MSSQL.Parameter("ReturnDescription", SqlDbType.VarChar, 100, ParameterDirection.Output, ReturnDescription)
                );

            try
            {
                ReturnValue = System.Convert.ToInt64(Outputs["ReturnValue"]);
            }
            catch { }

            try
            {
                ReturnDescription = System.Convert.ToString(Outputs["ReturnDescription"]);
            }
            catch { }

            return CallResult;
        }

        public static int P_NewsRead(SqlConnection conn, long SiteID, long NewsID, ref int ReturnValue, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_NewsRead(conn, ref ds, SiteID, NewsID, ref ReturnValue, ref ReturnDescription);
        }

        public static int P_NewsRead(SqlConnection conn, ref DataSet ds, long SiteID, long NewsID, ref int ReturnValue, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(conn, "P_NewsRead", ref ds, ref Outputs,
                new MSSQL.Parameter("SiteID", SqlDbType.BigInt, 0, ParameterDirection.Input, SiteID),
                new MSSQL.Parameter("NewsID", SqlDbType.BigInt, 0, ParameterDirection.Input, NewsID),
                new MSSQL.Parameter("ReturnValue", SqlDbType.Int, 4, ParameterDirection.Output, ReturnValue),
                new MSSQL.Parameter("ReturnDescription", SqlDbType.VarChar, 100, ParameterDirection.Output, ReturnDescription)
                );

            try
            {
                ReturnValue = System.Convert.ToInt32(Outputs["ReturnValue"]);
            }
            catch { }

            try
            {
                ReturnDescription = System.Convert.ToString(Outputs["ReturnDescription"]);
            }
            catch { }

            return CallResult;
        }

        public static int P_PoliciesAndRegulationAdd(SqlConnection conn, DateTime DateTime, string Title, string Content, bool isShow, ref long NewPoliciesAndRegulationID, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_PoliciesAndRegulationAdd(conn, ref ds, DateTime, Title, Content, isShow, ref NewPoliciesAndRegulationID, ref ReturnDescription);
        }

        public static int P_PoliciesAndRegulationAdd(SqlConnection conn, ref DataSet ds, DateTime DateTime, string Title, string Content, bool isShow, ref long NewPoliciesAndRegulationID, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(conn, "P_PoliciesAndRegulationAdd", ref ds, ref Outputs,
                new MSSQL.Parameter("DateTime", SqlDbType.DateTime, 0, ParameterDirection.Input, DateTime),
                new MSSQL.Parameter("Title", SqlDbType.VarChar, 0, ParameterDirection.Input, Title),
                new MSSQL.Parameter("Content", SqlDbType.VarChar, 0, ParameterDirection.Input, Content),
                new MSSQL.Parameter("isShow", SqlDbType.Bit, 0, ParameterDirection.Input, isShow),
                new MSSQL.Parameter("NewPoliciesAndRegulationID", SqlDbType.BigInt, 8, ParameterDirection.Output, NewPoliciesAndRegulationID),
                new MSSQL.Parameter("ReturnDescription", SqlDbType.VarChar, 100, ParameterDirection.Output, ReturnDescription)
                );

            try
            {
                NewPoliciesAndRegulationID = System.Convert.ToInt64(Outputs["NewPoliciesAndRegulationID"]);
            }
            catch { }

            try
            {
                ReturnDescription = System.Convert.ToString(Outputs["ReturnDescription"]);
            }
            catch { }

            return CallResult;
        }

        public static int P_PoliciesAndRegulationDelete(SqlConnection conn, long ID, ref int ReturnValue, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_PoliciesAndRegulationDelete(conn, ref ds, ID, ref ReturnValue, ref ReturnDescription);
        }

        public static int P_PoliciesAndRegulationDelete(SqlConnection conn, ref DataSet ds, long ID, ref int ReturnValue, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(conn, "P_PoliciesAndRegulationDelete", ref ds, ref Outputs,
                new MSSQL.Parameter("ID", SqlDbType.BigInt, 0, ParameterDirection.Input, ID),
                new MSSQL.Parameter("ReturnValue", SqlDbType.Int, 4, ParameterDirection.Output, ReturnValue),
                new MSSQL.Parameter("ReturnDescription", SqlDbType.VarChar, 100, ParameterDirection.Output, ReturnDescription)
                );

            try
            {
                ReturnValue = System.Convert.ToInt32(Outputs["ReturnValue"]);
            }
            catch { }

            try
            {
                ReturnDescription = System.Convert.ToString(Outputs["ReturnDescription"]);
            }
            catch { }

            return CallResult;
        }

        public static int P_PoliciesAndRegulationEdit(SqlConnection conn, long ID, DateTime DateTime, string Title, string Content, bool isShow, ref int ReturnValue, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_PoliciesAndRegulationEdit(conn, ref ds, ID, DateTime, Title, Content, isShow, ref ReturnValue, ref ReturnDescription);
        }

        public static int P_PoliciesAndRegulationEdit(SqlConnection conn, ref DataSet ds, long ID, DateTime DateTime, string Title, string Content, bool isShow, ref int ReturnValue, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(conn, "P_PoliciesAndRegulationEdit", ref ds, ref Outputs,
                new MSSQL.Parameter("ID", SqlDbType.BigInt, 0, ParameterDirection.Input, ID),
                new MSSQL.Parameter("DateTime", SqlDbType.DateTime, 0, ParameterDirection.Input, DateTime),
                new MSSQL.Parameter("Title", SqlDbType.VarChar, 0, ParameterDirection.Input, Title),
                new MSSQL.Parameter("Content", SqlDbType.VarChar, 0, ParameterDirection.Input, Content),
                new MSSQL.Parameter("isShow", SqlDbType.Bit, 0, ParameterDirection.Input, isShow),
                new MSSQL.Parameter("ReturnValue", SqlDbType.Int, 4, ParameterDirection.Output, ReturnValue),
                new MSSQL.Parameter("ReturnDescription", SqlDbType.VarChar, 100, ParameterDirection.Output, ReturnDescription)
                );

            try
            {
                ReturnValue = System.Convert.ToInt32(Outputs["ReturnValue"]);
            }
            catch { }

            try
            {
                ReturnDescription = System.Convert.ToString(Outputs["ReturnDescription"]);
            }
            catch { }

            return CallResult;
        }

        public static int P_PopUserBonus(SqlConnection conn, long Id, ref int ReturnValue, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_PopUserBonus(conn, ref ds, Id, ref ReturnValue, ref ReturnDescription);
        }

        public static int P_PopUserBonus(SqlConnection conn, ref DataSet ds, long Id, ref int ReturnValue, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(conn, "P_PopUserBonus", ref ds, ref Outputs,
                new MSSQL.Parameter("Id", SqlDbType.BigInt, 0, ParameterDirection.Input, Id),
                new MSSQL.Parameter("ReturnValue", SqlDbType.Int, 4, ParameterDirection.Output, ReturnValue),
                new MSSQL.Parameter("ReturnDescription", SqlDbType.VarChar, 100, ParameterDirection.Output, ReturnDescription)
                );

            try
            {
                ReturnValue = System.Convert.ToInt32(Outputs["ReturnValue"]);
            }
            catch { }

            try
            {
                ReturnDescription = System.Convert.ToString(Outputs["ReturnDescription"]);
            }
            catch { }

            return CallResult;
        }

        public static int P_Quash(SqlConnection conn, long SiteID, long BuyDetailID, ref int ReturnValue, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_Quash(conn, ref ds, SiteID, BuyDetailID, ref ReturnValue, ref ReturnDescription);
        }

        public static int P_Quash(SqlConnection conn, ref DataSet ds, long SiteID, long BuyDetailID, ref int ReturnValue, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(conn, "P_Quash", ref ds, ref Outputs,
                new MSSQL.Parameter("SiteID", SqlDbType.BigInt, 0, ParameterDirection.Input, SiteID),
                new MSSQL.Parameter("BuyDetailID", SqlDbType.BigInt, 0, ParameterDirection.Input, BuyDetailID),
                new MSSQL.Parameter("ReturnValue", SqlDbType.Int, 4, ParameterDirection.Output, ReturnValue),
                new MSSQL.Parameter("ReturnDescription", SqlDbType.VarChar, 100, ParameterDirection.Output, ReturnDescription)
                );

            try
            {
                ReturnValue = System.Convert.ToInt32(Outputs["ReturnValue"]);
            }
            catch { }

            try
            {
                ReturnDescription = System.Convert.ToString(Outputs["ReturnDescription"]);
            }
            catch { }

            return CallResult;
        }

        public static int P_QuashChaseTask(SqlConnection conn, long SiteID, long ChaseTaskID, bool isSystemQuash, ref int ReturnValue, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_QuashChaseTask(conn, ref ds, SiteID, ChaseTaskID, isSystemQuash, ref ReturnValue, ref ReturnDescription);
        }

        public static int P_QuashChaseTask(SqlConnection conn, ref DataSet ds, long SiteID, long ChaseTaskID, bool isSystemQuash, ref int ReturnValue, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(conn, "P_QuashChaseTask", ref ds, ref Outputs,
                new MSSQL.Parameter("SiteID", SqlDbType.BigInt, 0, ParameterDirection.Input, SiteID),
                new MSSQL.Parameter("ChaseTaskID", SqlDbType.BigInt, 0, ParameterDirection.Input, ChaseTaskID),
                new MSSQL.Parameter("isSystemQuash", SqlDbType.Bit, 0, ParameterDirection.Input, isSystemQuash),
                new MSSQL.Parameter("ReturnValue", SqlDbType.Int, 4, ParameterDirection.Output, ReturnValue),
                new MSSQL.Parameter("ReturnDescription", SqlDbType.VarChar, 100, ParameterDirection.Output, ReturnDescription)
                );

            try
            {
                ReturnValue = System.Convert.ToInt32(Outputs["ReturnValue"]);
            }
            catch { }

            try
            {
                ReturnDescription = System.Convert.ToString(Outputs["ReturnDescription"]);
            }
            catch { }

            return CallResult;
        }

        public static int P_QuashChaseTaskDetail(SqlConnection conn, long SiteID, long ChaseTaskDetailID, bool isSystemQuash, ref int ReturnValue, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_QuashChaseTaskDetail(conn, ref ds, SiteID, ChaseTaskDetailID, isSystemQuash, ref ReturnValue, ref ReturnDescription);
        }

        public static int P_QuashChaseTaskDetail(SqlConnection conn, ref DataSet ds, long SiteID, long ChaseTaskDetailID, bool isSystemQuash, ref int ReturnValue, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(conn, "P_QuashChaseTaskDetail", ref ds, ref Outputs,
                new MSSQL.Parameter("SiteID", SqlDbType.BigInt, 0, ParameterDirection.Input, SiteID),
                new MSSQL.Parameter("ChaseTaskDetailID", SqlDbType.BigInt, 0, ParameterDirection.Input, ChaseTaskDetailID),
                new MSSQL.Parameter("isSystemQuash", SqlDbType.Bit, 0, ParameterDirection.Input, isSystemQuash),
                new MSSQL.Parameter("ReturnValue", SqlDbType.Int, 4, ParameterDirection.Output, ReturnValue),
                new MSSQL.Parameter("ReturnDescription", SqlDbType.VarChar, 100, ParameterDirection.Output, ReturnDescription)
                );

            try
            {
                ReturnValue = System.Convert.ToInt32(Outputs["ReturnValue"]);
            }
            catch { }

            try
            {
                ReturnDescription = System.Convert.ToString(Outputs["ReturnDescription"]);
            }
            catch { }

            return CallResult;
        }

        public static int P_QuashScheme(SqlConnection conn, long SiteID, long SchemeID, bool isSystemQuash, bool isRelation, ref int ReturnValue, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_QuashScheme(conn, ref ds, SiteID, SchemeID, isSystemQuash, isRelation, ref ReturnValue, ref ReturnDescription);
        }

        public static int P_QuashScheme(SqlConnection conn, ref DataSet ds, long SiteID, long SchemeID, bool isSystemQuash, bool isRelation, ref int ReturnValue, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(conn, "P_QuashScheme", ref ds, ref Outputs,
                new MSSQL.Parameter("SiteID", SqlDbType.BigInt, 0, ParameterDirection.Input, SiteID),
                new MSSQL.Parameter("SchemeID", SqlDbType.BigInt, 0, ParameterDirection.Input, SchemeID),
                new MSSQL.Parameter("isSystemQuash", SqlDbType.Bit, 0, ParameterDirection.Input, isSystemQuash),
                new MSSQL.Parameter("isRelation", SqlDbType.Bit, 0, ParameterDirection.Input, isRelation),
                new MSSQL.Parameter("ReturnValue", SqlDbType.Int, 4, ParameterDirection.Output, ReturnValue),
                new MSSQL.Parameter("ReturnDescription", SqlDbType.VarChar, 100, ParameterDirection.Output, ReturnDescription)
                );

            try
            {
                ReturnValue = System.Convert.ToInt32(Outputs["ReturnValue"]);
            }
            catch { }

            try
            {
                ReturnDescription = System.Convert.ToString(Outputs["ReturnDescription"]);
            }
            catch { }

            return CallResult;
        }

        public static int P_QuashSchemeNoLotteryNumber(SqlConnection conn, ref int ReturnValue, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_QuashSchemeNoLotteryNumber(conn, ref ds, ref ReturnValue, ref ReturnDescription);
        }

        public static int P_QuashSchemeNoLotteryNumber(SqlConnection conn, ref DataSet ds, ref int ReturnValue, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(conn, "P_QuashSchemeNoLotteryNumber", ref ds, ref Outputs,
                new MSSQL.Parameter("ReturnValue", SqlDbType.Int, 4, ParameterDirection.Output, ReturnValue),
                new MSSQL.Parameter("ReturnDescription", SqlDbType.VarChar, 100, ParameterDirection.Output, ReturnDescription)
                );

            try
            {
                ReturnValue = System.Convert.ToInt32(Outputs["ReturnValue"]);
            }
            catch { }

            try
            {
                ReturnDescription = System.Convert.ToString(Outputs["ReturnDescription"]);
            }
            catch { }

            return CallResult;
        }

        public static int P_QuashTemp12345(SqlConnection conn, long SiteID, long BuyDetailID, ref int ReturnValue, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_QuashTemp12345(conn, ref ds, SiteID, BuyDetailID, ref ReturnValue, ref ReturnDescription);
        }

        public static int P_QuashTemp12345(SqlConnection conn, ref DataSet ds, long SiteID, long BuyDetailID, ref int ReturnValue, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(conn, "P_QuashTemp12345", ref ds, ref Outputs,
                new MSSQL.Parameter("SiteID", SqlDbType.BigInt, 0, ParameterDirection.Input, SiteID),
                new MSSQL.Parameter("BuyDetailID", SqlDbType.BigInt, 0, ParameterDirection.Input, BuyDetailID),
                new MSSQL.Parameter("ReturnValue", SqlDbType.Int, 4, ParameterDirection.Output, ReturnValue),
                new MSSQL.Parameter("ReturnDescription", SqlDbType.VarChar, 100, ParameterDirection.Output, ReturnDescription)
                );

            try
            {
                ReturnValue = System.Convert.ToInt32(Outputs["ReturnValue"]);
            }
            catch { }

            try
            {
                ReturnDescription = System.Convert.ToString(Outputs["ReturnDescription"]);
            }
            catch { }

            return CallResult;
        }

        public static int P_QuestionsAdd(SqlConnection conn, long SiteID, long UserID, short TypeID, string Telephone, string Content, ref long NewQuestionID, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_QuestionsAdd(conn, ref ds, SiteID, UserID, TypeID, Telephone, Content, ref NewQuestionID, ref ReturnDescription);
        }

        public static int P_QuestionsAdd(SqlConnection conn, ref DataSet ds, long SiteID, long UserID, short TypeID, string Telephone, string Content, ref long NewQuestionID, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(conn, "P_QuestionsAdd", ref ds, ref Outputs,
                new MSSQL.Parameter("SiteID", SqlDbType.BigInt, 0, ParameterDirection.Input, SiteID),
                new MSSQL.Parameter("UserID", SqlDbType.BigInt, 0, ParameterDirection.Input, UserID),
                new MSSQL.Parameter("TypeID", SqlDbType.SmallInt, 0, ParameterDirection.Input, TypeID),
                new MSSQL.Parameter("Telephone", SqlDbType.VarChar, 0, ParameterDirection.Input, Telephone),
                new MSSQL.Parameter("Content", SqlDbType.VarChar, 0, ParameterDirection.Input, Content),
                new MSSQL.Parameter("NewQuestionID", SqlDbType.BigInt, 8, ParameterDirection.Output, NewQuestionID),
                new MSSQL.Parameter("ReturnDescription", SqlDbType.VarChar, 100, ParameterDirection.Output, ReturnDescription)
                );

            try
            {
                NewQuestionID = System.Convert.ToInt64(Outputs["NewQuestionID"]);
            }
            catch { }

            try
            {
                ReturnDescription = System.Convert.ToString(Outputs["ReturnDescription"]);
            }
            catch { }

            return CallResult;
        }

        public static int P_QuestionsAnswer(SqlConnection conn, long SiteID, long QuestionID, string Answer, long AnswerOperatorID, ref int ReturnValue, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_QuestionsAnswer(conn, ref ds, SiteID, QuestionID, Answer, AnswerOperatorID, ref ReturnValue, ref ReturnDescription);
        }

        public static int P_QuestionsAnswer(SqlConnection conn, ref DataSet ds, long SiteID, long QuestionID, string Answer, long AnswerOperatorID, ref int ReturnValue, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(conn, "P_QuestionsAnswer", ref ds, ref Outputs,
                new MSSQL.Parameter("SiteID", SqlDbType.BigInt, 0, ParameterDirection.Input, SiteID),
                new MSSQL.Parameter("QuestionID", SqlDbType.BigInt, 0, ParameterDirection.Input, QuestionID),
                new MSSQL.Parameter("Answer", SqlDbType.VarChar, 0, ParameterDirection.Input, Answer),
                new MSSQL.Parameter("AnswerOperatorID", SqlDbType.BigInt, 0, ParameterDirection.Input, AnswerOperatorID),
                new MSSQL.Parameter("ReturnValue", SqlDbType.Int, 4, ParameterDirection.Output, ReturnValue),
                new MSSQL.Parameter("ReturnDescription", SqlDbType.VarChar, 100, ParameterDirection.Output, ReturnDescription)
                );

            try
            {
                ReturnValue = System.Convert.ToInt32(Outputs["ReturnValue"]);
            }
            catch { }

            try
            {
                ReturnDescription = System.Convert.ToString(Outputs["ReturnDescription"]);
            }
            catch { }

            return CallResult;
        }

        public static int P_QuestionsDelete(SqlConnection conn, long SiteID, long QuestionID, ref int ReturnValue, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_QuestionsDelete(conn, ref ds, SiteID, QuestionID, ref ReturnValue, ref ReturnDescription);
        }

        public static int P_QuestionsDelete(SqlConnection conn, ref DataSet ds, long SiteID, long QuestionID, ref int ReturnValue, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(conn, "P_QuestionsDelete", ref ds, ref Outputs,
                new MSSQL.Parameter("SiteID", SqlDbType.BigInt, 0, ParameterDirection.Input, SiteID),
                new MSSQL.Parameter("QuestionID", SqlDbType.BigInt, 0, ParameterDirection.Input, QuestionID),
                new MSSQL.Parameter("ReturnValue", SqlDbType.Int, 4, ParameterDirection.Output, ReturnValue),
                new MSSQL.Parameter("ReturnDescription", SqlDbType.VarChar, 100, ParameterDirection.Output, ReturnDescription)
                );

            try
            {
                ReturnValue = System.Convert.ToInt32(Outputs["ReturnValue"]);
            }
            catch { }

            try
            {
                ReturnDescription = System.Convert.ToString(Outputs["ReturnDescription"]);
            }
            catch { }

            return CallResult;
        }

        public static int P_QuestionsHandling(SqlConnection conn, long SiteID, long QuestionID, long HandleOperatorID, ref int ReturnValue, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_QuestionsHandling(conn, ref ds, SiteID, QuestionID, HandleOperatorID, ref ReturnValue, ref ReturnDescription);
        }

        public static int P_QuestionsHandling(SqlConnection conn, ref DataSet ds, long SiteID, long QuestionID, long HandleOperatorID, ref int ReturnValue, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(conn, "P_QuestionsHandling", ref ds, ref Outputs,
                new MSSQL.Parameter("SiteID", SqlDbType.BigInt, 0, ParameterDirection.Input, SiteID),
                new MSSQL.Parameter("QuestionID", SqlDbType.BigInt, 0, ParameterDirection.Input, QuestionID),
                new MSSQL.Parameter("HandleOperatorID", SqlDbType.BigInt, 0, ParameterDirection.Input, HandleOperatorID),
                new MSSQL.Parameter("ReturnValue", SqlDbType.Int, 4, ParameterDirection.Output, ReturnValue),
                new MSSQL.Parameter("ReturnDescription", SqlDbType.VarChar, 100, ParameterDirection.Output, ReturnDescription)
                );

            try
            {
                ReturnValue = System.Convert.ToInt32(Outputs["ReturnValue"]);
            }
            catch { }

            try
            {
                ReturnDescription = System.Convert.ToString(Outputs["ReturnDescription"]);
            }
            catch { }

            return CallResult;
        }

        public static int P_RebonusShares(SqlConnection conn, ref int ReturnValue, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_RebonusShares(conn, ref ds, ref ReturnValue, ref ReturnDescription);
        }

        public static int P_RebonusShares(SqlConnection conn, ref DataSet ds, ref int ReturnValue, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(conn, "P_RebonusShares", ref ds, ref Outputs,
                new MSSQL.Parameter("ReturnValue", SqlDbType.Int, 4, ParameterDirection.Output, ReturnValue),
                new MSSQL.Parameter("ReturnDescription", SqlDbType.VarChar, 100, ParameterDirection.Output, ReturnDescription)
                );

            try
            {
                ReturnValue = System.Convert.ToInt32(Outputs["ReturnValue"]);
            }
            catch { }

            try
            {
                ReturnDescription = System.Convert.ToString(Outputs["ReturnDescription"]);
            }
            catch { }

            return CallResult;
        }

        public static int P_SchemeAssure(SqlConnection conn, ref int ReturnValue, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_SchemeAssure(conn, ref ds, ref ReturnValue, ref ReturnDescription);
        }

        public static int P_SchemeAssure(SqlConnection conn, ref DataSet ds, ref int ReturnValue, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(conn, "P_SchemeAssure", ref ds, ref Outputs,
                new MSSQL.Parameter("ReturnValue", SqlDbType.Int, 4, ParameterDirection.Output, ReturnValue),
                new MSSQL.Parameter("ReturnDescription", SqlDbType.VarChar, 100, ParameterDirection.Output, ReturnDescription)
                );

            try
            {
                ReturnValue = System.Convert.ToInt32(Outputs["ReturnValue"]);
            }
            catch { }

            try
            {
                ReturnDescription = System.Convert.ToString(Outputs["ReturnDescription"]);
            }
            catch { }

            return CallResult;
        }

        public static int P_SchemeCalculatedBonus(SqlConnection conn, ref bool ReturnBool, ref int ReturnValue, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_SchemeCalculatedBonus(conn, ref ds, ref ReturnBool, ref ReturnValue, ref ReturnDescription);
        }

        public static int P_SchemeCalculatedBonus(SqlConnection conn, ref DataSet ds, ref bool ReturnBool, ref int ReturnValue, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(conn, "P_SchemeCalculatedBonus", ref ds, ref Outputs,
                new MSSQL.Parameter("ReturnBool", SqlDbType.Bit, 1, ParameterDirection.Output, ReturnBool),
                new MSSQL.Parameter("ReturnValue", SqlDbType.Int, 4, ParameterDirection.Output, ReturnValue),
                new MSSQL.Parameter("ReturnDescription", SqlDbType.VarChar, 100, ParameterDirection.Output, ReturnDescription)
                );

            try
            {
                ReturnBool = System.Convert.ToBoolean(Outputs["ReturnBool"]);
            }
            catch { }

            try
            {
                ReturnValue = System.Convert.ToInt32(Outputs["ReturnValue"]);
            }
            catch { }

            try
            {
                ReturnDescription = System.Convert.ToString(Outputs["ReturnDescription"]);
            }
            catch { }

            return CallResult;
        }

        public static int P_SchemeCalculatedScore(SqlConnection conn, long UserID, double DetailMoney, long SchemeID)
        {
            DataSet ds = null;

            return P_SchemeCalculatedScore(conn, ref ds, UserID, DetailMoney, SchemeID);
        }

        public static int P_SchemeCalculatedScore(SqlConnection conn, ref DataSet ds, long UserID, double DetailMoney, long SchemeID)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(conn, "P_SchemeCalculatedScore", ref ds, ref Outputs,
                new MSSQL.Parameter("UserID", SqlDbType.BigInt, 0, ParameterDirection.Input, UserID),
                new MSSQL.Parameter("DetailMoney", SqlDbType.Money, 0, ParameterDirection.Input, DetailMoney),
                new MSSQL.Parameter("SchemeID", SqlDbType.BigInt, 0, ParameterDirection.Input, SchemeID)
                );

            return CallResult;
        }

        public static int P_SchemePost(SqlConnection conn, int posterid, string poster, int fid, string title, string ip, string message, long schemeid, int typeid, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_SchemePost(conn, ref ds, posterid, poster, fid, title, ip, message, schemeid, typeid, ref ReturnDescription);
        }

        public static int P_SchemePost(SqlConnection conn, ref DataSet ds, int posterid, string poster, int fid, string title, string ip, string message, long schemeid, int typeid, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(conn, "P_SchemePost", ref ds, ref Outputs,
                new MSSQL.Parameter("posterid", SqlDbType.Int, 0, ParameterDirection.Input, posterid),
                new MSSQL.Parameter("poster", SqlDbType.VarChar, 0, ParameterDirection.Input, poster),
                new MSSQL.Parameter("fid", SqlDbType.Int, 0, ParameterDirection.Input, fid),
                new MSSQL.Parameter("title", SqlDbType.VarChar, 0, ParameterDirection.Input, title),
                new MSSQL.Parameter("ip", SqlDbType.VarChar, 0, ParameterDirection.Input, ip),
                new MSSQL.Parameter("message", SqlDbType.VarChar, 0, ParameterDirection.Input, message),
                new MSSQL.Parameter("schemeid", SqlDbType.BigInt, 0, ParameterDirection.Input, schemeid),
                new MSSQL.Parameter("typeid", SqlDbType.Int, 0, ParameterDirection.Input, typeid),
                new MSSQL.Parameter("ReturnDescription", SqlDbType.VarChar, 100, ParameterDirection.Output, ReturnDescription)
                );

            try
            {
                ReturnDescription = System.Convert.ToString(Outputs["ReturnDescription"]);
            }
            catch { }

            return CallResult;
        }

        public static int P_SchemePrintOut(SqlConnection conn, long SiteID, long SchemeID, long BuyOperatorID, short PrintOutType, string Identifiers, bool isOt, ref int ReturnValue, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_SchemePrintOut(conn, ref ds, SiteID, SchemeID, BuyOperatorID, PrintOutType, Identifiers, isOt, ref ReturnValue, ref ReturnDescription);
        }

        public static int P_SchemePrintOut(SqlConnection conn, ref DataSet ds, long SiteID, long SchemeID, long BuyOperatorID, short PrintOutType, string Identifiers, bool isOt, ref int ReturnValue, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(conn, "P_SchemePrintOut", ref ds, ref Outputs,
                new MSSQL.Parameter("SiteID", SqlDbType.BigInt, 0, ParameterDirection.Input, SiteID),
                new MSSQL.Parameter("SchemeID", SqlDbType.BigInt, 0, ParameterDirection.Input, SchemeID),
                new MSSQL.Parameter("BuyOperatorID", SqlDbType.BigInt, 0, ParameterDirection.Input, BuyOperatorID),
                new MSSQL.Parameter("PrintOutType", SqlDbType.SmallInt, 0, ParameterDirection.Input, PrintOutType),
                new MSSQL.Parameter("Identifiers", SqlDbType.VarChar, 0, ParameterDirection.Input, Identifiers),
                new MSSQL.Parameter("isOt", SqlDbType.Bit, 0, ParameterDirection.Input, isOt),
                new MSSQL.Parameter("ReturnValue", SqlDbType.Int, 4, ParameterDirection.Output, ReturnValue),
                new MSSQL.Parameter("ReturnDescription", SqlDbType.VarChar, 100, ParameterDirection.Output, ReturnDescription)
                );

            try
            {
                ReturnValue = System.Convert.ToInt32(Outputs["ReturnValue"]);
            }
            catch { }

            try
            {
                ReturnDescription = System.Convert.ToString(Outputs["ReturnDescription"]);
            }
            catch { }

            return CallResult;
        }

        public static int P_SchemesSendToCenterAdd(SqlConnection conn, long SchemeID, int PlayTypeID, string TicketXML, ref int ReturnValue, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_SchemesSendToCenterAdd(conn, ref ds, SchemeID, PlayTypeID, TicketXML, ref ReturnValue, ref ReturnDescription);
        }

        public static int P_SchemesSendToCenterAdd(SqlConnection conn, ref DataSet ds, long SchemeID, int PlayTypeID, string TicketXML, ref int ReturnValue, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(conn, "P_SchemesSendToCenterAdd", ref ds, ref Outputs,
                new MSSQL.Parameter("SchemeID", SqlDbType.BigInt, 0, ParameterDirection.Input, SchemeID),
                new MSSQL.Parameter("PlayTypeID", SqlDbType.Int, 0, ParameterDirection.Input, PlayTypeID),
                new MSSQL.Parameter("TicketXML", SqlDbType.NText, 0, ParameterDirection.Input, TicketXML),
                new MSSQL.Parameter("ReturnValue", SqlDbType.Int, 4, ParameterDirection.Output, ReturnValue),
                new MSSQL.Parameter("ReturnDescription", SqlDbType.VarChar, 100, ParameterDirection.Output, ReturnDescription)
                );

            try
            {
                ReturnValue = System.Convert.ToInt32(Outputs["ReturnValue"]);
            }
            catch { }

            try
            {
                ReturnDescription = System.Convert.ToString(Outputs["ReturnDescription"]);
            }
            catch { }

            return CallResult;
        }

        public static int P_SchemesSendToCenterAdd_Single(SqlConnection conn, long SchemeID, int PlayTypeID, double Money, int Multiple, string Ticket, bool isFirstWrite, ref long ID, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_SchemesSendToCenterAdd_Single(conn, ref ds, SchemeID, PlayTypeID, Money, Multiple, Ticket, isFirstWrite, ref ID, ref ReturnDescription);
        }

        public static int P_SchemesSendToCenterAdd_Single(SqlConnection conn, ref DataSet ds, long SchemeID, int PlayTypeID, double Money, int Multiple, string Ticket, bool isFirstWrite, ref long ID, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(conn, "P_SchemesSendToCenterAdd_Single", ref ds, ref Outputs,
                new MSSQL.Parameter("SchemeID", SqlDbType.BigInt, 0, ParameterDirection.Input, SchemeID),
                new MSSQL.Parameter("PlayTypeID", SqlDbType.Int, 0, ParameterDirection.Input, PlayTypeID),
                new MSSQL.Parameter("Money", SqlDbType.Money, 0, ParameterDirection.Input, Money),
                new MSSQL.Parameter("Multiple", SqlDbType.Int, 0, ParameterDirection.Input, Multiple),
                new MSSQL.Parameter("Ticket", SqlDbType.VarChar, 0, ParameterDirection.Input, Ticket),
                new MSSQL.Parameter("isFirstWrite", SqlDbType.Bit, 0, ParameterDirection.Input, isFirstWrite),
                new MSSQL.Parameter("ID", SqlDbType.BigInt, 8, ParameterDirection.Output, ID),
                new MSSQL.Parameter("ReturnDescription", SqlDbType.VarChar, 100, ParameterDirection.Output, ReturnDescription)
                );

            try
            {
                ID = System.Convert.ToInt64(Outputs["ID"]);
            }
            catch { }

            try
            {
                ReturnDescription = System.Convert.ToString(Outputs["ReturnDescription"]);
            }
            catch { }

            return CallResult;
        }

        public static int P_SchemesSendToCenterHandle(SqlConnection conn, string Identifiers, DateTime DealTime, bool IsSuccess, string Status, ref int ReturnValue, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_SchemesSendToCenterHandle(conn, ref ds, Identifiers, DealTime, IsSuccess, Status, ref ReturnValue, ref ReturnDescription);
        }

        public static int P_SchemesSendToCenterHandle(SqlConnection conn, ref DataSet ds, string Identifiers, DateTime DealTime, bool IsSuccess, string Status, ref int ReturnValue, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(conn, "P_SchemesSendToCenterHandle", ref ds, ref Outputs,
                new MSSQL.Parameter("Identifiers", SqlDbType.VarChar, 0, ParameterDirection.Input, Identifiers),
                new MSSQL.Parameter("DealTime", SqlDbType.DateTime, 0, ParameterDirection.Input, DealTime),
                new MSSQL.Parameter("IsSuccess", SqlDbType.Bit, 0, ParameterDirection.Input, IsSuccess),
                new MSSQL.Parameter("Status", SqlDbType.VarChar, 0, ParameterDirection.Input, Status),
                new MSSQL.Parameter("ReturnValue", SqlDbType.Int, 4, ParameterDirection.Output, ReturnValue),
                new MSSQL.Parameter("ReturnDescription", SqlDbType.VarChar, 100, ParameterDirection.Output, ReturnDescription)
                );

            try
            {
                ReturnValue = System.Convert.ToInt32(Outputs["ReturnValue"]);
            }
            catch { }

            try
            {
                ReturnDescription = System.Convert.ToString(Outputs["ReturnDescription"]);
            }
            catch { }

            return CallResult;
        }

        public static int P_SchemesSendToCenterHandleUniteAnte(SqlConnection conn, long SchemeID, DateTime DealTime, bool isOt, ref int ReturnValue, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_SchemesSendToCenterHandleUniteAnte(conn, ref ds, SchemeID, DealTime, isOt, ref ReturnValue, ref ReturnDescription);
        }

        public static int P_SchemesSendToCenterHandleUniteAnte(SqlConnection conn, ref DataSet ds, long SchemeID, DateTime DealTime, bool isOt, ref int ReturnValue, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(conn, "P_SchemesSendToCenterHandleUniteAnte", ref ds, ref Outputs,
                new MSSQL.Parameter("SchemeID", SqlDbType.BigInt, 0, ParameterDirection.Input, SchemeID),
                new MSSQL.Parameter("DealTime", SqlDbType.DateTime, 0, ParameterDirection.Input, DealTime),
                new MSSQL.Parameter("isOt", SqlDbType.Bit, 0, ParameterDirection.Input, isOt),
                new MSSQL.Parameter("ReturnValue", SqlDbType.Int, 4, ParameterDirection.Output, ReturnValue),
                new MSSQL.Parameter("ReturnDescription", SqlDbType.VarChar, 100, ParameterDirection.Output, ReturnDescription)
                );

            try
            {
                ReturnValue = System.Convert.ToInt32(Outputs["ReturnValue"]);
            }
            catch { }

            try
            {
                ReturnDescription = System.Convert.ToString(Outputs["ReturnDescription"]);
            }
            catch { }

            return CallResult;
        }

        public static int P_SchemeWinCalculatedScore(SqlConnection conn, long UserID, double WinMoneyNoWithTax, long SchemeID)
        {
            DataSet ds = null;

            return P_SchemeWinCalculatedScore(conn, ref ds, UserID, WinMoneyNoWithTax, SchemeID);
        }

        public static int P_SchemeWinCalculatedScore(SqlConnection conn, ref DataSet ds, long UserID, double WinMoneyNoWithTax, long SchemeID)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(conn, "P_SchemeWinCalculatedScore", ref ds, ref Outputs,
                new MSSQL.Parameter("UserID", SqlDbType.BigInt, 0, ParameterDirection.Input, UserID),
                new MSSQL.Parameter("WinMoneyNoWithTax", SqlDbType.Money, 0, ParameterDirection.Input, WinMoneyNoWithTax),
                new MSSQL.Parameter("SchemeID", SqlDbType.BigInt, 0, ParameterDirection.Input, SchemeID)
                );

            return CallResult;
        }

        public static int P_ScoreChange(SqlConnection conn, long UserID, long CommoditityID, ref int ReturnValue, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_ScoreChange(conn, ref ds, UserID, CommoditityID, ref ReturnValue, ref ReturnDescription);
        }

        public static int P_ScoreChange(SqlConnection conn, ref DataSet ds, long UserID, long CommoditityID, ref int ReturnValue, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(conn, "P_ScoreChange", ref ds, ref Outputs,
                new MSSQL.Parameter("UserID", SqlDbType.BigInt, 0, ParameterDirection.Input, UserID),
                new MSSQL.Parameter("CommoditityID", SqlDbType.BigInt, 0, ParameterDirection.Input, CommoditityID),
                new MSSQL.Parameter("ReturnValue", SqlDbType.Int, 4, ParameterDirection.Output, ReturnValue),
                new MSSQL.Parameter("ReturnDescription", SqlDbType.VarChar, 50, ParameterDirection.Output, ReturnDescription)
                );

            try
            {
                ReturnValue = System.Convert.ToInt32(Outputs["ReturnValue"]);
            }
            catch { }

            try
            {
                ReturnDescription = System.Convert.ToString(Outputs["ReturnDescription"]);
            }
            catch { }

            return CallResult;
        }

        public static int P_ScoringExchange(SqlConnection conn, long SiteID, long UserID, double Scoring, ref int ReturnValue, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_ScoringExchange(conn, ref ds, SiteID, UserID, Scoring, ref ReturnValue, ref ReturnDescription);
        }

        public static int P_ScoringExchange(SqlConnection conn, ref DataSet ds, long SiteID, long UserID, double Scoring, ref int ReturnValue, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(conn, "P_ScoringExchange", ref ds, ref Outputs,
                new MSSQL.Parameter("SiteID", SqlDbType.BigInt, 0, ParameterDirection.Input, SiteID),
                new MSSQL.Parameter("UserID", SqlDbType.BigInt, 0, ParameterDirection.Input, UserID),
                new MSSQL.Parameter("Scoring", SqlDbType.Float, 0, ParameterDirection.Input, Scoring),
                new MSSQL.Parameter("ReturnValue", SqlDbType.Int, 4, ParameterDirection.Output, ReturnValue),
                new MSSQL.Parameter("ReturnDescription", SqlDbType.VarChar, 100, ParameterDirection.Output, ReturnDescription)
                );

            try
            {
                ReturnValue = System.Convert.ToInt32(Outputs["ReturnValue"]);
            }
            catch { }

            try
            {
                ReturnDescription = System.Convert.ToString(Outputs["ReturnDescription"]);
            }
            catch { }

            return CallResult;
        }

        public static int P_SelectPaging(SqlConnection conn, string TableOrViewName, string FieldList, string OrderFieldList, string Condition, int PageSize, int PageIndex, ref long RowCount, ref int PageCount, ref int CurrentPageIndex, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_SelectPaging(conn, ref ds, TableOrViewName, FieldList, OrderFieldList, Condition, PageSize, PageIndex, ref RowCount, ref PageCount, ref CurrentPageIndex, ref ReturnDescription);
        }

        public static int P_SelectPaging(SqlConnection conn, ref DataSet ds, string TableOrViewName, string FieldList, string OrderFieldList, string Condition, int PageSize, int PageIndex, ref long RowCount, ref int PageCount, ref int CurrentPageIndex, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(conn, "P_SelectPaging", ref ds, ref Outputs,
                new MSSQL.Parameter("TableOrViewName", SqlDbType.VarChar, 0, ParameterDirection.Input, TableOrViewName),
                new MSSQL.Parameter("FieldList", SqlDbType.VarChar, 0, ParameterDirection.Input, FieldList),
                new MSSQL.Parameter("OrderFieldList", SqlDbType.VarChar, 0, ParameterDirection.Input, OrderFieldList),
                new MSSQL.Parameter("Condition", SqlDbType.VarChar, 0, ParameterDirection.Input, Condition),
                new MSSQL.Parameter("PageSize", SqlDbType.Int, 0, ParameterDirection.Input, PageSize),
                new MSSQL.Parameter("PageIndex", SqlDbType.Int, 0, ParameterDirection.Input, PageIndex),
                new MSSQL.Parameter("RowCount", SqlDbType.BigInt, 8, ParameterDirection.Output, RowCount),
                new MSSQL.Parameter("PageCount", SqlDbType.Int, 4, ParameterDirection.Output, PageCount),
                new MSSQL.Parameter("CurrentPageIndex", SqlDbType.Int, 4, ParameterDirection.Output, CurrentPageIndex),
                new MSSQL.Parameter("ReturnDescription", SqlDbType.VarChar, 100, ParameterDirection.Output, ReturnDescription)
                );

            try
            {
                RowCount = System.Convert.ToInt64(Outputs["RowCount"]);
            }
            catch { }

            try
            {
                PageCount = System.Convert.ToInt32(Outputs["PageCount"]);
            }
            catch { }

            try
            {
                CurrentPageIndex = System.Convert.ToInt32(Outputs["CurrentPageIndex"]);
            }
            catch { }

            try
            {
                ReturnDescription = System.Convert.ToString(Outputs["ReturnDescription"]);
            }
            catch { }

            return CallResult;
        }

        public static int P_SetFriendsWinInfo(SqlConnection conn, string SnsName, ref int ReturnValue, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_SetFriendsWinInfo(conn, ref ds, SnsName, ref ReturnValue, ref ReturnDescription);
        }

        public static int P_SetFriendsWinInfo(SqlConnection conn, ref DataSet ds, string SnsName, ref int ReturnValue, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(conn, "P_SetFriendsWinInfo", ref ds, ref Outputs,
                new MSSQL.Parameter("SnsName", SqlDbType.VarChar, 0, ParameterDirection.Input, SnsName),
                new MSSQL.Parameter("ReturnValue", SqlDbType.Int, 4, ParameterDirection.Output, ReturnValue),
                new MSSQL.Parameter("ReturnDescription", SqlDbType.VarChar, 100, ParameterDirection.Output, ReturnDescription)
                );

            try
            {
                ReturnValue = System.Convert.ToInt32(Outputs["ReturnValue"]);
            }
            catch { }

            try
            {
                ReturnDescription = System.Convert.ToString(Outputs["ReturnDescription"]);
            }
            catch { }

            return CallResult;
        }

        public static int P_SetMaxMultiple(SqlConnection conn, long IsuseID, int PlayTypeID, int MaxMultiple, ref int ReturnValue, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_SetMaxMultiple(conn, ref ds, IsuseID, PlayTypeID, MaxMultiple, ref ReturnValue, ref ReturnDescription);
        }

        public static int P_SetMaxMultiple(SqlConnection conn, ref DataSet ds, long IsuseID, int PlayTypeID, int MaxMultiple, ref int ReturnValue, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(conn, "P_SetMaxMultiple", ref ds, ref Outputs,
                new MSSQL.Parameter("IsuseID", SqlDbType.BigInt, 0, ParameterDirection.Input, IsuseID),
                new MSSQL.Parameter("PlayTypeID", SqlDbType.Int, 0, ParameterDirection.Input, PlayTypeID),
                new MSSQL.Parameter("MaxMultiple", SqlDbType.Int, 0, ParameterDirection.Input, MaxMultiple),
                new MSSQL.Parameter("ReturnValue", SqlDbType.Int, 4, ParameterDirection.Output, ReturnValue),
                new MSSQL.Parameter("ReturnDescription", SqlDbType.VarChar, 100, ParameterDirection.Output, ReturnDescription)
                );

            try
            {
                ReturnValue = System.Convert.ToInt32(Outputs["ReturnValue"]);
            }
            catch { }

            try
            {
                ReturnDescription = System.Convert.ToString(Outputs["ReturnDescription"]);
            }
            catch { }

            return CallResult;
        }

        public static int P_SetOptions(SqlConnection conn, string Key, string Value)
        {
            DataSet ds = null;

            return P_SetOptions(conn, ref ds, Key, Value);
        }

        public static int P_SetOptions(SqlConnection conn, ref DataSet ds, string Key, string Value)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(conn, "P_SetOptions", ref ds, ref Outputs,
                new MSSQL.Parameter("Key", SqlDbType.VarChar, 0, ParameterDirection.Input, Key),
                new MSSQL.Parameter("Value", SqlDbType.VarChar, 0, ParameterDirection.Input, Value)
                );

            return CallResult;
        }

        public static int P_SetSchemeOpenUsers(SqlConnection conn, long SiteID, long SchemeID, string UserList, ref int ReturnValue, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_SetSchemeOpenUsers(conn, ref ds, SiteID, SchemeID, UserList, ref ReturnValue, ref ReturnDescription);
        }

        public static int P_SetSchemeOpenUsers(SqlConnection conn, ref DataSet ds, long SiteID, long SchemeID, string UserList, ref int ReturnValue, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(conn, "P_SetSchemeOpenUsers", ref ds, ref Outputs,
                new MSSQL.Parameter("SiteID", SqlDbType.BigInt, 0, ParameterDirection.Input, SiteID),
                new MSSQL.Parameter("SchemeID", SqlDbType.BigInt, 0, ParameterDirection.Input, SchemeID),
                new MSSQL.Parameter("UserList", SqlDbType.VarChar, 0, ParameterDirection.Input, UserList),
                new MSSQL.Parameter("ReturnValue", SqlDbType.Int, 4, ParameterDirection.Output, ReturnValue),
                new MSSQL.Parameter("ReturnDescription", SqlDbType.VarChar, 100, ParameterDirection.Output, ReturnDescription)
                );

            try
            {
                ReturnValue = System.Convert.ToInt32(Outputs["ReturnValue"]);
            }
            catch { }

            try
            {
                ReturnDescription = System.Convert.ToString(Outputs["ReturnDescription"]);
            }
            catch { }

            return CallResult;
        }

        public static int P_SetSiteNotificationTemplate(SqlConnection conn, long SiteID, short Manner, string NotificationType, string Value)
        {
            DataSet ds = null;

            return P_SetSiteNotificationTemplate(conn, ref ds, SiteID, Manner, NotificationType, Value);
        }

        public static int P_SetSiteNotificationTemplate(SqlConnection conn, ref DataSet ds, long SiteID, short Manner, string NotificationType, string Value)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(conn, "P_SetSiteNotificationTemplate", ref ds, ref Outputs,
                new MSSQL.Parameter("SiteID", SqlDbType.BigInt, 0, ParameterDirection.Input, SiteID),
                new MSSQL.Parameter("Manner", SqlDbType.SmallInt, 0, ParameterDirection.Input, Manner),
                new MSSQL.Parameter("NotificationType", SqlDbType.VarChar, 0, ParameterDirection.Input, NotificationType),
                new MSSQL.Parameter("Value", SqlDbType.VarChar, 0, ParameterDirection.Input, Value)
                );

            return CallResult;
        }

        public static int P_SetSiteONState(SqlConnection conn, long SiteID, bool ON)
        {
            DataSet ds = null;

            return P_SetSiteONState(conn, ref ds, SiteID, ON);
        }

        public static int P_SetSiteONState(SqlConnection conn, ref DataSet ds, long SiteID, bool ON)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(conn, "P_SetSiteONState", ref ds, ref Outputs,
                new MSSQL.Parameter("SiteID", SqlDbType.BigInt, 0, ParameterDirection.Input, SiteID),
                new MSSQL.Parameter("ON", SqlDbType.Bit, 0, ParameterDirection.Input, ON)
                );

            return CallResult;
        }

        public static int P_SetSiteSendNotificationTypes(SqlConnection conn, long SiteID, short Manner, string SendNotificationTypeList, ref int ReturnValue, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_SetSiteSendNotificationTypes(conn, ref ds, SiteID, Manner, SendNotificationTypeList, ref ReturnValue, ref ReturnDescription);
        }

        public static int P_SetSiteSendNotificationTypes(SqlConnection conn, ref DataSet ds, long SiteID, short Manner, string SendNotificationTypeList, ref int ReturnValue, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(conn, "P_SetSiteSendNotificationTypes", ref ds, ref Outputs,
                new MSSQL.Parameter("SiteID", SqlDbType.BigInt, 0, ParameterDirection.Input, SiteID),
                new MSSQL.Parameter("Manner", SqlDbType.SmallInt, 0, ParameterDirection.Input, Manner),
                new MSSQL.Parameter("SendNotificationTypeList", SqlDbType.VarChar, 0, ParameterDirection.Input, SendNotificationTypeList),
                new MSSQL.Parameter("ReturnValue", SqlDbType.Int, 4, ParameterDirection.Output, ReturnValue),
                new MSSQL.Parameter("ReturnDescription", SqlDbType.VarChar, 100, ParameterDirection.Output, ReturnDescription)
                );

            try
            {
                ReturnValue = System.Convert.ToInt32(Outputs["ReturnValue"]);
            }
            catch { }

            try
            {
                ReturnDescription = System.Convert.ToString(Outputs["ReturnDescription"]);
            }
            catch { }

            return CallResult;
        }

        public static int P_SetSiteUrls(SqlConnection conn, long SiteID, string Urls, ref int ReturnValue, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_SetSiteUrls(conn, ref ds, SiteID, Urls, ref ReturnValue, ref ReturnDescription);
        }

        public static int P_SetSiteUrls(SqlConnection conn, ref DataSet ds, long SiteID, string Urls, ref int ReturnValue, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(conn, "P_SetSiteUrls", ref ds, ref Outputs,
                new MSSQL.Parameter("SiteID", SqlDbType.BigInt, 0, ParameterDirection.Input, SiteID),
                new MSSQL.Parameter("Urls", SqlDbType.VarChar, 0, ParameterDirection.Input, Urls),
                new MSSQL.Parameter("ReturnValue", SqlDbType.Int, 4, ParameterDirection.Output, ReturnValue),
                new MSSQL.Parameter("ReturnDescription", SqlDbType.VarChar, 100, ParameterDirection.Output, ReturnDescription)
                );

            try
            {
                ReturnValue = System.Convert.ToInt32(Outputs["ReturnValue"]);
            }
            catch { }

            try
            {
                ReturnDescription = System.Convert.ToString(Outputs["ReturnDescription"]);
            }
            catch { }

            return CallResult;
        }

        public static int P_SetUserAcceptNotificationTypes(SqlConnection conn, long SiteID, long UserID, short Manner, string AcceptNotificationTypeList, ref int ReturnValue, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_SetUserAcceptNotificationTypes(conn, ref ds, SiteID, UserID, Manner, AcceptNotificationTypeList, ref ReturnValue, ref ReturnDescription);
        }

        public static int P_SetUserAcceptNotificationTypes(SqlConnection conn, ref DataSet ds, long SiteID, long UserID, short Manner, string AcceptNotificationTypeList, ref int ReturnValue, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(conn, "P_SetUserAcceptNotificationTypes", ref ds, ref Outputs,
                new MSSQL.Parameter("SiteID", SqlDbType.BigInt, 0, ParameterDirection.Input, SiteID),
                new MSSQL.Parameter("UserID", SqlDbType.BigInt, 0, ParameterDirection.Input, UserID),
                new MSSQL.Parameter("Manner", SqlDbType.SmallInt, 0, ParameterDirection.Input, Manner),
                new MSSQL.Parameter("AcceptNotificationTypeList", SqlDbType.VarChar, 0, ParameterDirection.Input, AcceptNotificationTypeList),
                new MSSQL.Parameter("ReturnValue", SqlDbType.Int, 4, ParameterDirection.Output, ReturnValue),
                new MSSQL.Parameter("ReturnDescription", SqlDbType.VarChar, 100, ParameterDirection.Output, ReturnDescription)
                );

            try
            {
                ReturnValue = System.Convert.ToInt32(Outputs["ReturnValue"]);
            }
            catch { }

            try
            {
                ReturnDescription = System.Convert.ToString(Outputs["ReturnDescription"]);
            }
            catch { }

            return CallResult;
        }

        public static int P_SetUserCompetences(SqlConnection conn, long SiteID, long UserID, string CompetencesList, string GroupsList, ref int ReturnValue, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_SetUserCompetences(conn, ref ds, SiteID, UserID, CompetencesList, GroupsList, ref ReturnValue, ref ReturnDescription);
        }

        public static int P_SetUserCompetences(SqlConnection conn, ref DataSet ds, long SiteID, long UserID, string CompetencesList, string GroupsList, ref int ReturnValue, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(conn, "P_SetUserCompetences", ref ds, ref Outputs,
                new MSSQL.Parameter("SiteID", SqlDbType.BigInt, 0, ParameterDirection.Input, SiteID),
                new MSSQL.Parameter("UserID", SqlDbType.BigInt, 0, ParameterDirection.Input, UserID),
                new MSSQL.Parameter("CompetencesList", SqlDbType.VarChar, 0, ParameterDirection.Input, CompetencesList),
                new MSSQL.Parameter("GroupsList", SqlDbType.VarChar, 0, ParameterDirection.Input, GroupsList),
                new MSSQL.Parameter("ReturnValue", SqlDbType.Int, 4, ParameterDirection.Output, ReturnValue),
                new MSSQL.Parameter("ReturnDescription", SqlDbType.VarChar, 100, ParameterDirection.Output, ReturnDescription)
                );

            try
            {
                ReturnValue = System.Convert.ToInt32(Outputs["ReturnValue"]);
            }
            catch { }

            try
            {
                ReturnDescription = System.Convert.ToString(Outputs["ReturnDescription"]);
            }
            catch { }

            return CallResult;
        }

        public static int P_SiteAdd(SqlConnection conn, long SiteParentID, long OwnerUserID, string Name, string LogoUrl, string Company, string Address, string PostCode, string ResponsiblePerson, string ContactPerson, string Telephone, string Fax, string Mobile, string Email, string QQ, string ServiceTelephone, string ICPCert, short Level, bool ON, double BonusScale, int MaxSubSites, string UseLotteryListRestrictions, string UseLotteryList, string UseLotteryListQuickBuy, string Urls, ref long AdministratorID, ref long SiteID, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_SiteAdd(conn, ref ds, SiteParentID, OwnerUserID, Name, LogoUrl, Company, Address, PostCode, ResponsiblePerson, ContactPerson, Telephone, Fax, Mobile, Email, QQ, ServiceTelephone, ICPCert, Level, ON, BonusScale, MaxSubSites, UseLotteryListRestrictions, UseLotteryList, UseLotteryListQuickBuy, Urls, ref AdministratorID, ref SiteID, ref ReturnDescription);
        }

        public static int P_SiteAdd(SqlConnection conn, ref DataSet ds, long SiteParentID, long OwnerUserID, string Name, string LogoUrl, string Company, string Address, string PostCode, string ResponsiblePerson, string ContactPerson, string Telephone, string Fax, string Mobile, string Email, string QQ, string ServiceTelephone, string ICPCert, short Level, bool ON, double BonusScale, int MaxSubSites, string UseLotteryListRestrictions, string UseLotteryList, string UseLotteryListQuickBuy, string Urls, ref long AdministratorID, ref long SiteID, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(conn, "P_SiteAdd", ref ds, ref Outputs,
                new MSSQL.Parameter("SiteParentID", SqlDbType.BigInt, 0, ParameterDirection.Input, SiteParentID),
                new MSSQL.Parameter("OwnerUserID", SqlDbType.BigInt, 0, ParameterDirection.Input, OwnerUserID),
                new MSSQL.Parameter("Name", SqlDbType.VarChar, 0, ParameterDirection.Input, Name),
                new MSSQL.Parameter("LogoUrl", SqlDbType.VarChar, 0, ParameterDirection.Input, LogoUrl),
                new MSSQL.Parameter("Company", SqlDbType.VarChar, 0, ParameterDirection.Input, Company),
                new MSSQL.Parameter("Address", SqlDbType.VarChar, 0, ParameterDirection.Input, Address),
                new MSSQL.Parameter("PostCode", SqlDbType.VarChar, 0, ParameterDirection.Input, PostCode),
                new MSSQL.Parameter("ResponsiblePerson", SqlDbType.VarChar, 0, ParameterDirection.Input, ResponsiblePerson),
                new MSSQL.Parameter("ContactPerson", SqlDbType.VarChar, 0, ParameterDirection.Input, ContactPerson),
                new MSSQL.Parameter("Telephone", SqlDbType.VarChar, 0, ParameterDirection.Input, Telephone),
                new MSSQL.Parameter("Fax", SqlDbType.VarChar, 0, ParameterDirection.Input, Fax),
                new MSSQL.Parameter("Mobile", SqlDbType.VarChar, 0, ParameterDirection.Input, Mobile),
                new MSSQL.Parameter("Email", SqlDbType.VarChar, 0, ParameterDirection.Input, Email),
                new MSSQL.Parameter("QQ", SqlDbType.VarChar, 0, ParameterDirection.Input, QQ),
                new MSSQL.Parameter("ServiceTelephone", SqlDbType.VarChar, 0, ParameterDirection.Input, ServiceTelephone),
                new MSSQL.Parameter("ICPCert", SqlDbType.VarChar, 0, ParameterDirection.Input, ICPCert),
                new MSSQL.Parameter("Level", SqlDbType.SmallInt, 0, ParameterDirection.Input, Level),
                new MSSQL.Parameter("ON", SqlDbType.Bit, 0, ParameterDirection.Input, ON),
                new MSSQL.Parameter("BonusScale", SqlDbType.Float, 0, ParameterDirection.Input, BonusScale),
                new MSSQL.Parameter("MaxSubSites", SqlDbType.Int, 0, ParameterDirection.Input, MaxSubSites),
                new MSSQL.Parameter("UseLotteryListRestrictions", SqlDbType.VarChar, 0, ParameterDirection.Input, UseLotteryListRestrictions),
                new MSSQL.Parameter("UseLotteryList", SqlDbType.VarChar, 0, ParameterDirection.Input, UseLotteryList),
                new MSSQL.Parameter("UseLotteryListQuickBuy", SqlDbType.VarChar, 0, ParameterDirection.Input, UseLotteryListQuickBuy),
                new MSSQL.Parameter("Urls", SqlDbType.VarChar, 0, ParameterDirection.Input, Urls),
                new MSSQL.Parameter("AdministratorID", SqlDbType.BigInt, 8, ParameterDirection.Output, AdministratorID),
                new MSSQL.Parameter("SiteID", SqlDbType.BigInt, 8, ParameterDirection.Output, SiteID),
                new MSSQL.Parameter("ReturnDescription", SqlDbType.VarChar, 100, ParameterDirection.Output, ReturnDescription)
                );

            try
            {
                AdministratorID = System.Convert.ToInt64(Outputs["AdministratorID"]);
            }
            catch { }

            try
            {
                SiteID = System.Convert.ToInt64(Outputs["SiteID"]);
            }
            catch { }

            try
            {
                ReturnDescription = System.Convert.ToString(Outputs["ReturnDescription"]);
            }
            catch { }

            return CallResult;
        }

        public static int P_SiteAfficheAdd(SqlConnection conn, long SiteID, DateTime DateTime, string Title, string Content, bool isShow, bool isCommend, ref long NewAfficheID, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_SiteAfficheAdd(conn, ref ds, SiteID, DateTime, Title, Content, isShow, isCommend, ref NewAfficheID, ref ReturnDescription);
        }

        public static int P_SiteAfficheAdd(SqlConnection conn, ref DataSet ds, long SiteID, DateTime DateTime, string Title, string Content, bool isShow, bool isCommend, ref long NewAfficheID, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(conn, "P_SiteAfficheAdd", ref ds, ref Outputs,
                new MSSQL.Parameter("SiteID", SqlDbType.BigInt, 0, ParameterDirection.Input, SiteID),
                new MSSQL.Parameter("DateTime", SqlDbType.DateTime, 0, ParameterDirection.Input, DateTime),
                new MSSQL.Parameter("Title", SqlDbType.VarChar, 0, ParameterDirection.Input, Title),
                new MSSQL.Parameter("Content", SqlDbType.VarChar, 0, ParameterDirection.Input, Content),
                new MSSQL.Parameter("isShow", SqlDbType.Bit, 0, ParameterDirection.Input, isShow),
                new MSSQL.Parameter("isCommend", SqlDbType.Bit, 0, ParameterDirection.Input, isCommend),
                new MSSQL.Parameter("NewAfficheID", SqlDbType.BigInt, 8, ParameterDirection.Output, NewAfficheID),
                new MSSQL.Parameter("ReturnDescription", SqlDbType.VarChar, 100, ParameterDirection.Output, ReturnDescription)
                );

            try
            {
                NewAfficheID = System.Convert.ToInt64(Outputs["NewAfficheID"]);
            }
            catch { }

            try
            {
                ReturnDescription = System.Convert.ToString(Outputs["ReturnDescription"]);
            }
            catch { }

            return CallResult;
        }

        public static int P_SiteAfficheDelete(SqlConnection conn, long SiteID, long ID, ref int ReturnValue, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_SiteAfficheDelete(conn, ref ds, SiteID, ID, ref ReturnValue, ref ReturnDescription);
        }

        public static int P_SiteAfficheDelete(SqlConnection conn, ref DataSet ds, long SiteID, long ID, ref int ReturnValue, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(conn, "P_SiteAfficheDelete", ref ds, ref Outputs,
                new MSSQL.Parameter("SiteID", SqlDbType.BigInt, 0, ParameterDirection.Input, SiteID),
                new MSSQL.Parameter("ID", SqlDbType.BigInt, 0, ParameterDirection.Input, ID),
                new MSSQL.Parameter("ReturnValue", SqlDbType.Int, 4, ParameterDirection.Output, ReturnValue),
                new MSSQL.Parameter("ReturnDescription", SqlDbType.VarChar, 100, ParameterDirection.Output, ReturnDescription)
                );

            try
            {
                ReturnValue = System.Convert.ToInt32(Outputs["ReturnValue"]);
            }
            catch { }

            try
            {
                ReturnDescription = System.Convert.ToString(Outputs["ReturnDescription"]);
            }
            catch { }

            return CallResult;
        }

        public static int P_SiteAfficheEdit(SqlConnection conn, long SiteID, long ID, DateTime DateTime, string Title, string Content, bool isShow, bool isCommend, ref int ReturnValue, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_SiteAfficheEdit(conn, ref ds, SiteID, ID, DateTime, Title, Content, isShow, isCommend, ref ReturnValue, ref ReturnDescription);
        }

        public static int P_SiteAfficheEdit(SqlConnection conn, ref DataSet ds, long SiteID, long ID, DateTime DateTime, string Title, string Content, bool isShow, bool isCommend, ref int ReturnValue, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(conn, "P_SiteAfficheEdit", ref ds, ref Outputs,
                new MSSQL.Parameter("SiteID", SqlDbType.BigInt, 0, ParameterDirection.Input, SiteID),
                new MSSQL.Parameter("ID", SqlDbType.BigInt, 0, ParameterDirection.Input, ID),
                new MSSQL.Parameter("DateTime", SqlDbType.DateTime, 0, ParameterDirection.Input, DateTime),
                new MSSQL.Parameter("Title", SqlDbType.VarChar, 0, ParameterDirection.Input, Title),
                new MSSQL.Parameter("Content", SqlDbType.VarChar, 0, ParameterDirection.Input, Content),
                new MSSQL.Parameter("isShow", SqlDbType.Bit, 0, ParameterDirection.Input, isShow),
                new MSSQL.Parameter("isCommend", SqlDbType.Bit, 0, ParameterDirection.Input, isCommend),
                new MSSQL.Parameter("ReturnValue", SqlDbType.Int, 4, ParameterDirection.Output, ReturnValue),
                new MSSQL.Parameter("ReturnDescription", SqlDbType.VarChar, 100, ParameterDirection.Output, ReturnDescription)
                );

            try
            {
                ReturnValue = System.Convert.ToInt32(Outputs["ReturnValue"]);
            }
            catch { }

            try
            {
                ReturnDescription = System.Convert.ToString(Outputs["ReturnDescription"]);
            }
            catch { }

            return CallResult;
        }

        public static int P_SiteEdit(SqlConnection conn, long SiteID, string Name, string LogoUrl, string Company, string Address, string PostCode, string ResponsiblePerson, string ContactPerson, string Telephone, string Fax, string Mobile, string Email, string QQ, string ServiceTelephone, string ICPCert, bool ON, double BonusScale, int MaxSubSites, string UseLotteryListRestrictions, string UseLotteryList, string UseLotteryListQuickBuy, string Urls, ref int ReturnValue, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_SiteEdit(conn, ref ds, SiteID, Name, LogoUrl, Company, Address, PostCode, ResponsiblePerson, ContactPerson, Telephone, Fax, Mobile, Email, QQ, ServiceTelephone, ICPCert, ON, BonusScale, MaxSubSites, UseLotteryListRestrictions, UseLotteryList, UseLotteryListQuickBuy, Urls, ref ReturnValue, ref ReturnDescription);
        }

        public static int P_SiteEdit(SqlConnection conn, ref DataSet ds, long SiteID, string Name, string LogoUrl, string Company, string Address, string PostCode, string ResponsiblePerson, string ContactPerson, string Telephone, string Fax, string Mobile, string Email, string QQ, string ServiceTelephone, string ICPCert, bool ON, double BonusScale, int MaxSubSites, string UseLotteryListRestrictions, string UseLotteryList, string UseLotteryListQuickBuy, string Urls, ref int ReturnValue, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(conn, "P_SiteEdit", ref ds, ref Outputs,
                new MSSQL.Parameter("SiteID", SqlDbType.BigInt, 0, ParameterDirection.Input, SiteID),
                new MSSQL.Parameter("Name", SqlDbType.VarChar, 0, ParameterDirection.Input, Name),
                new MSSQL.Parameter("LogoUrl", SqlDbType.VarChar, 0, ParameterDirection.Input, LogoUrl),
                new MSSQL.Parameter("Company", SqlDbType.VarChar, 0, ParameterDirection.Input, Company),
                new MSSQL.Parameter("Address", SqlDbType.VarChar, 0, ParameterDirection.Input, Address),
                new MSSQL.Parameter("PostCode", SqlDbType.VarChar, 0, ParameterDirection.Input, PostCode),
                new MSSQL.Parameter("ResponsiblePerson", SqlDbType.VarChar, 0, ParameterDirection.Input, ResponsiblePerson),
                new MSSQL.Parameter("ContactPerson", SqlDbType.VarChar, 0, ParameterDirection.Input, ContactPerson),
                new MSSQL.Parameter("Telephone", SqlDbType.VarChar, 0, ParameterDirection.Input, Telephone),
                new MSSQL.Parameter("Fax", SqlDbType.VarChar, 0, ParameterDirection.Input, Fax),
                new MSSQL.Parameter("Mobile", SqlDbType.VarChar, 0, ParameterDirection.Input, Mobile),
                new MSSQL.Parameter("Email", SqlDbType.VarChar, 0, ParameterDirection.Input, Email),
                new MSSQL.Parameter("QQ", SqlDbType.VarChar, 0, ParameterDirection.Input, QQ),
                new MSSQL.Parameter("ServiceTelephone", SqlDbType.VarChar, 0, ParameterDirection.Input, ServiceTelephone),
                new MSSQL.Parameter("ICPCert", SqlDbType.VarChar, 0, ParameterDirection.Input, ICPCert),
                new MSSQL.Parameter("ON", SqlDbType.Bit, 0, ParameterDirection.Input, ON),
                new MSSQL.Parameter("BonusScale", SqlDbType.Float, 0, ParameterDirection.Input, BonusScale),
                new MSSQL.Parameter("MaxSubSites", SqlDbType.Int, 0, ParameterDirection.Input, MaxSubSites),
                new MSSQL.Parameter("UseLotteryListRestrictions", SqlDbType.VarChar, 0, ParameterDirection.Input, UseLotteryListRestrictions),
                new MSSQL.Parameter("UseLotteryList", SqlDbType.VarChar, 0, ParameterDirection.Input, UseLotteryList),
                new MSSQL.Parameter("UseLotteryListQuickBuy", SqlDbType.VarChar, 0, ParameterDirection.Input, UseLotteryListQuickBuy),
                new MSSQL.Parameter("Urls", SqlDbType.VarChar, 0, ParameterDirection.Input, Urls),
                new MSSQL.Parameter("ReturnValue", SqlDbType.Int, 4, ParameterDirection.Output, ReturnValue),
                new MSSQL.Parameter("ReturnDescription", SqlDbType.VarChar, 100, ParameterDirection.Output, ReturnDescription)
                );

            try
            {
                ReturnValue = System.Convert.ToInt32(Outputs["ReturnValue"]);
            }
            catch { }

            try
            {
                ReturnDescription = System.Convert.ToString(Outputs["ReturnDescription"]);
            }
            catch { }

            return CallResult;
        }

        public static int P_SoftDownloadAdd(SqlConnection conn, long SiteID, int LotteryID, DateTime DateTime, string Title, string FileUrl, string ImageUrl, string Content, bool isHot, bool isCommend, bool isShow, int ReadCount, ref long NewSoftDownloadID, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_SoftDownloadAdd(conn, ref ds, SiteID, LotteryID, DateTime, Title, FileUrl, ImageUrl, Content, isHot, isCommend, isShow, ReadCount, ref NewSoftDownloadID, ref ReturnDescription);
        }

        public static int P_SoftDownloadAdd(SqlConnection conn, ref DataSet ds, long SiteID, int LotteryID, DateTime DateTime, string Title, string FileUrl, string ImageUrl, string Content, bool isHot, bool isCommend, bool isShow, int ReadCount, ref long NewSoftDownloadID, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(conn, "P_SoftDownloadAdd", ref ds, ref Outputs,
                new MSSQL.Parameter("SiteID", SqlDbType.BigInt, 0, ParameterDirection.Input, SiteID),
                new MSSQL.Parameter("LotteryID", SqlDbType.Int, 0, ParameterDirection.Input, LotteryID),
                new MSSQL.Parameter("DateTime", SqlDbType.DateTime, 0, ParameterDirection.Input, DateTime),
                new MSSQL.Parameter("Title", SqlDbType.VarChar, 0, ParameterDirection.Input, Title),
                new MSSQL.Parameter("FileUrl", SqlDbType.VarChar, 0, ParameterDirection.Input, FileUrl),
                new MSSQL.Parameter("ImageUrl", SqlDbType.VarChar, 0, ParameterDirection.Input, ImageUrl),
                new MSSQL.Parameter("Content", SqlDbType.VarChar, 0, ParameterDirection.Input, Content),
                new MSSQL.Parameter("isHot", SqlDbType.Bit, 0, ParameterDirection.Input, isHot),
                new MSSQL.Parameter("isCommend", SqlDbType.Bit, 0, ParameterDirection.Input, isCommend),
                new MSSQL.Parameter("isShow", SqlDbType.Bit, 0, ParameterDirection.Input, isShow),
                new MSSQL.Parameter("ReadCount", SqlDbType.Int, 0, ParameterDirection.Input, ReadCount),
                new MSSQL.Parameter("NewSoftDownloadID", SqlDbType.BigInt, 8, ParameterDirection.Output, NewSoftDownloadID),
                new MSSQL.Parameter("ReturnDescription", SqlDbType.VarChar, 100, ParameterDirection.Output, ReturnDescription)
                );

            try
            {
                NewSoftDownloadID = System.Convert.ToInt64(Outputs["NewSoftDownloadID"]);
            }
            catch { }

            try
            {
                ReturnDescription = System.Convert.ToString(Outputs["ReturnDescription"]);
            }
            catch { }

            return CallResult;
        }

        public static int P_SoftDownloadDelete(SqlConnection conn, long SiteID, long ID, ref int ReturnValue, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_SoftDownloadDelete(conn, ref ds, SiteID, ID, ref ReturnValue, ref ReturnDescription);
        }

        public static int P_SoftDownloadDelete(SqlConnection conn, ref DataSet ds, long SiteID, long ID, ref int ReturnValue, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(conn, "P_SoftDownloadDelete", ref ds, ref Outputs,
                new MSSQL.Parameter("SiteID", SqlDbType.BigInt, 0, ParameterDirection.Input, SiteID),
                new MSSQL.Parameter("ID", SqlDbType.BigInt, 0, ParameterDirection.Input, ID),
                new MSSQL.Parameter("ReturnValue", SqlDbType.Int, 4, ParameterDirection.Output, ReturnValue),
                new MSSQL.Parameter("ReturnDescription", SqlDbType.VarChar, 100, ParameterDirection.Output, ReturnDescription)
                );

            try
            {
                ReturnValue = System.Convert.ToInt32(Outputs["ReturnValue"]);
            }
            catch { }

            try
            {
                ReturnDescription = System.Convert.ToString(Outputs["ReturnDescription"]);
            }
            catch { }

            return CallResult;
        }

        public static int P_SoftDownloadEdit(SqlConnection conn, long SiteID, long ID, int LotteryID, DateTime DateTime, string Title, string FileUrl, string ImageUrl, string Content, bool isHot, bool isCommend, bool isShow, int ReadCount, ref int ReturnValue, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_SoftDownloadEdit(conn, ref ds, SiteID, ID, LotteryID, DateTime, Title, FileUrl, ImageUrl, Content, isHot, isCommend, isShow, ReadCount, ref ReturnValue, ref ReturnDescription);
        }

        public static int P_SoftDownloadEdit(SqlConnection conn, ref DataSet ds, long SiteID, long ID, int LotteryID, DateTime DateTime, string Title, string FileUrl, string ImageUrl, string Content, bool isHot, bool isCommend, bool isShow, int ReadCount, ref int ReturnValue, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(conn, "P_SoftDownloadEdit", ref ds, ref Outputs,
                new MSSQL.Parameter("SiteID", SqlDbType.BigInt, 0, ParameterDirection.Input, SiteID),
                new MSSQL.Parameter("ID", SqlDbType.BigInt, 0, ParameterDirection.Input, ID),
                new MSSQL.Parameter("LotteryID", SqlDbType.Int, 0, ParameterDirection.Input, LotteryID),
                new MSSQL.Parameter("DateTime", SqlDbType.DateTime, 0, ParameterDirection.Input, DateTime),
                new MSSQL.Parameter("Title", SqlDbType.VarChar, 0, ParameterDirection.Input, Title),
                new MSSQL.Parameter("FileUrl", SqlDbType.VarChar, 0, ParameterDirection.Input, FileUrl),
                new MSSQL.Parameter("ImageUrl", SqlDbType.VarChar, 0, ParameterDirection.Input, ImageUrl),
                new MSSQL.Parameter("Content", SqlDbType.VarChar, 0, ParameterDirection.Input, Content),
                new MSSQL.Parameter("isHot", SqlDbType.Bit, 0, ParameterDirection.Input, isHot),
                new MSSQL.Parameter("isCommend", SqlDbType.Bit, 0, ParameterDirection.Input, isCommend),
                new MSSQL.Parameter("isShow", SqlDbType.Bit, 0, ParameterDirection.Input, isShow),
                new MSSQL.Parameter("ReadCount", SqlDbType.Int, 0, ParameterDirection.Input, ReadCount),
                new MSSQL.Parameter("ReturnValue", SqlDbType.Int, 4, ParameterDirection.Output, ReturnValue),
                new MSSQL.Parameter("ReturnDescription", SqlDbType.VarChar, 100, ParameterDirection.Output, ReturnDescription)
                );

            try
            {
                ReturnValue = System.Convert.ToInt32(Outputs["ReturnValue"]);
            }
            catch { }

            try
            {
                ReturnDescription = System.Convert.ToString(Outputs["ReturnDescription"]);
            }
            catch { }

            return CallResult;
        }

        public static int P_SurrogateNotificationAdd(SqlConnection conn, long SiteID, string Title, string Content, bool isShow, ref long SurrogateNotificationID, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_SurrogateNotificationAdd(conn, ref ds, SiteID, Title, Content, isShow, ref SurrogateNotificationID, ref ReturnDescription);
        }

        public static int P_SurrogateNotificationAdd(SqlConnection conn, ref DataSet ds, long SiteID, string Title, string Content, bool isShow, ref long SurrogateNotificationID, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(conn, "P_SurrogateNotificationAdd", ref ds, ref Outputs,
                new MSSQL.Parameter("SiteID", SqlDbType.BigInt, 0, ParameterDirection.Input, SiteID),
                new MSSQL.Parameter("Title", SqlDbType.VarChar, 0, ParameterDirection.Input, Title),
                new MSSQL.Parameter("Content", SqlDbType.VarChar, 0, ParameterDirection.Input, Content),
                new MSSQL.Parameter("isShow", SqlDbType.Bit, 0, ParameterDirection.Input, isShow),
                new MSSQL.Parameter("SurrogateNotificationID", SqlDbType.BigInt, 8, ParameterDirection.Output, SurrogateNotificationID),
                new MSSQL.Parameter("ReturnDescription", SqlDbType.VarChar, 100, ParameterDirection.Output, ReturnDescription)
                );

            try
            {
                SurrogateNotificationID = System.Convert.ToInt64(Outputs["SurrogateNotificationID"]);
            }
            catch { }

            try
            {
                ReturnDescription = System.Convert.ToString(Outputs["ReturnDescription"]);
            }
            catch { }

            return CallResult;
        }

        public static int P_SurrogateNotificationDelete(SqlConnection conn, long SiteID, long SurrogateNotificationID, ref int ReturnValue, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_SurrogateNotificationDelete(conn, ref ds, SiteID, SurrogateNotificationID, ref ReturnValue, ref ReturnDescription);
        }

        public static int P_SurrogateNotificationDelete(SqlConnection conn, ref DataSet ds, long SiteID, long SurrogateNotificationID, ref int ReturnValue, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(conn, "P_SurrogateNotificationDelete", ref ds, ref Outputs,
                new MSSQL.Parameter("SiteID", SqlDbType.BigInt, 0, ParameterDirection.Input, SiteID),
                new MSSQL.Parameter("SurrogateNotificationID", SqlDbType.BigInt, 0, ParameterDirection.Input, SurrogateNotificationID),
                new MSSQL.Parameter("ReturnValue", SqlDbType.Int, 4, ParameterDirection.Output, ReturnValue),
                new MSSQL.Parameter("ReturnDescription", SqlDbType.VarChar, 100, ParameterDirection.Output, ReturnDescription)
                );

            try
            {
                ReturnValue = System.Convert.ToInt32(Outputs["ReturnValue"]);
            }
            catch { }

            try
            {
                ReturnDescription = System.Convert.ToString(Outputs["ReturnDescription"]);
            }
            catch { }

            return CallResult;
        }

        public static int P_SurrogateNotificationEdit(SqlConnection conn, long SiteID, long SurrogateNotificationID, string Title, string Content, bool isShow, ref int ReturnValue, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_SurrogateNotificationEdit(conn, ref ds, SiteID, SurrogateNotificationID, Title, Content, isShow, ref ReturnValue, ref ReturnDescription);
        }

        public static int P_SurrogateNotificationEdit(SqlConnection conn, ref DataSet ds, long SiteID, long SurrogateNotificationID, string Title, string Content, bool isShow, ref int ReturnValue, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(conn, "P_SurrogateNotificationEdit", ref ds, ref Outputs,
                new MSSQL.Parameter("SiteID", SqlDbType.BigInt, 0, ParameterDirection.Input, SiteID),
                new MSSQL.Parameter("SurrogateNotificationID", SqlDbType.BigInt, 0, ParameterDirection.Input, SurrogateNotificationID),
                new MSSQL.Parameter("Title", SqlDbType.VarChar, 0, ParameterDirection.Input, Title),
                new MSSQL.Parameter("Content", SqlDbType.VarChar, 0, ParameterDirection.Input, Content),
                new MSSQL.Parameter("isShow", SqlDbType.Bit, 0, ParameterDirection.Input, isShow),
                new MSSQL.Parameter("ReturnValue", SqlDbType.Int, 4, ParameterDirection.Output, ReturnValue),
                new MSSQL.Parameter("ReturnDescription", SqlDbType.VarChar, 100, ParameterDirection.Output, ReturnDescription)
                );

            try
            {
                ReturnValue = System.Convert.ToInt32(Outputs["ReturnValue"]);
            }
            catch { }

            try
            {
                ReturnDescription = System.Convert.ToString(Outputs["ReturnDescription"]);
            }
            catch { }

            return CallResult;
        }

        public static int P_SurrogateTry(SqlConnection conn, long SiteID, long UserID, string Content, string Name, string LogoUrl, string Company, string Address, string PostCode, string ResponsiblePerson, string ContactPerson, string Telephone, string Fax, string Mobile, string Email, string QQ, string ServiceTelephone, string UseLotteryList, string Urls, ref int ReturnValue, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_SurrogateTry(conn, ref ds, SiteID, UserID, Content, Name, LogoUrl, Company, Address, PostCode, ResponsiblePerson, ContactPerson, Telephone, Fax, Mobile, Email, QQ, ServiceTelephone, UseLotteryList, Urls, ref ReturnValue, ref ReturnDescription);
        }

        public static int P_SurrogateTry(SqlConnection conn, ref DataSet ds, long SiteID, long UserID, string Content, string Name, string LogoUrl, string Company, string Address, string PostCode, string ResponsiblePerson, string ContactPerson, string Telephone, string Fax, string Mobile, string Email, string QQ, string ServiceTelephone, string UseLotteryList, string Urls, ref int ReturnValue, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(conn, "P_SurrogateTry", ref ds, ref Outputs,
                new MSSQL.Parameter("SiteID", SqlDbType.BigInt, 0, ParameterDirection.Input, SiteID),
                new MSSQL.Parameter("UserID", SqlDbType.BigInt, 0, ParameterDirection.Input, UserID),
                new MSSQL.Parameter("Content", SqlDbType.VarChar, 0, ParameterDirection.Input, Content),
                new MSSQL.Parameter("Name", SqlDbType.VarChar, 0, ParameterDirection.Input, Name),
                new MSSQL.Parameter("LogoUrl", SqlDbType.VarChar, 0, ParameterDirection.Input, LogoUrl),
                new MSSQL.Parameter("Company", SqlDbType.VarChar, 0, ParameterDirection.Input, Company),
                new MSSQL.Parameter("Address", SqlDbType.VarChar, 0, ParameterDirection.Input, Address),
                new MSSQL.Parameter("PostCode", SqlDbType.VarChar, 0, ParameterDirection.Input, PostCode),
                new MSSQL.Parameter("ResponsiblePerson", SqlDbType.VarChar, 0, ParameterDirection.Input, ResponsiblePerson),
                new MSSQL.Parameter("ContactPerson", SqlDbType.VarChar, 0, ParameterDirection.Input, ContactPerson),
                new MSSQL.Parameter("Telephone", SqlDbType.VarChar, 0, ParameterDirection.Input, Telephone),
                new MSSQL.Parameter("Fax", SqlDbType.VarChar, 0, ParameterDirection.Input, Fax),
                new MSSQL.Parameter("Mobile", SqlDbType.VarChar, 0, ParameterDirection.Input, Mobile),
                new MSSQL.Parameter("Email", SqlDbType.VarChar, 0, ParameterDirection.Input, Email),
                new MSSQL.Parameter("QQ", SqlDbType.VarChar, 0, ParameterDirection.Input, QQ),
                new MSSQL.Parameter("ServiceTelephone", SqlDbType.VarChar, 0, ParameterDirection.Input, ServiceTelephone),
                new MSSQL.Parameter("UseLotteryList", SqlDbType.VarChar, 0, ParameterDirection.Input, UseLotteryList),
                new MSSQL.Parameter("Urls", SqlDbType.VarChar, 0, ParameterDirection.Input, Urls),
                new MSSQL.Parameter("ReturnValue", SqlDbType.Int, 4, ParameterDirection.Output, ReturnValue),
                new MSSQL.Parameter("ReturnDescription", SqlDbType.VarChar, 100, ParameterDirection.Output, ReturnDescription)
                );

            try
            {
                ReturnValue = System.Convert.ToInt32(Outputs["ReturnValue"]);
            }
            catch { }

            try
            {
                ReturnDescription = System.Convert.ToString(Outputs["ReturnDescription"]);
            }
            catch { }

            return CallResult;
        }

        public static int P_SurrogateTryHandle(SqlConnection conn, long SiteID, long TryID, short HandleResult, ref int ReturnValue, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_SurrogateTryHandle(conn, ref ds, SiteID, TryID, HandleResult, ref ReturnValue, ref ReturnDescription);
        }

        public static int P_SurrogateTryHandle(SqlConnection conn, ref DataSet ds, long SiteID, long TryID, short HandleResult, ref int ReturnValue, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(conn, "P_SurrogateTryHandle", ref ds, ref Outputs,
                new MSSQL.Parameter("SiteID", SqlDbType.BigInt, 0, ParameterDirection.Input, SiteID),
                new MSSQL.Parameter("TryID", SqlDbType.BigInt, 0, ParameterDirection.Input, TryID),
                new MSSQL.Parameter("HandleResult", SqlDbType.SmallInt, 0, ParameterDirection.Input, HandleResult),
                new MSSQL.Parameter("ReturnValue", SqlDbType.Int, 4, ParameterDirection.Output, ReturnValue),
                new MSSQL.Parameter("ReturnDescription", SqlDbType.VarChar, 100, ParameterDirection.Output, ReturnDescription)
                );

            try
            {
                ReturnValue = System.Convert.ToInt32(Outputs["ReturnValue"]);
            }
            catch { }

            try
            {
                ReturnDescription = System.Convert.ToString(Outputs["ReturnDescription"]);
            }
            catch { }

            return CallResult;
        }

        public static int P_SystemEnd(SqlConnection conn, ref int ReturnValue, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_SystemEnd(conn, ref ds, ref ReturnValue, ref ReturnDescription);
        }

        public static int P_SystemEnd(SqlConnection conn, ref DataSet ds, ref int ReturnValue, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(conn, "P_SystemEnd", ref ds, ref Outputs,
                new MSSQL.Parameter("ReturnValue", SqlDbType.Int, 4, ParameterDirection.Output, ReturnValue),
                new MSSQL.Parameter("ReturnDescription", SqlDbType.VarChar, 100, ParameterDirection.Output, ReturnDescription)
                );

            try
            {
                ReturnValue = System.Convert.ToInt32(Outputs["ReturnValue"]);
            }
            catch { }

            try
            {
                ReturnDescription = System.Convert.ToString(Outputs["ReturnDescription"]);
            }
            catch { }

            return CallResult;
        }

        public static int P_SystemEndSchemePrintOut(SqlConnection conn, ref int ReturnValue, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_SystemEndSchemePrintOut(conn, ref ds, ref ReturnValue, ref ReturnDescription);
        }

        public static int P_SystemEndSchemePrintOut(SqlConnection conn, ref DataSet ds, ref int ReturnValue, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(conn, "P_SystemEndSchemePrintOut", ref ds, ref Outputs,
                new MSSQL.Parameter("ReturnValue", SqlDbType.Int, 4, ParameterDirection.Output, ReturnValue),
                new MSSQL.Parameter("ReturnDescription", SqlDbType.VarChar, 100, ParameterDirection.Output, ReturnDescription)
                );

            try
            {
                ReturnValue = System.Convert.ToInt32(Outputs["ReturnValue"]);
            }
            catch { }

            try
            {
                ReturnDescription = System.Convert.ToString(Outputs["ReturnDescription"]);
            }
            catch { }

            return CallResult;
        }

        public static int P_TrendChart_11YDJ_WINNUM(SqlConnection conn, DateTime DateTime, long LotteryID, ref int ReturnValue, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_TrendChart_11YDJ_WINNUM(conn, ref ds, DateTime, LotteryID, ref ReturnValue, ref ReturnDescription);
        }

        public static int P_TrendChart_11YDJ_WINNUM(SqlConnection conn, ref DataSet ds, DateTime DateTime, long LotteryID, ref int ReturnValue, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(conn, "P_TrendChart_11YDJ_WINNUM", ref ds, ref Outputs,
                new MSSQL.Parameter("DateTime", SqlDbType.DateTime, 0, ParameterDirection.Input, DateTime),
                new MSSQL.Parameter("LotteryID", SqlDbType.BigInt, 0, ParameterDirection.Input, LotteryID),
                new MSSQL.Parameter("ReturnValue", SqlDbType.Int, 4, ParameterDirection.Output, ReturnValue),
                new MSSQL.Parameter("ReturnDescription", SqlDbType.VarChar, 100, ParameterDirection.Output, ReturnDescription)
                );

            try
            {
                ReturnValue = System.Convert.ToInt32(Outputs["ReturnValue"]);
            }
            catch { }

            try
            {
                ReturnDescription = System.Convert.ToString(Outputs["ReturnDescription"]);
            }
            catch { }

            return CallResult;
        }

        public static int P_TrendChart_15X5_CGXMB(SqlConnection conn)
        {
            DataSet ds = null;

            return P_TrendChart_15X5_CGXMB(conn, ref ds);
        }

        public static int P_TrendChart_15X5_CGXMB(SqlConnection conn, ref DataSet ds)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(conn, "P_TrendChart_15X5_CGXMB", ref ds, ref Outputs);

            return CallResult;
        }

        public static int P_TrendChart_15X5_HMFB(SqlConnection conn)
        {
            DataSet ds = null;

            return P_TrendChart_15X5_HMFB(conn, ref ds);
        }

        public static int P_TrendChart_15X5_HMFB(SqlConnection conn, ref DataSet ds)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(conn, "P_TrendChart_15X5_HMFB", ref ds, ref Outputs);

            return CallResult;
        }

        public static int P_TrendChart_22X5_HMFB(SqlConnection conn, int IsuseNum)
        {
            DataSet ds = null;

            return P_TrendChart_22X5_HMFB(conn, ref ds, IsuseNum);
        }

        public static int P_TrendChart_22X5_HMFB(SqlConnection conn, ref DataSet ds, int IsuseNum)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(conn, "P_TrendChart_22X5_HMFB", ref ds, ref Outputs,
                new MSSQL.Parameter("IsuseNum", SqlDbType.Int, 0, ParameterDirection.Input, IsuseNum)
                );

            return CallResult;
        }

        public static int P_TrendChart_22X5_HMLR(SqlConnection conn, int IsuseNum)
        {
            DataSet ds = null;

            return P_TrendChart_22X5_HMLR(conn, ref ds, IsuseNum);
        }

        public static int P_TrendChart_22X5_HMLR(SqlConnection conn, ref DataSet ds, int IsuseNum)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(conn, "P_TrendChart_22X5_HMLR", ref ds, ref Outputs,
                new MSSQL.Parameter("IsuseNum", SqlDbType.Int, 0, ParameterDirection.Input, IsuseNum)
                );

            return CallResult;
        }

        public static int P_TrendChart_22X5_HMLRjj(SqlConnection conn, int IsuseNum)
        {
            DataSet ds = null;

            return P_TrendChart_22X5_HMLRjj(conn, ref ds, IsuseNum);
        }

        public static int P_TrendChart_22X5_HMLRjj(SqlConnection conn, ref DataSet ds, int IsuseNum)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(conn, "P_TrendChart_22X5_HMLRjj", ref ds, ref Outputs,
                new MSSQL.Parameter("IsuseNum", SqlDbType.Int, 0, ParameterDirection.Input, IsuseNum)
                );

            return CallResult;
        }

        public static int P_TrendChart_22X5_HZ_Heng(SqlConnection conn)
        {
            DataSet ds = null;

            return P_TrendChart_22X5_HZ_Heng(conn, ref ds);
        }

        public static int P_TrendChart_22X5_HZ_Heng(SqlConnection conn, ref DataSet ds)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(conn, "P_TrendChart_22X5_HZ_Heng", ref ds, ref Outputs);

            return CallResult;
        }

        public static int P_TrendChart_22X5_HZzong(SqlConnection conn)
        {
            DataSet ds = null;

            return P_TrendChart_22X5_HZzong(conn, ref ds);
        }

        public static int P_TrendChart_22X5_HZzong(SqlConnection conn, ref DataSet ds)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(conn, "P_TrendChart_22X5_HZzong", ref ds, ref Outputs);

            return CallResult;
        }

        public static int P_TrendChart_22X5_JO(SqlConnection conn)
        {
            DataSet ds = null;

            return P_TrendChart_22X5_JO(conn, ref ds);
        }

        public static int P_TrendChart_22X5_JO(SqlConnection conn, ref DataSet ds)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(conn, "P_TrendChart_22X5_JO", ref ds, ref Outputs);

            return CallResult;
        }

        public static int P_TrendChart_22X5_LH(SqlConnection conn)
        {
            DataSet ds = null;

            return P_TrendChart_22X5_LH(conn, ref ds);
        }

        public static int P_TrendChart_22X5_LH(SqlConnection conn, ref DataSet ds)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(conn, "P_TrendChart_22X5_LH", ref ds, ref Outputs);

            return CallResult;
        }

        public static int P_TrendChart_22X5_WeiHaoCF(SqlConnection conn)
        {
            DataSet ds = null;

            return P_TrendChart_22X5_WeiHaoCF(conn, ref ds);
        }

        public static int P_TrendChart_22X5_WeiHaoCF(SqlConnection conn, ref DataSet ds)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(conn, "P_TrendChart_22X5_WeiHaoCF", ref ds, ref Outputs);

            return CallResult;
        }

        public static int P_TrendChart_22X5_WH(SqlConnection conn)
        {
            DataSet ds = null;

            return P_TrendChart_22X5_WH(conn, ref ds);
        }

        public static int P_TrendChart_22X5_WH(SqlConnection conn, ref DataSet ds)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(conn, "P_TrendChart_22X5_WH", ref ds, ref Outputs);

            return CallResult;
        }

        public static int P_TrendChart_22X5_YS(SqlConnection conn)
        {
            DataSet ds = null;

            return P_TrendChart_22X5_YS(conn, ref ds);
        }

        public static int P_TrendChart_22X5_YS(SqlConnection conn, ref DataSet ds)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(conn, "P_TrendChart_22X5_YS", ref ds, ref Outputs);

            return CallResult;
        }

        public static int P_TrendChart_3D_C3YS(SqlConnection conn)
        {
            DataSet ds = null;

            return P_TrendChart_3D_C3YS(conn, ref ds);
        }

        public static int P_TrendChart_3D_C3YS(SqlConnection conn, ref DataSet ds)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(conn, "P_TrendChart_3D_C3YS", ref ds, ref Outputs);

            return CallResult;
        }

        public static int P_TrendChart_3D_DZX(SqlConnection conn)
        {
            DataSet ds = null;

            return P_TrendChart_3D_DZX(conn, ref ds);
        }

        public static int P_TrendChart_3D_DZX(SqlConnection conn, ref DataSet ds)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(conn, "P_TrendChart_3D_DZX", ref ds, ref Outputs);

            return CallResult;
        }

        public static int P_TrendChart_3D_HZ(SqlConnection conn)
        {
            DataSet ds = null;

            return P_TrendChart_3D_HZ(conn, ref ds);
        }

        public static int P_TrendChart_3D_HZ(SqlConnection conn, ref DataSet ds)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(conn, "P_TrendChart_3D_HZ", ref ds, ref Outputs);

            return CallResult;
        }

        public static int P_TrendChart_3D_KD(SqlConnection conn)
        {
            DataSet ds = null;

            return P_TrendChart_3D_KD(conn, ref ds);
        }

        public static int P_TrendChart_3D_KD(SqlConnection conn, ref DataSet ds)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(conn, "P_TrendChart_3D_KD", ref ds, ref Outputs);

            return CallResult;
        }

        public static int P_TrendChart_3D_XTZST(SqlConnection conn)
        {
            DataSet ds = null;

            return P_TrendChart_3D_XTZST(conn, ref ds);
        }

        public static int P_TrendChart_3D_XTZST(SqlConnection conn, ref DataSet ds)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(conn, "P_TrendChart_3D_XTZST", ref ds, ref Outputs);

            return CallResult;
        }

        public static int P_TrendChart_3D_ZHFB(SqlConnection conn)
        {
            DataSet ds = null;

            return P_TrendChart_3D_ZHFB(conn, ref ds);
        }

        public static int P_TrendChart_3D_ZHFB(SqlConnection conn, ref DataSet ds)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(conn, "P_TrendChart_3D_ZHFB", ref ds, ref Outputs);

            return CallResult;
        }

        public static int P_TrendChart_3D_ZHXT(SqlConnection conn)
        {
            DataSet ds = null;

            return P_TrendChart_3D_ZHXT(conn, ref ds);
        }

        public static int P_TrendChart_3D_ZHXT(SqlConnection conn, ref DataSet ds)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(conn, "P_TrendChart_3D_ZHXT", ref ds, ref Outputs);

            return CallResult;
        }

        public static int P_TrendChart_4D_CGXMB(SqlConnection conn)
        {
            DataSet ds = null;

            return P_TrendChart_4D_CGXMB(conn, ref ds);
        }

        public static int P_TrendChart_4D_CGXMB(SqlConnection conn, ref DataSet ds)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(conn, "P_TrendChart_4D_CGXMB", ref ds, ref Outputs);

            return CallResult;
        }

        public static int P_TrendChart_4D_ZHFB(SqlConnection conn)
        {
            DataSet ds = null;

            return P_TrendChart_4D_ZHFB(conn, ref ds);
        }

        public static int P_TrendChart_4D_ZHFB(SqlConnection conn, ref DataSet ds)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(conn, "P_TrendChart_4D_ZHFB", ref ds, ref Outputs);

            return CallResult;
        }

        public static int P_TrendChart_7CL_HMFB(SqlConnection conn)
        {
            DataSet ds = null;

            return P_TrendChart_7CL_HMFB(conn, ref ds);
        }

        public static int P_TrendChart_7CL_HMFB(SqlConnection conn, ref DataSet ds)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(conn, "P_TrendChart_7CL_HMFB", ref ds, ref Outputs);

            return CallResult;
        }

        public static int P_TrendChart_7LC_CGXMB(SqlConnection conn)
        {
            DataSet ds = null;

            return P_TrendChart_7LC_CGXMB(conn, ref ds);
        }

        public static int P_TrendChart_7LC_CGXMB(SqlConnection conn, ref DataSet ds)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(conn, "P_TrendChart_7LC_CGXMB", ref ds, ref Outputs);

            return CallResult;
        }

        public static int P_TrendChart_7X_012(SqlConnection conn)
        {
            DataSet ds = null;

            return P_TrendChart_7X_012(conn, ref ds);
        }

        public static int P_TrendChart_7X_012(SqlConnection conn, ref DataSet ds)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(conn, "P_TrendChart_7X_012", ref ds, ref Outputs);

            return CallResult;
        }

        public static int P_TrendChart_7X_CF(SqlConnection conn)
        {
            DataSet ds = null;

            return P_TrendChart_7X_CF(conn, ref ds);
        }

        public static int P_TrendChart_7X_CF(SqlConnection conn, ref DataSet ds)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(conn, "P_TrendChart_7X_CF", ref ds, ref Outputs);

            return CallResult;
        }

        public static int P_TrendChart_7X_DX(SqlConnection conn)
        {
            DataSet ds = null;

            return P_TrendChart_7X_DX(conn, ref ds);
        }

        public static int P_TrendChart_7X_DX(SqlConnection conn, ref DataSet ds)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(conn, "P_TrendChart_7X_DX", ref ds, ref Outputs);

            return CallResult;
        }

        public static int P_TrendChart_7X_DZX(SqlConnection conn)
        {
            DataSet ds = null;

            return P_TrendChart_7X_DZX(conn, ref ds);
        }

        public static int P_TrendChart_7X_DZX(SqlConnection conn, ref DataSet ds)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(conn, "P_TrendChart_7X_DZX", ref ds, ref Outputs);

            return CallResult;
        }

        public static int P_TrendChart_7X_HMFB(SqlConnection conn)
        {
            DataSet ds = null;

            return P_TrendChart_7X_HMFB(conn, ref ds);
        }

        public static int P_TrendChart_7X_HMFB(SqlConnection conn, ref DataSet ds)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(conn, "P_TrendChart_7X_HMFB", ref ds, ref Outputs);

            return CallResult;
        }

        public static int P_TrendChart_7X_HZHeng(SqlConnection conn)
        {
            DataSet ds = null;

            return P_TrendChart_7X_HZHeng(conn, ref ds);
        }

        public static int P_TrendChart_7X_HZHeng(SqlConnection conn, ref DataSet ds)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(conn, "P_TrendChart_7X_HZHeng", ref ds, ref Outputs);

            return CallResult;
        }

        public static int P_TrendChart_7X_HZzhong(SqlConnection conn)
        {
            DataSet ds = null;

            return P_TrendChart_7X_HZzhong(conn, ref ds);
        }

        public static int P_TrendChart_7X_HZzhong(SqlConnection conn, ref DataSet ds)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(conn, "P_TrendChart_7X_HZzhong", ref ds, ref Outputs);

            return CallResult;
        }

        public static int P_TrendChart_7X_JO(SqlConnection conn)
        {
            DataSet ds = null;

            return P_TrendChart_7X_JO(conn, ref ds);
        }

        public static int P_TrendChart_7X_JO(SqlConnection conn, ref DataSet ds)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(conn, "P_TrendChart_7X_JO", ref ds, ref Outputs);

            return CallResult;
        }

        public static int P_TrendChart_7X_LH(SqlConnection conn)
        {
            DataSet ds = null;

            return P_TrendChart_7X_LH(conn, ref ds);
        }

        public static int P_TrendChart_7X_LH(SqlConnection conn, ref DataSet ds)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(conn, "P_TrendChart_7X_LH", ref ds, ref Outputs);

            return CallResult;
        }

        public static int P_TrendChart_7X_YS(SqlConnection conn)
        {
            DataSet ds = null;

            return P_TrendChart_7X_YS(conn, ref ds);
        }

        public static int P_TrendChart_7X_YS(SqlConnection conn, ref DataSet ds)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(conn, "P_TrendChart_7X_YS", ref ds, ref Outputs);

            return CallResult;
        }

        public static int P_TrendChart_7X_ZH(SqlConnection conn)
        {
            DataSet ds = null;

            return P_TrendChart_7X_ZH(conn, ref ds);
        }

        public static int P_TrendChart_7X_ZH(SqlConnection conn, ref DataSet ds)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(conn, "P_TrendChart_7X_ZH", ref ds, ref Outputs);

            return CallResult;
        }

        public static int P_TrendChart_7XC_CGXMB(SqlConnection conn)
        {
            DataSet ds = null;

            return P_TrendChart_7XC_CGXMB(conn, ref ds);
        }

        public static int P_TrendChart_7XC_CGXMB(SqlConnection conn, ref DataSet ds)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(conn, "P_TrendChart_7XC_CGXMB", ref ds, ref Outputs);

            return CallResult;
        }

        public static int P_TrendChart_CJDLT_HMFB(SqlConnection conn, int IsuseNum)
        {
            DataSet ds = null;

            return P_TrendChart_CJDLT_HMFB(conn, ref ds, IsuseNum);
        }

        public static int P_TrendChart_CJDLT_HMFB(SqlConnection conn, ref DataSet ds, int IsuseNum)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(conn, "P_TrendChart_CJDLT_HMFB", ref ds, ref Outputs,
                new MSSQL.Parameter("IsuseNum", SqlDbType.Int, 0, ParameterDirection.Input, IsuseNum)
                );

            return CallResult;
        }

        public static int P_TrendChart_CJDLT_HMLR_JiMa(SqlConnection conn, int IsuseNum)
        {
            DataSet ds = null;

            return P_TrendChart_CJDLT_HMLR_JiMa(conn, ref ds, IsuseNum);
        }

        public static int P_TrendChart_CJDLT_HMLR_JiMa(SqlConnection conn, ref DataSet ds, int IsuseNum)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(conn, "P_TrendChart_CJDLT_HMLR_JiMa", ref ds, ref Outputs,
                new MSSQL.Parameter("IsuseNum", SqlDbType.Int, 0, ParameterDirection.Input, IsuseNum)
                );

            return CallResult;
        }

        public static int P_TrendChart_CJDLT_HMLR_JiMajj(SqlConnection conn, int IsuseNum)
        {
            DataSet ds = null;

            return P_TrendChart_CJDLT_HMLR_JiMajj(conn, ref ds, IsuseNum);
        }

        public static int P_TrendChart_CJDLT_HMLR_JiMajj(SqlConnection conn, ref DataSet ds, int IsuseNum)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(conn, "P_TrendChart_CJDLT_HMLR_JiMajj", ref ds, ref Outputs,
                new MSSQL.Parameter("IsuseNum", SqlDbType.Int, 0, ParameterDirection.Input, IsuseNum)
                );

            return CallResult;
        }

        public static int P_TrendChart_CJDLT_HMLR_Tema(SqlConnection conn, int IsuseNum)
        {
            DataSet ds = null;

            return P_TrendChart_CJDLT_HMLR_Tema(conn, ref ds, IsuseNum);
        }

        public static int P_TrendChart_CJDLT_HMLR_Tema(SqlConnection conn, ref DataSet ds, int IsuseNum)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(conn, "P_TrendChart_CJDLT_HMLR_Tema", ref ds, ref Outputs,
                new MSSQL.Parameter("IsuseNum", SqlDbType.Int, 0, ParameterDirection.Input, IsuseNum)
                );

            return CallResult;
        }

        public static int P_TrendChart_CJDLT_HMLR_Temajj(SqlConnection conn, int IsuseNum)
        {
            DataSet ds = null;

            return P_TrendChart_CJDLT_HMLR_Temajj(conn, ref ds, IsuseNum);
        }

        public static int P_TrendChart_CJDLT_HMLR_Temajj(SqlConnection conn, ref DataSet ds, int IsuseNum)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(conn, "P_TrendChart_CJDLT_HMLR_Temajj", ref ds, ref Outputs,
                new MSSQL.Parameter("IsuseNum", SqlDbType.Int, 0, ParameterDirection.Input, IsuseNum)
                );

            return CallResult;
        }

        public static int P_TrendChart_CJDLT_HZ_Heng(SqlConnection conn)
        {
            DataSet ds = null;

            return P_TrendChart_CJDLT_HZ_Heng(conn, ref ds);
        }

        public static int P_TrendChart_CJDLT_HZ_Heng(SqlConnection conn, ref DataSet ds)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(conn, "P_TrendChart_CJDLT_HZ_Heng", ref ds, ref Outputs);

            return CallResult;
        }

        public static int P_TrendChart_CJDLT_HZzong(SqlConnection conn)
        {
            DataSet ds = null;

            return P_TrendChart_CJDLT_HZzong(conn, ref ds);
        }

        public static int P_TrendChart_CJDLT_HZzong(SqlConnection conn, ref DataSet ds)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(conn, "P_TrendChart_CJDLT_HZzong", ref ds, ref Outputs);

            return CallResult;
        }

        public static int P_TrendChart_CJDLT_jima(SqlConnection conn)
        {
            DataSet ds = null;

            return P_TrendChart_CJDLT_jima(conn, ref ds);
        }

        public static int P_TrendChart_CJDLT_jima(SqlConnection conn, ref DataSet ds)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(conn, "P_TrendChart_CJDLT_jima", ref ds, ref Outputs);

            return CallResult;
        }

        public static int P_TrendChart_CJDLT_jimaYL(SqlConnection conn, int IsuseNum)
        {
            DataSet ds = null;

            return P_TrendChart_CJDLT_jimaYL(conn, ref ds, IsuseNum);
        }

        public static int P_TrendChart_CJDLT_jimaYL(SqlConnection conn, ref DataSet ds, int IsuseNum)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(conn, "P_TrendChart_CJDLT_jimaYL", ref ds, ref Outputs,
                new MSSQL.Parameter("IsuseNum", SqlDbType.Int, 0, ParameterDirection.Input, IsuseNum)
                );

            return CallResult;
        }

        public static int P_TrendChart_CJDLT_Jiou(SqlConnection conn)
        {
            DataSet ds = null;

            return P_TrendChart_CJDLT_Jiou(conn, ref ds);
        }

        public static int P_TrendChart_CJDLT_Jiou(SqlConnection conn, ref DataSet ds)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(conn, "P_TrendChart_CJDLT_Jiou", ref ds, ref Outputs);

            return CallResult;
        }

        public static int P_TrendChart_CJDLT_LH(SqlConnection conn)
        {
            DataSet ds = null;

            return P_TrendChart_CJDLT_LH(conn, ref ds);
        }

        public static int P_TrendChart_CJDLT_LH(SqlConnection conn, ref DataSet ds)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(conn, "P_TrendChart_CJDLT_LH", ref ds, ref Outputs);

            return CallResult;
        }

        public static int P_TrendChart_CJDLT_tema(SqlConnection conn)
        {
            DataSet ds = null;

            return P_TrendChart_CJDLT_tema(conn, ref ds);
        }

        public static int P_TrendChart_CJDLT_tema(SqlConnection conn, ref DataSet ds)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(conn, "P_TrendChart_CJDLT_tema", ref ds, ref Outputs);

            return CallResult;
        }

        public static int P_TrendChart_CJDLT_TeMa_WH(SqlConnection conn)
        {
            DataSet ds = null;

            return P_TrendChart_CJDLT_TeMa_WH(conn, ref ds);
        }

        public static int P_TrendChart_CJDLT_TeMa_WH(SqlConnection conn, ref DataSet ds)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(conn, "P_TrendChart_CJDLT_TeMa_WH", ref ds, ref Outputs);

            return CallResult;
        }

        public static int P_TrendChart_CJDLT_TemaYL(SqlConnection conn, int IsuseNum)
        {
            DataSet ds = null;

            return P_TrendChart_CJDLT_TemaYL(conn, ref ds, IsuseNum);
        }

        public static int P_TrendChart_CJDLT_TemaYL(SqlConnection conn, ref DataSet ds, int IsuseNum)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(conn, "P_TrendChart_CJDLT_TemaYL", ref ds, ref Outputs,
                new MSSQL.Parameter("IsuseNum", SqlDbType.Int, 0, ParameterDirection.Input, IsuseNum)
                );

            return CallResult;
        }

        public static int P_TrendChart_CJDLT_WH(SqlConnection conn)
        {
            DataSet ds = null;

            return P_TrendChart_CJDLT_WH(conn, ref ds);
        }

        public static int P_TrendChart_CJDLT_WH(SqlConnection conn, ref DataSet ds)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(conn, "P_TrendChart_CJDLT_WH", ref ds, ref Outputs);

            return CallResult;
        }

        public static int P_TrendChart_CJDLT_YS(SqlConnection conn)
        {
            DataSet ds = null;

            return P_TrendChart_CJDLT_YS(conn, ref ds);
        }

        public static int P_TrendChart_CJDLT_YS(SqlConnection conn, ref DataSet ds)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(conn, "P_TrendChart_CJDLT_YS", ref ds, ref Outputs);

            return CallResult;
        }

        public static int P_TrendChart_DF6J1_ZHFB(SqlConnection conn)
        {
            DataSet ds = null;

            return P_TrendChart_DF6J1_ZHFB(conn, ref ds);
        }

        public static int P_TrendChart_DF6J1_ZHFB(SqlConnection conn, ref DataSet ds)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(conn, "P_TrendChart_DF6J1_ZHFB", ref ds, ref Outputs);

            return CallResult;
        }

        public static int P_TrendChart_FC3D(SqlConnection conn, int IsuseNum)
        {
            DataSet ds = null;

            return P_TrendChart_FC3D(conn, ref ds, IsuseNum);
        }

        public static int P_TrendChart_FC3D(SqlConnection conn, ref DataSet ds, int IsuseNum)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(conn, "P_TrendChart_FC3D", ref ds, ref Outputs,
                new MSSQL.Parameter("IsuseNum", SqlDbType.Int, 0, ParameterDirection.Input, IsuseNum)
                );

            return CallResult;
        }

        public static int P_TrendChart_KLPK_012(SqlConnection conn)
        {
            DataSet ds = null;

            return P_TrendChart_KLPK_012(conn, ref ds);
        }

        public static int P_TrendChart_KLPK_012(SqlConnection conn, ref DataSet ds)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(conn, "P_TrendChart_KLPK_012", ref ds, ref Outputs);

            return CallResult;
        }

        public static int P_TrendChart_KLPK_DX(SqlConnection conn)
        {
            DataSet ds = null;

            return P_TrendChart_KLPK_DX(conn, ref ds);
        }

        public static int P_TrendChart_KLPK_DX(SqlConnection conn, ref DataSet ds)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(conn, "P_TrendChart_KLPK_DX", ref ds, ref Outputs);

            return CallResult;
        }

        public static int P_TrendChart_KLPK_DZX(SqlConnection conn)
        {
            DataSet ds = null;

            return P_TrendChart_KLPK_DZX(conn, ref ds);
        }

        public static int P_TrendChart_KLPK_DZX(SqlConnection conn, ref DataSet ds)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(conn, "P_TrendChart_KLPK_DZX", ref ds, ref Outputs);

            return CallResult;
        }

        public static int P_TrendChart_KLPK_KJFB(SqlConnection conn)
        {
            DataSet ds = null;

            return P_TrendChart_KLPK_KJFB(conn, ref ds);
        }

        public static int P_TrendChart_KLPK_KJFB(SqlConnection conn, ref DataSet ds)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(conn, "P_TrendChart_KLPK_KJFB", ref ds, ref Outputs);

            return CallResult;
        }

        public static int P_TrendChart_KLPK_ZH(SqlConnection conn)
        {
            DataSet ds = null;

            return P_TrendChart_KLPK_ZH(conn, ref ds);
        }

        public static int P_TrendChart_KLPK_ZH(SqlConnection conn, ref DataSet ds)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(conn, "P_TrendChart_KLPK_ZH", ref ds, ref Outputs);

            return CallResult;
        }

        public static int P_TrendChart_PL3(SqlConnection conn)
        {
            DataSet ds = null;

            return P_TrendChart_PL3(conn, ref ds);
        }

        public static int P_TrendChart_PL3(SqlConnection conn, ref DataSet ds)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(conn, "P_TrendChart_PL3", ref ds, ref Outputs);

            return CallResult;
        }

        public static int P_TrendChart_PL3_012(SqlConnection conn, int IsuseNum)
        {
            DataSet ds = null;

            return P_TrendChart_PL3_012(conn, ref ds, IsuseNum);
        }

        public static int P_TrendChart_PL3_012(SqlConnection conn, ref DataSet ds, int IsuseNum)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(conn, "P_TrendChart_PL3_012", ref ds, ref Outputs,
                new MSSQL.Parameter("IsuseNum", SqlDbType.Int, 0, ParameterDirection.Input, IsuseNum)
                );

            return CallResult;
        }

        public static int P_TrendChart_PL3_DX(SqlConnection conn, int IsuseNum)
        {
            DataSet ds = null;

            return P_TrendChart_PL3_DX(conn, ref ds, IsuseNum);
        }

        public static int P_TrendChart_PL3_DX(SqlConnection conn, ref DataSet ds, int IsuseNum)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(conn, "P_TrendChart_PL3_DX", ref ds, ref Outputs,
                new MSSQL.Parameter("IsuseNum", SqlDbType.Int, 0, ParameterDirection.Input, IsuseNum)
                );

            return CallResult;
        }

        public static int P_TrendChart_PL3_DZX(SqlConnection conn, int IsuseNum)
        {
            DataSet ds = null;

            return P_TrendChart_PL3_DZX(conn, ref ds, IsuseNum);
        }

        public static int P_TrendChart_PL3_DZX(SqlConnection conn, ref DataSet ds, int IsuseNum)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(conn, "P_TrendChart_PL3_DZX", ref ds, ref Outputs,
                new MSSQL.Parameter("IsuseNum", SqlDbType.Int, 0, ParameterDirection.Input, IsuseNum)
                );

            return CallResult;
        }

        public static int P_TrendChart_PL3_HMFB(SqlConnection conn, int IsuseNum)
        {
            DataSet ds = null;

            return P_TrendChart_PL3_HMFB(conn, ref ds, IsuseNum);
        }

        public static int P_TrendChart_PL3_HMFB(SqlConnection conn, ref DataSet ds, int IsuseNum)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(conn, "P_TrendChart_PL3_HMFB", ref ds, ref Outputs,
                new MSSQL.Parameter("IsuseNum", SqlDbType.Int, 0, ParameterDirection.Input, IsuseNum)
                );

            return CallResult;
        }

        public static int P_TrendChart_PL3_HZ(SqlConnection conn, int IsuseNum)
        {
            DataSet ds = null;

            return P_TrendChart_PL3_HZ(conn, ref ds, IsuseNum);
        }

        public static int P_TrendChart_PL3_HZ(SqlConnection conn, ref DataSet ds, int IsuseNum)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(conn, "P_TrendChart_PL3_HZ", ref ds, ref Outputs,
                new MSSQL.Parameter("IsuseNum", SqlDbType.Int, 0, ParameterDirection.Input, IsuseNum)
                );

            return CallResult;
        }

        public static int P_TrendChart_PL3_JO(SqlConnection conn, int IsuseNum)
        {
            DataSet ds = null;

            return P_TrendChart_PL3_JO(conn, ref ds, IsuseNum);
        }

        public static int P_TrendChart_PL3_JO(SqlConnection conn, ref DataSet ds, int IsuseNum)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(conn, "P_TrendChart_PL3_JO", ref ds, ref Outputs,
                new MSSQL.Parameter("IsuseNum", SqlDbType.Int, 0, ParameterDirection.Input, IsuseNum)
                );

            return CallResult;
        }

        public static int P_TrendChart_PL3_KD(SqlConnection conn, int IsuseNum)
        {
            DataSet ds = null;

            return P_TrendChart_PL3_KD(conn, ref ds, IsuseNum);
        }

        public static int P_TrendChart_PL3_KD(SqlConnection conn, ref DataSet ds, int IsuseNum)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(conn, "P_TrendChart_PL3_KD", ref ds, ref Outputs,
                new MSSQL.Parameter("IsuseNum", SqlDbType.Int, 0, ParameterDirection.Input, IsuseNum)
                );

            return CallResult;
        }

        public static int P_TrendChart_PL3_LH(SqlConnection conn)
        {
            DataSet ds = null;

            return P_TrendChart_PL3_LH(conn, ref ds);
        }

        public static int P_TrendChart_PL3_LH(SqlConnection conn, ref DataSet ds)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(conn, "P_TrendChart_PL3_LH", ref ds, ref Outputs);

            return CallResult;
        }

        public static int P_TrendChart_PL3_WH(SqlConnection conn, int IsuseNum)
        {
            DataSet ds = null;

            return P_TrendChart_PL3_WH(conn, ref ds, IsuseNum);
        }

        public static int P_TrendChart_PL3_WH(SqlConnection conn, ref DataSet ds, int IsuseNum)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(conn, "P_TrendChart_PL3_WH", ref ds, ref Outputs,
                new MSSQL.Parameter("IsuseNum", SqlDbType.Int, 0, ParameterDirection.Input, IsuseNum)
                );

            return CallResult;
        }

        public static int P_TrendChart_PL3_YS(SqlConnection conn)
        {
            DataSet ds = null;

            return P_TrendChart_PL3_YS(conn, ref ds);
        }

        public static int P_TrendChart_PL3_YS(SqlConnection conn, ref DataSet ds)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(conn, "P_TrendChart_PL3_YS", ref ds, ref Outputs);

            return CallResult;
        }

        public static int P_TrendChart_PL3_ZH(SqlConnection conn, int IsuseNum)
        {
            DataSet ds = null;

            return P_TrendChart_PL3_ZH(conn, ref ds, IsuseNum);
        }

        public static int P_TrendChart_PL3_ZH(SqlConnection conn, ref DataSet ds, int IsuseNum)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(conn, "P_TrendChart_PL3_ZH", ref ds, ref Outputs,
                new MSSQL.Parameter("IsuseNum", SqlDbType.Int, 0, ParameterDirection.Input, IsuseNum)
                );

            return CallResult;
        }

        public static int P_TrendChart_PL3_ZX(SqlConnection conn, int IsuseNum)
        {
            DataSet ds = null;

            return P_TrendChart_PL3_ZX(conn, ref ds, IsuseNum);
        }

        public static int P_TrendChart_PL3_ZX(SqlConnection conn, ref DataSet ds, int IsuseNum)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(conn, "P_TrendChart_PL3_ZX", ref ds, ref Outputs,
                new MSSQL.Parameter("IsuseNum", SqlDbType.Int, 0, ParameterDirection.Input, IsuseNum)
                );

            return CallResult;
        }

        public static int P_TrendChart_PL5_012(SqlConnection conn)
        {
            DataSet ds = null;

            return P_TrendChart_PL5_012(conn, ref ds);
        }

        public static int P_TrendChart_PL5_012(SqlConnection conn, ref DataSet ds)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(conn, "P_TrendChart_PL5_012", ref ds, ref Outputs);

            return CallResult;
        }

        public static int P_TrendChart_PL5_CF(SqlConnection conn)
        {
            DataSet ds = null;

            return P_TrendChart_PL5_CF(conn, ref ds);
        }

        public static int P_TrendChart_PL5_CF(SqlConnection conn, ref DataSet ds)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(conn, "P_TrendChart_PL5_CF", ref ds, ref Outputs);

            return CallResult;
        }

        public static int P_TrendChart_PL5_DX(SqlConnection conn)
        {
            DataSet ds = null;

            return P_TrendChart_PL5_DX(conn, ref ds);
        }

        public static int P_TrendChart_PL5_DX(SqlConnection conn, ref DataSet ds)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(conn, "P_TrendChart_PL5_DX", ref ds, ref Outputs);

            return CallResult;
        }

        public static int P_TrendChart_PL5_DZX(SqlConnection conn)
        {
            DataSet ds = null;

            return P_TrendChart_PL5_DZX(conn, ref ds);
        }

        public static int P_TrendChart_PL5_DZX(SqlConnection conn, ref DataSet ds)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(conn, "P_TrendChart_PL5_DZX", ref ds, ref Outputs);

            return CallResult;
        }

        public static int P_TrendChart_PL5_HMFB(SqlConnection conn, int IsuseNum)
        {
            DataSet ds = null;

            return P_TrendChart_PL5_HMFB(conn, ref ds, IsuseNum);
        }

        public static int P_TrendChart_PL5_HMFB(SqlConnection conn, ref DataSet ds, int IsuseNum)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(conn, "P_TrendChart_PL5_HMFB", ref ds, ref Outputs,
                new MSSQL.Parameter("IsuseNum", SqlDbType.Int, 0, ParameterDirection.Input, IsuseNum)
                );

            return CallResult;
        }

        public static int P_TrendChart_PL5_HZ(SqlConnection conn, int IsuseNum)
        {
            DataSet ds = null;

            return P_TrendChart_PL5_HZ(conn, ref ds, IsuseNum);
        }

        public static int P_TrendChart_PL5_HZ(SqlConnection conn, ref DataSet ds, int IsuseNum)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(conn, "P_TrendChart_PL5_HZ", ref ds, ref Outputs,
                new MSSQL.Parameter("IsuseNum", SqlDbType.Int, 0, ParameterDirection.Input, IsuseNum)
                );

            return CallResult;
        }

        public static int P_TrendChart_PL5_JO(SqlConnection conn)
        {
            DataSet ds = null;

            return P_TrendChart_PL5_JO(conn, ref ds);
        }

        public static int P_TrendChart_PL5_JO(SqlConnection conn, ref DataSet ds)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(conn, "P_TrendChart_PL5_JO", ref ds, ref Outputs);

            return CallResult;
        }

        public static int P_TrendChart_PL5_LH(SqlConnection conn)
        {
            DataSet ds = null;

            return P_TrendChart_PL5_LH(conn, ref ds);
        }

        public static int P_TrendChart_PL5_LH(SqlConnection conn, ref DataSet ds)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(conn, "P_TrendChart_PL5_LH", ref ds, ref Outputs);

            return CallResult;
        }

        public static int P_TrendChart_PL5_YS(SqlConnection conn)
        {
            DataSet ds = null;

            return P_TrendChart_PL5_YS(conn, ref ds);
        }

        public static int P_TrendChart_PL5_YS(SqlConnection conn, ref DataSet ds)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(conn, "P_TrendChart_PL5_YS", ref ds, ref Outputs);

            return CallResult;
        }

        public static int P_TrendChart_PL5_ZH(SqlConnection conn)
        {
            DataSet ds = null;

            return P_TrendChart_PL5_ZH(conn, ref ds);
        }

        public static int P_TrendChart_PL5_ZH(SqlConnection conn, ref DataSet ds)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(conn, "P_TrendChart_PL5_ZH", ref ds, ref Outputs);

            return CallResult;
        }

        public static int P_TrendChart_SHSSL_012(SqlConnection conn)
        {
            DataSet ds = null;

            return P_TrendChart_SHSSL_012(conn, ref ds);
        }

        public static int P_TrendChart_SHSSL_012(SqlConnection conn, ref DataSet ds)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(conn, "P_TrendChart_SHSSL_012", ref ds, ref Outputs);

            return CallResult;
        }

        public static int P_TrendChart_SHSSL_DX(SqlConnection conn)
        {
            DataSet ds = null;

            return P_TrendChart_SHSSL_DX(conn, ref ds);
        }

        public static int P_TrendChart_SHSSL_DX(SqlConnection conn, ref DataSet ds)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(conn, "P_TrendChart_SHSSL_DX", ref ds, ref Outputs);

            return CallResult;
        }

        public static int P_TrendChart_SHSSL_HZ(SqlConnection conn)
        {
            DataSet ds = null;

            return P_TrendChart_SHSSL_HZ(conn, ref ds);
        }

        public static int P_TrendChart_SHSSL_HZ(SqlConnection conn, ref DataSet ds)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(conn, "P_TrendChart_SHSSL_HZ", ref ds, ref Outputs);

            return CallResult;
        }

        public static int P_TrendChart_SHSSL_JO(SqlConnection conn)
        {
            DataSet ds = null;

            return P_TrendChart_SHSSL_JO(conn, ref ds);
        }

        public static int P_TrendChart_SHSSL_JO(SqlConnection conn, ref DataSet ds)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(conn, "P_TrendChart_SHSSL_JO", ref ds, ref Outputs);

            return CallResult;
        }

        public static int P_TrendChart_SHSSL_ZH(SqlConnection conn)
        {
            DataSet ds = null;

            return P_TrendChart_SHSSL_ZH(conn, ref ds);
        }

        public static int P_TrendChart_SHSSL_ZH(SqlConnection conn, ref DataSet ds)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(conn, "P_TrendChart_SHSSL_ZH", ref ds, ref Outputs);

            return CallResult;
        }

        public static int P_TrendChart_SHSSL_ZHFB(SqlConnection conn)
        {
            DataSet ds = null;

            return P_TrendChart_SHSSL_ZHFB(conn, ref ds);
        }

        public static int P_TrendChart_SHSSL_ZHFB(SqlConnection conn, ref DataSet ds)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(conn, "P_TrendChart_SHSSL_ZHFB", ref ds, ref Outputs);

            return CallResult;
        }

        public static int P_TrendChart_SSC_2X_012_ZST(SqlConnection conn)
        {
            DataSet ds = null;

            return P_TrendChart_SSC_2X_012_ZST(conn, ref ds);
        }

        public static int P_TrendChart_SSC_2X_012_ZST(SqlConnection conn, ref DataSet ds)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(conn, "P_TrendChart_SSC_2X_012_ZST", ref ds, ref Outputs);

            return CallResult;
        }

        public static int P_TrendChart_SSC_2XDXDSZST(SqlConnection conn)
        {
            DataSet ds = null;

            return P_TrendChart_SSC_2XDXDSZST(conn, ref ds);
        }

        public static int P_TrendChart_SSC_2XDXDSZST(SqlConnection conn, ref DataSet ds)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(conn, "P_TrendChart_SSC_2XDXDSZST", ref ds, ref Outputs);

            return CallResult;
        }

        public static int P_TrendChart_SSC_2XHZWZST(SqlConnection conn)
        {
            DataSet ds = null;

            return P_TrendChart_SSC_2XHZWZST(conn, ref ds);
        }

        public static int P_TrendChart_SSC_2XHZWZST(SqlConnection conn, ref DataSet ds)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(conn, "P_TrendChart_SSC_2XHZWZST", ref ds, ref Outputs);

            return CallResult;
        }

        public static int P_TrendChart_SSC_2XHZZST(SqlConnection conn)
        {
            DataSet ds = null;

            return P_TrendChart_SSC_2XHZZST(conn, ref ds);
        }

        public static int P_TrendChart_SSC_2XHZZST(SqlConnection conn, ref DataSet ds)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(conn, "P_TrendChart_SSC_2XHZZST", ref ds, ref Outputs);

            return CallResult;
        }

        public static int P_TrendChart_SSC_2XKDZST(SqlConnection conn)
        {
            DataSet ds = null;

            return P_TrendChart_SSC_2XKDZST(conn, ref ds);
        }

        public static int P_TrendChart_SSC_2XKDZST(SqlConnection conn, ref DataSet ds)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(conn, "P_TrendChart_SSC_2XKDZST", ref ds, ref Outputs);

            return CallResult;
        }

        public static int P_TrendChart_SSC_2XMaxZST(SqlConnection conn)
        {
            DataSet ds = null;

            return P_TrendChart_SSC_2XMaxZST(conn, ref ds);
        }

        public static int P_TrendChart_SSC_2XMaxZST(SqlConnection conn, ref DataSet ds)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(conn, "P_TrendChart_SSC_2XMaxZST", ref ds, ref Outputs);

            return CallResult;
        }

        public static int P_TrendChart_SSC_2XMINZST(SqlConnection conn)
        {
            DataSet ds = null;

            return P_TrendChart_SSC_2XMINZST(conn, ref ds);
        }

        public static int P_TrendChart_SSC_2XMINZST(SqlConnection conn, ref DataSet ds)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(conn, "P_TrendChart_SSC_2XMINZST", ref ds, ref Outputs);

            return CallResult;
        }

        public static int P_TrendChart_SSC_2XPJZZST(SqlConnection conn)
        {
            DataSet ds = null;

            return P_TrendChart_SSC_2XPJZZST(conn, ref ds);
        }

        public static int P_TrendChart_SSC_2XPJZZST(SqlConnection conn, ref DataSet ds)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(conn, "P_TrendChart_SSC_2XPJZZST", ref ds, ref Outputs);

            return CallResult;
        }

        public static int P_TrendChart_SSC_2XZHFBZST(SqlConnection conn)
        {
            DataSet ds = null;

            return P_TrendChart_SSC_2XZHFBZST(conn, ref ds);
        }

        public static int P_TrendChart_SSC_2XZHFBZST(SqlConnection conn, ref DataSet ds)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(conn, "P_TrendChart_SSC_2XZHFBZST", ref ds, ref Outputs);

            return CallResult;
        }

        public static int P_TrendChart_SSC_3X_DX012_ZST(SqlConnection conn)
        {
            DataSet ds = null;

            return P_TrendChart_SSC_3X_DX012_ZST(conn, ref ds);
        }

        public static int P_TrendChart_SSC_3X_DX012_ZST(SqlConnection conn, ref DataSet ds)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(conn, "P_TrendChart_SSC_3X_DX012_ZST", ref ds, ref Outputs);

            return CallResult;
        }

        public static int P_TrendChart_SSC_3X_ZX012_ZST(SqlConnection conn)
        {
            DataSet ds = null;

            return P_TrendChart_SSC_3X_ZX012_ZST(conn, ref ds);
        }

        public static int P_TrendChart_SSC_3X_ZX012_ZST(SqlConnection conn, ref DataSet ds)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(conn, "P_TrendChart_SSC_3X_ZX012_ZST", ref ds, ref Outputs);

            return CallResult;
        }

        public static int P_TrendChart_SSC_3XDXZST(SqlConnection conn)
        {
            DataSet ds = null;

            return P_TrendChart_SSC_3XDXZST(conn, ref ds);
        }

        public static int P_TrendChart_SSC_3XDXZST(SqlConnection conn, ref DataSet ds)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(conn, "P_TrendChart_SSC_3XDXZST", ref ds, ref Outputs);

            return CallResult;
        }

        public static int P_TrendChart_SSC_3XHZWZST(SqlConnection conn)
        {
            DataSet ds = null;

            return P_TrendChart_SSC_3XHZWZST(conn, ref ds);
        }

        public static int P_TrendChart_SSC_3XHZWZST(SqlConnection conn, ref DataSet ds)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(conn, "P_TrendChart_SSC_3XHZWZST", ref ds, ref Outputs);

            return CallResult;
        }

        public static int P_TrendChart_SSC_3XHZZST(SqlConnection conn)
        {
            DataSet ds = null;

            return P_TrendChart_SSC_3XHZZST(conn, ref ds);
        }

        public static int P_TrendChart_SSC_3XHZZST(SqlConnection conn, ref DataSet ds)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(conn, "P_TrendChart_SSC_3XHZZST", ref ds, ref Outputs);

            return CallResult;
        }

        public static int P_TrendChart_SSC_3XJOZST(SqlConnection conn)
        {
            DataSet ds = null;

            return P_TrendChart_SSC_3XJOZST(conn, ref ds);
        }

        public static int P_TrendChart_SSC_3XJOZST(SqlConnection conn, ref DataSet ds)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(conn, "P_TrendChart_SSC_3XJOZST", ref ds, ref Outputs);

            return CallResult;
        }

        public static int P_TrendChart_SSC_3XKDZST(SqlConnection conn)
        {
            DataSet ds = null;

            return P_TrendChart_SSC_3XKDZST(conn, ref ds);
        }

        public static int P_TrendChart_SSC_3XKDZST(SqlConnection conn, ref DataSet ds)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(conn, "P_TrendChart_SSC_3XKDZST", ref ds, ref Outputs);

            return CallResult;
        }

        public static int P_TrendChart_SSC_3XPJZZST(SqlConnection conn)
        {
            DataSet ds = null;

            return P_TrendChart_SSC_3XPJZZST(conn, ref ds);
        }

        public static int P_TrendChart_SSC_3XPJZZST(SqlConnection conn, ref DataSet ds)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(conn, "P_TrendChart_SSC_3XPJZZST", ref ds, ref Outputs);

            return CallResult;
        }

        public static int P_TrendChart_SSC_3XZHFBZST(SqlConnection conn)
        {
            DataSet ds = null;

            return P_TrendChart_SSC_3XZHFBZST(conn, ref ds);
        }

        public static int P_TrendChart_SSC_3XZHFBZST(SqlConnection conn, ref DataSet ds)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(conn, "P_TrendChart_SSC_3XZHFBZST", ref ds, ref Outputs);

            return CallResult;
        }

        public static int P_TrendChart_SSC_3XZHZST(SqlConnection conn)
        {
            DataSet ds = null;

            return P_TrendChart_SSC_3XZHZST(conn, ref ds);
        }

        public static int P_TrendChart_SSC_3XZHZST(SqlConnection conn, ref DataSet ds)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(conn, "P_TrendChart_SSC_3XZHZST", ref ds, ref Outputs);

            return CallResult;
        }

        public static int P_TrendChart_SSC_3XZST(SqlConnection conn)
        {
            DataSet ds = null;

            return P_TrendChart_SSC_3XZST(conn, ref ds);
        }

        public static int P_TrendChart_SSC_3XZST(SqlConnection conn, ref DataSet ds)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(conn, "P_TrendChart_SSC_3XZST", ref ds, ref Outputs);

            return CallResult;
        }

        public static int P_TrendChart_SSC_4XDXZST(SqlConnection conn)
        {
            DataSet ds = null;

            return P_TrendChart_SSC_4XDXZST(conn, ref ds);
        }

        public static int P_TrendChart_SSC_4XDXZST(SqlConnection conn, ref DataSet ds)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(conn, "P_TrendChart_SSC_4XDXZST", ref ds, ref Outputs);

            return CallResult;
        }

        public static int P_TrendChart_SSC_4XHZZST(SqlConnection conn)
        {
            DataSet ds = null;

            return P_TrendChart_SSC_4XHZZST(conn, ref ds);
        }

        public static int P_TrendChart_SSC_4XHZZST(SqlConnection conn, ref DataSet ds)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(conn, "P_TrendChart_SSC_4XHZZST", ref ds, ref Outputs);

            return CallResult;
        }

        public static int P_TrendChart_SSC_4XJOZST(SqlConnection conn)
        {
            DataSet ds = null;

            return P_TrendChart_SSC_4XJOZST(conn, ref ds);
        }

        public static int P_TrendChart_SSC_4XJOZST(SqlConnection conn, ref DataSet ds)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(conn, "P_TrendChart_SSC_4XJOZST", ref ds, ref Outputs);

            return CallResult;
        }

        public static int P_TrendChart_SSC_4XKDZST(SqlConnection conn)
        {
            DataSet ds = null;

            return P_TrendChart_SSC_4XKDZST(conn, ref ds);
        }

        public static int P_TrendChart_SSC_4XKDZST(SqlConnection conn, ref DataSet ds)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(conn, "P_TrendChart_SSC_4XKDZST", ref ds, ref Outputs);

            return CallResult;
        }

        public static int P_TrendChart_SSC_4XPJZZST(SqlConnection conn)
        {
            DataSet ds = null;

            return P_TrendChart_SSC_4XPJZZST(conn, ref ds);
        }

        public static int P_TrendChart_SSC_4XPJZZST(SqlConnection conn, ref DataSet ds)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(conn, "P_TrendChart_SSC_4XPJZZST", ref ds, ref Outputs);

            return CallResult;
        }

        public static int P_TrendChart_SSC_4XZHFBZST(SqlConnection conn)
        {
            DataSet ds = null;

            return P_TrendChart_SSC_4XZHFBZST(conn, ref ds);
        }

        public static int P_TrendChart_SSC_4XZHFBZST(SqlConnection conn, ref DataSet ds)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(conn, "P_TrendChart_SSC_4XZHFBZST", ref ds, ref Outputs);

            return CallResult;
        }

        public static int P_TrendChart_SSC_4XZHZST(SqlConnection conn)
        {
            DataSet ds = null;

            return P_TrendChart_SSC_4XZHZST(conn, ref ds);
        }

        public static int P_TrendChart_SSC_4XZHZST(SqlConnection conn, ref DataSet ds)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(conn, "P_TrendChart_SSC_4XZHZST", ref ds, ref Outputs);

            return CallResult;
        }

        public static int P_TrendChart_SSC_4XZST(SqlConnection conn)
        {
            DataSet ds = null;

            return P_TrendChart_SSC_4XZST(conn, ref ds);
        }

        public static int P_TrendChart_SSC_4XZST(SqlConnection conn, ref DataSet ds)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(conn, "P_TrendChart_SSC_4XZST", ref ds, ref Outputs);

            return CallResult;
        }

        public static int P_TrendChart_SSC_5XDXZST(SqlConnection conn)
        {
            DataSet ds = null;

            return P_TrendChart_SSC_5XDXZST(conn, ref ds);
        }

        public static int P_TrendChart_SSC_5XDXZST(SqlConnection conn, ref DataSet ds)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(conn, "P_TrendChart_SSC_5XDXZST", ref ds, ref Outputs);

            return CallResult;
        }

        public static int P_TrendChart_SSC_5XHZZST(SqlConnection conn)
        {
            DataSet ds = null;

            return P_TrendChart_SSC_5XHZZST(conn, ref ds);
        }

        public static int P_TrendChart_SSC_5XHZZST(SqlConnection conn, ref DataSet ds)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(conn, "P_TrendChart_SSC_5XHZZST", ref ds, ref Outputs);

            return CallResult;
        }

        public static int P_TrendChart_SSC_5XJOZST(SqlConnection conn)
        {
            DataSet ds = null;

            return P_TrendChart_SSC_5XJOZST(conn, ref ds);
        }

        public static int P_TrendChart_SSC_5XJOZST(SqlConnection conn, ref DataSet ds)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(conn, "P_TrendChart_SSC_5XJOZST", ref ds, ref Outputs);

            return CallResult;
        }

        public static int P_TrendChart_SSC_5XKDZST(SqlConnection conn)
        {
            DataSet ds = null;

            return P_TrendChart_SSC_5XKDZST(conn, ref ds);
        }

        public static int P_TrendChart_SSC_5XKDZST(SqlConnection conn, ref DataSet ds)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(conn, "P_TrendChart_SSC_5XKDZST", ref ds, ref Outputs);

            return CallResult;
        }

        public static int P_TrendChart_SSC_5XPJZZST(SqlConnection conn)
        {
            DataSet ds = null;

            return P_TrendChart_SSC_5XPJZZST(conn, ref ds);
        }

        public static int P_TrendChart_SSC_5XPJZZST(SqlConnection conn, ref DataSet ds)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(conn, "P_TrendChart_SSC_5XPJZZST", ref ds, ref Outputs);

            return CallResult;
        }

        public static int P_TrendChart_SSC_5XZHFBZST(SqlConnection conn)
        {
            DataSet ds = null;

            return P_TrendChart_SSC_5XZHFBZST(conn, ref ds);
        }

        public static int P_TrendChart_SSC_5XZHFBZST(SqlConnection conn, ref DataSet ds)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(conn, "P_TrendChart_SSC_5XZHFBZST", ref ds, ref Outputs);

            return CallResult;
        }

        public static int P_TrendChart_SSC_5XZHZST(SqlConnection conn)
        {
            DataSet ds = null;

            return P_TrendChart_SSC_5XZHZST(conn, ref ds);
        }

        public static int P_TrendChart_SSC_5XZHZST(SqlConnection conn, ref DataSet ds)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(conn, "P_TrendChart_SSC_5XZHZST", ref ds, ref Outputs);

            return CallResult;
        }

        public static int P_TrendChart_SSC_5XZST(SqlConnection conn)
        {
            DataSet ds = null;

            return P_TrendChart_SSC_5XZST(conn, ref ds);
        }

        public static int P_TrendChart_SSC_5XZST(SqlConnection conn, ref DataSet ds)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(conn, "P_TrendChart_SSC_5XZST", ref ds, ref Outputs);

            return CallResult;
        }

        public static int P_TrendChart_SSQ_3FQ(SqlConnection conn)
        {
            DataSet ds = null;

            return P_TrendChart_SSQ_3FQ(conn, ref ds);
        }

        public static int P_TrendChart_SSQ_3FQ(SqlConnection conn, ref DataSet ds)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(conn, "P_TrendChart_SSQ_3FQ", ref ds, ref Outputs);

            return CallResult;
        }

        public static int P_TrendChart_SSQ_BQZH(SqlConnection conn)
        {
            DataSet ds = null;

            return P_TrendChart_SSQ_BQZH(conn, ref ds);
        }

        public static int P_TrendChart_SSQ_BQZH(SqlConnection conn, ref DataSet ds)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(conn, "P_TrendChart_SSQ_BQZH", ref ds, ref Outputs);

            return CallResult;
        }

        public static int P_TrendChart_SSQ_CGXMB(SqlConnection conn)
        {
            DataSet ds = null;

            return P_TrendChart_SSQ_CGXMB(conn, ref ds);
        }

        public static int P_TrendChart_SSQ_CGXMB(SqlConnection conn, ref DataSet ds)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(conn, "P_TrendChart_SSQ_CGXMB", ref ds, ref Outputs);

            return CallResult;
        }

        public static int P_TrendChart_SSQ_DX(SqlConnection conn)
        {
            DataSet ds = null;

            return P_TrendChart_SSQ_DX(conn, ref ds);
        }

        public static int P_TrendChart_SSQ_DX(SqlConnection conn, ref DataSet ds)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(conn, "P_TrendChart_SSQ_DX", ref ds, ref Outputs);

            return CallResult;
        }

        public static int P_TrendChart_SSQ_HL(SqlConnection conn)
        {
            DataSet ds = null;

            return P_TrendChart_SSQ_HL(conn, ref ds);
        }

        public static int P_TrendChart_SSQ_HL(SqlConnection conn, ref DataSet ds)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(conn, "P_TrendChart_SSQ_HL", ref ds, ref Outputs);

            return CallResult;
        }

        public static int P_TrendChart_SSQ_HMFB(SqlConnection conn)
        {
            DataSet ds = null;

            return P_TrendChart_SSQ_HMFB(conn, ref ds);
        }

        public static int P_TrendChart_SSQ_HMFB(SqlConnection conn, ref DataSet ds)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(conn, "P_TrendChart_SSQ_HMFB", ref ds, ref Outputs);

            return CallResult;
        }

        public static int P_TrendChart_SSQ_JO(SqlConnection conn)
        {
            DataSet ds = null;

            return P_TrendChart_SSQ_JO(conn, ref ds);
        }

        public static int P_TrendChart_SSQ_JO(SqlConnection conn, ref DataSet ds)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(conn, "P_TrendChart_SSQ_JO", ref ds, ref Outputs);

            return CallResult;
        }

        public static int P_TrendChart_SSQ_ZH(SqlConnection conn)
        {
            DataSet ds = null;

            return P_TrendChart_SSQ_ZH(conn, ref ds);
        }

        public static int P_TrendChart_SSQ_ZH(SqlConnection conn, ref DataSet ds)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(conn, "P_TrendChart_SSQ_ZH", ref ds, ref Outputs);

            return CallResult;
        }

        public static int P_UserAdd(SqlConnection conn, long SiteID, string Name, string RealityName, string Password, string PasswordAdv, int CityID, string Sex, DateTime BirthDay, string IDCardNumber, string Address, string Email, bool isEmailValided, string QQ, bool isQQValided, string Telephone, string Mobile, bool isMobileValided, bool isPrivacy, short UserType, short BankType, string BankName, string BankCardNumber, long CommenderID, long CpsID, string AlipayName, string Memo, string VisitSource, ref long UserID, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_UserAdd(conn, ref ds, SiteID, Name, RealityName, Password, PasswordAdv, CityID, Sex, BirthDay, IDCardNumber, Address, Email, isEmailValided, QQ, isQQValided, Telephone, Mobile, isMobileValided, isPrivacy, UserType, BankType, BankName, BankCardNumber, CommenderID, CpsID, AlipayName, Memo, VisitSource, ref UserID, ref ReturnDescription);
        }

        public static int P_UserAdd(SqlConnection conn, ref DataSet ds, long SiteID, string Name, string RealityName, string Password, string PasswordAdv, int CityID, string Sex, DateTime BirthDay, string IDCardNumber, string Address, string Email, bool isEmailValided, string QQ, bool isQQValided, string Telephone, string Mobile, bool isMobileValided, bool isPrivacy, short UserType, short BankType, string BankName, string BankCardNumber, long CommenderID, long CpsID, string AlipayName, string Memo, string VisitSource, ref long UserID, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(conn, "P_UserAdd", ref ds, ref Outputs,
                new MSSQL.Parameter("SiteID", SqlDbType.BigInt, 0, ParameterDirection.Input, SiteID),
                new MSSQL.Parameter("Name", SqlDbType.VarChar, 0, ParameterDirection.Input, Name),
                new MSSQL.Parameter("RealityName", SqlDbType.VarChar, 0, ParameterDirection.Input, RealityName),
                new MSSQL.Parameter("Password", SqlDbType.VarChar, 0, ParameterDirection.Input, Password),
                new MSSQL.Parameter("PasswordAdv", SqlDbType.VarChar, 0, ParameterDirection.Input, PasswordAdv),
                new MSSQL.Parameter("CityID", SqlDbType.Int, 0, ParameterDirection.Input, CityID),
                new MSSQL.Parameter("Sex", SqlDbType.VarChar, 0, ParameterDirection.Input, Sex),
                new MSSQL.Parameter("BirthDay", SqlDbType.DateTime, 0, ParameterDirection.Input, BirthDay),
                new MSSQL.Parameter("IDCardNumber", SqlDbType.VarChar, 0, ParameterDirection.Input, IDCardNumber),
                new MSSQL.Parameter("Address", SqlDbType.VarChar, 0, ParameterDirection.Input, Address),
                new MSSQL.Parameter("Email", SqlDbType.VarChar, 0, ParameterDirection.Input, Email),
                new MSSQL.Parameter("isEmailValided", SqlDbType.Bit, 0, ParameterDirection.Input, isEmailValided),
                new MSSQL.Parameter("QQ", SqlDbType.VarChar, 0, ParameterDirection.Input, QQ),
                new MSSQL.Parameter("isQQValided", SqlDbType.Bit, 0, ParameterDirection.Input, isQQValided),
                new MSSQL.Parameter("Telephone", SqlDbType.VarChar, 0, ParameterDirection.Input, Telephone),
                new MSSQL.Parameter("Mobile", SqlDbType.VarChar, 0, ParameterDirection.Input, Mobile),
                new MSSQL.Parameter("isMobileValided", SqlDbType.Bit, 0, ParameterDirection.Input, isMobileValided),
                new MSSQL.Parameter("isPrivacy", SqlDbType.Bit, 0, ParameterDirection.Input, isPrivacy),
                new MSSQL.Parameter("UserType", SqlDbType.SmallInt, 0, ParameterDirection.Input, UserType),
                new MSSQL.Parameter("BankType", SqlDbType.SmallInt, 0, ParameterDirection.Input, BankType),
                new MSSQL.Parameter("BankName", SqlDbType.VarChar, 0, ParameterDirection.Input, BankName),
                new MSSQL.Parameter("BankCardNumber", SqlDbType.VarChar, 0, ParameterDirection.Input, BankCardNumber),
                new MSSQL.Parameter("CommenderID", SqlDbType.BigInt, 0, ParameterDirection.Input, CommenderID),
                new MSSQL.Parameter("CpsID", SqlDbType.BigInt, 0, ParameterDirection.Input, CpsID),
                new MSSQL.Parameter("AlipayName", SqlDbType.VarChar, 0, ParameterDirection.Input, AlipayName),
                new MSSQL.Parameter("Memo", SqlDbType.VarChar, 0, ParameterDirection.Input, Memo),
                new MSSQL.Parameter("VisitSource", SqlDbType.VarChar, 0, ParameterDirection.Input, VisitSource),
                new MSSQL.Parameter("UserID", SqlDbType.BigInt, 8, ParameterDirection.Output, UserID),
                new MSSQL.Parameter("ReturnDescription", SqlDbType.VarChar, 100, ParameterDirection.Output, ReturnDescription)
                );

            try
            {
                UserID = System.Convert.ToInt64(Outputs["UserID"]);
            }
            catch { }

            try
            {
                ReturnDescription = System.Convert.ToString(Outputs["ReturnDescription"]);
            }
            catch { }

            return CallResult;
        }

        public static int P_UserAddMoney(SqlConnection conn, long SiteID, long UserID, double Money, double FormalitiesFees, string PayNumber, string PayBank, string Memo, ref int ReturnValue, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_UserAddMoney(conn, ref ds, SiteID, UserID, Money, FormalitiesFees, PayNumber, PayBank, Memo, ref ReturnValue, ref ReturnDescription);
        }

        public static int P_UserAddMoney(SqlConnection conn, ref DataSet ds, long SiteID, long UserID, double Money, double FormalitiesFees, string PayNumber, string PayBank, string Memo, ref int ReturnValue, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(conn, "P_UserAddMoney", ref ds, ref Outputs,
                new MSSQL.Parameter("SiteID", SqlDbType.BigInt, 0, ParameterDirection.Input, SiteID),
                new MSSQL.Parameter("UserID", SqlDbType.BigInt, 0, ParameterDirection.Input, UserID),
                new MSSQL.Parameter("Money", SqlDbType.Money, 0, ParameterDirection.Input, Money),
                new MSSQL.Parameter("FormalitiesFees", SqlDbType.Money, 0, ParameterDirection.Input, FormalitiesFees),
                new MSSQL.Parameter("PayNumber", SqlDbType.VarChar, 0, ParameterDirection.Input, PayNumber),
                new MSSQL.Parameter("PayBank", SqlDbType.VarChar, 0, ParameterDirection.Input, PayBank),
                new MSSQL.Parameter("Memo", SqlDbType.VarChar, 0, ParameterDirection.Input, Memo),
                new MSSQL.Parameter("ReturnValue", SqlDbType.Int, 4, ParameterDirection.Output, ReturnValue),
                new MSSQL.Parameter("ReturnDescription", SqlDbType.VarChar, 100, ParameterDirection.Output, ReturnDescription)
                );

            try
            {
                ReturnValue = System.Convert.ToInt32(Outputs["ReturnValue"]);
            }
            catch { }

            try
            {
                ReturnDescription = System.Convert.ToString(Outputs["ReturnDescription"]);
            }
            catch { }

            return CallResult;
        }

        public static int P_UserAddMoneyManual(SqlConnection conn, long SiteID, long UserID, double Money, string Memo, long OperatorID, ref int ReturnValue, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_UserAddMoneyManual(conn, ref ds, SiteID, UserID, Money, Memo, OperatorID, ref ReturnValue, ref ReturnDescription);
        }

        public static int P_UserAddMoneyManual(SqlConnection conn, ref DataSet ds, long SiteID, long UserID, double Money, string Memo, long OperatorID, ref int ReturnValue, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(conn, "P_UserAddMoneyManual", ref ds, ref Outputs,
                new MSSQL.Parameter("SiteID", SqlDbType.BigInt, 0, ParameterDirection.Input, SiteID),
                new MSSQL.Parameter("UserID", SqlDbType.BigInt, 0, ParameterDirection.Input, UserID),
                new MSSQL.Parameter("Money", SqlDbType.Money, 0, ParameterDirection.Input, Money),
                new MSSQL.Parameter("Memo", SqlDbType.VarChar, 0, ParameterDirection.Input, Memo),
                new MSSQL.Parameter("OperatorID", SqlDbType.BigInt, 0, ParameterDirection.Input, OperatorID),
                new MSSQL.Parameter("ReturnValue", SqlDbType.Int, 4, ParameterDirection.Output, ReturnValue),
                new MSSQL.Parameter("ReturnDescription", SqlDbType.VarChar, 100, ParameterDirection.Output, ReturnDescription)
                );

            try
            {
                ReturnValue = System.Convert.ToInt32(Outputs["ReturnValue"]);
            }
            catch { }

            try
            {
                ReturnDescription = System.Convert.ToString(Outputs["ReturnDescription"]);
            }
            catch { }

            return CallResult;
        }

        public static int P_UserBankDetailEdit(SqlConnection conn, long SiteID, long UserID, string BankTypeName, string BankName, string BankCardNumber, string BankInProvinceName, string BankInCityName, string BankUserName, ref int ReturnValue, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_UserBankDetailEdit(conn, ref ds, SiteID, UserID, BankTypeName, BankName, BankCardNumber, BankInProvinceName, BankInCityName, BankUserName, ref ReturnValue, ref ReturnDescription);
        }

        public static int P_UserBankDetailEdit(SqlConnection conn, ref DataSet ds, long SiteID, long UserID, string BankTypeName, string BankName, string BankCardNumber, string BankInProvinceName, string BankInCityName, string BankUserName, ref int ReturnValue, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(conn, "P_UserBankDetailEdit", ref ds, ref Outputs,
                new MSSQL.Parameter("SiteID", SqlDbType.BigInt, 0, ParameterDirection.Input, SiteID),
                new MSSQL.Parameter("UserID", SqlDbType.BigInt, 0, ParameterDirection.Input, UserID),
                new MSSQL.Parameter("BankTypeName", SqlDbType.VarChar, 0, ParameterDirection.Input, BankTypeName),
                new MSSQL.Parameter("BankName", SqlDbType.VarChar, 0, ParameterDirection.Input, BankName),
                new MSSQL.Parameter("BankCardNumber", SqlDbType.VarChar, 0, ParameterDirection.Input, BankCardNumber),
                new MSSQL.Parameter("BankInProvinceName", SqlDbType.VarChar, 0, ParameterDirection.Input, BankInProvinceName),
                new MSSQL.Parameter("BankInCityName", SqlDbType.VarChar, 0, ParameterDirection.Input, BankInCityName),
                new MSSQL.Parameter("BankUserName", SqlDbType.VarChar, 0, ParameterDirection.Input, BankUserName),
                new MSSQL.Parameter("ReturnValue", SqlDbType.Int, 4, ParameterDirection.Output, ReturnValue),
                new MSSQL.Parameter("ReturnDescription", SqlDbType.VarChar, 100, ParameterDirection.Output, ReturnDescription)
                );

            try
            {
                ReturnValue = System.Convert.ToInt32(Outputs["ReturnValue"]);
            }
            catch { }

            try
            {
                ReturnDescription = System.Convert.ToString(Outputs["ReturnDescription"]);
            }
            catch { }

            return CallResult;
        }

        public static int P_UserDistillPayByAlipay(SqlConnection conn, long HandleOperatorID, string FileName, string IDs, int PaymentType, ref int ReturnValue, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_UserDistillPayByAlipay(conn, ref ds, HandleOperatorID, FileName, IDs, PaymentType, ref ReturnValue, ref ReturnDescription);
        }

        public static int P_UserDistillPayByAlipay(SqlConnection conn, ref DataSet ds, long HandleOperatorID, string FileName, string IDs, int PaymentType, ref int ReturnValue, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(conn, "P_UserDistillPayByAlipay", ref ds, ref Outputs,
                new MSSQL.Parameter("HandleOperatorID", SqlDbType.BigInt, 0, ParameterDirection.Input, HandleOperatorID),
                new MSSQL.Parameter("FileName", SqlDbType.VarChar, 0, ParameterDirection.Input, FileName),
                new MSSQL.Parameter("IDs", SqlDbType.VarChar, 0, ParameterDirection.Input, IDs),
                new MSSQL.Parameter("PaymentType", SqlDbType.Int, 0, ParameterDirection.Input, PaymentType),
                new MSSQL.Parameter("ReturnValue", SqlDbType.Int, 4, ParameterDirection.Output, ReturnValue),
                new MSSQL.Parameter("ReturnDescription", SqlDbType.VarChar, 100, ParameterDirection.Output, ReturnDescription)
                );

            try
            {
                ReturnValue = System.Convert.ToInt32(Outputs["ReturnValue"]);
            }
            catch { }

            try
            {
                ReturnDescription = System.Convert.ToString(Outputs["ReturnDescription"]);
            }
            catch { }

            return CallResult;
        }

        public static int P_UserDistillPayByAlipaySuccess(SqlConnection conn, long SiteID, long DistillID, long HandleOperatorID, ref int ReturnValue, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_UserDistillPayByAlipaySuccess(conn, ref ds, SiteID, DistillID, HandleOperatorID, ref ReturnValue, ref ReturnDescription);
        }

        public static int P_UserDistillPayByAlipaySuccess(SqlConnection conn, ref DataSet ds, long SiteID, long DistillID, long HandleOperatorID, ref int ReturnValue, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(conn, "P_UserDistillPayByAlipaySuccess", ref ds, ref Outputs,
                new MSSQL.Parameter("SiteID", SqlDbType.BigInt, 0, ParameterDirection.Input, SiteID),
                new MSSQL.Parameter("DistillID", SqlDbType.BigInt, 0, ParameterDirection.Input, DistillID),
                new MSSQL.Parameter("HandleOperatorID", SqlDbType.BigInt, 0, ParameterDirection.Input, HandleOperatorID),
                new MSSQL.Parameter("ReturnValue", SqlDbType.Int, 4, ParameterDirection.Output, ReturnValue),
                new MSSQL.Parameter("ReturnDescription", SqlDbType.VarChar, 100, ParameterDirection.Output, ReturnDescription)
                );

            try
            {
                ReturnValue = System.Convert.ToInt32(Outputs["ReturnValue"]);
            }
            catch { }

            try
            {
                ReturnDescription = System.Convert.ToString(Outputs["ReturnDescription"]);
            }
            catch { }

            return CallResult;
        }

        public static int P_UserDistillPayByAlipayUnsuccess(SqlConnection conn, long SiteID, long DistillID, string Memo, ref int ReturnValue, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_UserDistillPayByAlipayUnsuccess(conn, ref ds, SiteID, DistillID, Memo, ref ReturnValue, ref ReturnDescription);
        }

        public static int P_UserDistillPayByAlipayUnsuccess(SqlConnection conn, ref DataSet ds, long SiteID, long DistillID, string Memo, ref int ReturnValue, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(conn, "P_UserDistillPayByAlipayUnsuccess", ref ds, ref Outputs,
                new MSSQL.Parameter("SiteID", SqlDbType.BigInt, 0, ParameterDirection.Input, SiteID),
                new MSSQL.Parameter("DistillID", SqlDbType.BigInt, 0, ParameterDirection.Input, DistillID),
                new MSSQL.Parameter("Memo", SqlDbType.VarChar, 0, ParameterDirection.Input, Memo),
                new MSSQL.Parameter("ReturnValue", SqlDbType.Int, 4, ParameterDirection.Output, ReturnValue),
                new MSSQL.Parameter("ReturnDescription", SqlDbType.VarChar, 100, ParameterDirection.Output, ReturnDescription)
                );

            try
            {
                ReturnValue = System.Convert.ToInt32(Outputs["ReturnValue"]);
            }
            catch { }

            try
            {
                ReturnDescription = System.Convert.ToString(Outputs["ReturnDescription"]);
            }
            catch { }

            return CallResult;
        }

        public static int P_UserDistillPayByAlipayWriteLog(SqlConnection conn, string Content)
        {
            DataSet ds = null;

            return P_UserDistillPayByAlipayWriteLog(conn, ref ds, Content);
        }

        public static int P_UserDistillPayByAlipayWriteLog(SqlConnection conn, ref DataSet ds, string Content)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(conn, "P_UserDistillPayByAlipayWriteLog", ref ds, ref Outputs,
                new MSSQL.Parameter("Content", SqlDbType.VarChar, 0, ParameterDirection.Input, Content)
                );

            return CallResult;
        }

        public static int P_UserEditByID(SqlConnection conn, long SiteID, long UserID, string Name, string RealityName, string Password, string PasswordAdv, int CityID, string Sex, DateTime BirthDay, string IDCardNumber, string Address, string Email, bool isEmailValided, string QQ, bool IsQQValided, string Telephone, string Mobile, bool isMobileValided, bool isPrivacy, bool isCanLogin, bool isAllowWinScore, short UserType, short BankType, string BankName, string BankCardNumber, double ScoringOfSelfBuy, double ScoringOfCommendBuy, short Level, string AlipayID, string AlipayName, bool isAlipayNameValided, double PromotionMemberBonusScale, double PromotionSiteBonusScale, bool IsCrossLogin, string Reason, ref int ReturnValue, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_UserEditByID(conn, ref ds, SiteID, UserID, Name, RealityName, Password, PasswordAdv, CityID, Sex, BirthDay, IDCardNumber, Address, Email, isEmailValided, QQ, IsQQValided, Telephone, Mobile, isMobileValided, isPrivacy, isCanLogin, isAllowWinScore, UserType, BankType, BankName, BankCardNumber, ScoringOfSelfBuy, ScoringOfCommendBuy, Level, AlipayID, AlipayName, isAlipayNameValided, PromotionMemberBonusScale, PromotionSiteBonusScale, IsCrossLogin, Reason, ref ReturnValue, ref ReturnDescription);
        }

        public static int P_UserEditByID(SqlConnection conn, ref DataSet ds, long SiteID, long UserID, string Name, string RealityName, string Password, string PasswordAdv, int CityID, string Sex, DateTime BirthDay, string IDCardNumber, string Address, string Email, bool isEmailValided, string QQ, bool IsQQValided, string Telephone, string Mobile, bool isMobileValided, bool isPrivacy, bool isCanLogin, bool isAllowWinScore, short UserType, short BankType, string BankName, string BankCardNumber, double ScoringOfSelfBuy, double ScoringOfCommendBuy, short Level, string AlipayID, string AlipayName, bool isAlipayNameValided, double PromotionMemberBonusScale, double PromotionSiteBonusScale, bool IsCrossLogin, string Reason, ref int ReturnValue, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(conn, "P_UserEditByID", ref ds, ref Outputs,
                new MSSQL.Parameter("SiteID", SqlDbType.BigInt, 0, ParameterDirection.Input, SiteID),
                new MSSQL.Parameter("UserID", SqlDbType.BigInt, 0, ParameterDirection.Input, UserID),
                new MSSQL.Parameter("Name", SqlDbType.VarChar, 0, ParameterDirection.Input, Name),
                new MSSQL.Parameter("RealityName", SqlDbType.VarChar, 0, ParameterDirection.Input, RealityName),
                new MSSQL.Parameter("Password", SqlDbType.VarChar, 0, ParameterDirection.Input, Password),
                new MSSQL.Parameter("PasswordAdv", SqlDbType.VarChar, 0, ParameterDirection.Input, PasswordAdv),
                new MSSQL.Parameter("CityID", SqlDbType.Int, 0, ParameterDirection.Input, CityID),
                new MSSQL.Parameter("Sex", SqlDbType.VarChar, 0, ParameterDirection.Input, Sex),
                new MSSQL.Parameter("BirthDay", SqlDbType.DateTime, 0, ParameterDirection.Input, BirthDay),
                new MSSQL.Parameter("IDCardNumber", SqlDbType.VarChar, 0, ParameterDirection.Input, IDCardNumber),
                new MSSQL.Parameter("Address", SqlDbType.VarChar, 0, ParameterDirection.Input, Address),
                new MSSQL.Parameter("Email", SqlDbType.VarChar, 0, ParameterDirection.Input, Email),
                new MSSQL.Parameter("isEmailValided", SqlDbType.Bit, 0, ParameterDirection.Input, isEmailValided),
                new MSSQL.Parameter("QQ", SqlDbType.VarChar, 0, ParameterDirection.Input, QQ),
                new MSSQL.Parameter("IsQQValided", SqlDbType.Bit, 0, ParameterDirection.Input, IsQQValided),
                new MSSQL.Parameter("Telephone", SqlDbType.VarChar, 0, ParameterDirection.Input, Telephone),
                new MSSQL.Parameter("Mobile", SqlDbType.VarChar, 0, ParameterDirection.Input, Mobile),
                new MSSQL.Parameter("isMobileValided", SqlDbType.Bit, 0, ParameterDirection.Input, isMobileValided),
                new MSSQL.Parameter("isPrivacy", SqlDbType.Bit, 0, ParameterDirection.Input, isPrivacy),
                new MSSQL.Parameter("isCanLogin", SqlDbType.Bit, 0, ParameterDirection.Input, isCanLogin),
                new MSSQL.Parameter("isAllowWinScore", SqlDbType.Bit, 0, ParameterDirection.Input, isAllowWinScore),
                new MSSQL.Parameter("UserType", SqlDbType.SmallInt, 0, ParameterDirection.Input, UserType),
                new MSSQL.Parameter("BankType", SqlDbType.SmallInt, 0, ParameterDirection.Input, BankType),
                new MSSQL.Parameter("BankName", SqlDbType.VarChar, 0, ParameterDirection.Input, BankName),
                new MSSQL.Parameter("BankCardNumber", SqlDbType.VarChar, 0, ParameterDirection.Input, BankCardNumber),
                new MSSQL.Parameter("ScoringOfSelfBuy", SqlDbType.Float, 0, ParameterDirection.Input, ScoringOfSelfBuy),
                new MSSQL.Parameter("ScoringOfCommendBuy", SqlDbType.Float, 0, ParameterDirection.Input, ScoringOfCommendBuy),
                new MSSQL.Parameter("Level", SqlDbType.SmallInt, 0, ParameterDirection.Input, Level),
                new MSSQL.Parameter("AlipayID", SqlDbType.VarChar, 0, ParameterDirection.Input, AlipayID),
                new MSSQL.Parameter("AlipayName", SqlDbType.VarChar, 0, ParameterDirection.Input, AlipayName),
                new MSSQL.Parameter("isAlipayNameValided", SqlDbType.Bit, 0, ParameterDirection.Input, isAlipayNameValided),
                new MSSQL.Parameter("PromotionMemberBonusScale", SqlDbType.Float, 0, ParameterDirection.Input, PromotionMemberBonusScale),
                new MSSQL.Parameter("PromotionSiteBonusScale", SqlDbType.Float, 0, ParameterDirection.Input, PromotionSiteBonusScale),
                new MSSQL.Parameter("IsCrossLogin", SqlDbType.Bit, 0, ParameterDirection.Input, IsCrossLogin),
                new MSSQL.Parameter("Reason", SqlDbType.VarChar, 0, ParameterDirection.Input, Reason),
                new MSSQL.Parameter("ReturnValue", SqlDbType.Int, 4, ParameterDirection.Output, ReturnValue),
                new MSSQL.Parameter("ReturnDescription", SqlDbType.VarChar, 100, ParameterDirection.Output, ReturnDescription)
                );

            try
            {
                ReturnValue = System.Convert.ToInt32(Outputs["ReturnValue"]);
            }
            catch { }

            try
            {
                ReturnDescription = System.Convert.ToString(Outputs["ReturnDescription"]);
            }
            catch { }

            return CallResult;
        }

        public static int P_UserEditByName(SqlConnection conn, long SiteID, long UserID, string Name, string RealityName, string Password, string PasswordAdv, int CityID, string Sex, DateTime BirthDay, string IDCardNumber, string Address, string Email, bool isEmailValided, string QQ, bool IsQQValided, string Telephone, string Mobile, bool isMobileValided, bool isPrivacy, bool isCanLogin, bool isAllowWinScore, short UserType, short BankType, string BankName, string BankCardNumber, double ScoringOfSelfBuy, double ScoringOfCommendBuy, short Level, string AlipayID, string AlipayName, bool isAlipayNameValided, double PromotionMemberBonusScale, double PromotionSiteBonusScale, bool IsCrossLogin, string Reason, ref int ReturnValue, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_UserEditByName(conn, ref ds, SiteID, UserID, Name, RealityName, Password, PasswordAdv, CityID, Sex, BirthDay, IDCardNumber, Address, Email, isEmailValided, QQ, IsQQValided, Telephone, Mobile, isMobileValided, isPrivacy, isCanLogin, isAllowWinScore, UserType, BankType, BankName, BankCardNumber, ScoringOfSelfBuy, ScoringOfCommendBuy, Level, AlipayID, AlipayName, isAlipayNameValided, PromotionMemberBonusScale, PromotionSiteBonusScale, IsCrossLogin, Reason, ref ReturnValue, ref ReturnDescription);
        }

        public static int P_UserEditByName(SqlConnection conn, ref DataSet ds, long SiteID, long UserID, string Name, string RealityName, string Password, string PasswordAdv, int CityID, string Sex, DateTime BirthDay, string IDCardNumber, string Address, string Email, bool isEmailValided, string QQ, bool IsQQValided, string Telephone, string Mobile, bool isMobileValided, bool isPrivacy, bool isCanLogin, bool isAllowWinScore, short UserType, short BankType, string BankName, string BankCardNumber, double ScoringOfSelfBuy, double ScoringOfCommendBuy, short Level, string AlipayID, string AlipayName, bool isAlipayNameValided, double PromotionMemberBonusScale, double PromotionSiteBonusScale, bool IsCrossLogin, string Reason, ref int ReturnValue, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(conn, "P_UserEditByName", ref ds, ref Outputs,
                new MSSQL.Parameter("SiteID", SqlDbType.BigInt, 0, ParameterDirection.Input, SiteID),
                new MSSQL.Parameter("UserID", SqlDbType.BigInt, 0, ParameterDirection.Input, UserID),
                new MSSQL.Parameter("Name", SqlDbType.VarChar, 0, ParameterDirection.Input, Name),
                new MSSQL.Parameter("RealityName", SqlDbType.VarChar, 0, ParameterDirection.Input, RealityName),
                new MSSQL.Parameter("Password", SqlDbType.VarChar, 0, ParameterDirection.Input, Password),
                new MSSQL.Parameter("PasswordAdv", SqlDbType.VarChar, 0, ParameterDirection.Input, PasswordAdv),
                new MSSQL.Parameter("CityID", SqlDbType.Int, 0, ParameterDirection.Input, CityID),
                new MSSQL.Parameter("Sex", SqlDbType.VarChar, 0, ParameterDirection.Input, Sex),
                new MSSQL.Parameter("BirthDay", SqlDbType.DateTime, 0, ParameterDirection.Input, BirthDay),
                new MSSQL.Parameter("IDCardNumber", SqlDbType.VarChar, 0, ParameterDirection.Input, IDCardNumber),
                new MSSQL.Parameter("Address", SqlDbType.VarChar, 0, ParameterDirection.Input, Address),
                new MSSQL.Parameter("Email", SqlDbType.VarChar, 0, ParameterDirection.Input, Email),
                new MSSQL.Parameter("isEmailValided", SqlDbType.Bit, 0, ParameterDirection.Input, isEmailValided),
                new MSSQL.Parameter("QQ", SqlDbType.VarChar, 0, ParameterDirection.Input, QQ),
                new MSSQL.Parameter("IsQQValided", SqlDbType.Bit, 0, ParameterDirection.Input, IsQQValided),
                new MSSQL.Parameter("Telephone", SqlDbType.VarChar, 0, ParameterDirection.Input, Telephone),
                new MSSQL.Parameter("Mobile", SqlDbType.VarChar, 0, ParameterDirection.Input, Mobile),
                new MSSQL.Parameter("isMobileValided", SqlDbType.Bit, 0, ParameterDirection.Input, isMobileValided),
                new MSSQL.Parameter("isPrivacy", SqlDbType.Bit, 0, ParameterDirection.Input, isPrivacy),
                new MSSQL.Parameter("isCanLogin", SqlDbType.Bit, 0, ParameterDirection.Input, isCanLogin),
                new MSSQL.Parameter("isAllowWinScore", SqlDbType.Bit, 0, ParameterDirection.Input, isAllowWinScore),
                new MSSQL.Parameter("UserType", SqlDbType.SmallInt, 0, ParameterDirection.Input, UserType),
                new MSSQL.Parameter("BankType", SqlDbType.SmallInt, 0, ParameterDirection.Input, BankType),
                new MSSQL.Parameter("BankName", SqlDbType.VarChar, 0, ParameterDirection.Input, BankName),
                new MSSQL.Parameter("BankCardNumber", SqlDbType.VarChar, 0, ParameterDirection.Input, BankCardNumber),
                new MSSQL.Parameter("ScoringOfSelfBuy", SqlDbType.Float, 0, ParameterDirection.Input, ScoringOfSelfBuy),
                new MSSQL.Parameter("ScoringOfCommendBuy", SqlDbType.Float, 0, ParameterDirection.Input, ScoringOfCommendBuy),
                new MSSQL.Parameter("Level", SqlDbType.SmallInt, 0, ParameterDirection.Input, Level),
                new MSSQL.Parameter("AlipayID", SqlDbType.VarChar, 0, ParameterDirection.Input, AlipayID),
                new MSSQL.Parameter("AlipayName", SqlDbType.VarChar, 0, ParameterDirection.Input, AlipayName),
                new MSSQL.Parameter("isAlipayNameValided", SqlDbType.Bit, 0, ParameterDirection.Input, isAlipayNameValided),
                new MSSQL.Parameter("PromotionMemberBonusScale", SqlDbType.Float, 0, ParameterDirection.Input, PromotionMemberBonusScale),
                new MSSQL.Parameter("PromotionSiteBonusScale", SqlDbType.Float, 0, ParameterDirection.Input, PromotionSiteBonusScale),
                new MSSQL.Parameter("IsCrossLogin", SqlDbType.Bit, 0, ParameterDirection.Input, IsCrossLogin),
                new MSSQL.Parameter("Reason", SqlDbType.VarChar, 0, ParameterDirection.Input, Reason),
                new MSSQL.Parameter("ReturnValue", SqlDbType.Int, 4, ParameterDirection.Output, ReturnValue),
                new MSSQL.Parameter("ReturnDescription", SqlDbType.VarChar, 100, ParameterDirection.Output, ReturnDescription)
                );

            try
            {
                ReturnValue = System.Convert.ToInt32(Outputs["ReturnValue"]);
            }
            catch { }

            try
            {
                ReturnDescription = System.Convert.ToString(Outputs["ReturnDescription"]);
            }
            catch { }

            return CallResult;
        }

        public static int P_UserForInitiateFollowSchemeDelete(SqlConnection conn, long SiteID, long UsersForInitiateFollowSchemeID, ref int ReturnValue, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_UserForInitiateFollowSchemeDelete(conn, ref ds, SiteID, UsersForInitiateFollowSchemeID, ref ReturnValue, ref ReturnDescription);
        }

        public static int P_UserForInitiateFollowSchemeDelete(SqlConnection conn, ref DataSet ds, long SiteID, long UsersForInitiateFollowSchemeID, ref int ReturnValue, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(conn, "P_UserForInitiateFollowSchemeDelete", ref ds, ref Outputs,
                new MSSQL.Parameter("SiteID", SqlDbType.BigInt, 0, ParameterDirection.Input, SiteID),
                new MSSQL.Parameter("UsersForInitiateFollowSchemeID", SqlDbType.BigInt, 0, ParameterDirection.Input, UsersForInitiateFollowSchemeID),
                new MSSQL.Parameter("ReturnValue", SqlDbType.Int, 4, ParameterDirection.Output, ReturnValue),
                new MSSQL.Parameter("ReturnDescription", SqlDbType.VarChar, 100, ParameterDirection.Output, ReturnDescription)
                );

            try
            {
                ReturnValue = System.Convert.ToInt32(Outputs["ReturnValue"]);
            }
            catch { }

            try
            {
                ReturnDescription = System.Convert.ToString(Outputs["ReturnDescription"]);
            }
            catch { }

            return CallResult;
        }

        public static int P_UserForInitiateFollowSchemeEdit(SqlConnection conn, long SiteID, long UsersForInitiateFollowSchemeID, string Description, int MaxNumberOf, ref int ReturnValue, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_UserForInitiateFollowSchemeEdit(conn, ref ds, SiteID, UsersForInitiateFollowSchemeID, Description, MaxNumberOf, ref ReturnValue, ref ReturnDescription);
        }

        public static int P_UserForInitiateFollowSchemeEdit(SqlConnection conn, ref DataSet ds, long SiteID, long UsersForInitiateFollowSchemeID, string Description, int MaxNumberOf, ref int ReturnValue, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(conn, "P_UserForInitiateFollowSchemeEdit", ref ds, ref Outputs,
                new MSSQL.Parameter("SiteID", SqlDbType.BigInt, 0, ParameterDirection.Input, SiteID),
                new MSSQL.Parameter("UsersForInitiateFollowSchemeID", SqlDbType.BigInt, 0, ParameterDirection.Input, UsersForInitiateFollowSchemeID),
                new MSSQL.Parameter("Description", SqlDbType.VarChar, 0, ParameterDirection.Input, Description),
                new MSSQL.Parameter("MaxNumberOf", SqlDbType.Int, 0, ParameterDirection.Input, MaxNumberOf),
                new MSSQL.Parameter("ReturnValue", SqlDbType.Int, 4, ParameterDirection.Output, ReturnValue),
                new MSSQL.Parameter("ReturnDescription", SqlDbType.VarChar, 100, ParameterDirection.Output, ReturnDescription)
                );

            try
            {
                ReturnValue = System.Convert.ToInt32(Outputs["ReturnValue"]);
            }
            catch { }

            try
            {
                ReturnDescription = System.Convert.ToString(Outputs["ReturnDescription"]);
            }
            catch { }

            return CallResult;
        }

        public static int P_UserForInitiateFollowSchemeTry(SqlConnection conn, long SiteID, long UserID, int PlayTypeID, string Description, ref long NewUserForInitiateFollowSchemeTryID, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_UserForInitiateFollowSchemeTry(conn, ref ds, SiteID, UserID, PlayTypeID, Description, ref NewUserForInitiateFollowSchemeTryID, ref ReturnDescription);
        }

        public static int P_UserForInitiateFollowSchemeTry(SqlConnection conn, ref DataSet ds, long SiteID, long UserID, int PlayTypeID, string Description, ref long NewUserForInitiateFollowSchemeTryID, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(conn, "P_UserForInitiateFollowSchemeTry", ref ds, ref Outputs,
                new MSSQL.Parameter("SiteID", SqlDbType.BigInt, 0, ParameterDirection.Input, SiteID),
                new MSSQL.Parameter("UserID", SqlDbType.BigInt, 0, ParameterDirection.Input, UserID),
                new MSSQL.Parameter("PlayTypeID", SqlDbType.Int, 0, ParameterDirection.Input, PlayTypeID),
                new MSSQL.Parameter("Description", SqlDbType.VarChar, 0, ParameterDirection.Input, Description),
                new MSSQL.Parameter("NewUserForInitiateFollowSchemeTryID", SqlDbType.BigInt, 8, ParameterDirection.Output, NewUserForInitiateFollowSchemeTryID),
                new MSSQL.Parameter("ReturnDescription", SqlDbType.VarChar, 100, ParameterDirection.Output, ReturnDescription)
                );

            try
            {
                NewUserForInitiateFollowSchemeTryID = System.Convert.ToInt64(Outputs["NewUserForInitiateFollowSchemeTryID"]);
            }
            catch { }

            try
            {
                ReturnDescription = System.Convert.ToString(Outputs["ReturnDescription"]);
            }
            catch { }

            return CallResult;
        }

        public static int P_UserForInitiateFollowSchemeTryHandle(SqlConnection conn, long SiteID, long UserForInitiateFollowSchemeTryID, short HandleResult, string Description, int MaxNumberOf, ref int ReturnValue, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_UserForInitiateFollowSchemeTryHandle(conn, ref ds, SiteID, UserForInitiateFollowSchemeTryID, HandleResult, Description, MaxNumberOf, ref ReturnValue, ref ReturnDescription);
        }

        public static int P_UserForInitiateFollowSchemeTryHandle(SqlConnection conn, ref DataSet ds, long SiteID, long UserForInitiateFollowSchemeTryID, short HandleResult, string Description, int MaxNumberOf, ref int ReturnValue, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(conn, "P_UserForInitiateFollowSchemeTryHandle", ref ds, ref Outputs,
                new MSSQL.Parameter("SiteID", SqlDbType.BigInt, 0, ParameterDirection.Input, SiteID),
                new MSSQL.Parameter("UserForInitiateFollowSchemeTryID", SqlDbType.BigInt, 0, ParameterDirection.Input, UserForInitiateFollowSchemeTryID),
                new MSSQL.Parameter("HandleResult", SqlDbType.SmallInt, 0, ParameterDirection.Input, HandleResult),
                new MSSQL.Parameter("Description", SqlDbType.VarChar, 0, ParameterDirection.Input, Description),
                new MSSQL.Parameter("MaxNumberOf", SqlDbType.Int, 0, ParameterDirection.Input, MaxNumberOf),
                new MSSQL.Parameter("ReturnValue", SqlDbType.Int, 4, ParameterDirection.Output, ReturnValue),
                new MSSQL.Parameter("ReturnDescription", SqlDbType.VarChar, 100, ParameterDirection.Output, ReturnDescription)
                );

            try
            {
                ReturnValue = System.Convert.ToInt32(Outputs["ReturnValue"]);
            }
            catch { }

            try
            {
                ReturnDescription = System.Convert.ToString(Outputs["ReturnDescription"]);
            }
            catch { }

            return CallResult;
        }

        public static int P_UserLogOut(SqlConnection conn, long SiteID, long UserID, string Reason, ref int ReturnValue, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_UserLogOut(conn, ref ds, SiteID, UserID, Reason, ref ReturnValue, ref ReturnDescription);
        }

        public static int P_UserLogOut(SqlConnection conn, ref DataSet ds, long SiteID, long UserID, string Reason, ref int ReturnValue, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(conn, "P_UserLogOut", ref ds, ref Outputs,
                new MSSQL.Parameter("SiteID", SqlDbType.BigInt, 0, ParameterDirection.Input, SiteID),
                new MSSQL.Parameter("UserID", SqlDbType.BigInt, 0, ParameterDirection.Input, UserID),
                new MSSQL.Parameter("Reason", SqlDbType.VarChar, 0, ParameterDirection.Input, Reason),
                new MSSQL.Parameter("ReturnValue", SqlDbType.Int, 4, ParameterDirection.Output, ReturnValue),
                new MSSQL.Parameter("ReturnDescription", SqlDbType.VarChar, 100, ParameterDirection.Output, ReturnDescription)
                );

            try
            {
                ReturnValue = System.Convert.ToInt32(Outputs["ReturnValue"]);
            }
            catch { }

            try
            {
                ReturnDescription = System.Convert.ToString(Outputs["ReturnDescription"]);
            }
            catch { }

            return CallResult;
        }

        public static int P_UserPaySMSCost(SqlConnection conn, long SiteID, long UserID, string Mobile, int Num, ref int ReturnValue, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_UserPaySMSCost(conn, ref ds, SiteID, UserID, Mobile, Num, ref ReturnValue, ref ReturnDescription);
        }

        public static int P_UserPaySMSCost(SqlConnection conn, ref DataSet ds, long SiteID, long UserID, string Mobile, int Num, ref int ReturnValue, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(conn, "P_UserPaySMSCost", ref ds, ref Outputs,
                new MSSQL.Parameter("SiteID", SqlDbType.BigInt, 0, ParameterDirection.Input, SiteID),
                new MSSQL.Parameter("UserID", SqlDbType.BigInt, 0, ParameterDirection.Input, UserID),
                new MSSQL.Parameter("Mobile", SqlDbType.VarChar, 0, ParameterDirection.Input, Mobile),
                new MSSQL.Parameter("Num", SqlDbType.Int, 0, ParameterDirection.Input, Num),
                new MSSQL.Parameter("ReturnValue", SqlDbType.Int, 4, ParameterDirection.Output, ReturnValue),
                new MSSQL.Parameter("ReturnDescription", SqlDbType.VarChar, 100, ParameterDirection.Output, ReturnDescription)
                );

            try
            {
                ReturnValue = System.Convert.ToInt32(Outputs["ReturnValue"]);
            }
            catch { }

            try
            {
                ReturnDescription = System.Convert.ToString(Outputs["ReturnDescription"]);
            }
            catch { }

            return CallResult;
        }

        public static int P_UserQQBind(SqlConnection conn, long UserID, string QQ, ref int ReturnValue, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_UserQQBind(conn, ref ds, UserID, QQ, ref ReturnValue, ref ReturnDescription);
        }

        public static int P_UserQQBind(SqlConnection conn, ref DataSet ds, long UserID, string QQ, ref int ReturnValue, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(conn, "P_UserQQBind", ref ds, ref Outputs,
                new MSSQL.Parameter("UserID", SqlDbType.BigInt, 0, ParameterDirection.Input, UserID),
                new MSSQL.Parameter("QQ", SqlDbType.VarChar, 0, ParameterDirection.Input, QQ),
                new MSSQL.Parameter("ReturnValue", SqlDbType.Int, 4, ParameterDirection.Output, ReturnValue),
                new MSSQL.Parameter("ReturnDescription", SqlDbType.VarChar, 50, ParameterDirection.Output, ReturnDescription)
                );

            try
            {
                ReturnValue = System.Convert.ToInt32(Outputs["ReturnValue"]);
            }
            catch { }

            try
            {
                ReturnDescription = System.Convert.ToString(Outputs["ReturnDescription"]);
            }
            catch { }

            return CallResult;
        }

        public static int P_ViewUserBonus(SqlConnection conn, long userid, ref int ReturnValue, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_ViewUserBonus(conn, ref ds, userid, ref ReturnValue, ref ReturnDescription);
        }

        public static int P_ViewUserBonus(SqlConnection conn, ref DataSet ds, long userid, ref int ReturnValue, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(conn, "P_ViewUserBonus", ref ds, ref Outputs,
                new MSSQL.Parameter("userid", SqlDbType.BigInt, 0, ParameterDirection.Input, userid),
                new MSSQL.Parameter("ReturnValue", SqlDbType.Int, 4, ParameterDirection.Output, ReturnValue),
                new MSSQL.Parameter("ReturnDescription", SqlDbType.VarChar, 100, ParameterDirection.Output, ReturnDescription)
                );

            try
            {
                ReturnValue = System.Convert.ToInt32(Outputs["ReturnValue"]);
            }
            catch { }

            try
            {
                ReturnDescription = System.Convert.ToString(Outputs["ReturnDescription"]);
            }
            catch { }

            return CallResult;
        }

        public static int P_Win(SqlConnection conn, long IsuseID, string WinLotteryNumber, string OpenAffiche, long OpenOperatorID, bool isEndTheIsuse, ref int SchemeCount, ref int QuashCount, ref int WinCount, ref int WinNoBuyCount, ref bool isEndOpen, ref int ReturnValue, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_Win(conn, ref ds, IsuseID, WinLotteryNumber, OpenAffiche, OpenOperatorID, isEndTheIsuse, ref SchemeCount, ref QuashCount, ref WinCount, ref WinNoBuyCount, ref isEndOpen, ref ReturnValue, ref ReturnDescription);
        }

        public static int P_Win(SqlConnection conn, ref DataSet ds, long IsuseID, string WinLotteryNumber, string OpenAffiche, long OpenOperatorID, bool isEndTheIsuse, ref int SchemeCount, ref int QuashCount, ref int WinCount, ref int WinNoBuyCount, ref bool isEndOpen, ref int ReturnValue, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(conn, "P_Win", ref ds, ref Outputs,
                new MSSQL.Parameter("IsuseID", SqlDbType.BigInt, 0, ParameterDirection.Input, IsuseID),
                new MSSQL.Parameter("WinLotteryNumber", SqlDbType.VarChar, 0, ParameterDirection.Input, WinLotteryNumber),
                new MSSQL.Parameter("OpenAffiche", SqlDbType.VarChar, 0, ParameterDirection.Input, OpenAffiche),
                new MSSQL.Parameter("OpenOperatorID", SqlDbType.BigInt, 0, ParameterDirection.Input, OpenOperatorID),
                new MSSQL.Parameter("isEndTheIsuse", SqlDbType.Bit, 0, ParameterDirection.Input, isEndTheIsuse),
                new MSSQL.Parameter("SchemeCount", SqlDbType.Int, 4, ParameterDirection.Output, SchemeCount),
                new MSSQL.Parameter("QuashCount", SqlDbType.Int, 4, ParameterDirection.Output, QuashCount),
                new MSSQL.Parameter("WinCount", SqlDbType.Int, 4, ParameterDirection.Output, WinCount),
                new MSSQL.Parameter("WinNoBuyCount", SqlDbType.Int, 4, ParameterDirection.Output, WinNoBuyCount),
                new MSSQL.Parameter("isEndOpen", SqlDbType.Bit, 1, ParameterDirection.Output, isEndOpen),
                new MSSQL.Parameter("ReturnValue", SqlDbType.Int, 4, ParameterDirection.Output, ReturnValue),
                new MSSQL.Parameter("ReturnDescription", SqlDbType.VarChar, 100, ParameterDirection.Output, ReturnDescription)
                );

            try
            {
                SchemeCount = System.Convert.ToInt32(Outputs["SchemeCount"]);
            }
            catch { }

            try
            {
                QuashCount = System.Convert.ToInt32(Outputs["QuashCount"]);
            }
            catch { }

            try
            {
                WinCount = System.Convert.ToInt32(Outputs["WinCount"]);
            }
            catch { }

            try
            {
                WinNoBuyCount = System.Convert.ToInt32(Outputs["WinNoBuyCount"]);
            }
            catch { }

            try
            {
                isEndOpen = System.Convert.ToBoolean(Outputs["isEndOpen"]);
            }
            catch { }

            try
            {
                ReturnValue = System.Convert.ToInt32(Outputs["ReturnValue"]);
            }
            catch { }

            try
            {
                ReturnDescription = System.Convert.ToString(Outputs["ReturnDescription"]);
            }
            catch { }

            return CallResult;
        }

        public static int P_WinByOpenManual(SqlConnection conn, long SiteID, long SchemeID, double WinMoney, double WinMoneyNoWithTax, string WinDescription, long OpenOperatorID, ref int ReturnValue, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_WinByOpenManual(conn, ref ds, SiteID, SchemeID, WinMoney, WinMoneyNoWithTax, WinDescription, OpenOperatorID, ref ReturnValue, ref ReturnDescription);
        }

        public static int P_WinByOpenManual(SqlConnection conn, ref DataSet ds, long SiteID, long SchemeID, double WinMoney, double WinMoneyNoWithTax, string WinDescription, long OpenOperatorID, ref int ReturnValue, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(conn, "P_WinByOpenManual", ref ds, ref Outputs,
                new MSSQL.Parameter("SiteID", SqlDbType.BigInt, 0, ParameterDirection.Input, SiteID),
                new MSSQL.Parameter("SchemeID", SqlDbType.BigInt, 0, ParameterDirection.Input, SchemeID),
                new MSSQL.Parameter("WinMoney", SqlDbType.Money, 0, ParameterDirection.Input, WinMoney),
                new MSSQL.Parameter("WinMoneyNoWithTax", SqlDbType.Money, 0, ParameterDirection.Input, WinMoneyNoWithTax),
                new MSSQL.Parameter("WinDescription", SqlDbType.VarChar, 0, ParameterDirection.Input, WinDescription),
                new MSSQL.Parameter("OpenOperatorID", SqlDbType.BigInt, 0, ParameterDirection.Input, OpenOperatorID),
                new MSSQL.Parameter("ReturnValue", SqlDbType.Int, 4, ParameterDirection.Output, ReturnValue),
                new MSSQL.Parameter("ReturnDescription", SqlDbType.VarChar, 100, ParameterDirection.Output, ReturnDescription)
                );

            try
            {
                ReturnValue = System.Convert.ToInt32(Outputs["ReturnValue"]);
            }
            catch { }

            try
            {
                ReturnDescription = System.Convert.ToString(Outputs["ReturnDescription"]);
            }
            catch { }

            return CallResult;
        }

        public static int P_WriteSchemeChatContents(SqlConnection conn, long SiteID, long SchemeID, long FromUserID, long ToUserID, short Type, string Content, ref int ReturnValue, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_WriteSchemeChatContents(conn, ref ds, SiteID, SchemeID, FromUserID, ToUserID, Type, Content, ref ReturnValue, ref ReturnDescription);
        }

        public static int P_WriteSchemeChatContents(SqlConnection conn, ref DataSet ds, long SiteID, long SchemeID, long FromUserID, long ToUserID, short Type, string Content, ref int ReturnValue, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(conn, "P_WriteSchemeChatContents", ref ds, ref Outputs,
                new MSSQL.Parameter("SiteID", SqlDbType.BigInt, 0, ParameterDirection.Input, SiteID),
                new MSSQL.Parameter("SchemeID", SqlDbType.BigInt, 0, ParameterDirection.Input, SchemeID),
                new MSSQL.Parameter("FromUserID", SqlDbType.BigInt, 0, ParameterDirection.Input, FromUserID),
                new MSSQL.Parameter("ToUserID", SqlDbType.BigInt, 0, ParameterDirection.Input, ToUserID),
                new MSSQL.Parameter("Type", SqlDbType.SmallInt, 0, ParameterDirection.Input, Type),
                new MSSQL.Parameter("Content", SqlDbType.VarChar, 0, ParameterDirection.Input, Content),
                new MSSQL.Parameter("ReturnValue", SqlDbType.Int, 4, ParameterDirection.Output, ReturnValue),
                new MSSQL.Parameter("ReturnDescription", SqlDbType.VarChar, 100, ParameterDirection.Output, ReturnDescription)
                );

            try
            {
                ReturnValue = System.Convert.ToInt32(Outputs["ReturnValue"]);
            }
            catch { }

            try
            {
                ReturnDescription = System.Convert.ToString(Outputs["ReturnDescription"]);
            }
            catch { }

            return CallResult;
        }

        public static int P_WriteSMS(SqlConnection conn, long SiteID, long SMSID, string From, string To, string Content, ref long NewSMSID)
        {
            DataSet ds = null;

            return P_WriteSMS(conn, ref ds, SiteID, SMSID, From, To, Content, ref NewSMSID);
        }

        public static int P_WriteSMS(SqlConnection conn, ref DataSet ds, long SiteID, long SMSID, string From, string To, string Content, ref long NewSMSID)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(conn, "P_WriteSMS", ref ds, ref Outputs,
                new MSSQL.Parameter("SiteID", SqlDbType.BigInt, 0, ParameterDirection.Input, SiteID),
                new MSSQL.Parameter("SMSID", SqlDbType.BigInt, 0, ParameterDirection.Input, SMSID),
                new MSSQL.Parameter("From", SqlDbType.VarChar, 0, ParameterDirection.Input, From),
                new MSSQL.Parameter("To", SqlDbType.VarChar, 0, ParameterDirection.Input, To),
                new MSSQL.Parameter("Content", SqlDbType.VarChar, 0, ParameterDirection.Input, Content),
                new MSSQL.Parameter("NewSMSID", SqlDbType.BigInt, 8, ParameterDirection.Output, NewSMSID)
                );

            try
            {
                NewSMSID = System.Convert.ToInt64(Outputs["NewSMSID"]);
            }
            catch { }

            return CallResult;
        }

        public static int P_WriteStationSMS(SqlConnection conn, long SiteID, long SourceID, long AimID, short Type, string Content, ref long NewSMSID)
        {
            DataSet ds = null;

            return P_WriteStationSMS(conn, ref ds, SiteID, SourceID, AimID, Type, Content, ref NewSMSID);
        }

        public static int P_WriteStationSMS(SqlConnection conn, ref DataSet ds, long SiteID, long SourceID, long AimID, short Type, string Content, ref long NewSMSID)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(conn, "P_WriteStationSMS", ref ds, ref Outputs,
                new MSSQL.Parameter("SiteID", SqlDbType.BigInt, 0, ParameterDirection.Input, SiteID),
                new MSSQL.Parameter("SourceID", SqlDbType.BigInt, 0, ParameterDirection.Input, SourceID),
                new MSSQL.Parameter("AimID", SqlDbType.BigInt, 0, ParameterDirection.Input, AimID),
                new MSSQL.Parameter("Type", SqlDbType.SmallInt, 0, ParameterDirection.Input, Type),
                new MSSQL.Parameter("Content", SqlDbType.VarChar, 0, ParameterDirection.Input, Content),
                new MSSQL.Parameter("NewSMSID", SqlDbType.BigInt, 8, ParameterDirection.Output, NewSMSID)
                );

            try
            {
                NewSMSID = System.Convert.ToInt64(Outputs["NewSMSID"]);
            }
            catch { }

            return CallResult;
        }

        public static int P_WriteSystemLog(SqlConnection conn, long SiteID, long UserID, string IPAddress, short Description)
        {
            DataSet ds = null;

            return P_WriteSystemLog(conn, ref ds, SiteID, UserID, IPAddress, Description);
        }

        public static int P_WriteSystemLog(SqlConnection conn, ref DataSet ds, long SiteID, long UserID, string IPAddress, short Description)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(conn, "P_WriteSystemLog", ref ds, ref Outputs,
                new MSSQL.Parameter("SiteID", SqlDbType.BigInt, 0, ParameterDirection.Input, SiteID),
                new MSSQL.Parameter("UserID", SqlDbType.BigInt, 0, ParameterDirection.Input, UserID),
                new MSSQL.Parameter("IPAddress", SqlDbType.VarChar, 0, ParameterDirection.Input, IPAddress),
                new MSSQL.Parameter("Description", SqlDbType.SmallInt, 0, ParameterDirection.Input, Description)
                );

            return CallResult;
        }
    }
}
