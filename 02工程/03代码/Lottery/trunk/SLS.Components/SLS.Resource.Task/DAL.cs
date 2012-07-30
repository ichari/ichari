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
        public class Field
        {
            private object _Value;

            public object Parent;
            public string Name;
            public string CanonicalIdentifierName;
            public SqlDbType DbType;
            public bool ReadOnly;
            public object Value
            {
                get
                {
                    return _Value;
                }
                set
                {
                    if (ReadOnly)
                    {
                        throw new Exception("the member “" + Name + "” is ReadOnly.");
                    }

                    _Value = value;

                    if (Parent != null)
                    {
                        ((TableBase)Parent).Fields.Add(this);
                    }
                }
            }

            public Field(object parent, string name, string canonicalidentifiername, SqlDbType dbtype, bool _readonly)
            {
                Parent = parent;
                Name = name;
                CanonicalIdentifierName = canonicalidentifiername;
                DbType = dbtype;
                ReadOnly = _readonly;
            }
        }

        public class FieldCollection
        {
            private ArrayList al = new ArrayList();

            public int Count
            {
                get
                {
                    return al.Count;
                }
            }

            public void Add(object obj)
            {
                al.Add(obj);
            }

            public void Clear()
            {
                al.Clear();
            }

            public Field this[int Index]
            {
                get
                {
                    if ((Count < 1) || (Index < 0) || (Index > Count))
                    {
                        return null;
                    }

                    return (Field)al[Index];
                }
            }
        }

        public class TableBase
        {
            public string TableName = "";
            public FieldCollection Fields = new FieldCollection();

            public DataTable Open(string ConnectionString, string FieldList, string Condition, string Order)
            {
                FieldList = FieldList.Trim();
                Condition = Condition.Trim();
                Order = Order.Trim();

                return MSSQL.Select(ConnectionString, "select " + (FieldList == "" ? "*" : FieldList) + " from [" + TableName + "]" + (Condition == "" ? "" : " where " + Condition) + (Order == "" ? "" : " order by " + Order));
            }

            public long GetCount(string ConnectionString, string Condition)
            {
                Condition = Condition.Trim();

                object Result = MSSQL.ExecuteScalar(ConnectionString, "select count(*) from [" + TableName + "]" + (Condition == "" ? "" : " where " + Condition));

                if (Result == null)
                {
                    return 0;
                }

                return long.Parse(Result.ToString());
            }

            public long Insert(string ConnectionString)
            {
                if (Fields.Count < 1)
                {
                    return -101;
                }

                string InsertFieldsList = "";
                string InsertValuesList = "";
                MSSQL.Parameter[] Parameters = new MSSQL.Parameter[Fields.Count];

                for (int i = 0; i < Fields.Count; i++)
                {
                    if (i > 0)
                    {
                        InsertFieldsList += ", ";
                        InsertValuesList += ", ";
                    }

                    InsertFieldsList += "[" + Fields[i].Name + "]";
                    InsertValuesList += "@" + Fields[i].CanonicalIdentifierName;

                    Parameters[i] = new MSSQL.Parameter(Fields[i].CanonicalIdentifierName, Fields[i].DbType, 0, ParameterDirection.Input, Fields[i].Value);
                }

                string CommandText = "insert into [" + TableName + "] (" + InsertFieldsList + ") values (" + InsertValuesList + "); select isnull(cast(scope_identity() as bigint), -99999999)";

                object objResult = MSSQL.ExecuteScalar(ConnectionString, CommandText, Parameters);

                if (objResult == null)
                {
                    return -102;
                }

                Fields.Clear();

                long Result = (long)objResult;

                if (Result == -99999999)
                {
                    return 0;
                }

                return Result;
            }

            public long Delete(string ConnectionString, string Condition)
            {
                Condition = Condition.Trim();

                object objResult = MSSQL.ExecuteScalar(ConnectionString, "delete from [" + TableName + "]" + (Condition == "" ? "" : " where " + Condition) + "; select isnull(cast(rowcount_big() as bigint), -99999999)");

                if (objResult == null)
                {
                    return -102;
                }

                Fields.Clear();

                long Result = (long)objResult;

                if (Result == -99999999)
                {
                    return 0;
                }

                return Result;
            }

            public long Update(string ConnectionString, string Condition)
            {
                if (Fields.Count < 1)
                {
                    return -101;
                }

                Condition = Condition.Trim();

                string CommandText = "update [" + TableName + "] set ";
                MSSQL.Parameter[] Parameters = new MSSQL.Parameter[Fields.Count];

                for (int i = 0; i < Fields.Count; i++)
                {
                    if (i > 0)
                    {
                        CommandText += ", ";
                    }

                    CommandText += "[" + Fields[i].Name + "] = @" + Fields[i].CanonicalIdentifierName;

                    Parameters[i] = new MSSQL.Parameter(Fields[i].CanonicalIdentifierName, Fields[i].DbType, 0, ParameterDirection.Input, Fields[i].Value);
                }

                if (!String.IsNullOrEmpty(Condition))
                {
                    CommandText += " where " + Condition;
                }

                CommandText += "; select isnull(cast(rowcount_big() as bigint), -99999999)";

                object objResult = MSSQL.ExecuteScalar(ConnectionString, CommandText, Parameters);

                if (objResult == null)
                {
                    return -102;
                }

                Fields.Clear();

                long Result = (long)objResult;

                if (Result == -99999999)
                {
                    return 0;
                }

                return Result;
            }
        }

        public class T_ActiveAllBuyStar : TableBase
        {
            public Field ID;
            public Field LotterieID;
            public Field UserList;
            public Field Order;

            public T_ActiveAllBuyStar()
            {
                TableName = "T_ActiveAllBuyStar";

                ID = new Field(this, "ID", "ID", SqlDbType.Int, true);
                LotterieID = new Field(this, "LotterieID", "LotterieID", SqlDbType.Int, false);
                UserList = new Field(this, "UserList", "UserList", SqlDbType.VarChar, false);
                Order = new Field(this, "Order", "Order", SqlDbType.Int, false);
            }
        }

        public class T_Activities21CN : TableBase
        {
            public Field ID;
            public Field UserID;
            public Field DateTime;
            public Field AlipayName;
            public Field IsReward1;
            public Field DayBalanceAdd;
            public Field IsReward2;
            public Field DaySchemeMoney;
            public Field IsReward10;
            public Field DayWinMoney;
            public Field IsReward200;

            public T_Activities21CN()
            {
                TableName = "T_Activities21CN";

                ID = new Field(this, "ID", "ID", SqlDbType.BigInt, true);
                UserID = new Field(this, "UserID", "UserID", SqlDbType.BigInt, false);
                DateTime = new Field(this, "DateTime", "DateTime", SqlDbType.DateTime, false);
                AlipayName = new Field(this, "AlipayName", "AlipayName", SqlDbType.VarChar, false);
                IsReward1 = new Field(this, "IsReward1", "IsReward1", SqlDbType.Bit, false);
                DayBalanceAdd = new Field(this, "DayBalanceAdd", "DayBalanceAdd", SqlDbType.Money, false);
                IsReward2 = new Field(this, "IsReward2", "IsReward2", SqlDbType.Bit, false);
                DaySchemeMoney = new Field(this, "DaySchemeMoney", "DaySchemeMoney", SqlDbType.Money, false);
                IsReward10 = new Field(this, "IsReward10", "IsReward10", SqlDbType.Bit, false);
                DayWinMoney = new Field(this, "DayWinMoney", "DayWinMoney", SqlDbType.Money, false);
                IsReward200 = new Field(this, "IsReward200", "IsReward200", SqlDbType.Bit, false);
            }
        }

        public class T_Activities360 : TableBase
        {
            public Field ID;
            public Field UserID;
            public Field DateTime;
            public Field AlipayName;
            public Field IsReward1;
            public Field DayBalanceAdd;
            public Field IsReward2;
            public Field DaySchemeMoney;
            public Field IsReward10;
            public Field DayWinMoney;
            public Field IsReward200;

            public T_Activities360()
            {
                TableName = "T_Activities360";

                ID = new Field(this, "ID", "ID", SqlDbType.BigInt, true);
                UserID = new Field(this, "UserID", "UserID", SqlDbType.BigInt, false);
                DateTime = new Field(this, "DateTime", "DateTime", SqlDbType.DateTime, false);
                AlipayName = new Field(this, "AlipayName", "AlipayName", SqlDbType.VarChar, false);
                IsReward1 = new Field(this, "IsReward1", "IsReward1", SqlDbType.Bit, false);
                DayBalanceAdd = new Field(this, "DayBalanceAdd", "DayBalanceAdd", SqlDbType.Money, false);
                IsReward2 = new Field(this, "IsReward2", "IsReward2", SqlDbType.Bit, false);
                DaySchemeMoney = new Field(this, "DaySchemeMoney", "DaySchemeMoney", SqlDbType.Money, false);
                IsReward10 = new Field(this, "IsReward10", "IsReward10", SqlDbType.Bit, false);
                DayWinMoney = new Field(this, "DayWinMoney", "DayWinMoney", SqlDbType.Money, false);
                IsReward200 = new Field(this, "IsReward200", "IsReward200", SqlDbType.Bit, false);
            }
        }

        public class T_ActivitiesAlipay : TableBase
        {
            public Field ID;
            public Field UserID;
            public Field DateTime;
            public Field AlipayName;
            public Field IsReward1;
            public Field DayBalanceAdd;
            public Field IsReward2;
            public Field DaySchemeMoney;
            public Field IsReward10;
            public Field DayWinMoney;
            public Field IsReward200;

            public T_ActivitiesAlipay()
            {
                TableName = "T_ActivitiesAlipay";

                ID = new Field(this, "ID", "ID", SqlDbType.BigInt, true);
                UserID = new Field(this, "UserID", "UserID", SqlDbType.BigInt, false);
                DateTime = new Field(this, "DateTime", "DateTime", SqlDbType.DateTime, false);
                AlipayName = new Field(this, "AlipayName", "AlipayName", SqlDbType.VarChar, false);
                IsReward1 = new Field(this, "IsReward1", "IsReward1", SqlDbType.Bit, false);
                DayBalanceAdd = new Field(this, "DayBalanceAdd", "DayBalanceAdd", SqlDbType.Money, false);
                IsReward2 = new Field(this, "IsReward2", "IsReward2", SqlDbType.Bit, false);
                DaySchemeMoney = new Field(this, "DaySchemeMoney", "DaySchemeMoney", SqlDbType.Money, false);
                IsReward10 = new Field(this, "IsReward10", "IsReward10", SqlDbType.Bit, false);
                DayWinMoney = new Field(this, "DayWinMoney", "DayWinMoney", SqlDbType.Money, false);
                IsReward200 = new Field(this, "IsReward200", "IsReward200", SqlDbType.Bit, false);
            }
        }

        public class T_ActivitiesMytv365 : TableBase
        {
            public Field ID;
            public Field UserID;
            public Field DateTime;
            public Field AlipayName;
            public Field IsReward1;
            public Field DayBalanceAdd;
            public Field IsReward2;
            public Field DaySchemeMoney;
            public Field IsReward10;
            public Field DayWinMoney;
            public Field IsReward200;

            public T_ActivitiesMytv365()
            {
                TableName = "T_ActivitiesMytv365";

                ID = new Field(this, "ID", "ID", SqlDbType.BigInt, true);
                UserID = new Field(this, "UserID", "UserID", SqlDbType.BigInt, false);
                DateTime = new Field(this, "DateTime", "DateTime", SqlDbType.DateTime, false);
                AlipayName = new Field(this, "AlipayName", "AlipayName", SqlDbType.VarChar, false);
                IsReward1 = new Field(this, "IsReward1", "IsReward1", SqlDbType.Bit, false);
                DayBalanceAdd = new Field(this, "DayBalanceAdd", "DayBalanceAdd", SqlDbType.Money, false);
                IsReward2 = new Field(this, "IsReward2", "IsReward2", SqlDbType.Bit, false);
                DaySchemeMoney = new Field(this, "DaySchemeMoney", "DaySchemeMoney", SqlDbType.Money, false);
                IsReward10 = new Field(this, "IsReward10", "IsReward10", SqlDbType.Bit, false);
                DayWinMoney = new Field(this, "DayWinMoney", "DayWinMoney", SqlDbType.Money, false);
                IsReward200 = new Field(this, "IsReward200", "IsReward200", SqlDbType.Bit, false);
            }
        }

        public class T_ActivitiesZJL : TableBase
        {
            public Field ID;
            public Field UserID;
            public Field DateTime;
            public Field AlipayName;
            public Field IsReward1;
            public Field DayBalanceAdd;
            public Field IsReward2;
            public Field DaySchemeMoney;
            public Field IsReward10;
            public Field DayWinMoney;
            public Field IsReward200;

            public T_ActivitiesZJL()
            {
                TableName = "T_ActivitiesZJL";

                ID = new Field(this, "ID", "ID", SqlDbType.BigInt, true);
                UserID = new Field(this, "UserID", "UserID", SqlDbType.BigInt, false);
                DateTime = new Field(this, "DateTime", "DateTime", SqlDbType.DateTime, false);
                AlipayName = new Field(this, "AlipayName", "AlipayName", SqlDbType.VarChar, false);
                IsReward1 = new Field(this, "IsReward1", "IsReward1", SqlDbType.Bit, false);
                DayBalanceAdd = new Field(this, "DayBalanceAdd", "DayBalanceAdd", SqlDbType.Money, false);
                IsReward2 = new Field(this, "IsReward2", "IsReward2", SqlDbType.Bit, false);
                DaySchemeMoney = new Field(this, "DaySchemeMoney", "DaySchemeMoney", SqlDbType.Money, false);
                IsReward10 = new Field(this, "IsReward10", "IsReward10", SqlDbType.Bit, false);
                DayWinMoney = new Field(this, "DayWinMoney", "DayWinMoney", SqlDbType.Money, false);
                IsReward200 = new Field(this, "IsReward200", "IsReward200", SqlDbType.Bit, false);
            }
        }

        public class T_Advertisements : TableBase
        {
            public Field ID;
            public Field LotteryID;
            public Field Name;
            public Field Title;
            public Field Url;
            public Field DateTime;
            public Field Order;
            public Field isShow;

            public T_Advertisements()
            {
                TableName = "T_Advertisements";

                ID = new Field(this, "ID", "ID", SqlDbType.Int, true);
                LotteryID = new Field(this, "LotteryID", "LotteryID", SqlDbType.Int, false);
                Name = new Field(this, "Name", "Name", SqlDbType.VarChar, false);
                Title = new Field(this, "Title", "Title", SqlDbType.VarChar, false);
                Url = new Field(this, "Url", "Url", SqlDbType.VarChar, false);
                DateTime = new Field(this, "DateTime", "DateTime", SqlDbType.DateTime, false);
                Order = new Field(this, "Order", "Order", SqlDbType.Int, false);
                isShow = new Field(this, "isShow", "isShow", SqlDbType.Bit, false);
            }
        }

        public class T_AlipayBuyTemp : TableBase
        {
            public Field ID;
            public Field SiteID;
            public Field UserID;
            public Field DateTime;
            public Field Money;
            public Field HandleResult;
            public Field SchemeID;
            public Field ChaseTaskID;
            public Field IsChase;
            public Field IsCoBuy;
            public Field LotteryID;
            public Field IsuseID;
            public Field PlayTypeID;
            public Field StopwhenwinMoney;
            public Field AdditionasXml;
            public Field Title;
            public Field Multiple;
            public Field BuyMoney;
            public Field SumMoney;
            public Field AssureMoney;
            public Field Share;
            public Field BuyShare;
            public Field AssureShare;
            public Field SecrecyLevel;
            public Field Description;
            public Field LotteryNumber;
            public Field UpdateloadFileContent;
            public Field OpenUsers;
            public Field Number;

            public T_AlipayBuyTemp()
            {
                TableName = "T_AlipayBuyTemp";

                ID = new Field(this, "ID", "ID", SqlDbType.BigInt, true);
                SiteID = new Field(this, "SiteID", "SiteID", SqlDbType.BigInt, false);
                UserID = new Field(this, "UserID", "UserID", SqlDbType.BigInt, false);
                DateTime = new Field(this, "DateTime", "DateTime", SqlDbType.DateTime, false);
                Money = new Field(this, "Money", "Money", SqlDbType.Money, false);
                HandleResult = new Field(this, "HandleResult", "HandleResult", SqlDbType.SmallInt, false);
                SchemeID = new Field(this, "SchemeID", "SchemeID", SqlDbType.BigInt, false);
                ChaseTaskID = new Field(this, "ChaseTaskID", "ChaseTaskID", SqlDbType.BigInt, false);
                IsChase = new Field(this, "IsChase", "IsChase", SqlDbType.Bit, false);
                IsCoBuy = new Field(this, "IsCoBuy", "IsCoBuy", SqlDbType.Bit, false);
                LotteryID = new Field(this, "LotteryID", "LotteryID", SqlDbType.Int, false);
                IsuseID = new Field(this, "IsuseID", "IsuseID", SqlDbType.BigInt, false);
                PlayTypeID = new Field(this, "PlayTypeID", "PlayTypeID", SqlDbType.Int, false);
                StopwhenwinMoney = new Field(this, "StopwhenwinMoney", "StopwhenwinMoney", SqlDbType.Money, false);
                AdditionasXml = new Field(this, "AdditionasXml", "AdditionasXml", SqlDbType.NText, false);
                Title = new Field(this, "Title", "Title", SqlDbType.VarChar, false);
                Multiple = new Field(this, "Multiple", "Multiple", SqlDbType.Int, false);
                BuyMoney = new Field(this, "BuyMoney", "BuyMoney", SqlDbType.Money, false);
                SumMoney = new Field(this, "SumMoney", "SumMoney", SqlDbType.Money, false);
                AssureMoney = new Field(this, "AssureMoney", "AssureMoney", SqlDbType.Money, false);
                Share = new Field(this, "Share", "Share", SqlDbType.Int, false);
                BuyShare = new Field(this, "BuyShare", "BuyShare", SqlDbType.Int, false);
                AssureShare = new Field(this, "AssureShare", "AssureShare", SqlDbType.Int, false);
                SecrecyLevel = new Field(this, "SecrecyLevel", "SecrecyLevel", SqlDbType.SmallInt, false);
                Description = new Field(this, "Description", "Description", SqlDbType.VarChar, false);
                LotteryNumber = new Field(this, "LotteryNumber", "LotteryNumber", SqlDbType.VarChar, false);
                UpdateloadFileContent = new Field(this, "UpdateloadFileContent", "UpdateloadFileContent", SqlDbType.VarChar, false);
                OpenUsers = new Field(this, "OpenUsers", "OpenUsers", SqlDbType.VarChar, false);
                Number = new Field(this, "Number", "Number", SqlDbType.Int, false);
            }
        }

        public class T_AlipayRegDonate : TableBase
        {
            public Field ID;
            public Field UserID;
            public Field DateTime;
            public Field AlipayName;
            public Field HandleResult;

            public T_AlipayRegDonate()
            {
                TableName = "T_AlipayRegDonate";

                ID = new Field(this, "ID", "ID", SqlDbType.BigInt, true);
                UserID = new Field(this, "UserID", "UserID", SqlDbType.BigInt, false);
                DateTime = new Field(this, "DateTime", "DateTime", SqlDbType.DateTime, false);
                AlipayName = new Field(this, "AlipayName", "AlipayName", SqlDbType.VarChar, false);
                HandleResult = new Field(this, "HandleResult", "HandleResult", SqlDbType.SmallInt, false);
            }
        }

        public class T_BankDetails : TableBase
        {
            public Field ID;
            public Field ProvinceName;
            public Field CityName;
            public Field BankTypeName;
            public Field BankName;

            public T_BankDetails()
            {
                TableName = "T_BankDetails";

                ID = new Field(this, "ID", "ID", SqlDbType.BigInt, true);
                ProvinceName = new Field(this, "ProvinceName", "ProvinceName", SqlDbType.VarChar, false);
                CityName = new Field(this, "CityName", "CityName", SqlDbType.VarChar, false);
                BankTypeName = new Field(this, "BankTypeName", "BankTypeName", SqlDbType.VarChar, false);
                BankName = new Field(this, "BankName", "BankName", SqlDbType.VarChar, false);
            }
        }

        public class T_Banks : TableBase
        {
            public Field ID;
            public Field Name;
            public Field Order;

            public T_Banks()
            {
                TableName = "T_Banks";

                ID = new Field(this, "ID", "ID", SqlDbType.SmallInt, false);
                Name = new Field(this, "Name", "Name", SqlDbType.VarChar, false);
                Order = new Field(this, "Order", "Order", SqlDbType.SmallInt, false);
            }
        }

        public class T_BuyDetails : TableBase
        {
            public Field ID;
            public Field SiteID;
            public Field UserID;
            public Field DateTime;
            public Field SchemeID;
            public Field Share;
            public Field QuashStatus;
            public Field isWhenInitiate;
            public Field WinMoneyNoWithTax;
            public Field isAutoFollowScheme;
            public Field DetailMoney;

            public T_BuyDetails()
            {
                TableName = "T_BuyDetails";

                ID = new Field(this, "ID", "ID", SqlDbType.BigInt, true);
                SiteID = new Field(this, "SiteID", "SiteID", SqlDbType.BigInt, false);
                UserID = new Field(this, "UserID", "UserID", SqlDbType.BigInt, false);
                DateTime = new Field(this, "DateTime", "DateTime", SqlDbType.DateTime, false);
                SchemeID = new Field(this, "SchemeID", "SchemeID", SqlDbType.BigInt, false);
                Share = new Field(this, "Share", "Share", SqlDbType.Int, false);
                QuashStatus = new Field(this, "QuashStatus", "QuashStatus", SqlDbType.SmallInt, false);
                isWhenInitiate = new Field(this, "isWhenInitiate", "isWhenInitiate", SqlDbType.Bit, false);
                WinMoneyNoWithTax = new Field(this, "WinMoneyNoWithTax", "WinMoneyNoWithTax", SqlDbType.Money, false);
                isAutoFollowScheme = new Field(this, "isAutoFollowScheme", "isAutoFollowScheme", SqlDbType.Bit, false);
                DetailMoney = new Field(this, "DetailMoney", "DetailMoney", SqlDbType.Money, false);
            }
        }

        public class T_CardPasswordAgentDetails : TableBase
        {
            public Field ID;
            public Field AgentID;
            public Field DateTime;
            public Field OperatorType;
            public Field Amount;
            public Field Memo;

            public T_CardPasswordAgentDetails()
            {
                TableName = "T_CardPasswordAgentDetails";

                ID = new Field(this, "ID", "ID", SqlDbType.BigInt, true);
                AgentID = new Field(this, "AgentID", "AgentID", SqlDbType.BigInt, false);
                DateTime = new Field(this, "DateTime", "DateTime", SqlDbType.DateTime, false);
                OperatorType = new Field(this, "OperatorType", "OperatorType", SqlDbType.SmallInt, false);
                Amount = new Field(this, "Amount", "Amount", SqlDbType.Money, false);
                Memo = new Field(this, "Memo", "Memo", SqlDbType.VarChar, false);
            }
        }

        public class T_CardPasswordAgents : TableBase
        {
            public Field ID;
            public Field Name;
            public Field Key;
            public Field Password;
            public Field Company;
            public Field Url;
            public Field State;
            public Field IPAddressLimit;
            public Field Balance;

            public T_CardPasswordAgents()
            {
                TableName = "T_CardPasswordAgents";

                ID = new Field(this, "ID", "ID", SqlDbType.Int, false);
                Name = new Field(this, "Name", "Name", SqlDbType.VarChar, false);
                Key = new Field(this, "Key", "Key", SqlDbType.VarChar, false);
                Password = new Field(this, "Password", "Password", SqlDbType.VarChar, false);
                Company = new Field(this, "Company", "Company", SqlDbType.VarChar, false);
                Url = new Field(this, "Url", "Url", SqlDbType.VarChar, false);
                State = new Field(this, "State", "State", SqlDbType.SmallInt, false);
                IPAddressLimit = new Field(this, "IPAddressLimit", "IPAddressLimit", SqlDbType.VarChar, false);
                Balance = new Field(this, "Balance", "Balance", SqlDbType.Money, false);
            }
        }

        public class T_CardPasswordAgentsTrys : TableBase
        {
            public Field ID;
            public Field Name;
            public Field Departments;
            public Field Money;
            public Field Type;
            public Field AgentTitle;
            public Field Place;
            public Field SchemeDetails;
            public Field InitUserCount;
            public Field InitUserMoneyCount;
            public Field AddedUserByDay;
            public Field AddedMoneyByMonth;
            public Field State;

            public T_CardPasswordAgentsTrys()
            {
                TableName = "T_CardPasswordAgentsTrys";

                ID = new Field(this, "ID", "ID", SqlDbType.Int, true);
                Name = new Field(this, "Name", "Name", SqlDbType.VarChar, false);
                Departments = new Field(this, "Departments", "Departments", SqlDbType.VarChar, false);
                Money = new Field(this, "Money", "Money", SqlDbType.Money, false);
                Type = new Field(this, "Type", "Type", SqlDbType.VarChar, false);
                AgentTitle = new Field(this, "AgentTitle", "AgentTitle", SqlDbType.VarChar, false);
                Place = new Field(this, "Place", "Place", SqlDbType.VarChar, false);
                SchemeDetails = new Field(this, "SchemeDetails", "SchemeDetails", SqlDbType.VarChar, false);
                InitUserCount = new Field(this, "InitUserCount", "InitUserCount", SqlDbType.BigInt, false);
                InitUserMoneyCount = new Field(this, "InitUserMoneyCount", "InitUserMoneyCount", SqlDbType.Money, false);
                AddedUserByDay = new Field(this, "AddedUserByDay", "AddedUserByDay", SqlDbType.BigInt, false);
                AddedMoneyByMonth = new Field(this, "AddedMoneyByMonth", "AddedMoneyByMonth", SqlDbType.Money, false);
                State = new Field(this, "State", "State", SqlDbType.SmallInt, false);
            }
        }

        public class T_CardPasswords : TableBase
        {
            public Field ID;
            public Field AgentID;
            public Field DateTime;
            public Field Period;
            public Field Money;
            public Field State;
            public Field UserID;
            public Field UseDateTime;

            public T_CardPasswords()
            {
                TableName = "T_CardPasswords";

                ID = new Field(this, "ID", "ID", SqlDbType.BigInt, true);
                AgentID = new Field(this, "AgentID", "AgentID", SqlDbType.Int, false);
                DateTime = new Field(this, "DateTime", "DateTime", SqlDbType.DateTime, false);
                Period = new Field(this, "Period", "Period", SqlDbType.DateTime, false);
                Money = new Field(this, "Money", "Money", SqlDbType.NChar, false);
                State = new Field(this, "State", "State", SqlDbType.SmallInt, false);
                UserID = new Field(this, "UserID", "UserID", SqlDbType.BigInt, false);
                UseDateTime = new Field(this, "UseDateTime", "UseDateTime", SqlDbType.DateTime, false);
            }
        }

        public class T_CardPasswordsValid : TableBase
        {
            public Field ID;
            public Field DateTime;
            public Field UserID;
            public Field Mobile;
            public Field CardPasswordsNum;

            public T_CardPasswordsValid()
            {
                TableName = "T_CardPasswordsValid";

                ID = new Field(this, "ID", "ID", SqlDbType.BigInt, true);
                DateTime = new Field(this, "DateTime", "DateTime", SqlDbType.DateTime, false);
                UserID = new Field(this, "UserID", "UserID", SqlDbType.BigInt, false);
                Mobile = new Field(this, "Mobile", "Mobile", SqlDbType.VarChar, false);
                CardPasswordsNum = new Field(this, "CardPasswordsNum", "CardPasswordsNum", SqlDbType.VarChar, false);
            }
        }

        public class T_CardPasswordTryErrors : TableBase
        {
            public Field ID;
            public Field UserID;
            public Field DateTime;
            public Field Number;

            public T_CardPasswordTryErrors()
            {
                TableName = "T_CardPasswordTryErrors";

                ID = new Field(this, "ID", "ID", SqlDbType.BigInt, true);
                UserID = new Field(this, "UserID", "UserID", SqlDbType.BigInt, false);
                DateTime = new Field(this, "DateTime", "DateTime", SqlDbType.DateTime, false);
                Number = new Field(this, "Number", "Number", SqlDbType.VarChar, false);
            }
        }

        public class T_CelebComments : TableBase
        {
            public Field ID;
            public Field CelebID;
            public Field DateTime;
            public Field CommentserID;
            public Field CommentserName;
            public Field isShow;
            public Field Content;

            public T_CelebComments()
            {
                TableName = "T_CelebComments";

                ID = new Field(this, "ID", "ID", SqlDbType.BigInt, true);
                CelebID = new Field(this, "CelebID", "CelebID", SqlDbType.BigInt, false);
                DateTime = new Field(this, "DateTime", "DateTime", SqlDbType.DateTime, false);
                CommentserID = new Field(this, "CommentserID", "CommentserID", SqlDbType.BigInt, false);
                CommentserName = new Field(this, "CommentserName", "CommentserName", SqlDbType.VarChar, false);
                isShow = new Field(this, "isShow", "isShow", SqlDbType.Bit, false);
                Content = new Field(this, "Content", "Content", SqlDbType.VarChar, false);
            }
        }

        public class T_Celebs : TableBase
        {
            public Field ID;
            public Field SiteID;
            public Field UserID;
            public Field Title;
            public Field Order;
            public Field isRecommended;
            public Field Intro;
            public Field Say;
            public Field Comment;
            public Field Score;

            public T_Celebs()
            {
                TableName = "T_Celebs";

                ID = new Field(this, "ID", "ID", SqlDbType.BigInt, true);
                SiteID = new Field(this, "SiteID", "SiteID", SqlDbType.BigInt, false);
                UserID = new Field(this, "UserID", "UserID", SqlDbType.BigInt, false);
                Title = new Field(this, "Title", "Title", SqlDbType.VarChar, false);
                Order = new Field(this, "Order", "Order", SqlDbType.BigInt, false);
                isRecommended = new Field(this, "isRecommended", "isRecommended", SqlDbType.Bit, false);
                Intro = new Field(this, "Intro", "Intro", SqlDbType.VarChar, false);
                Say = new Field(this, "Say", "Say", SqlDbType.VarChar, false);
                Comment = new Field(this, "Comment", "Comment", SqlDbType.VarChar, false);
                Score = new Field(this, "Score", "Score", SqlDbType.VarChar, false);
            }
        }

        public class T_ChaseLotteryNumber : TableBase
        {
            public Field ID;
            public Field ChaseID;
            public Field LotteryNumber;

            public T_ChaseLotteryNumber()
            {
                TableName = "T_ChaseLotteryNumber";

                ID = new Field(this, "ID", "ID", SqlDbType.BigInt, true);
                ChaseID = new Field(this, "ChaseID", "ChaseID", SqlDbType.BigInt, false);
                LotteryNumber = new Field(this, "LotteryNumber", "LotteryNumber", SqlDbType.VarChar, false);
            }
        }

        public class T_Chases : TableBase
        {
            public Field ID;
            public Field UserID;
            public Field LotteryID;
            public Field Type;
            public Field StartTime;
            public Field EndTime;
            public Field DateTime;
            public Field IsuseCount;
            public Field Multiple;
            public Field Nums;
            public Field BetType;
            public Field LotteryNumber;
            public Field StopTypeWhenWin;
            public Field StopTypeWhenWinMoney;
            public Field QuashStatus;
            public Field Money;
            public Field Title;

            public T_Chases()
            {
                TableName = "T_Chases";

                ID = new Field(this, "ID", "ID", SqlDbType.BigInt, true);
                UserID = new Field(this, "UserID", "UserID", SqlDbType.BigInt, false);
                LotteryID = new Field(this, "LotteryID", "LotteryID", SqlDbType.Int, false);
                Type = new Field(this, "Type", "Type", SqlDbType.SmallInt, false);
                StartTime = new Field(this, "StartTime", "StartTime", SqlDbType.DateTime, false);
                EndTime = new Field(this, "EndTime", "EndTime", SqlDbType.DateTime, false);
                DateTime = new Field(this, "DateTime", "DateTime", SqlDbType.DateTime, false);
                IsuseCount = new Field(this, "IsuseCount", "IsuseCount", SqlDbType.Int, false);
                Multiple = new Field(this, "Multiple", "Multiple", SqlDbType.Int, false);
                Nums = new Field(this, "Nums", "Nums", SqlDbType.Int, false);
                BetType = new Field(this, "BetType", "BetType", SqlDbType.SmallInt, false);
                LotteryNumber = new Field(this, "LotteryNumber", "LotteryNumber", SqlDbType.VarChar, false);
                StopTypeWhenWin = new Field(this, "StopTypeWhenWin", "StopTypeWhenWin", SqlDbType.SmallInt, false);
                StopTypeWhenWinMoney = new Field(this, "StopTypeWhenWinMoney", "StopTypeWhenWinMoney", SqlDbType.Money, false);
                QuashStatus = new Field(this, "QuashStatus", "QuashStatus", SqlDbType.SmallInt, false);
                Money = new Field(this, "Money", "Money", SqlDbType.Money, false);
                Title = new Field(this, "Title", "Title", SqlDbType.VarChar, false);
            }
        }

        public class T_ChaseTaskDetails : TableBase
        {
            public Field ID;
            public Field SiteID;
            public Field ChaseTaskID;
            public Field DateTime;
            public Field IsuseID;
            public Field PlayTypeID;
            public Field Multiple;
            public Field Money;
            public Field QuashStatus;
            public Field Executed;
            public Field SchemeID;
            public Field SecrecyLevel;
            public Field LotteryNumber;
            public Field Share;
            public Field BuyedShare;
            public Field AssureShare;

            public T_ChaseTaskDetails()
            {
                TableName = "T_ChaseTaskDetails";

                ID = new Field(this, "ID", "ID", SqlDbType.BigInt, true);
                SiteID = new Field(this, "SiteID", "SiteID", SqlDbType.BigInt, false);
                ChaseTaskID = new Field(this, "ChaseTaskID", "ChaseTaskID", SqlDbType.BigInt, false);
                DateTime = new Field(this, "DateTime", "DateTime", SqlDbType.DateTime, false);
                IsuseID = new Field(this, "IsuseID", "IsuseID", SqlDbType.BigInt, false);
                PlayTypeID = new Field(this, "PlayTypeID", "PlayTypeID", SqlDbType.Int, false);
                Multiple = new Field(this, "Multiple", "Multiple", SqlDbType.Int, false);
                Money = new Field(this, "Money", "Money", SqlDbType.Money, false);
                QuashStatus = new Field(this, "QuashStatus", "QuashStatus", SqlDbType.SmallInt, false);
                Executed = new Field(this, "Executed", "Executed", SqlDbType.Bit, false);
                SchemeID = new Field(this, "SchemeID", "SchemeID", SqlDbType.BigInt, false);
                SecrecyLevel = new Field(this, "SecrecyLevel", "SecrecyLevel", SqlDbType.SmallInt, false);
                LotteryNumber = new Field(this, "LotteryNumber", "LotteryNumber", SqlDbType.VarChar, false);
                Share = new Field(this, "Share", "Share", SqlDbType.Int, false);
                BuyedShare = new Field(this, "BuyedShare", "BuyedShare", SqlDbType.Int, false);
                AssureShare = new Field(this, "AssureShare", "AssureShare", SqlDbType.Int, false);
            }
        }

        public class T_ChaseTasks : TableBase
        {
            public Field ID;
            public Field SiteID;
            public Field UserID;
            public Field DateTime;
            public Field Title;
            public Field LotteryID;
            public Field QuashStatus;
            public Field StopWhenWinMoney;
            public Field Description;
            public Field SchemeBonusScale;

            public T_ChaseTasks()
            {
                TableName = "T_ChaseTasks";

                ID = new Field(this, "ID", "ID", SqlDbType.BigInt, true);
                SiteID = new Field(this, "SiteID", "SiteID", SqlDbType.BigInt, false);
                UserID = new Field(this, "UserID", "UserID", SqlDbType.BigInt, false);
                DateTime = new Field(this, "DateTime", "DateTime", SqlDbType.DateTime, false);
                Title = new Field(this, "Title", "Title", SqlDbType.VarChar, false);
                LotteryID = new Field(this, "LotteryID", "LotteryID", SqlDbType.Int, false);
                QuashStatus = new Field(this, "QuashStatus", "QuashStatus", SqlDbType.SmallInt, false);
                StopWhenWinMoney = new Field(this, "StopWhenWinMoney", "StopWhenWinMoney", SqlDbType.Money, false);
                Description = new Field(this, "Description", "Description", SqlDbType.VarChar, false);
                SchemeBonusScale = new Field(this, "SchemeBonusScale", "SchemeBonusScale", SqlDbType.Float, false);
            }
        }

        public class T_Citys : TableBase
        {
            public Field ID;
            public Field ProvinceID;
            public Field Name;

            public T_Citys()
            {
                TableName = "T_Citys";

                ID = new Field(this, "ID", "ID", SqlDbType.Int, false);
                ProvinceID = new Field(this, "ProvinceID", "ProvinceID", SqlDbType.Int, false);
                Name = new Field(this, "Name", "Name", SqlDbType.VarChar, false);
            }
        }

        public class T_Competences : TableBase
        {
            public Field ID;
            public Field Name;
            public Field Code;
            public Field Description;

            public T_Competences()
            {
                TableName = "T_Competences";

                ID = new Field(this, "ID", "ID", SqlDbType.SmallInt, false);
                Name = new Field(this, "Name", "Name", SqlDbType.VarChar, false);
                Code = new Field(this, "Code", "Code", SqlDbType.VarChar, false);
                Description = new Field(this, "Description", "Description", SqlDbType.VarChar, false);
            }
        }

        public class T_CompetencesOfGroups : TableBase
        {
            public Field GroupID;
            public Field CompetenceID;

            public T_CompetencesOfGroups()
            {
                TableName = "T_CompetencesOfGroups";

                GroupID = new Field(this, "GroupID", "GroupID", SqlDbType.SmallInt, false);
                CompetenceID = new Field(this, "CompetenceID", "CompetenceID", SqlDbType.SmallInt, false);
            }
        }

        public class T_CompetencesOfUsers : TableBase
        {
            public Field UserID;
            public Field CompetenceID;

            public T_CompetencesOfUsers()
            {
                TableName = "T_CompetencesOfUsers";

                UserID = new Field(this, "UserID", "UserID", SqlDbType.BigInt, false);
                CompetenceID = new Field(this, "CompetenceID", "CompetenceID", SqlDbType.SmallInt, false);
            }
        }

        public class T_Cps : TableBase
        {
            public Field ID;
            public Field SiteID;
            public Field OwnerUserID;
            public Field Name;
            public Field DateTime;
            public Field Url;
            public Field LogoUrl;
            public Field BonusScale;
            public Field ON;
            public Field Company;
            public Field Address;
            public Field PostCode;
            public Field ResponsiblePerson;
            public Field ContactPerson;
            public Field Telephone;
            public Field Fax;
            public Field Mobile;
            public Field Email;
            public Field QQ;
            public Field ServiceTelephone;
            public Field MD5Key;
            public Field Type;
            public Field DomainName;
            public Field ParentID;
            public Field OperatorID;
            public Field CommendID;
            public Field IsShow;
            public Field PageTitleName;
            public Field PageHeadConctroFilelName;
            public Field PageFootConctrolFilelName;

            public T_Cps()
            {
                TableName = "T_Cps";

                ID = new Field(this, "ID", "ID", SqlDbType.BigInt, false);
                SiteID = new Field(this, "SiteID", "SiteID", SqlDbType.BigInt, false);
                OwnerUserID = new Field(this, "OwnerUserID", "OwnerUserID", SqlDbType.BigInt, false);
                Name = new Field(this, "Name", "Name", SqlDbType.VarChar, false);
                DateTime = new Field(this, "DateTime", "DateTime", SqlDbType.DateTime, false);
                Url = new Field(this, "Url", "Url", SqlDbType.VarChar, false);
                LogoUrl = new Field(this, "LogoUrl", "LogoUrl", SqlDbType.VarChar, false);
                BonusScale = new Field(this, "BonusScale", "BonusScale", SqlDbType.Float, false);
                ON = new Field(this, "ON", "ON", SqlDbType.Bit, false);
                Company = new Field(this, "Company", "Company", SqlDbType.VarChar, false);
                Address = new Field(this, "Address", "Address", SqlDbType.VarChar, false);
                PostCode = new Field(this, "PostCode", "PostCode", SqlDbType.VarChar, false);
                ResponsiblePerson = new Field(this, "ResponsiblePerson", "ResponsiblePerson", SqlDbType.VarChar, false);
                ContactPerson = new Field(this, "ContactPerson", "ContactPerson", SqlDbType.VarChar, false);
                Telephone = new Field(this, "Telephone", "Telephone", SqlDbType.VarChar, false);
                Fax = new Field(this, "Fax", "Fax", SqlDbType.VarChar, false);
                Mobile = new Field(this, "Mobile", "Mobile", SqlDbType.VarChar, false);
                Email = new Field(this, "Email", "Email", SqlDbType.VarChar, false);
                QQ = new Field(this, "QQ", "QQ", SqlDbType.VarChar, false);
                ServiceTelephone = new Field(this, "ServiceTelephone", "ServiceTelephone", SqlDbType.VarChar, false);
                MD5Key = new Field(this, "MD5Key", "MD5Key", SqlDbType.VarChar, false);
                Type = new Field(this, "Type", "Type", SqlDbType.SmallInt, false);
                DomainName = new Field(this, "DomainName", "DomainName", SqlDbType.VarChar, false);
                ParentID = new Field(this, "ParentID", "ParentID", SqlDbType.BigInt, false);
                OperatorID = new Field(this, "OperatorID", "OperatorID", SqlDbType.BigInt, false);
                CommendID = new Field(this, "CommendID", "CommendID", SqlDbType.BigInt, false);
                IsShow = new Field(this, "IsShow", "IsShow", SqlDbType.Bit, false);
                PageTitleName = new Field(this, "PageTitleName", "PageTitleName", SqlDbType.VarChar, false);
                PageHeadConctroFilelName = new Field(this, "PageHeadConctroFilelName", "PageHeadConctroFilelName", SqlDbType.VarChar, false);
                PageFootConctrolFilelName = new Field(this, "PageFootConctrolFilelName", "PageFootConctrolFilelName", SqlDbType.VarChar, false);
            }
        }

        public class T_Cps_Help : TableBase
        {
            public Field ID;
            public Field Title;
            public Field Content;

            public T_Cps_Help()
            {
                TableName = "T_Cps_Help";

                ID = new Field(this, "ID", "ID", SqlDbType.Int, true);
                Title = new Field(this, "Title", "Title", SqlDbType.VarChar, false);
                Content = new Field(this, "Content", "Content", SqlDbType.VarChar, false);
            }
        }

        public class T_CpsAccountRevenue : TableBase
        {
            public Field ID;
            public Field CpsID;
            public Field DayTime;
            public Field TotalUserCount;
            public Field DayNewUserCount;
            public Field DayNewUserPayCount;
            public Field DayNewUserPaySum;
            public Field CpsBonus;
            public Field CpsWithSiteMoneySum;

            public T_CpsAccountRevenue()
            {
                TableName = "T_CpsAccountRevenue";

                ID = new Field(this, "ID", "ID", SqlDbType.BigInt, true);
                CpsID = new Field(this, "CpsID", "CpsID", SqlDbType.Int, false);
                DayTime = new Field(this, "DayTime", "DayTime", SqlDbType.DateTime, false);
                TotalUserCount = new Field(this, "TotalUserCount", "TotalUserCount", SqlDbType.Int, false);
                DayNewUserCount = new Field(this, "DayNewUserCount", "DayNewUserCount", SqlDbType.Int, false);
                DayNewUserPayCount = new Field(this, "DayNewUserPayCount", "DayNewUserPayCount", SqlDbType.Int, false);
                DayNewUserPaySum = new Field(this, "DayNewUserPaySum", "DayNewUserPaySum", SqlDbType.Money, false);
                CpsBonus = new Field(this, "CpsBonus", "CpsBonus", SqlDbType.Money, false);
                CpsWithSiteMoneySum = new Field(this, "CpsWithSiteMoneySum", "CpsWithSiteMoneySum", SqlDbType.Money, false);
            }
        }

        public class T_CpsBonusDetails : TableBase
        {
            public Field ID;
            public Field OwnerUserID;
            public Field FromSystem;
            public Field DateTime;
            public Field Money;
            public Field BonusScale;
            public Field IsAddInAllowBonus;
            public Field OperatorType;
            public Field FromUserID;
            public Field FromUserCpsID;
            public Field SchemeID;
            public Field BuyDetailID;
            public Field PayNumber;
            public Field PayBank;
            public Field OperatorID;
            public Field Memo;

            public T_CpsBonusDetails()
            {
                TableName = "T_CpsBonusDetails";

                ID = new Field(this, "ID", "ID", SqlDbType.BigInt, true);
                OwnerUserID = new Field(this, "OwnerUserID", "OwnerUserID", SqlDbType.BigInt, false);
                FromSystem = new Field(this, "FromSystem", "FromSystem", SqlDbType.SmallInt, false);
                DateTime = new Field(this, "DateTime", "DateTime", SqlDbType.DateTime, false);
                Money = new Field(this, "Money", "Money", SqlDbType.Money, false);
                BonusScale = new Field(this, "BonusScale", "BonusScale", SqlDbType.Float, false);
                IsAddInAllowBonus = new Field(this, "IsAddInAllowBonus", "IsAddInAllowBonus", SqlDbType.Bit, false);
                OperatorType = new Field(this, "OperatorType", "OperatorType", SqlDbType.SmallInt, false);
                FromUserID = new Field(this, "FromUserID", "FromUserID", SqlDbType.BigInt, false);
                FromUserCpsID = new Field(this, "FromUserCpsID", "FromUserCpsID", SqlDbType.BigInt, false);
                SchemeID = new Field(this, "SchemeID", "SchemeID", SqlDbType.BigInt, false);
                BuyDetailID = new Field(this, "BuyDetailID", "BuyDetailID", SqlDbType.BigInt, false);
                PayNumber = new Field(this, "PayNumber", "PayNumber", SqlDbType.VarChar, false);
                PayBank = new Field(this, "PayBank", "PayBank", SqlDbType.VarChar, false);
                OperatorID = new Field(this, "OperatorID", "OperatorID", SqlDbType.BigInt, false);
                Memo = new Field(this, "Memo", "Memo", SqlDbType.VarChar, false);
            }
        }

        public class T_CpsLog : TableBase
        {
            public Field ID;
            public Field Datetime;
            public Field LogContent;

            public T_CpsLog()
            {
                TableName = "T_CpsLog";

                ID = new Field(this, "ID", "ID", SqlDbType.BigInt, true);
                Datetime = new Field(this, "Datetime", "Datetime", SqlDbType.DateTime, false);
                LogContent = new Field(this, "LogContent", "LogContent", SqlDbType.VarChar, false);
            }
        }

        public class T_CpsTrys : TableBase
        {
            public Field ID;
            public Field SiteID;
            public Field UserID;
            public Field DateTime;
            public Field HandleResult;
            public Field HandlelDateTime;
            public Field Name;
            public Field Url;
            public Field LogoUrl;
            public Field Company;
            public Field Address;
            public Field PostCode;
            public Field ResponsiblePerson;
            public Field ContactPerson;
            public Field Telephone;
            public Field Fax;
            public Field Mobile;
            public Field Email;
            public Field QQ;
            public Field ServiceTelephone;
            public Field MD5Key;
            public Field Type;
            public Field DomainName;
            public Field ParentID;
            public Field BonusScale;
            public Field CommendID;
            public Field Content;

            public T_CpsTrys()
            {
                TableName = "T_CpsTrys";

                ID = new Field(this, "ID", "ID", SqlDbType.BigInt, false);
                SiteID = new Field(this, "SiteID", "SiteID", SqlDbType.BigInt, false);
                UserID = new Field(this, "UserID", "UserID", SqlDbType.BigInt, false);
                DateTime = new Field(this, "DateTime", "DateTime", SqlDbType.DateTime, false);
                HandleResult = new Field(this, "HandleResult", "HandleResult", SqlDbType.SmallInt, false);
                HandlelDateTime = new Field(this, "HandlelDateTime", "HandlelDateTime", SqlDbType.DateTime, false);
                Name = new Field(this, "Name", "Name", SqlDbType.VarChar, false);
                Url = new Field(this, "Url", "Url", SqlDbType.VarChar, false);
                LogoUrl = new Field(this, "LogoUrl", "LogoUrl", SqlDbType.VarChar, false);
                Company = new Field(this, "Company", "Company", SqlDbType.VarChar, false);
                Address = new Field(this, "Address", "Address", SqlDbType.VarChar, false);
                PostCode = new Field(this, "PostCode", "PostCode", SqlDbType.VarChar, false);
                ResponsiblePerson = new Field(this, "ResponsiblePerson", "ResponsiblePerson", SqlDbType.VarChar, false);
                ContactPerson = new Field(this, "ContactPerson", "ContactPerson", SqlDbType.VarChar, false);
                Telephone = new Field(this, "Telephone", "Telephone", SqlDbType.VarChar, false);
                Fax = new Field(this, "Fax", "Fax", SqlDbType.VarChar, false);
                Mobile = new Field(this, "Mobile", "Mobile", SqlDbType.VarChar, false);
                Email = new Field(this, "Email", "Email", SqlDbType.VarChar, false);
                QQ = new Field(this, "QQ", "QQ", SqlDbType.VarChar, false);
                ServiceTelephone = new Field(this, "ServiceTelephone", "ServiceTelephone", SqlDbType.VarChar, false);
                MD5Key = new Field(this, "MD5Key", "MD5Key", SqlDbType.VarChar, false);
                Type = new Field(this, "Type", "Type", SqlDbType.SmallInt, false);
                DomainName = new Field(this, "DomainName", "DomainName", SqlDbType.VarChar, false);
                ParentID = new Field(this, "ParentID", "ParentID", SqlDbType.BigInt, false);
                BonusScale = new Field(this, "BonusScale", "BonusScale", SqlDbType.Float, false);
                CommendID = new Field(this, "CommendID", "CommendID", SqlDbType.BigInt, false);
                Content = new Field(this, "Content", "Content", SqlDbType.VarChar, false);
            }
        }

        public class T_CpsType : TableBase
        {
            public Field ID;
            public Field Name;

            public T_CpsType()
            {
                TableName = "T_CpsType";

                ID = new Field(this, "ID", "ID", SqlDbType.SmallInt, true);
                Name = new Field(this, "Name", "Name", SqlDbType.VarChar, false);
            }
        }

        public class T_CustomFollowSchemes : TableBase
        {
            public Field ID;
            public Field SiteID;
            public Field UserID;
            public Field DateTime;
            public Field FollowSchemeID;
            public Field MoneyStart;
            public Field MoneyEnd;
            public Field BuyShareStart;
            public Field BuyShareEnd;
            public Field Type;

            public T_CustomFollowSchemes()
            {
                TableName = "T_CustomFollowSchemes";

                ID = new Field(this, "ID", "ID", SqlDbType.BigInt, true);
                SiteID = new Field(this, "SiteID", "SiteID", SqlDbType.BigInt, false);
                UserID = new Field(this, "UserID", "UserID", SqlDbType.BigInt, false);
                DateTime = new Field(this, "DateTime", "DateTime", SqlDbType.DateTime, false);
                FollowSchemeID = new Field(this, "FollowSchemeID", "FollowSchemeID", SqlDbType.BigInt, false);
                MoneyStart = new Field(this, "MoneyStart", "MoneyStart", SqlDbType.Money, false);
                MoneyEnd = new Field(this, "MoneyEnd", "MoneyEnd", SqlDbType.Money, false);
                BuyShareStart = new Field(this, "BuyShareStart", "BuyShareStart", SqlDbType.Int, false);
                BuyShareEnd = new Field(this, "BuyShareEnd", "BuyShareEnd", SqlDbType.Int, false);
                Type = new Field(this, "Type", "Type", SqlDbType.SmallInt, false);
            }
        }

        public class T_CustomFriendFollowSchemes : TableBase
        {
            public Field ID;
            public Field SiteID;
            public Field UserID;
            public Field FollowUserID;
            public Field LotteryID;
            public Field PlayTypeID;
            public Field MoneyStart;
            public Field MoneyEnd;
            public Field BuyShareStart;
            public Field BuyShareEnd;
            public Field Type;
            public Field DateTime;

            public T_CustomFriendFollowSchemes()
            {
                TableName = "T_CustomFriendFollowSchemes";

                ID = new Field(this, "ID", "ID", SqlDbType.BigInt, true);
                SiteID = new Field(this, "SiteID", "SiteID", SqlDbType.BigInt, false);
                UserID = new Field(this, "UserID", "UserID", SqlDbType.BigInt, false);
                FollowUserID = new Field(this, "FollowUserID", "FollowUserID", SqlDbType.BigInt, false);
                LotteryID = new Field(this, "LotteryID", "LotteryID", SqlDbType.Int, false);
                PlayTypeID = new Field(this, "PlayTypeID", "PlayTypeID", SqlDbType.Int, false);
                MoneyStart = new Field(this, "MoneyStart", "MoneyStart", SqlDbType.Money, false);
                MoneyEnd = new Field(this, "MoneyEnd", "MoneyEnd", SqlDbType.Money, false);
                BuyShareStart = new Field(this, "BuyShareStart", "BuyShareStart", SqlDbType.Int, false);
                BuyShareEnd = new Field(this, "BuyShareEnd", "BuyShareEnd", SqlDbType.Int, false);
                Type = new Field(this, "Type", "Type", SqlDbType.SmallInt, false);
                DateTime = new Field(this, "DateTime", "DateTime", SqlDbType.DateTime, false);
            }
        }

        public class T_Downloads : TableBase
        {
            public Field ID;
            public Field SiteID;
            public Field DateTime;
            public Field Title;
            public Field FileUrl;
            public Field isShow;

            public T_Downloads()
            {
                TableName = "T_Downloads";

                ID = new Field(this, "ID", "ID", SqlDbType.BigInt, true);
                SiteID = new Field(this, "SiteID", "SiteID", SqlDbType.BigInt, false);
                DateTime = new Field(this, "DateTime", "DateTime", SqlDbType.DateTime, false);
                Title = new Field(this, "Title", "Title", SqlDbType.VarChar, false);
                FileUrl = new Field(this, "FileUrl", "FileUrl", SqlDbType.VarChar, false);
                isShow = new Field(this, "isShow", "isShow", SqlDbType.Bit, false);
            }
        }

        public class T_ElectronTicketAgentDetails : TableBase
        {
            public Field ID;
            public Field AgentID;
            public Field DateTime;
            public Field OperatorType;
            public Field Amount;
            public Field Memo;

            public T_ElectronTicketAgentDetails()
            {
                TableName = "T_ElectronTicketAgentDetails";

                ID = new Field(this, "ID", "ID", SqlDbType.BigInt, true);
                AgentID = new Field(this, "AgentID", "AgentID", SqlDbType.BigInt, false);
                DateTime = new Field(this, "DateTime", "DateTime", SqlDbType.DateTime, false);
                OperatorType = new Field(this, "OperatorType", "OperatorType", SqlDbType.SmallInt, false);
                Amount = new Field(this, "Amount", "Amount", SqlDbType.Money, false);
                Memo = new Field(this, "Memo", "Memo", SqlDbType.VarChar, false);
            }
        }

        public class T_ElectronTicketAgents : TableBase
        {
            public Field ID;
            public Field Name;
            public Field Key;
            public Field Password;
            public Field Company;
            public Field Url;
            public Field Balance;
            public Field State;
            public Field UseLotteryList;
            public Field IPAddressLimit;

            public T_ElectronTicketAgents()
            {
                TableName = "T_ElectronTicketAgents";

                ID = new Field(this, "ID", "ID", SqlDbType.BigInt, true);
                Name = new Field(this, "Name", "Name", SqlDbType.VarChar, false);
                Key = new Field(this, "Key", "Key", SqlDbType.VarChar, false);
                Password = new Field(this, "Password", "Password", SqlDbType.VarChar, false);
                Company = new Field(this, "Company", "Company", SqlDbType.VarChar, false);
                Url = new Field(this, "Url", "Url", SqlDbType.VarChar, false);
                Balance = new Field(this, "Balance", "Balance", SqlDbType.Money, false);
                State = new Field(this, "State", "State", SqlDbType.SmallInt, false);
                UseLotteryList = new Field(this, "UseLotteryList", "UseLotteryList", SqlDbType.VarChar, false);
                IPAddressLimit = new Field(this, "IPAddressLimit", "IPAddressLimit", SqlDbType.VarChar, false);
            }
        }

        public class T_ElectronTicketAgentSchemeDetails : TableBase
        {
            public Field ID;
            public Field SchemeID;
            public Field Name;
            public Field AlipayName;
            public Field RealityName;
            public Field IDCard;
            public Field Telephone;
            public Field Mobile;
            public Field Email;
            public Field Share;
            public Field Amount;

            public T_ElectronTicketAgentSchemeDetails()
            {
                TableName = "T_ElectronTicketAgentSchemeDetails";

                ID = new Field(this, "ID", "ID", SqlDbType.BigInt, true);
                SchemeID = new Field(this, "SchemeID", "SchemeID", SqlDbType.BigInt, false);
                Name = new Field(this, "Name", "Name", SqlDbType.VarChar, false);
                AlipayName = new Field(this, "AlipayName", "AlipayName", SqlDbType.VarChar, false);
                RealityName = new Field(this, "RealityName", "RealityName", SqlDbType.VarChar, false);
                IDCard = new Field(this, "IDCard", "IDCard", SqlDbType.VarChar, false);
                Telephone = new Field(this, "Telephone", "Telephone", SqlDbType.VarChar, false);
                Mobile = new Field(this, "Mobile", "Mobile", SqlDbType.VarChar, false);
                Email = new Field(this, "Email", "Email", SqlDbType.VarChar, false);
                Share = new Field(this, "Share", "Share", SqlDbType.Int, false);
                Amount = new Field(this, "Amount", "Amount", SqlDbType.Money, false);
            }
        }

        public class T_ElectronTicketAgentSchemes : TableBase
        {
            public Field ID;
            public Field AgentID;
            public Field DateTime;
            public Field SchemeNumber;
            public Field LotteryID;
            public Field PlayTypeID;
            public Field IsuseID;
            public Field Amount;
            public Field Multiple;
            public Field Share;
            public Field InitiateName;
            public Field InitiateAlipayName;
            public Field InitiateAlipayID;
            public Field InitiateRealityName;
            public Field InitiateIDCard;
            public Field InitiateTelephone;
            public Field InitiateMobile;
            public Field InitiateEmail;
            public Field InitiateBonusScale;
            public Field InitiateBonusLimitLower;
            public Field InitiateBonusLimitUpper;
            public Field State;
            public Field BettingDescription;
            public Field WinMoney;
            public Field WinMoneyWithoutTax;
            public Field WinDescription;
            public Field Identifiers;
            public Field WriteOff;
            public Field LotteryNumber;

            public T_ElectronTicketAgentSchemes()
            {
                TableName = "T_ElectronTicketAgentSchemes";

                ID = new Field(this, "ID", "ID", SqlDbType.BigInt, true);
                AgentID = new Field(this, "AgentID", "AgentID", SqlDbType.BigInt, false);
                DateTime = new Field(this, "DateTime", "DateTime", SqlDbType.DateTime, false);
                SchemeNumber = new Field(this, "SchemeNumber", "SchemeNumber", SqlDbType.VarChar, false);
                LotteryID = new Field(this, "LotteryID", "LotteryID", SqlDbType.Int, false);
                PlayTypeID = new Field(this, "PlayTypeID", "PlayTypeID", SqlDbType.Int, false);
                IsuseID = new Field(this, "IsuseID", "IsuseID", SqlDbType.BigInt, false);
                Amount = new Field(this, "Amount", "Amount", SqlDbType.Money, false);
                Multiple = new Field(this, "Multiple", "Multiple", SqlDbType.Int, false);
                Share = new Field(this, "Share", "Share", SqlDbType.Int, false);
                InitiateName = new Field(this, "InitiateName", "InitiateName", SqlDbType.VarChar, false);
                InitiateAlipayName = new Field(this, "InitiateAlipayName", "InitiateAlipayName", SqlDbType.VarChar, false);
                InitiateAlipayID = new Field(this, "InitiateAlipayID", "InitiateAlipayID", SqlDbType.VarChar, false);
                InitiateRealityName = new Field(this, "InitiateRealityName", "InitiateRealityName", SqlDbType.VarChar, false);
                InitiateIDCard = new Field(this, "InitiateIDCard", "InitiateIDCard", SqlDbType.VarChar, false);
                InitiateTelephone = new Field(this, "InitiateTelephone", "InitiateTelephone", SqlDbType.VarChar, false);
                InitiateMobile = new Field(this, "InitiateMobile", "InitiateMobile", SqlDbType.VarChar, false);
                InitiateEmail = new Field(this, "InitiateEmail", "InitiateEmail", SqlDbType.VarChar, false);
                InitiateBonusScale = new Field(this, "InitiateBonusScale", "InitiateBonusScale", SqlDbType.Float, false);
                InitiateBonusLimitLower = new Field(this, "InitiateBonusLimitLower", "InitiateBonusLimitLower", SqlDbType.Money, false);
                InitiateBonusLimitUpper = new Field(this, "InitiateBonusLimitUpper", "InitiateBonusLimitUpper", SqlDbType.Money, false);
                State = new Field(this, "State", "State", SqlDbType.SmallInt, false);
                BettingDescription = new Field(this, "BettingDescription", "BettingDescription", SqlDbType.VarChar, false);
                WinMoney = new Field(this, "WinMoney", "WinMoney", SqlDbType.Money, false);
                WinMoneyWithoutTax = new Field(this, "WinMoneyWithoutTax", "WinMoneyWithoutTax", SqlDbType.Money, false);
                WinDescription = new Field(this, "WinDescription", "WinDescription", SqlDbType.VarChar, false);
                Identifiers = new Field(this, "Identifiers", "Identifiers", SqlDbType.VarChar, false);
                WriteOff = new Field(this, "WriteOff", "WriteOff", SqlDbType.Bit, false);
                LotteryNumber = new Field(this, "LotteryNumber", "LotteryNumber", SqlDbType.VarChar, false);
            }
        }

        public class T_ElectronTicketAgentSchemesElectronTickets : TableBase
        {
            public Field ID;
            public Field DateTime;
            public Field SchemeID;
            public Field PlayTypeID;
            public Field Money;
            public Field Multiple;
            public Field Sends;
            public Field HandleDateTime;
            public Field HandleResult;
            public Field HandleDescription;
            public Field Identifiers;
            public Field Ticket;

            public T_ElectronTicketAgentSchemesElectronTickets()
            {
                TableName = "T_ElectronTicketAgentSchemesElectronTickets";

                ID = new Field(this, "ID", "ID", SqlDbType.BigInt, false);
                DateTime = new Field(this, "DateTime", "DateTime", SqlDbType.DateTime, false);
                SchemeID = new Field(this, "SchemeID", "SchemeID", SqlDbType.BigInt, false);
                PlayTypeID = new Field(this, "PlayTypeID", "PlayTypeID", SqlDbType.Int, false);
                Money = new Field(this, "Money", "Money", SqlDbType.Money, false);
                Multiple = new Field(this, "Multiple", "Multiple", SqlDbType.Int, false);
                Sends = new Field(this, "Sends", "Sends", SqlDbType.SmallInt, false);
                HandleDateTime = new Field(this, "HandleDateTime", "HandleDateTime", SqlDbType.DateTime, false);
                HandleResult = new Field(this, "HandleResult", "HandleResult", SqlDbType.SmallInt, false);
                HandleDescription = new Field(this, "HandleDescription", "HandleDescription", SqlDbType.VarChar, false);
                Identifiers = new Field(this, "Identifiers", "Identifiers", SqlDbType.VarChar, false);
                Ticket = new Field(this, "Ticket", "Ticket", SqlDbType.VarChar, false);
            }
        }

        public class T_ElectronTicketAgentSchemesNumber : TableBase
        {
            public Field AgendID;
            public Field SchemeNumber;

            public T_ElectronTicketAgentSchemesNumber()
            {
                TableName = "T_ElectronTicketAgentSchemesNumber";

                AgendID = new Field(this, "AgendID", "AgendID", SqlDbType.BigInt, false);
                SchemeNumber = new Field(this, "SchemeNumber", "SchemeNumber", SqlDbType.VarChar, false);
            }
        }

        public class T_ElectronTicketAgentSchemesSendToCenter : TableBase
        {
            public Field ID;
            public Field DateTime;
            public Field SchemeID;
            public Field PlayTypeID;
            public Field Money;
            public Field Multiple;
            public Field Sends;
            public Field HandleDateTime;
            public Field HandleResult;
            public Field HandleDescription;
            public Field Identifiers;
            public Field Ticket;

            public T_ElectronTicketAgentSchemesSendToCenter()
            {
                TableName = "T_ElectronTicketAgentSchemesSendToCenter";

                ID = new Field(this, "ID", "ID", SqlDbType.BigInt, true);
                DateTime = new Field(this, "DateTime", "DateTime", SqlDbType.DateTime, false);
                SchemeID = new Field(this, "SchemeID", "SchemeID", SqlDbType.BigInt, false);
                PlayTypeID = new Field(this, "PlayTypeID", "PlayTypeID", SqlDbType.Int, false);
                Money = new Field(this, "Money", "Money", SqlDbType.Money, false);
                Multiple = new Field(this, "Multiple", "Multiple", SqlDbType.Int, false);
                Sends = new Field(this, "Sends", "Sends", SqlDbType.SmallInt, false);
                HandleDateTime = new Field(this, "HandleDateTime", "HandleDateTime", SqlDbType.DateTime, false);
                HandleResult = new Field(this, "HandleResult", "HandleResult", SqlDbType.SmallInt, false);
                HandleDescription = new Field(this, "HandleDescription", "HandleDescription", SqlDbType.VarChar, false);
                Identifiers = new Field(this, "Identifiers", "Identifiers", SqlDbType.VarChar, false);
                Ticket = new Field(this, "Ticket", "Ticket", SqlDbType.VarChar, false);
            }
        }

        public class T_ElectronTicketLog : TableBase
        {
            public Field id;
            public Field TransType;
            public Field datetime;
            public Field Send;
            public Field TransMessage;

            public T_ElectronTicketLog()
            {
                TableName = "T_ElectronTicketLog";

                id = new Field(this, "id", "id", SqlDbType.BigInt, true);
                TransType = new Field(this, "TransType", "TransType", SqlDbType.VarChar, false);
                datetime = new Field(this, "datetime", "datetime", SqlDbType.DateTime, false);
                Send = new Field(this, "Send", "Send", SqlDbType.Bit, false);
                TransMessage = new Field(this, "TransMessage", "TransMessage", SqlDbType.VarChar, false);
            }
        }

        public class T_ExecutedChases : TableBase
        {
            public Field ChaseID;
            public Field SchemeID;

            public T_ExecutedChases()
            {
                TableName = "T_ExecutedChases";

                ChaseID = new Field(this, "ChaseID", "ChaseID", SqlDbType.BigInt, false);
                SchemeID = new Field(this, "SchemeID", "SchemeID", SqlDbType.BigInt, false);
            }
        }

        public class T_Experts : TableBase
        {
            public Field ID;
            public Field SiteID;
            public Field UserID;
            public Field DateTime;
            public Field LotteryID;
            public Field Description;
            public Field isCanIssued;
            public Field MaxPrice;
            public Field BonusScale;
            public Field ON;
            public Field ReadCount;
            public Field isCommend;

            public T_Experts()
            {
                TableName = "T_Experts";

                ID = new Field(this, "ID", "ID", SqlDbType.BigInt, true);
                SiteID = new Field(this, "SiteID", "SiteID", SqlDbType.BigInt, false);
                UserID = new Field(this, "UserID", "UserID", SqlDbType.BigInt, false);
                DateTime = new Field(this, "DateTime", "DateTime", SqlDbType.DateTime, false);
                LotteryID = new Field(this, "LotteryID", "LotteryID", SqlDbType.Int, false);
                Description = new Field(this, "Description", "Description", SqlDbType.VarChar, false);
                isCanIssued = new Field(this, "isCanIssued", "isCanIssued", SqlDbType.Bit, false);
                MaxPrice = new Field(this, "MaxPrice", "MaxPrice", SqlDbType.Money, false);
                BonusScale = new Field(this, "BonusScale", "BonusScale", SqlDbType.Float, false);
                ON = new Field(this, "ON", "ON", SqlDbType.Bit, false);
                ReadCount = new Field(this, "ReadCount", "ReadCount", SqlDbType.Int, false);
                isCommend = new Field(this, "isCommend", "isCommend", SqlDbType.Bit, false);
            }
        }

        public class T_ExpertsCommendRead : TableBase
        {
            public Field ID;
            public Field SiteID;
            public Field ExpertsCommendID;
            public Field UserID;

            public T_ExpertsCommendRead()
            {
                TableName = "T_ExpertsCommendRead";

                ID = new Field(this, "ID", "ID", SqlDbType.BigInt, true);
                SiteID = new Field(this, "SiteID", "SiteID", SqlDbType.BigInt, false);
                ExpertsCommendID = new Field(this, "ExpertsCommendID", "ExpertsCommendID", SqlDbType.BigInt, false);
                UserID = new Field(this, "UserID", "UserID", SqlDbType.BigInt, false);
            }
        }

        public class T_ExpertsCommends : TableBase
        {
            public Field ID;
            public Field SiteID;
            public Field ExpertsID;
            public Field DateTime;
            public Field IsuseID;
            public Field Title;
            public Field Price;
            public Field ReadCount;
            public Field WinMoney;
            public Field isCommend;
            public Field Content;
            public Field Number;

            public T_ExpertsCommends()
            {
                TableName = "T_ExpertsCommends";

                ID = new Field(this, "ID", "ID", SqlDbType.BigInt, true);
                SiteID = new Field(this, "SiteID", "SiteID", SqlDbType.BigInt, false);
                ExpertsID = new Field(this, "ExpertsID", "ExpertsID", SqlDbType.BigInt, false);
                DateTime = new Field(this, "DateTime", "DateTime", SqlDbType.DateTime, false);
                IsuseID = new Field(this, "IsuseID", "IsuseID", SqlDbType.BigInt, false);
                Title = new Field(this, "Title", "Title", SqlDbType.VarChar, false);
                Price = new Field(this, "Price", "Price", SqlDbType.Money, false);
                ReadCount = new Field(this, "ReadCount", "ReadCount", SqlDbType.Int, false);
                WinMoney = new Field(this, "WinMoney", "WinMoney", SqlDbType.Money, false);
                isCommend = new Field(this, "isCommend", "isCommend", SqlDbType.Bit, false);
                Content = new Field(this, "Content", "Content", SqlDbType.VarChar, false);
                Number = new Field(this, "Number", "Number", SqlDbType.VarChar, false);
            }
        }

        public class T_ExpertsPredict : TableBase
        {
            public Field ID;
            public Field Name;
            public Field DateTime;
            public Field LotteryID;
            public Field Description;
            public Field ON;
            public Field URL;

            public T_ExpertsPredict()
            {
                TableName = "T_ExpertsPredict";

                ID = new Field(this, "ID", "ID", SqlDbType.Int, true);
                Name = new Field(this, "Name", "Name", SqlDbType.VarChar, false);
                DateTime = new Field(this, "DateTime", "DateTime", SqlDbType.DateTime, false);
                LotteryID = new Field(this, "LotteryID", "LotteryID", SqlDbType.Int, false);
                Description = new Field(this, "Description", "Description", SqlDbType.VarChar, false);
                ON = new Field(this, "ON", "ON", SqlDbType.Bit, false);
                URL = new Field(this, "URL", "URL", SqlDbType.VarChar, false);
            }
        }

        public class T_ExpertsPredictNews : TableBase
        {
            public Field ID;
            public Field DateTime;
            public Field ExpertsPredictID;
            public Field Description;
            public Field ON;
            public Field URL;
            public Field isWinning;

            public T_ExpertsPredictNews()
            {
                TableName = "T_ExpertsPredictNews";

                ID = new Field(this, "ID", "ID", SqlDbType.BigInt, true);
                DateTime = new Field(this, "DateTime", "DateTime", SqlDbType.DateTime, false);
                ExpertsPredictID = new Field(this, "ExpertsPredictID", "ExpertsPredictID", SqlDbType.Int, false);
                Description = new Field(this, "Description", "Description", SqlDbType.VarChar, false);
                ON = new Field(this, "ON", "ON", SqlDbType.Bit, false);
                URL = new Field(this, "URL", "URL", SqlDbType.VarChar, false);
                isWinning = new Field(this, "isWinning", "isWinning", SqlDbType.Bit, false);
            }
        }

        public class T_ExpertsTrys : TableBase
        {
            public Field ID;
            public Field SiteID;
            public Field UserID;
            public Field DateTime;
            public Field LotteryID;
            public Field Description;
            public Field MaxPrice;
            public Field BonusScale;
            public Field HandleResult;
            public Field HandleDateTime;

            public T_ExpertsTrys()
            {
                TableName = "T_ExpertsTrys";

                ID = new Field(this, "ID", "ID", SqlDbType.BigInt, true);
                SiteID = new Field(this, "SiteID", "SiteID", SqlDbType.BigInt, false);
                UserID = new Field(this, "UserID", "UserID", SqlDbType.BigInt, false);
                DateTime = new Field(this, "DateTime", "DateTime", SqlDbType.DateTime, false);
                LotteryID = new Field(this, "LotteryID", "LotteryID", SqlDbType.Int, false);
                Description = new Field(this, "Description", "Description", SqlDbType.VarChar, false);
                MaxPrice = new Field(this, "MaxPrice", "MaxPrice", SqlDbType.Money, false);
                BonusScale = new Field(this, "BonusScale", "BonusScale", SqlDbType.Float, false);
                HandleResult = new Field(this, "HandleResult", "HandleResult", SqlDbType.SmallInt, false);
                HandleDateTime = new Field(this, "HandleDateTime", "HandleDateTime", SqlDbType.DateTime, false);
            }
        }

        public class T_ExpertsWinCommends : TableBase
        {
            public Field ID;
            public Field SiteID;
            public Field UserID;
            public Field DateTime;
            public Field IsuseID;
            public Field Title;
            public Field isShow;
            public Field ON;
            public Field ReadCount;
            public Field isCommend;
            public Field Content;

            public T_ExpertsWinCommends()
            {
                TableName = "T_ExpertsWinCommends";

                ID = new Field(this, "ID", "ID", SqlDbType.BigInt, true);
                SiteID = new Field(this, "SiteID", "SiteID", SqlDbType.BigInt, false);
                UserID = new Field(this, "UserID", "UserID", SqlDbType.BigInt, false);
                DateTime = new Field(this, "DateTime", "DateTime", SqlDbType.DateTime, false);
                IsuseID = new Field(this, "IsuseID", "IsuseID", SqlDbType.BigInt, false);
                Title = new Field(this, "Title", "Title", SqlDbType.VarChar, false);
                isShow = new Field(this, "isShow", "isShow", SqlDbType.Bit, false);
                ON = new Field(this, "ON", "ON", SqlDbType.Bit, false);
                ReadCount = new Field(this, "ReadCount", "ReadCount", SqlDbType.Int, false);
                isCommend = new Field(this, "isCommend", "isCommend", SqlDbType.Bit, false);
                Content = new Field(this, "Content", "Content", SqlDbType.VarChar, false);
            }
        }

        public class T_FloatNotify : TableBase
        {
            public Field ID;
            public Field Title;
            public Field Color;
            public Field Url;
            public Field DateTime;
            public Field Order;
            public Field isShow;

            public T_FloatNotify()
            {
                TableName = "T_FloatNotify";

                ID = new Field(this, "ID", "ID", SqlDbType.Int, true);
                Title = new Field(this, "Title", "Title", SqlDbType.VarChar, false);
                Color = new Field(this, "Color", "Color", SqlDbType.VarChar, false);
                Url = new Field(this, "Url", "Url", SqlDbType.VarChar, false);
                DateTime = new Field(this, "DateTime", "DateTime", SqlDbType.DateTime, false);
                Order = new Field(this, "Order", "Order", SqlDbType.Int, false);
                isShow = new Field(this, "isShow", "isShow", SqlDbType.Bit, false);
            }
        }

        public class T_FootballLeagueTypes : TableBase
        {
            public Field ID;
            public Field Name;
            public Field Code;
            public Field MarkersColor;
            public Field Description;
            public Field Order;
            public Field isUse;

            public T_FootballLeagueTypes()
            {
                TableName = "T_FootballLeagueTypes";

                ID = new Field(this, "ID", "ID", SqlDbType.Int, true);
                Name = new Field(this, "Name", "Name", SqlDbType.VarChar, false);
                Code = new Field(this, "Code", "Code", SqlDbType.VarChar, false);
                MarkersColor = new Field(this, "MarkersColor", "MarkersColor", SqlDbType.VarChar, false);
                Description = new Field(this, "Description", "Description", SqlDbType.VarChar, false);
                Order = new Field(this, "Order", "Order", SqlDbType.Int, false);
                isUse = new Field(this, "isUse", "isUse", SqlDbType.Bit, false);
            }
        }

        public class T_FriendshipLinks : TableBase
        {
            public Field ID;
            public Field SiteID;
            public Field LinkName;
            public Field LogoUrl;
            public Field Url;
            public Field Order;
            public Field isShow;

            public T_FriendshipLinks()
            {
                TableName = "T_FriendshipLinks";

                ID = new Field(this, "ID", "ID", SqlDbType.Int, true);
                SiteID = new Field(this, "SiteID", "SiteID", SqlDbType.BigInt, false);
                LinkName = new Field(this, "LinkName", "LinkName", SqlDbType.VarChar, false);
                LogoUrl = new Field(this, "LogoUrl", "LogoUrl", SqlDbType.VarChar, false);
                Url = new Field(this, "Url", "Url", SqlDbType.VarChar, false);
                Order = new Field(this, "Order", "Order", SqlDbType.Int, false);
                isShow = new Field(this, "isShow", "isShow", SqlDbType.Bit, false);
            }
        }

        public class T_Helps : TableBase
        {
            public Field ID;
            public Field Title;
            public Field Content;

            public T_Helps()
            {
                TableName = "T_Helps";

                ID = new Field(this, "ID", "ID", SqlDbType.BigInt, true);
                Title = new Field(this, "Title", "Title", SqlDbType.VarChar, false);
                Content = new Field(this, "Content", "Content", SqlDbType.VarChar, false);
            }
        }

        public class T_IPAddress : TableBase
        {
            public Field IPStart;
            public Field IPEnd;
            public Field Country;
            public Field City;

            public T_IPAddress()
            {
                TableName = "T_IPAddress";

                IPStart = new Field(this, "IPStart", "IPStart", SqlDbType.Float, false);
                IPEnd = new Field(this, "IPEnd", "IPEnd", SqlDbType.Float, false);
                Country = new Field(this, "Country", "Country", SqlDbType.NVarChar, false);
                City = new Field(this, "City", "City", SqlDbType.NVarChar, false);
            }
        }

        public class T_IsShowedCustomFollowSchemesForIcaile : TableBase
        {
            public Field ID;

            public T_IsShowedCustomFollowSchemesForIcaile()
            {
                TableName = "T_IsShowedCustomFollowSchemesForIcaile";

                ID = new Field(this, "ID", "ID", SqlDbType.BigInt, false);
            }
        }

        public class T_IsuseBonuses : TableBase
        {
            public Field ID;
            public Field UserID;
            public Field IsuseID;
            public Field defaultMoney;
            public Field DefaultMoneyNoWithTax;

            public T_IsuseBonuses()
            {
                TableName = "T_IsuseBonuses";

                ID = new Field(this, "ID", "ID", SqlDbType.BigInt, true);
                UserID = new Field(this, "UserID", "UserID", SqlDbType.BigInt, false);
                IsuseID = new Field(this, "IsuseID", "IsuseID", SqlDbType.BigInt, false);
                defaultMoney = new Field(this, "defaultMoney", "defaultMoney", SqlDbType.Money, false);
                DefaultMoneyNoWithTax = new Field(this, "DefaultMoneyNoWithTax", "DefaultMoneyNoWithTax", SqlDbType.Money, false);
            }
        }

        public class T_IsuseForJQC : TableBase
        {
            public Field ID;
            public Field IsuseID;
            public Field No;
            public Field Team;
            public Field DateTime;

            public T_IsuseForJQC()
            {
                TableName = "T_IsuseForJQC";

                ID = new Field(this, "ID", "ID", SqlDbType.BigInt, true);
                IsuseID = new Field(this, "IsuseID", "IsuseID", SqlDbType.BigInt, false);
                No = new Field(this, "No", "No", SqlDbType.SmallInt, false);
                Team = new Field(this, "Team", "Team", SqlDbType.VarChar, false);
                DateTime = new Field(this, "DateTime", "DateTime", SqlDbType.VarChar, false);
            }
        }

        public class T_IsuseForLCBQC : TableBase
        {
            public Field ID;
            public Field IsuseID;
            public Field No;
            public Field HostTeam;
            public Field QuestTeam;
            public Field DateTime;

            public T_IsuseForLCBQC()
            {
                TableName = "T_IsuseForLCBQC";

                ID = new Field(this, "ID", "ID", SqlDbType.BigInt, true);
                IsuseID = new Field(this, "IsuseID", "IsuseID", SqlDbType.BigInt, false);
                No = new Field(this, "No", "No", SqlDbType.SmallInt, false);
                HostTeam = new Field(this, "HostTeam", "HostTeam", SqlDbType.VarChar, false);
                QuestTeam = new Field(this, "QuestTeam", "QuestTeam", SqlDbType.VarChar, false);
                DateTime = new Field(this, "DateTime", "DateTime", SqlDbType.VarChar, false);
            }
        }

        public class T_IsuseForLCDC : TableBase
        {
            public Field ID;
            public Field IsuseID;
            public Field No;
            public Field HostTeam;
            public Field QuestTeam;
            public Field DateTime;

            public T_IsuseForLCDC()
            {
                TableName = "T_IsuseForLCDC";

                ID = new Field(this, "ID", "ID", SqlDbType.BigInt, true);
                IsuseID = new Field(this, "IsuseID", "IsuseID", SqlDbType.BigInt, false);
                No = new Field(this, "No", "No", SqlDbType.SmallInt, false);
                HostTeam = new Field(this, "HostTeam", "HostTeam", SqlDbType.VarChar, false);
                QuestTeam = new Field(this, "QuestTeam", "QuestTeam", SqlDbType.VarChar, false);
                DateTime = new Field(this, "DateTime", "DateTime", SqlDbType.VarChar, false);
            }
        }

        public class T_IsuseForSFC : TableBase
        {
            public Field ID;
            public Field IsuseID;
            public Field No;
            public Field HostTeam;
            public Field QuestTeam;
            public Field DateTime;

            public T_IsuseForSFC()
            {
                TableName = "T_IsuseForSFC";

                ID = new Field(this, "ID", "ID", SqlDbType.BigInt, true);
                IsuseID = new Field(this, "IsuseID", "IsuseID", SqlDbType.BigInt, false);
                No = new Field(this, "No", "No", SqlDbType.SmallInt, false);
                HostTeam = new Field(this, "HostTeam", "HostTeam", SqlDbType.VarChar, false);
                QuestTeam = new Field(this, "QuestTeam", "QuestTeam", SqlDbType.VarChar, false);
                DateTime = new Field(this, "DateTime", "DateTime", SqlDbType.VarChar, false);
            }
        }

        public class T_IsuseForZCDC : TableBase
        {
            public Field ID;
            public Field IsuseID;
            public Field LeagueTypeID;
            public Field No;
            public Field HostTeam;
            public Field QuestTeam;
            public Field LetBall;
            public Field DateTime;
            public Field HalftimeResult;
            public Field Result;
            public Field SPFResult;
            public Field SPF_SP;
            public Field ZJQResult;
            public Field ZJQ_SP;
            public Field SXDSResult;
            public Field SXDS_SP;
            public Field ZQBFResult;
            public Field ZQBF_SP;
            public Field BQCSPFResult;
            public Field BQCSPF_SP;
            public Field AnalysisURL;

            public T_IsuseForZCDC()
            {
                TableName = "T_IsuseForZCDC";

                ID = new Field(this, "ID", "ID", SqlDbType.BigInt, true);
                IsuseID = new Field(this, "IsuseID", "IsuseID", SqlDbType.BigInt, false);
                LeagueTypeID = new Field(this, "LeagueTypeID", "LeagueTypeID", SqlDbType.Int, false);
                No = new Field(this, "No", "No", SqlDbType.SmallInt, false);
                HostTeam = new Field(this, "HostTeam", "HostTeam", SqlDbType.VarChar, false);
                QuestTeam = new Field(this, "QuestTeam", "QuestTeam", SqlDbType.VarChar, false);
                LetBall = new Field(this, "LetBall", "LetBall", SqlDbType.SmallInt, false);
                DateTime = new Field(this, "DateTime", "DateTime", SqlDbType.VarChar, false);
                HalftimeResult = new Field(this, "HalftimeResult", "HalftimeResult", SqlDbType.VarChar, false);
                Result = new Field(this, "Result", "Result", SqlDbType.VarChar, false);
                SPFResult = new Field(this, "SPFResult", "SPFResult", SqlDbType.VarChar, false);
                SPF_SP = new Field(this, "SPF_SP", "SPF_SP", SqlDbType.Float, false);
                ZJQResult = new Field(this, "ZJQResult", "ZJQResult", SqlDbType.VarChar, false);
                ZJQ_SP = new Field(this, "ZJQ_SP", "ZJQ_SP", SqlDbType.Float, false);
                SXDSResult = new Field(this, "SXDSResult", "SXDSResult", SqlDbType.VarChar, false);
                SXDS_SP = new Field(this, "SXDS_SP", "SXDS_SP", SqlDbType.Float, false);
                ZQBFResult = new Field(this, "ZQBFResult", "ZQBFResult", SqlDbType.VarChar, false);
                ZQBF_SP = new Field(this, "ZQBF_SP", "ZQBF_SP", SqlDbType.Float, false);
                BQCSPFResult = new Field(this, "BQCSPFResult", "BQCSPFResult", SqlDbType.VarChar, false);
                BQCSPF_SP = new Field(this, "BQCSPF_SP", "BQCSPF_SP", SqlDbType.Float, false);
                AnalysisURL = new Field(this, "AnalysisURL", "AnalysisURL", SqlDbType.VarChar, false);
            }
        }

        public class T_Isuses : TableBase
        {
            public Field ID;
            public Field LotteryID;
            public Field Name;
            public Field StartTime;
            public Field EndTime;
            public Field ChaseExecuted;
            public Field IsOpened;
            public Field WinLotteryNumber;
            public Field OpenOperatorID;
            public Field State;
            public Field StateUpdateTime;
            public Field OpenAffiche;

            public T_Isuses()
            {
                TableName = "T_Isuses";

                ID = new Field(this, "ID", "ID", SqlDbType.BigInt, true);
                LotteryID = new Field(this, "LotteryID", "LotteryID", SqlDbType.Int, false);
                Name = new Field(this, "Name", "Name", SqlDbType.VarChar, false);
                StartTime = new Field(this, "StartTime", "StartTime", SqlDbType.DateTime, false);
                EndTime = new Field(this, "EndTime", "EndTime", SqlDbType.DateTime, false);
                ChaseExecuted = new Field(this, "ChaseExecuted", "ChaseExecuted", SqlDbType.Bit, false);
                IsOpened = new Field(this, "IsOpened", "IsOpened", SqlDbType.Bit, false);
                WinLotteryNumber = new Field(this, "WinLotteryNumber", "WinLotteryNumber", SqlDbType.VarChar, false);
                OpenOperatorID = new Field(this, "OpenOperatorID", "OpenOperatorID", SqlDbType.BigInt, false);
                State = new Field(this, "State", "State", SqlDbType.SmallInt, false);
                StateUpdateTime = new Field(this, "StateUpdateTime", "StateUpdateTime", SqlDbType.DateTime, false);
                OpenAffiche = new Field(this, "OpenAffiche", "OpenAffiche", SqlDbType.VarChar, false);
            }
        }

        public class T_Lotteries : TableBase
        {
            public Field ID;
            public Field Name;
            public Field Code;
            public Field MaxChaseIsuse;
            public Field ChaseExecuteDeferMinute;
            public Field Order;
            public Field WinNumberExemple;
            public Field IntervalType;
            public Field PrintOutType;
            public Field TypeID;
            public Field Type2;
            public Field Agreement;
            public Field Explain;
            public Field SchemeExemple;
            public Field OpenAfficheTemplate;

            public T_Lotteries()
            {
                TableName = "T_Lotteries";

                ID = new Field(this, "ID", "ID", SqlDbType.Int, false);
                Name = new Field(this, "Name", "Name", SqlDbType.VarChar, false);
                Code = new Field(this, "Code", "Code", SqlDbType.VarChar, false);
                MaxChaseIsuse = new Field(this, "MaxChaseIsuse", "MaxChaseIsuse", SqlDbType.VarChar, false);
                ChaseExecuteDeferMinute = new Field(this, "ChaseExecuteDeferMinute", "ChaseExecuteDeferMinute", SqlDbType.Int, false);
                Order = new Field(this, "Order", "Order", SqlDbType.Int, false);
                WinNumberExemple = new Field(this, "WinNumberExemple", "WinNumberExemple", SqlDbType.VarChar, false);
                IntervalType = new Field(this, "IntervalType", "IntervalType", SqlDbType.VarChar, false);
                PrintOutType = new Field(this, "PrintOutType", "PrintOutType", SqlDbType.SmallInt, false);
                TypeID = new Field(this, "TypeID", "TypeID", SqlDbType.SmallInt, false);
                Type2 = new Field(this, "Type2", "Type2", SqlDbType.SmallInt, false);
                Agreement = new Field(this, "Agreement", "Agreement", SqlDbType.VarChar, false);
                Explain = new Field(this, "Explain", "Explain", SqlDbType.VarChar, false);
                SchemeExemple = new Field(this, "SchemeExemple", "SchemeExemple", SqlDbType.VarChar, false);
                OpenAfficheTemplate = new Field(this, "OpenAfficheTemplate", "OpenAfficheTemplate", SqlDbType.VarChar, false);
            }
        }

        public class T_LotteryToolLinks : TableBase
        {
            public Field ID;
            public Field SiteID;
            public Field LotteryID;
            public Field LinkName;
            public Field LogoUrl;
            public Field Url;
            public Field Order;
            public Field isShow;

            public T_LotteryToolLinks()
            {
                TableName = "T_LotteryToolLinks";

                ID = new Field(this, "ID", "ID", SqlDbType.Int, true);
                SiteID = new Field(this, "SiteID", "SiteID", SqlDbType.BigInt, false);
                LotteryID = new Field(this, "LotteryID", "LotteryID", SqlDbType.Int, false);
                LinkName = new Field(this, "LinkName", "LinkName", SqlDbType.VarChar, false);
                LogoUrl = new Field(this, "LogoUrl", "LogoUrl", SqlDbType.VarChar, false);
                Url = new Field(this, "Url", "Url", SqlDbType.VarChar, false);
                Order = new Field(this, "Order", "Order", SqlDbType.Int, false);
                isShow = new Field(this, "isShow", "isShow", SqlDbType.Bit, false);
            }
        }

        public class T_LotteryType : TableBase
        {
            public Field ID;
            public Field ParentID;
            public Field Name;
            public Field Description;
            public Field Order;

            public T_LotteryType()
            {
                TableName = "T_LotteryType";

                ID = new Field(this, "ID", "ID", SqlDbType.SmallInt, true);
                ParentID = new Field(this, "ParentID", "ParentID", SqlDbType.SmallInt, false);
                Name = new Field(this, "Name", "Name", SqlDbType.VarChar, false);
                Description = new Field(this, "Description", "Description", SqlDbType.VarChar, false);
                Order = new Field(this, "Order", "Order", SqlDbType.SmallInt, false);
            }
        }

        public class T_LuckNumber : TableBase
        {
            public Field ID;
            public Field LotteryID;
            public Field Type;
            public Field Name;
            public Field LotteryNumber;
            public Field DateTime;

            public T_LuckNumber()
            {
                TableName = "T_LuckNumber";

                ID = new Field(this, "ID", "ID", SqlDbType.BigInt, true);
                LotteryID = new Field(this, "LotteryID", "LotteryID", SqlDbType.Int, false);
                Type = new Field(this, "Type", "Type", SqlDbType.SmallInt, false);
                Name = new Field(this, "Name", "Name", SqlDbType.VarChar, false);
                LotteryNumber = new Field(this, "LotteryNumber", "LotteryNumber", SqlDbType.VarChar, false);
                DateTime = new Field(this, "DateTime", "DateTime", SqlDbType.DateTime, false);
            }
        }

        public class T_MarketOutlook : TableBase
        {
            public Field ID;
            public Field DateTime;
            public Field Title;
            public Field isShow;
            public Field Content;

            public T_MarketOutlook()
            {
                TableName = "T_MarketOutlook";

                ID = new Field(this, "ID", "ID", SqlDbType.BigInt, true);
                DateTime = new Field(this, "DateTime", "DateTime", SqlDbType.DateTime, false);
                Title = new Field(this, "Title", "Title", SqlDbType.VarChar, false);
                isShow = new Field(this, "isShow", "isShow", SqlDbType.Bit, false);
                Content = new Field(this, "Content", "Content", SqlDbType.VarChar, false);
            }
        }

        public class T_MaxMultiple : TableBase
        {
            public Field ID;
            public Field IsuseID;
            public Field PlayTypeID;
            public Field MaxMultiple;

            public T_MaxMultiple()
            {
                TableName = "T_MaxMultiple";

                ID = new Field(this, "ID", "ID", SqlDbType.BigInt, true);
                IsuseID = new Field(this, "IsuseID", "IsuseID", SqlDbType.BigInt, false);
                PlayTypeID = new Field(this, "PlayTypeID", "PlayTypeID", SqlDbType.Int, false);
                MaxMultiple = new Field(this, "MaxMultiple", "MaxMultiple", SqlDbType.Int, false);
            }
        }

        public class T_News : TableBase
        {
            public Field ID;
            public Field SiteID;
            public Field TypeID;
            public Field DateTime;
            public Field Title;
            public Field ImageUrl;
            public Field isShow;
            public Field isHasImage;
            public Field isCanComments;
            public Field isCommend;
            public Field isHot;
            public Field ReadCount;
            public Field Content;
            public Field IsusesId;

            public T_News()
            {
                TableName = "T_News";

                ID = new Field(this, "ID", "ID", SqlDbType.BigInt, true);
                SiteID = new Field(this, "SiteID", "SiteID", SqlDbType.BigInt, false);
                TypeID = new Field(this, "TypeID", "TypeID", SqlDbType.Int, false);
                DateTime = new Field(this, "DateTime", "DateTime", SqlDbType.DateTime, false);
                Title = new Field(this, "Title", "Title", SqlDbType.VarChar, false);
                ImageUrl = new Field(this, "ImageUrl", "ImageUrl", SqlDbType.VarChar, false);
                isShow = new Field(this, "isShow", "isShow", SqlDbType.Bit, false);
                isHasImage = new Field(this, "isHasImage", "isHasImage", SqlDbType.Bit, false);
                isCanComments = new Field(this, "isCanComments", "isCanComments", SqlDbType.Bit, false);
                isCommend = new Field(this, "isCommend", "isCommend", SqlDbType.Bit, false);
                isHot = new Field(this, "isHot", "isHot", SqlDbType.Bit, false);
                ReadCount = new Field(this, "ReadCount", "ReadCount", SqlDbType.BigInt, false);
                Content = new Field(this, "Content", "Content", SqlDbType.VarChar, false);
                IsusesId = new Field(this, "IsusesId", "IsusesId", SqlDbType.Int, false);
            }
        }

        public class T_NewsComments : TableBase
        {
            public Field ID;
            public Field NewsID;
            public Field DateTime;
            public Field CommentserID;
            public Field CommentserName;
            public Field isShow;
            public Field Content;

            public T_NewsComments()
            {
                TableName = "T_NewsComments";

                ID = new Field(this, "ID", "ID", SqlDbType.BigInt, true);
                NewsID = new Field(this, "NewsID", "NewsID", SqlDbType.BigInt, false);
                DateTime = new Field(this, "DateTime", "DateTime", SqlDbType.DateTime, false);
                CommentserID = new Field(this, "CommentserID", "CommentserID", SqlDbType.BigInt, false);
                CommentserName = new Field(this, "CommentserName", "CommentserName", SqlDbType.VarChar, false);
                isShow = new Field(this, "isShow", "isShow", SqlDbType.Bit, false);
                Content = new Field(this, "Content", "Content", SqlDbType.VarChar, false);
            }
        }

        public class T_NewsPaperIsuses : TableBase
        {
            public Field ID;
            public Field Name;
            public Field StartTime;
            public Field EndTime;
            public Field NPMessage;

            public T_NewsPaperIsuses()
            {
                TableName = "T_NewsPaperIsuses";

                ID = new Field(this, "ID", "ID", SqlDbType.Int, true);
                Name = new Field(this, "Name", "Name", SqlDbType.VarChar, false);
                StartTime = new Field(this, "StartTime", "StartTime", SqlDbType.DateTime, false);
                EndTime = new Field(this, "EndTime", "EndTime", SqlDbType.DateTime, false);
                NPMessage = new Field(this, "NPMessage", "NPMessage", SqlDbType.VarChar, false);
            }
        }

        public class T_NewsPaperTypes : TableBase
        {
            public Field ID;
            public Field SiteID;
            public Field ParentID;
            public Field Name;
            public Field isShow;
            public Field isSystem;

            public T_NewsPaperTypes()
            {
                TableName = "T_NewsPaperTypes";

                ID = new Field(this, "ID", "ID", SqlDbType.Int, true);
                SiteID = new Field(this, "SiteID", "SiteID", SqlDbType.BigInt, false);
                ParentID = new Field(this, "ParentID", "ParentID", SqlDbType.Int, false);
                Name = new Field(this, "Name", "Name", SqlDbType.VarChar, false);
                isShow = new Field(this, "isShow", "isShow", SqlDbType.Bit, false);
                isSystem = new Field(this, "isSystem", "isSystem", SqlDbType.Bit, false);
            }
        }

        public class T_NewsTypes : TableBase
        {
            public Field ID;
            public Field SiteID;
            public Field ParentID;
            public Field Name;
            public Field isShow;
            public Field isSystem;

            public T_NewsTypes()
            {
                TableName = "T_NewsTypes";

                ID = new Field(this, "ID", "ID", SqlDbType.Int, true);
                SiteID = new Field(this, "SiteID", "SiteID", SqlDbType.BigInt, false);
                ParentID = new Field(this, "ParentID", "ParentID", SqlDbType.Int, false);
                Name = new Field(this, "Name", "Name", SqlDbType.VarChar, false);
                isShow = new Field(this, "isShow", "isShow", SqlDbType.Bit, false);
                isSystem = new Field(this, "isSystem", "isSystem", SqlDbType.Bit, false);
            }
        }

        public class T_NotificationTypes : TableBase
        {
            public Field ID;
            public Field Name;
            public Field Code;
            public Field Description;
            public Field TemplateEmail;
            public Field TemplateStationSMS;
            public Field TemplateSMS;

            public T_NotificationTypes()
            {
                TableName = "T_NotificationTypes";

                ID = new Field(this, "ID", "ID", SqlDbType.SmallInt, false);
                Name = new Field(this, "Name", "Name", SqlDbType.VarChar, false);
                Code = new Field(this, "Code", "Code", SqlDbType.VarChar, false);
                Description = new Field(this, "Description", "Description", SqlDbType.VarChar, false);
                TemplateEmail = new Field(this, "TemplateEmail", "TemplateEmail", SqlDbType.VarChar, false);
                TemplateStationSMS = new Field(this, "TemplateStationSMS", "TemplateStationSMS", SqlDbType.VarChar, false);
                TemplateSMS = new Field(this, "TemplateSMS", "TemplateSMS", SqlDbType.VarChar, false);
            }
        }

        public class T_Options : TableBase
        {
            public Field ID;
            public Field Key;
            public Field Value;
            public Field Description;

            public T_Options()
            {
                TableName = "T_Options";

                ID = new Field(this, "ID", "ID", SqlDbType.SmallInt, false);
                Key = new Field(this, "Key", "Key", SqlDbType.VarChar, false);
                Value = new Field(this, "Value", "Value", SqlDbType.VarChar, false);
                Description = new Field(this, "Description", "Description", SqlDbType.VarChar, false);
            }
        }

        public class T_Personages : TableBase
        {
            public Field ID;
            public Field LotteryID;
            public Field UserName;
            public Field DateTime;
            public Field Order;
            public Field IsShow;

            public T_Personages()
            {
                TableName = "T_Personages";

                ID = new Field(this, "ID", "ID", SqlDbType.Int, true);
                LotteryID = new Field(this, "LotteryID", "LotteryID", SqlDbType.Int, false);
                UserName = new Field(this, "UserName", "UserName", SqlDbType.VarChar, false);
                DateTime = new Field(this, "DateTime", "DateTime", SqlDbType.DateTime, false);
                Order = new Field(this, "Order", "Order", SqlDbType.Int, false);
                IsShow = new Field(this, "IsShow", "IsShow", SqlDbType.Bit, false);
            }
        }

        public class T_PlayTypes : TableBase
        {
            public Field ID;
            public Field LotteryID;
            public Field Name;
            public Field SystemEndAheadMinute;
            public Field Price;
            public Field BuyFileName;
            public Field MaxFollowSchemeNumberOf;
            public Field MaxMultiple;

            public T_PlayTypes()
            {
                TableName = "T_PlayTypes";

                ID = new Field(this, "ID", "ID", SqlDbType.Int, false);
                LotteryID = new Field(this, "LotteryID", "LotteryID", SqlDbType.Int, false);
                Name = new Field(this, "Name", "Name", SqlDbType.VarChar, false);
                SystemEndAheadMinute = new Field(this, "SystemEndAheadMinute", "SystemEndAheadMinute", SqlDbType.Int, false);
                Price = new Field(this, "Price", "Price", SqlDbType.Money, false);
                BuyFileName = new Field(this, "BuyFileName", "BuyFileName", SqlDbType.VarChar, false);
                MaxFollowSchemeNumberOf = new Field(this, "MaxFollowSchemeNumberOf", "MaxFollowSchemeNumberOf", SqlDbType.Int, false);
                MaxMultiple = new Field(this, "MaxMultiple", "MaxMultiple", SqlDbType.Int, false);
            }
        }

        public class T_PoliciesAndRegulations : TableBase
        {
            public Field ID;
            public Field DateTime;
            public Field Title;
            public Field isShow;
            public Field Content;

            public T_PoliciesAndRegulations()
            {
                TableName = "T_PoliciesAndRegulations";

                ID = new Field(this, "ID", "ID", SqlDbType.BigInt, true);
                DateTime = new Field(this, "DateTime", "DateTime", SqlDbType.DateTime, false);
                Title = new Field(this, "Title", "Title", SqlDbType.VarChar, false);
                isShow = new Field(this, "isShow", "isShow", SqlDbType.Bit, false);
                Content = new Field(this, "Content", "Content", SqlDbType.VarChar, false);
            }
        }

        public class T_Provinces : TableBase
        {
            public Field ID;
            public Field Name;

            public T_Provinces()
            {
                TableName = "T_Provinces";

                ID = new Field(this, "ID", "ID", SqlDbType.Int, false);
                Name = new Field(this, "Name", "Name", SqlDbType.VarChar, false);
            }
        }

        public class T_Questionnaire : TableBase
        {
            public Field ID;
            public Field Title;
            public Field Options1Content;
            public Field Options2Content;
            public Field Options3Content;
            public Field Options4Content;
            public Field Options1Count;
            public Field Options2Count;
            public Field Options3Count;
            public Field Options4Count;

            public T_Questionnaire()
            {
                TableName = "T_Questionnaire";

                ID = new Field(this, "ID", "ID", SqlDbType.Int, true);
                Title = new Field(this, "Title", "Title", SqlDbType.VarChar, false);
                Options1Content = new Field(this, "Options1Content", "Options1Content", SqlDbType.VarChar, false);
                Options2Content = new Field(this, "Options2Content", "Options2Content", SqlDbType.VarChar, false);
                Options3Content = new Field(this, "Options3Content", "Options3Content", SqlDbType.VarChar, false);
                Options4Content = new Field(this, "Options4Content", "Options4Content", SqlDbType.VarChar, false);
                Options1Count = new Field(this, "Options1Count", "Options1Count", SqlDbType.Int, false);
                Options2Count = new Field(this, "Options2Count", "Options2Count", SqlDbType.Int, false);
                Options3Count = new Field(this, "Options3Count", "Options3Count", SqlDbType.Int, false);
                Options4Count = new Field(this, "Options4Count", "Options4Count", SqlDbType.Int, false);
            }
        }

        public class T_Questions : TableBase
        {
            public Field ID;
            public Field SiteID;
            public Field UserID;
            public Field DateTime;
            public Field TypeID;
            public Field Telephone;
            public Field AnswerStatus;
            public Field HandleDateTime;
            public Field HandleOperatorID;
            public Field AnswerOperatorID;
            public Field AnswerDateTime;
            public Field Content;
            public Field Answer;

            public T_Questions()
            {
                TableName = "T_Questions";

                ID = new Field(this, "ID", "ID", SqlDbType.BigInt, true);
                SiteID = new Field(this, "SiteID", "SiteID", SqlDbType.BigInt, false);
                UserID = new Field(this, "UserID", "UserID", SqlDbType.BigInt, false);
                DateTime = new Field(this, "DateTime", "DateTime", SqlDbType.DateTime, false);
                TypeID = new Field(this, "TypeID", "TypeID", SqlDbType.SmallInt, false);
                Telephone = new Field(this, "Telephone", "Telephone", SqlDbType.VarChar, false);
                AnswerStatus = new Field(this, "AnswerStatus", "AnswerStatus", SqlDbType.SmallInt, false);
                HandleDateTime = new Field(this, "HandleDateTime", "HandleDateTime", SqlDbType.DateTime, false);
                HandleOperatorID = new Field(this, "HandleOperatorID", "HandleOperatorID", SqlDbType.BigInt, false);
                AnswerOperatorID = new Field(this, "AnswerOperatorID", "AnswerOperatorID", SqlDbType.BigInt, false);
                AnswerDateTime = new Field(this, "AnswerDateTime", "AnswerDateTime", SqlDbType.DateTime, false);
                Content = new Field(this, "Content", "Content", SqlDbType.VarChar, false);
                Answer = new Field(this, "Answer", "Answer", SqlDbType.VarChar, false);
            }
        }

        public class T_QuestionTypes : TableBase
        {
            public Field ID;
            public Field Name;
            public Field Description;
            public Field UseType;

            public T_QuestionTypes()
            {
                TableName = "T_QuestionTypes";

                ID = new Field(this, "ID", "ID", SqlDbType.SmallInt, false);
                Name = new Field(this, "Name", "Name", SqlDbType.VarChar, false);
                Description = new Field(this, "Description", "Description", SqlDbType.VarChar, false);
                UseType = new Field(this, "UseType", "UseType", SqlDbType.SmallInt, false);
            }
        }

        public class T_RecallingAllBuyStar : TableBase
        {
            public Field ID;
            public Field SchemesID;
            public Field InitiatorName;
            public Field SchemesMoney;
            public Field SchemesWinMoney;
            public Field ProfitIndex;
            public Field State;

            public T_RecallingAllBuyStar()
            {
                TableName = "T_RecallingAllBuyStar";

                ID = new Field(this, "ID", "ID", SqlDbType.Int, true);
                SchemesID = new Field(this, "SchemesID", "SchemesID", SqlDbType.Int, false);
                InitiatorName = new Field(this, "InitiatorName", "InitiatorName", SqlDbType.VarChar, false);
                SchemesMoney = new Field(this, "SchemesMoney", "SchemesMoney", SqlDbType.VarChar, false);
                SchemesWinMoney = new Field(this, "SchemesWinMoney", "SchemesWinMoney", SqlDbType.VarChar, false);
                ProfitIndex = new Field(this, "ProfitIndex", "ProfitIndex", SqlDbType.VarChar, false);
                State = new Field(this, "State", "State", SqlDbType.Int, false);
            }
        }

        public class T_SchemeChatContents : TableBase
        {
            public Field ID;
            public Field SiteID;
            public Field SchemeID;
            public Field DateTime;
            public Field FromUserID;
            public Field ToUserID;
            public Field Type;
            public Field Content;

            public T_SchemeChatContents()
            {
                TableName = "T_SchemeChatContents";

                ID = new Field(this, "ID", "ID", SqlDbType.BigInt, true);
                SiteID = new Field(this, "SiteID", "SiteID", SqlDbType.BigInt, false);
                SchemeID = new Field(this, "SchemeID", "SchemeID", SqlDbType.BigInt, false);
                DateTime = new Field(this, "DateTime", "DateTime", SqlDbType.DateTime, false);
                FromUserID = new Field(this, "FromUserID", "FromUserID", SqlDbType.BigInt, false);
                ToUserID = new Field(this, "ToUserID", "ToUserID", SqlDbType.BigInt, false);
                Type = new Field(this, "Type", "Type", SqlDbType.SmallInt, false);
                Content = new Field(this, "Content", "Content", SqlDbType.VarChar, false);
            }
        }

        public class T_SchemeChatContentsReaded : TableBase
        {
            public Field ContentID;
            public Field UserID;

            public T_SchemeChatContentsReaded()
            {
                TableName = "T_SchemeChatContentsReaded";

                ContentID = new Field(this, "ContentID", "ContentID", SqlDbType.BigInt, false);
                UserID = new Field(this, "UserID", "UserID", SqlDbType.BigInt, false);
            }
        }

        public class T_SchemeElectronTickets : TableBase
        {
            public Field ID;
            public Field DateTime;
            public Field SchemeID;
            public Field PlayTypeID;
            public Field Money;
            public Field Multiple;
            public Field Sends;
            public Field HandleDateTime;
            public Field HandleResult;
            public Field HandleDescription;
            public Field Identifiers;
            public Field Ticket;

            public T_SchemeElectronTickets()
            {
                TableName = "T_SchemeElectronTickets";

                ID = new Field(this, "ID", "ID", SqlDbType.BigInt, false);
                DateTime = new Field(this, "DateTime", "DateTime", SqlDbType.DateTime, false);
                SchemeID = new Field(this, "SchemeID", "SchemeID", SqlDbType.BigInt, false);
                PlayTypeID = new Field(this, "PlayTypeID", "PlayTypeID", SqlDbType.Int, false);
                Money = new Field(this, "Money", "Money", SqlDbType.Money, false);
                Multiple = new Field(this, "Multiple", "Multiple", SqlDbType.Int, false);
                Sends = new Field(this, "Sends", "Sends", SqlDbType.SmallInt, false);
                HandleDateTime = new Field(this, "HandleDateTime", "HandleDateTime", SqlDbType.DateTime, false);
                HandleResult = new Field(this, "HandleResult", "HandleResult", SqlDbType.SmallInt, false);
                HandleDescription = new Field(this, "HandleDescription", "HandleDescription", SqlDbType.VarChar, false);
                Identifiers = new Field(this, "Identifiers", "Identifiers", SqlDbType.VarChar, false);
                Ticket = new Field(this, "Ticket", "Ticket", SqlDbType.VarChar, false);
            }
        }

        public class T_SchemeOpenUsers : TableBase
        {
            public Field SchemeID;
            public Field UserID;

            public T_SchemeOpenUsers()
            {
                TableName = "T_SchemeOpenUsers";

                SchemeID = new Field(this, "SchemeID", "SchemeID", SqlDbType.BigInt, false);
                UserID = new Field(this, "UserID", "UserID", SqlDbType.BigInt, false);
            }
        }

        public class T_Schemes : TableBase
        {
            public Field ID;
            public Field SiteID;
            public Field DateTime;
            public Field SchemeNumber;
            public Field Title;
            public Field InitiateUserID;
            public Field IsuseID;
            public Field PlayTypeID;
            public Field Multiple;
            public Field Money;
            public Field AssureMoney;
            public Field Share;
            public Field SecrecyLevel;
            public Field QuashStatus;
            public Field Buyed;
            public Field BuyOperatorID;
            public Field PrintOutType;
            public Field Identifiers;
            public Field isOpened;
            public Field OpenOperatorID;
            public Field WinMoney;
            public Field WinMoneyNoWithTax;
            public Field InitiateBonus;
            public Field AtTopStatus;
            public Field isCanChat;
            public Field PreWinMoney;
            public Field PreWinMoneyNoWithTax;
            public Field EditWinMoney;
            public Field EditWinMoneyNoWithTax;
            public Field BuyedShare;
            public Field Schedule;
            public Field ReSchedule;
            public Field IsSchemeCalculatedBonus;
            public Field Description;
            public Field LotteryNumber;
            public Field UploadFileContent;
            public Field WinDescription;
            public Field WinImage;
            public Field UpdateDatetime;
            public Field PrintOutDateTime;
            public Field Ot;
            public Field OutTo;
            public Field CorrelationSchemeID;
            public Field SchemeBonusScale;

            public T_Schemes()
            {
                TableName = "T_Schemes";

                ID = new Field(this, "ID", "ID", SqlDbType.BigInt, true);
                SiteID = new Field(this, "SiteID", "SiteID", SqlDbType.BigInt, false);
                DateTime = new Field(this, "DateTime", "DateTime", SqlDbType.DateTime, false);
                SchemeNumber = new Field(this, "SchemeNumber", "SchemeNumber", SqlDbType.VarChar, false);
                Title = new Field(this, "Title", "Title", SqlDbType.VarChar, false);
                InitiateUserID = new Field(this, "InitiateUserID", "InitiateUserID", SqlDbType.BigInt, false);
                IsuseID = new Field(this, "IsuseID", "IsuseID", SqlDbType.BigInt, false);
                PlayTypeID = new Field(this, "PlayTypeID", "PlayTypeID", SqlDbType.Int, false);
                Multiple = new Field(this, "Multiple", "Multiple", SqlDbType.Int, false);
                Money = new Field(this, "Money", "Money", SqlDbType.Money, false);
                AssureMoney = new Field(this, "AssureMoney", "AssureMoney", SqlDbType.Money, false);
                Share = new Field(this, "Share", "Share", SqlDbType.Int, false);
                SecrecyLevel = new Field(this, "SecrecyLevel", "SecrecyLevel", SqlDbType.SmallInt, false);
                QuashStatus = new Field(this, "QuashStatus", "QuashStatus", SqlDbType.SmallInt, false);
                Buyed = new Field(this, "Buyed", "Buyed", SqlDbType.Bit, false);
                BuyOperatorID = new Field(this, "BuyOperatorID", "BuyOperatorID", SqlDbType.BigInt, false);
                PrintOutType = new Field(this, "PrintOutType", "PrintOutType", SqlDbType.SmallInt, false);
                Identifiers = new Field(this, "Identifiers", "Identifiers", SqlDbType.VarChar, false);
                isOpened = new Field(this, "isOpened", "isOpened", SqlDbType.Bit, false);
                OpenOperatorID = new Field(this, "OpenOperatorID", "OpenOperatorID", SqlDbType.BigInt, false);
                WinMoney = new Field(this, "WinMoney", "WinMoney", SqlDbType.Money, false);
                WinMoneyNoWithTax = new Field(this, "WinMoneyNoWithTax", "WinMoneyNoWithTax", SqlDbType.Money, false);
                InitiateBonus = new Field(this, "InitiateBonus", "InitiateBonus", SqlDbType.Money, false);
                AtTopStatus = new Field(this, "AtTopStatus", "AtTopStatus", SqlDbType.SmallInt, false);
                isCanChat = new Field(this, "isCanChat", "isCanChat", SqlDbType.Bit, false);
                PreWinMoney = new Field(this, "PreWinMoney", "PreWinMoney", SqlDbType.Money, false);
                PreWinMoneyNoWithTax = new Field(this, "PreWinMoneyNoWithTax", "PreWinMoneyNoWithTax", SqlDbType.Money, false);
                EditWinMoney = new Field(this, "EditWinMoney", "EditWinMoney", SqlDbType.Money, false);
                EditWinMoneyNoWithTax = new Field(this, "EditWinMoneyNoWithTax", "EditWinMoneyNoWithTax", SqlDbType.Money, false);
                BuyedShare = new Field(this, "BuyedShare", "BuyedShare", SqlDbType.Int, false);
                Schedule = new Field(this, "Schedule", "Schedule", SqlDbType.Float, false);
                ReSchedule = new Field(this, "ReSchedule", "ReSchedule", SqlDbType.Float, false);
                IsSchemeCalculatedBonus = new Field(this, "IsSchemeCalculatedBonus", "IsSchemeCalculatedBonus", SqlDbType.Bit, false);
                Description = new Field(this, "Description", "Description", SqlDbType.VarChar, false);
                LotteryNumber = new Field(this, "LotteryNumber", "LotteryNumber", SqlDbType.VarChar, false);
                UploadFileContent = new Field(this, "UploadFileContent", "UploadFileContent", SqlDbType.VarChar, false);
                WinDescription = new Field(this, "WinDescription", "WinDescription", SqlDbType.VarChar, false);
                WinImage = new Field(this, "WinImage", "WinImage", SqlDbType.VarChar, false);
                UpdateDatetime = new Field(this, "UpdateDatetime", "UpdateDatetime", SqlDbType.DateTime, false);
                PrintOutDateTime = new Field(this, "PrintOutDateTime", "PrintOutDateTime", SqlDbType.DateTime, false);
                Ot = new Field(this, "Ot", "Ot", SqlDbType.SmallInt, false);
                OutTo = new Field(this, "OutTo", "OutTo", SqlDbType.SmallInt, false);
                CorrelationSchemeID = new Field(this, "CorrelationSchemeID", "CorrelationSchemeID", SqlDbType.BigInt, false);
                SchemeBonusScale = new Field(this, "SchemeBonusScale", "SchemeBonusScale", SqlDbType.Float, false);
            }
        }

        public class T_SchemesNumber : TableBase
        {
            public Field ID;
            public Field DateTime;
            public Field SchemeID;
            public Field Money;
            public Field Multiple;
            public Field LotteryNumber;

            public T_SchemesNumber()
            {
                TableName = "T_SchemesNumber";

                ID = new Field(this, "ID", "ID", SqlDbType.BigInt, true);
                DateTime = new Field(this, "DateTime", "DateTime", SqlDbType.DateTime, false);
                SchemeID = new Field(this, "SchemeID", "SchemeID", SqlDbType.BigInt, false);
                Money = new Field(this, "Money", "Money", SqlDbType.Money, false);
                Multiple = new Field(this, "Multiple", "Multiple", SqlDbType.Int, false);
                LotteryNumber = new Field(this, "LotteryNumber", "LotteryNumber", SqlDbType.VarChar, false);
            }
        }

        public class T_SchemesSendToCenter : TableBase
        {
            public Field ID;
            public Field DateTime;
            public Field SchemeID;
            public Field PlayTypeID;
            public Field Money;
            public Field Multiple;
            public Field Sends;
            public Field HandleDateTime;
            public Field HandleResult;
            public Field HandleDescription;
            public Field Identifiers;
            public Field Ticket;

            public T_SchemesSendToCenter()
            {
                TableName = "T_SchemesSendToCenter";

                ID = new Field(this, "ID", "ID", SqlDbType.BigInt, true);
                DateTime = new Field(this, "DateTime", "DateTime", SqlDbType.DateTime, false);
                SchemeID = new Field(this, "SchemeID", "SchemeID", SqlDbType.BigInt, false);
                PlayTypeID = new Field(this, "PlayTypeID", "PlayTypeID", SqlDbType.Int, false);
                Money = new Field(this, "Money", "Money", SqlDbType.Money, false);
                Multiple = new Field(this, "Multiple", "Multiple", SqlDbType.Int, false);
                Sends = new Field(this, "Sends", "Sends", SqlDbType.SmallInt, false);
                HandleDateTime = new Field(this, "HandleDateTime", "HandleDateTime", SqlDbType.DateTime, false);
                HandleResult = new Field(this, "HandleResult", "HandleResult", SqlDbType.SmallInt, false);
                HandleDescription = new Field(this, "HandleDescription", "HandleDescription", SqlDbType.VarChar, false);
                Identifiers = new Field(this, "Identifiers", "Identifiers", SqlDbType.VarChar, false);
                Ticket = new Field(this, "Ticket", "Ticket", SqlDbType.VarChar, false);
            }
        }

        public class T_SchemeSupperCobuy : TableBase
        {
            public Field ID;
            public Field SchemeID;
            public Field TypeState;

            public T_SchemeSupperCobuy()
            {
                TableName = "T_SchemeSupperCobuy";

                ID = new Field(this, "ID", "ID", SqlDbType.Int, true);
                SchemeID = new Field(this, "SchemeID", "SchemeID", SqlDbType.BigInt, false);
                TypeState = new Field(this, "TypeState", "TypeState", SqlDbType.Int, false);
            }
        }

        public class T_SchemeToTicketed : TableBase
        {
            public Field OurOrAgent;
            public Field SchemeID;

            public T_SchemeToTicketed()
            {
                TableName = "T_SchemeToTicketed";

                OurOrAgent = new Field(this, "OurOrAgent", "OurOrAgent", SqlDbType.SmallInt, false);
                SchemeID = new Field(this, "SchemeID", "SchemeID", SqlDbType.BigInt, false);
            }
        }

        public class T_SchemeUpload : TableBase
        {
            public Field LotteryID;
            public Field SchemeContent;

            public T_SchemeUpload()
            {
                TableName = "T_SchemeUpload";

                LotteryID = new Field(this, "LotteryID", "LotteryID", SqlDbType.BigInt, false);
                SchemeContent = new Field(this, "SchemeContent", "SchemeContent", SqlDbType.VarChar, false);
            }
        }

        public class T_ScoreChange : TableBase
        {
            public Field ID;
            public Field UserID;
            public Field ComodityID;
            public Field Type;
            public Field DateTime;
            public Field Score;
            public Field IsWin;

            public T_ScoreChange()
            {
                TableName = "T_ScoreChange";

                ID = new Field(this, "ID", "ID", SqlDbType.BigInt, true);
                UserID = new Field(this, "UserID", "UserID", SqlDbType.BigInt, false);
                ComodityID = new Field(this, "ComodityID", "ComodityID", SqlDbType.BigInt, false);
                Type = new Field(this, "Type", "Type", SqlDbType.SmallInt, false);
                DateTime = new Field(this, "DateTime", "DateTime", SqlDbType.DateTime, false);
                Score = new Field(this, "Score", "Score", SqlDbType.Int, false);
                IsWin = new Field(this, "IsWin", "IsWin", SqlDbType.Bit, false);
            }
        }

        public class T_ScoreChangeAddress : TableBase
        {
            public Field UserID;
            public Field Name;
            public Field Address;
            public Field PostCode;
            public Field Phone;
            public Field Mobile;
            public Field Memo;

            public T_ScoreChangeAddress()
            {
                TableName = "T_ScoreChangeAddress";

                UserID = new Field(this, "UserID", "UserID", SqlDbType.BigInt, false);
                Name = new Field(this, "Name", "Name", SqlDbType.VarChar, false);
                Address = new Field(this, "Address", "Address", SqlDbType.VarChar, false);
                PostCode = new Field(this, "PostCode", "PostCode", SqlDbType.VarChar, false);
                Phone = new Field(this, "Phone", "Phone", SqlDbType.VarChar, false);
                Mobile = new Field(this, "Mobile", "Mobile", SqlDbType.VarChar, false);
                Memo = new Field(this, "Memo", "Memo", SqlDbType.VarChar, false);
            }
        }

        public class T_ScoreCommodities : TableBase
        {
            public Field ID;
            public Field TypeID;
            public Field Name;
            public Field Qty;
            public Field ChangedScore;
            public Field DrawedScore;
            public Field Images;
            public Field Introduce;
            public Field IsCanChange;
            public Field IsCanDraw;
            public Field IsCommend;

            public T_ScoreCommodities()
            {
                TableName = "T_ScoreCommodities";

                ID = new Field(this, "ID", "ID", SqlDbType.BigInt, true);
                TypeID = new Field(this, "TypeID", "TypeID", SqlDbType.Int, false);
                Name = new Field(this, "Name", "Name", SqlDbType.VarChar, false);
                Qty = new Field(this, "Qty", "Qty", SqlDbType.Int, false);
                ChangedScore = new Field(this, "ChangedScore", "ChangedScore", SqlDbType.Int, false);
                DrawedScore = new Field(this, "DrawedScore", "DrawedScore", SqlDbType.Int, false);
                Images = new Field(this, "Images", "Images", SqlDbType.VarChar, false);
                Introduce = new Field(this, "Introduce", "Introduce", SqlDbType.VarChar, false);
                IsCanChange = new Field(this, "IsCanChange", "IsCanChange", SqlDbType.Bit, false);
                IsCanDraw = new Field(this, "IsCanDraw", "IsCanDraw", SqlDbType.Bit, false);
                IsCommend = new Field(this, "IsCommend", "IsCommend", SqlDbType.Bit, false);
            }
        }

        public class T_ScoreCommodityType : TableBase
        {
            public Field ID;
            public Field ParentID;
            public Field Name;

            public T_ScoreCommodityType()
            {
                TableName = "T_ScoreCommodityType";

                ID = new Field(this, "ID", "ID", SqlDbType.Int, true);
                ParentID = new Field(this, "ParentID", "ParentID", SqlDbType.Int, false);
                Name = new Field(this, "Name", "Name", SqlDbType.VarChar, false);
            }
        }

        public class T_ScoreGoldType : TableBase
        {
            public Field ID;
            public Field IsScore;
            public Field TypeId;
            public Field TypeName;

            public T_ScoreGoldType()
            {
                TableName = "T_ScoreGoldType";

                ID = new Field(this, "ID", "ID", SqlDbType.Int, true);
                IsScore = new Field(this, "IsScore", "IsScore", SqlDbType.Bit, false);
                TypeId = new Field(this, "TypeId", "TypeId", SqlDbType.Int, false);
                TypeName = new Field(this, "TypeName", "TypeName", SqlDbType.NVarChar, false);
            }
        }

        public class T_ScorePresentIn : TableBase
        {
            public Field ID;
            public Field PresentID;
            public Field Qty;
            public Field CreateTime;
            public Field OperatorID;

            public T_ScorePresentIn()
            {
                TableName = "T_ScorePresentIn";

                ID = new Field(this, "ID", "ID", SqlDbType.BigInt, true);
                PresentID = new Field(this, "PresentID", "PresentID", SqlDbType.BigInt, false);
                Qty = new Field(this, "Qty", "Qty", SqlDbType.Int, false);
                CreateTime = new Field(this, "CreateTime", "CreateTime", SqlDbType.SmallDateTime, false);
                OperatorID = new Field(this, "OperatorID", "OperatorID", SqlDbType.BigInt, false);
            }
        }

        public class T_ScorePresentInventory : TableBase
        {
            public Field ID;
            public Field TypeID;
            public Field Qty;
            public Field Price;
            public Field ShopID;
            public Field PresentName;
            public Field ProductImage;
            public Field ProductDetail;
            public Field PhotoDir;

            public T_ScorePresentInventory()
            {
                TableName = "T_ScorePresentInventory";

                ID = new Field(this, "ID", "ID", SqlDbType.BigInt, true);
                TypeID = new Field(this, "TypeID", "TypeID", SqlDbType.BigInt, false);
                Qty = new Field(this, "Qty", "Qty", SqlDbType.Int, false);
                Price = new Field(this, "Price", "Price", SqlDbType.Money, false);
                ShopID = new Field(this, "ShopID", "ShopID", SqlDbType.BigInt, false);
                PresentName = new Field(this, "PresentName", "PresentName", SqlDbType.NVarChar, false);
                ProductImage = new Field(this, "ProductImage", "ProductImage", SqlDbType.Image, false);
                ProductDetail = new Field(this, "ProductDetail", "ProductDetail", SqlDbType.NText, false);
                PhotoDir = new Field(this, "PhotoDir", "PhotoDir", SqlDbType.VarChar, false);
            }
        }

        public class T_ScorePresentOut : TableBase
        {
            public Field ID;
            public Field UserID;
            public Field UserName;
            public Field Qty;
            public Field ChangeID;
            public Field CreateDate;
            public Field Status;

            public T_ScorePresentOut()
            {
                TableName = "T_ScorePresentOut";

                ID = new Field(this, "ID", "ID", SqlDbType.BigInt, true);
                UserID = new Field(this, "UserID", "UserID", SqlDbType.BigInt, false);
                UserName = new Field(this, "UserName", "UserName", SqlDbType.NVarChar, false);
                Qty = new Field(this, "Qty", "Qty", SqlDbType.Int, false);
                ChangeID = new Field(this, "ChangeID", "ChangeID", SqlDbType.BigInt, false);
                CreateDate = new Field(this, "CreateDate", "CreateDate", SqlDbType.SmallDateTime, false);
                Status = new Field(this, "Status", "Status", SqlDbType.SmallInt, false);
            }
        }

        public class T_ScorePresentRule : TableBase
        {
            public Field ID;
            public Field PresentID;
            public Field ChangeType;
            public Field Qty;
            public Field ScoreNumber;
            public Field GoldNumber;
            public Field ChangeMemo;
            public Field PresentOrder;
            public Field Status;
            public Field IsScore;
            public Field AllowWinMember;
            public Field IsHot;
            public Field TimeStart;
            public Field TimeEnd;

            public T_ScorePresentRule()
            {
                TableName = "T_ScorePresentRule";

                ID = new Field(this, "ID", "ID", SqlDbType.BigInt, true);
                PresentID = new Field(this, "PresentID", "PresentID", SqlDbType.BigInt, false);
                ChangeType = new Field(this, "ChangeType", "ChangeType", SqlDbType.SmallInt, false);
                Qty = new Field(this, "Qty", "Qty", SqlDbType.Int, false);
                ScoreNumber = new Field(this, "ScoreNumber", "ScoreNumber", SqlDbType.Int, false);
                GoldNumber = new Field(this, "GoldNumber", "GoldNumber", SqlDbType.Int, false);
                ChangeMemo = new Field(this, "ChangeMemo", "ChangeMemo", SqlDbType.NVarChar, false);
                PresentOrder = new Field(this, "PresentOrder", "PresentOrder", SqlDbType.Int, false);
                Status = new Field(this, "Status", "Status", SqlDbType.SmallInt, false);
                IsScore = new Field(this, "IsScore", "IsScore", SqlDbType.SmallInt, false);
                AllowWinMember = new Field(this, "AllowWinMember", "AllowWinMember", SqlDbType.NVarChar, false);
                IsHot = new Field(this, "IsHot", "IsHot", SqlDbType.Bit, false);
                TimeStart = new Field(this, "TimeStart", "TimeStart", SqlDbType.SmallDateTime, false);
                TimeEnd = new Field(this, "TimeEnd", "TimeEnd", SqlDbType.SmallDateTime, false);
            }
        }

        public class T_ScorePresentShop : TableBase
        {
            public Field ID;
            public Field ShopName;
            public Field Telephone;
            public Field MobilePhone;
            public Field ContactName;

            public T_ScorePresentShop()
            {
                TableName = "T_ScorePresentShop";

                ID = new Field(this, "ID", "ID", SqlDbType.BigInt, true);
                ShopName = new Field(this, "ShopName", "ShopName", SqlDbType.NVarChar, false);
                Telephone = new Field(this, "Telephone", "Telephone", SqlDbType.VarChar, false);
                MobilePhone = new Field(this, "MobilePhone", "MobilePhone", SqlDbType.VarChar, false);
                ContactName = new Field(this, "ContactName", "ContactName", SqlDbType.NVarChar, false);
            }
        }

        public class T_ScorePresentType : TableBase
        {
            public Field ID;
            public Field ParentID;
            public Field TypeName;
            public Field CreateDate;
            public Field OperatorID;

            public T_ScorePresentType()
            {
                TableName = "T_ScorePresentType";

                ID = new Field(this, "ID", "ID", SqlDbType.BigInt, true);
                ParentID = new Field(this, "ParentID", "ParentID", SqlDbType.BigInt, false);
                TypeName = new Field(this, "TypeName", "TypeName", SqlDbType.NVarChar, false);
                CreateDate = new Field(this, "CreateDate", "CreateDate", SqlDbType.SmallDateTime, false);
                OperatorID = new Field(this, "OperatorID", "OperatorID", SqlDbType.BigInt, false);
            }
        }

        public class T_ScoreUserAddress : TableBase
        {
            public Field UserID;
            public Field PresentCityID;
            public Field PresentAddress;
            public Field PresentPostCode;
            public Field PresentPhone;
            public Field PresentMobile;
            public Field PresentContact;
            public Field PresentMemo;

            public T_ScoreUserAddress()
            {
                TableName = "T_ScoreUserAddress";

                UserID = new Field(this, "UserID", "UserID", SqlDbType.BigInt, false);
                PresentCityID = new Field(this, "PresentCityID", "PresentCityID", SqlDbType.NChar, false);
                PresentAddress = new Field(this, "PresentAddress", "PresentAddress", SqlDbType.NVarChar, false);
                PresentPostCode = new Field(this, "PresentPostCode", "PresentPostCode", SqlDbType.VarChar, false);
                PresentPhone = new Field(this, "PresentPhone", "PresentPhone", SqlDbType.VarChar, false);
                PresentMobile = new Field(this, "PresentMobile", "PresentMobile", SqlDbType.VarChar, false);
                PresentContact = new Field(this, "PresentContact", "PresentContact", SqlDbType.NVarChar, false);
                PresentMemo = new Field(this, "PresentMemo", "PresentMemo", SqlDbType.NVarChar, false);
            }
        }

        public class T_ScoreUserChange : TableBase
        {
            public Field ID;
            public Field UserID;
            public Field PresentRuleID;
            public Field ChangeType;
            public Field UseScore;
            public Field IsGoldScore;
            public Field IsGetPresent;
            public Field CreateDate;
            public Field Qty;

            public T_ScoreUserChange()
            {
                TableName = "T_ScoreUserChange";

                ID = new Field(this, "ID", "ID", SqlDbType.BigInt, true);
                UserID = new Field(this, "UserID", "UserID", SqlDbType.BigInt, false);
                PresentRuleID = new Field(this, "PresentRuleID", "PresentRuleID", SqlDbType.BigInt, false);
                ChangeType = new Field(this, "ChangeType", "ChangeType", SqlDbType.SmallInt, false);
                UseScore = new Field(this, "UseScore", "UseScore", SqlDbType.Int, false);
                IsGoldScore = new Field(this, "IsGoldScore", "IsGoldScore", SqlDbType.SmallInt, false);
                IsGetPresent = new Field(this, "IsGetPresent", "IsGetPresent", SqlDbType.Bit, false);
                CreateDate = new Field(this, "CreateDate", "CreateDate", SqlDbType.SmallDateTime, false);
                Qty = new Field(this, "Qty", "Qty", SqlDbType.Int, false);
            }
        }

        public class T_SiteAffiches : TableBase
        {
            public Field ID;
            public Field SiteID;
            public Field DateTime;
            public Field Title;
            public Field isShow;
            public Field isCommend;
            public Field Content;

            public T_SiteAffiches()
            {
                TableName = "T_SiteAffiches";

                ID = new Field(this, "ID", "ID", SqlDbType.BigInt, true);
                SiteID = new Field(this, "SiteID", "SiteID", SqlDbType.BigInt, false);
                DateTime = new Field(this, "DateTime", "DateTime", SqlDbType.DateTime, false);
                Title = new Field(this, "Title", "Title", SqlDbType.VarChar, false);
                isShow = new Field(this, "isShow", "isShow", SqlDbType.Bit, false);
                isCommend = new Field(this, "isCommend", "isCommend", SqlDbType.Bit, false);
                Content = new Field(this, "Content", "Content", SqlDbType.VarChar, false);
            }
        }

        public class T_Sites : TableBase
        {
            public Field ID;
            public Field ParentID;
            public Field OwnerUserID;
            public Field Name;
            public Field LogoUrl;
            public Field Company;
            public Field Address;
            public Field PostCode;
            public Field ResponsiblePerson;
            public Field ContactPerson;
            public Field Telephone;
            public Field Fax;
            public Field Mobile;
            public Field Email;
            public Field QQ;
            public Field ServiceTelephone;
            public Field ICPCert;
            public Field Level;
            public Field ON;
            public Field BonusScale;
            public Field MaxSubSites;
            public Field UseLotteryListRestrictions;
            public Field UseLotteryList;
            public Field UseLotteryListQuickBuy;
            public Field Opt_BettingStationName;
            public Field Opt_BettingStationNumber;
            public Field Opt_BettingStationAddress;
            public Field Opt_BettingStationTelephone;
            public Field Opt_BettingStationContactPreson;
            public Field Opt_EmailServer_From;
            public Field Opt_EmailServer_EmailServer;
            public Field Opt_EmailServer_UserName;
            public Field Opt_EmailServer_Password;
            public Field Opt_ISP_HostName;
            public Field Opt_ISP_HostPort;
            public Field Opt_ISP_UserID;
            public Field Opt_ISP_UserPassword;
            public Field Opt_ISP_RegCode;
            public Field Opt_ISP_ServiceNumber;
            public Field Opt_ForumUrl;
            public Field Opt_MobileCheckCharset;
            public Field Opt_MobileCheckStringLength;
            public Field Opt_SMSPayType;
            public Field Opt_SMSPrice;
            public Field Opt_isUseCheckCode;
            public Field Opt_CheckCodeCharset;
            public Field Opt_isWriteLog;
            public Field Opt_InitiateSchemeBonusScale;
            public Field Opt_InitiateSchemeMinBuyScale;
            public Field Opt_InitiateSchemeMinBuyAndAssureScale;
            public Field Opt_InitiateSchemeMaxNum;
            public Field Opt_InitiateFollowSchemeMaxNum;
            public Field Opt_QuashSchemeMaxNum;
            public Field Opt_FullSchemeCanQuash;
            public Field Opt_SchemeMinMoney;
            public Field Opt_SchemeMaxMoney;
            public Field Opt_FirstPageUnionBuyMaxRows;
            public Field Opt_isFirstPageUnionBuyWithAll;
            public Field Opt_isBuyValidPasswordAdv;
            public Field Opt_MaxShowLotteryNumberRows;
            public Field Opt_LotteryCountOfMenuBarRow;
            public Field Opt_ScoringOfSelfBuy;
            public Field Opt_ScoringOfCommendBuy;
            public Field Opt_ScoringExchangeRate;
            public Field Opt_Scoring_Status_ON;
            public Field Opt_SchemeChatRoom_StopChatDaysAfterOpened;
            public Field Opt_SchemeChatRoom_MaxChatNumberOf;
            public Field Opt_isShowFloatAD;
            public Field Opt_MemberSharing_Alipay_Status_ON;
            public Field Opt_CpsBonusScale;
            public Field Opt_Cps_Status_ON;
            public Field Opt_Experts_Status_ON;
            public Field Opt_PageTitle;
            public Field Opt_PageKeywords;
            public Field Opt_DefaultFirstPageType;
            public Field Opt_DefaultLotteryFirstPageType;
            public Field Opt_LotteryChannelPage;
            public Field Opt_isShowSMSSubscriptionNavigate;
            public Field Opt_isShowChartNavigate;
            public Field Opt_RoomStyle;
            public Field Opt_RoomLogoUrl;
            public Field Opt_UpdateLotteryDateTime;
            public Field Opt_InitiateSchemeLimitLowerScaleMoney;
            public Field Opt_InitiateSchemeLimitLowerScale;
            public Field Opt_InitiateSchemeLimitUpperScaleMoney;
            public Field Opt_InitiateSchemeLimitUpperScale;
            public Field Opt_About;
            public Field Opt_RightFloatADContent;
            public Field Opt_ContactUS;
            public Field Opt_UserRegisterAgreement;
            public Field Opt_SurrogateFAQ;
            public Field Opt_OfficialAuthorization;
            public Field Opt_CompanyQualification;
            public Field Opt_ExpertsNote;
            public Field Opt_SMSSubscription;
            public Field Opt_LawAffirmsThat;
            public Field Opt_CpsPolicies;
            public Field TemplateEmail_Register;
            public Field TemplateEmail_RegisterAdv;
            public Field TemplateEmail_ForgetPassword;
            public Field TemplateEmail_UserEdit;
            public Field TemplateEmail_UserEditAdv;
            public Field TemplateEmail_InitiateScheme;
            public Field TemplateEmail_JoinScheme;
            public Field TemplateEmail_InitiateChaseTask;
            public Field TemplateEmail_ExecChaseTaskDetail;
            public Field TemplateEmail_TryDistill;
            public Field TemplateEmail_DistillAccept;
            public Field TemplateEmail_DistillNoAccept;
            public Field TemplateEmail_Quash;
            public Field TemplateEmail_QuashScheme;
            public Field TemplateEmail_QuashChaseTaskDetail;
            public Field TemplateEmail_QuashChaseTask;
            public Field TemplateEmail_Win;
            public Field TemplateEmail_MobileValid;
            public Field TemplateEmail_MobileValided;
            public Field TemplateStationSMS_Register;
            public Field TemplateStationSMS_RegisterAdv;
            public Field TemplateStationSMS_ForgetPassword;
            public Field TemplateStationSMS_UserEdit;
            public Field TemplateStationSMS_UserEditAdv;
            public Field TemplateStationSMS_InitiateScheme;
            public Field TemplateStationSMS_JoinScheme;
            public Field TemplateStationSMS_InitiateChaseTask;
            public Field TemplateStationSMS_ExecChaseTaskDetail;
            public Field TemplateStationSMS_TryDistill;
            public Field TemplateStationSMS_DistillAccept;
            public Field TemplateStationSMS_DistillNoAccept;
            public Field TemplateStationSMS_Quash;
            public Field TemplateStationSMS_QuashScheme;
            public Field TemplateStationSMS_QuashChaseTaskDetail;
            public Field TemplateStationSMS_QuashChaseTask;
            public Field TemplateStationSMS_Win;
            public Field TemplateStationSMS_MobileValid;
            public Field TemplateStationSMS_MobileValided;
            public Field TemplateSMS_Register;
            public Field TemplateSMS_RegisterAdv;
            public Field TemplateSMS_ForgetPassword;
            public Field TemplateSMS_UserEdit;
            public Field TemplateSMS_UserEditAdv;
            public Field TemplateSMS_InitiateScheme;
            public Field TemplateSMS_JoinScheme;
            public Field TemplateSMS_InitiateChaseTask;
            public Field TemplateSMS_ExecChaseTaskDetail;
            public Field TemplateSMS_TryDistill;
            public Field TemplateSMS_DistillAccept;
            public Field TemplateSMS_DistillNoAccept;
            public Field TemplateSMS_Quash;
            public Field TemplateSMS_QuashScheme;
            public Field TemplateSMS_QuashChaseTaskDetail;
            public Field TemplateSMS_QuashChaseTask;
            public Field TemplateSMS_Win;
            public Field TemplateSMS_MobileValid;
            public Field TemplateSMS_MobileValided;
            public Field Opt_CPSRegisterAgreement;
            public Field Opt_PromotionMemberBonusScale;
            public Field Opt_PromotionSiteBonusScale;
            public Field Opt_Promotion_Status_ON;
            public Field Opt_FloatNotifiesTime;
            public Field Opt_Score_Compendium;
            public Field Opt_Score_PrententType;

            public T_Sites()
            {
                TableName = "T_Sites";

                ID = new Field(this, "ID", "ID", SqlDbType.BigInt, true);
                ParentID = new Field(this, "ParentID", "ParentID", SqlDbType.BigInt, false);
                OwnerUserID = new Field(this, "OwnerUserID", "OwnerUserID", SqlDbType.BigInt, false);
                Name = new Field(this, "Name", "Name", SqlDbType.VarChar, false);
                LogoUrl = new Field(this, "LogoUrl", "LogoUrl", SqlDbType.VarChar, false);
                Company = new Field(this, "Company", "Company", SqlDbType.VarChar, false);
                Address = new Field(this, "Address", "Address", SqlDbType.VarChar, false);
                PostCode = new Field(this, "PostCode", "PostCode", SqlDbType.VarChar, false);
                ResponsiblePerson = new Field(this, "ResponsiblePerson", "ResponsiblePerson", SqlDbType.VarChar, false);
                ContactPerson = new Field(this, "ContactPerson", "ContactPerson", SqlDbType.VarChar, false);
                Telephone = new Field(this, "Telephone", "Telephone", SqlDbType.VarChar, false);
                Fax = new Field(this, "Fax", "Fax", SqlDbType.VarChar, false);
                Mobile = new Field(this, "Mobile", "Mobile", SqlDbType.VarChar, false);
                Email = new Field(this, "Email", "Email", SqlDbType.VarChar, false);
                QQ = new Field(this, "QQ", "QQ", SqlDbType.VarChar, false);
                ServiceTelephone = new Field(this, "ServiceTelephone", "ServiceTelephone", SqlDbType.VarChar, false);
                ICPCert = new Field(this, "ICPCert", "ICPCert", SqlDbType.VarChar, false);
                Level = new Field(this, "Level", "Level", SqlDbType.SmallInt, false);
                ON = new Field(this, "ON", "ON", SqlDbType.Bit, false);
                BonusScale = new Field(this, "BonusScale", "BonusScale", SqlDbType.Float, false);
                MaxSubSites = new Field(this, "MaxSubSites", "MaxSubSites", SqlDbType.Int, false);
                UseLotteryListRestrictions = new Field(this, "UseLotteryListRestrictions", "UseLotteryListRestrictions", SqlDbType.VarChar, false);
                UseLotteryList = new Field(this, "UseLotteryList", "UseLotteryList", SqlDbType.VarChar, false);
                UseLotteryListQuickBuy = new Field(this, "UseLotteryListQuickBuy", "UseLotteryListQuickBuy", SqlDbType.VarChar, false);
                Opt_BettingStationName = new Field(this, "Opt_BettingStationName", "Opt_BettingStationName", SqlDbType.VarChar, false);
                Opt_BettingStationNumber = new Field(this, "Opt_BettingStationNumber", "Opt_BettingStationNumber", SqlDbType.VarChar, false);
                Opt_BettingStationAddress = new Field(this, "Opt_BettingStationAddress", "Opt_BettingStationAddress", SqlDbType.VarChar, false);
                Opt_BettingStationTelephone = new Field(this, "Opt_BettingStationTelephone", "Opt_BettingStationTelephone", SqlDbType.VarChar, false);
                Opt_BettingStationContactPreson = new Field(this, "Opt_BettingStationContactPreson", "Opt_BettingStationContactPreson", SqlDbType.VarChar, false);
                Opt_EmailServer_From = new Field(this, "Opt_EmailServer_From", "Opt_EmailServer_From", SqlDbType.VarChar, false);
                Opt_EmailServer_EmailServer = new Field(this, "Opt_EmailServer_EmailServer", "Opt_EmailServer_EmailServer", SqlDbType.VarChar, false);
                Opt_EmailServer_UserName = new Field(this, "Opt_EmailServer_UserName", "Opt_EmailServer_UserName", SqlDbType.VarChar, false);
                Opt_EmailServer_Password = new Field(this, "Opt_EmailServer_Password", "Opt_EmailServer_Password", SqlDbType.VarChar, false);
                Opt_ISP_HostName = new Field(this, "Opt_ISP_HostName", "Opt_ISP_HostName", SqlDbType.VarChar, false);
                Opt_ISP_HostPort = new Field(this, "Opt_ISP_HostPort", "Opt_ISP_HostPort", SqlDbType.VarChar, false);
                Opt_ISP_UserID = new Field(this, "Opt_ISP_UserID", "Opt_ISP_UserID", SqlDbType.VarChar, false);
                Opt_ISP_UserPassword = new Field(this, "Opt_ISP_UserPassword", "Opt_ISP_UserPassword", SqlDbType.VarChar, false);
                Opt_ISP_RegCode = new Field(this, "Opt_ISP_RegCode", "Opt_ISP_RegCode", SqlDbType.VarChar, false);
                Opt_ISP_ServiceNumber = new Field(this, "Opt_ISP_ServiceNumber", "Opt_ISP_ServiceNumber", SqlDbType.VarChar, false);
                Opt_ForumUrl = new Field(this, "Opt_ForumUrl", "Opt_ForumUrl", SqlDbType.VarChar, false);
                Opt_MobileCheckCharset = new Field(this, "Opt_MobileCheckCharset", "Opt_MobileCheckCharset", SqlDbType.SmallInt, false);
                Opt_MobileCheckStringLength = new Field(this, "Opt_MobileCheckStringLength", "Opt_MobileCheckStringLength", SqlDbType.SmallInt, false);
                Opt_SMSPayType = new Field(this, "Opt_SMSPayType", "Opt_SMSPayType", SqlDbType.SmallInt, false);
                Opt_SMSPrice = new Field(this, "Opt_SMSPrice", "Opt_SMSPrice", SqlDbType.Money, false);
                Opt_isUseCheckCode = new Field(this, "Opt_isUseCheckCode", "Opt_isUseCheckCode", SqlDbType.Bit, false);
                Opt_CheckCodeCharset = new Field(this, "Opt_CheckCodeCharset", "Opt_CheckCodeCharset", SqlDbType.SmallInt, false);
                Opt_isWriteLog = new Field(this, "Opt_isWriteLog", "Opt_isWriteLog", SqlDbType.Bit, false);
                Opt_InitiateSchemeBonusScale = new Field(this, "Opt_InitiateSchemeBonusScale", "Opt_InitiateSchemeBonusScale", SqlDbType.Float, false);
                Opt_InitiateSchemeMinBuyScale = new Field(this, "Opt_InitiateSchemeMinBuyScale", "Opt_InitiateSchemeMinBuyScale", SqlDbType.Float, false);
                Opt_InitiateSchemeMinBuyAndAssureScale = new Field(this, "Opt_InitiateSchemeMinBuyAndAssureScale", "Opt_InitiateSchemeMinBuyAndAssureScale", SqlDbType.Float, false);
                Opt_InitiateSchemeMaxNum = new Field(this, "Opt_InitiateSchemeMaxNum", "Opt_InitiateSchemeMaxNum", SqlDbType.SmallInt, false);
                Opt_InitiateFollowSchemeMaxNum = new Field(this, "Opt_InitiateFollowSchemeMaxNum", "Opt_InitiateFollowSchemeMaxNum", SqlDbType.SmallInt, false);
                Opt_QuashSchemeMaxNum = new Field(this, "Opt_QuashSchemeMaxNum", "Opt_QuashSchemeMaxNum", SqlDbType.SmallInt, false);
                Opt_FullSchemeCanQuash = new Field(this, "Opt_FullSchemeCanQuash", "Opt_FullSchemeCanQuash", SqlDbType.Bit, false);
                Opt_SchemeMinMoney = new Field(this, "Opt_SchemeMinMoney", "Opt_SchemeMinMoney", SqlDbType.Money, false);
                Opt_SchemeMaxMoney = new Field(this, "Opt_SchemeMaxMoney", "Opt_SchemeMaxMoney", SqlDbType.Money, false);
                Opt_FirstPageUnionBuyMaxRows = new Field(this, "Opt_FirstPageUnionBuyMaxRows", "Opt_FirstPageUnionBuyMaxRows", SqlDbType.SmallInt, false);
                Opt_isFirstPageUnionBuyWithAll = new Field(this, "Opt_isFirstPageUnionBuyWithAll", "Opt_isFirstPageUnionBuyWithAll", SqlDbType.Bit, false);
                Opt_isBuyValidPasswordAdv = new Field(this, "Opt_isBuyValidPasswordAdv", "Opt_isBuyValidPasswordAdv", SqlDbType.Bit, false);
                Opt_MaxShowLotteryNumberRows = new Field(this, "Opt_MaxShowLotteryNumberRows", "Opt_MaxShowLotteryNumberRows", SqlDbType.SmallInt, false);
                Opt_LotteryCountOfMenuBarRow = new Field(this, "Opt_LotteryCountOfMenuBarRow", "Opt_LotteryCountOfMenuBarRow", SqlDbType.SmallInt, false);
                Opt_ScoringOfSelfBuy = new Field(this, "Opt_ScoringOfSelfBuy", "Opt_ScoringOfSelfBuy", SqlDbType.Float, false);
                Opt_ScoringOfCommendBuy = new Field(this, "Opt_ScoringOfCommendBuy", "Opt_ScoringOfCommendBuy", SqlDbType.Float, false);
                Opt_ScoringExchangeRate = new Field(this, "Opt_ScoringExchangeRate", "Opt_ScoringExchangeRate", SqlDbType.Float, false);
                Opt_Scoring_Status_ON = new Field(this, "Opt_Scoring_Status_ON", "Opt_Scoring_Status_ON", SqlDbType.Bit, false);
                Opt_SchemeChatRoom_StopChatDaysAfterOpened = new Field(this, "Opt_SchemeChatRoom_StopChatDaysAfterOpened", "Opt_SchemeChatRoom_StopChatDaysAfterOpened", SqlDbType.SmallInt, false);
                Opt_SchemeChatRoom_MaxChatNumberOf = new Field(this, "Opt_SchemeChatRoom_MaxChatNumberOf", "Opt_SchemeChatRoom_MaxChatNumberOf", SqlDbType.SmallInt, false);
                Opt_isShowFloatAD = new Field(this, "Opt_isShowFloatAD", "Opt_isShowFloatAD", SqlDbType.Bit, false);
                Opt_MemberSharing_Alipay_Status_ON = new Field(this, "Opt_MemberSharing_Alipay_Status_ON", "Opt_MemberSharing_Alipay_Status_ON", SqlDbType.Bit, false);
                Opt_CpsBonusScale = new Field(this, "Opt_CpsBonusScale", "Opt_CpsBonusScale", SqlDbType.Float, false);
                Opt_Cps_Status_ON = new Field(this, "Opt_Cps_Status_ON", "Opt_Cps_Status_ON", SqlDbType.Bit, false);
                Opt_Experts_Status_ON = new Field(this, "Opt_Experts_Status_ON", "Opt_Experts_Status_ON", SqlDbType.Bit, false);
                Opt_PageTitle = new Field(this, "Opt_PageTitle", "Opt_PageTitle", SqlDbType.VarChar, false);
                Opt_PageKeywords = new Field(this, "Opt_PageKeywords", "Opt_PageKeywords", SqlDbType.VarChar, false);
                Opt_DefaultFirstPageType = new Field(this, "Opt_DefaultFirstPageType", "Opt_DefaultFirstPageType", SqlDbType.SmallInt, false);
                Opt_DefaultLotteryFirstPageType = new Field(this, "Opt_DefaultLotteryFirstPageType", "Opt_DefaultLotteryFirstPageType", SqlDbType.SmallInt, false);
                Opt_LotteryChannelPage = new Field(this, "Opt_LotteryChannelPage", "Opt_LotteryChannelPage", SqlDbType.VarChar, false);
                Opt_isShowSMSSubscriptionNavigate = new Field(this, "Opt_isShowSMSSubscriptionNavigate", "Opt_isShowSMSSubscriptionNavigate", SqlDbType.Bit, false);
                Opt_isShowChartNavigate = new Field(this, "Opt_isShowChartNavigate", "Opt_isShowChartNavigate", SqlDbType.Bit, false);
                Opt_RoomStyle = new Field(this, "Opt_RoomStyle", "Opt_RoomStyle", SqlDbType.SmallInt, false);
                Opt_RoomLogoUrl = new Field(this, "Opt_RoomLogoUrl", "Opt_RoomLogoUrl", SqlDbType.VarChar, false);
                Opt_UpdateLotteryDateTime = new Field(this, "Opt_UpdateLotteryDateTime", "Opt_UpdateLotteryDateTime", SqlDbType.DateTime, false);
                Opt_InitiateSchemeLimitLowerScaleMoney = new Field(this, "Opt_InitiateSchemeLimitLowerScaleMoney", "Opt_InitiateSchemeLimitLowerScaleMoney", SqlDbType.Money, false);
                Opt_InitiateSchemeLimitLowerScale = new Field(this, "Opt_InitiateSchemeLimitLowerScale", "Opt_InitiateSchemeLimitLowerScale", SqlDbType.Float, false);
                Opt_InitiateSchemeLimitUpperScaleMoney = new Field(this, "Opt_InitiateSchemeLimitUpperScaleMoney", "Opt_InitiateSchemeLimitUpperScaleMoney", SqlDbType.Money, false);
                Opt_InitiateSchemeLimitUpperScale = new Field(this, "Opt_InitiateSchemeLimitUpperScale", "Opt_InitiateSchemeLimitUpperScale", SqlDbType.Float, false);
                Opt_About = new Field(this, "Opt_About", "Opt_About", SqlDbType.VarChar, false);
                Opt_RightFloatADContent = new Field(this, "Opt_RightFloatADContent", "Opt_RightFloatADContent", SqlDbType.VarChar, false);
                Opt_ContactUS = new Field(this, "Opt_ContactUS", "Opt_ContactUS", SqlDbType.VarChar, false);
                Opt_UserRegisterAgreement = new Field(this, "Opt_UserRegisterAgreement", "Opt_UserRegisterAgreement", SqlDbType.VarChar, false);
                Opt_SurrogateFAQ = new Field(this, "Opt_SurrogateFAQ", "Opt_SurrogateFAQ", SqlDbType.VarChar, false);
                Opt_OfficialAuthorization = new Field(this, "Opt_OfficialAuthorization", "Opt_OfficialAuthorization", SqlDbType.VarChar, false);
                Opt_CompanyQualification = new Field(this, "Opt_CompanyQualification", "Opt_CompanyQualification", SqlDbType.VarChar, false);
                Opt_ExpertsNote = new Field(this, "Opt_ExpertsNote", "Opt_ExpertsNote", SqlDbType.VarChar, false);
                Opt_SMSSubscription = new Field(this, "Opt_SMSSubscription", "Opt_SMSSubscription", SqlDbType.VarChar, false);
                Opt_LawAffirmsThat = new Field(this, "Opt_LawAffirmsThat", "Opt_LawAffirmsThat", SqlDbType.VarChar, false);
                Opt_CpsPolicies = new Field(this, "Opt_CpsPolicies", "Opt_CpsPolicies", SqlDbType.VarChar, false);
                TemplateEmail_Register = new Field(this, "TemplateEmail_Register", "TemplateEmail_Register", SqlDbType.VarChar, false);
                TemplateEmail_RegisterAdv = new Field(this, "TemplateEmail_RegisterAdv", "TemplateEmail_RegisterAdv", SqlDbType.VarChar, false);
                TemplateEmail_ForgetPassword = new Field(this, "TemplateEmail_ForgetPassword", "TemplateEmail_ForgetPassword", SqlDbType.VarChar, false);
                TemplateEmail_UserEdit = new Field(this, "TemplateEmail_UserEdit", "TemplateEmail_UserEdit", SqlDbType.VarChar, false);
                TemplateEmail_UserEditAdv = new Field(this, "TemplateEmail_UserEditAdv", "TemplateEmail_UserEditAdv", SqlDbType.VarChar, false);
                TemplateEmail_InitiateScheme = new Field(this, "TemplateEmail_InitiateScheme", "TemplateEmail_InitiateScheme", SqlDbType.VarChar, false);
                TemplateEmail_JoinScheme = new Field(this, "TemplateEmail_JoinScheme", "TemplateEmail_JoinScheme", SqlDbType.VarChar, false);
                TemplateEmail_InitiateChaseTask = new Field(this, "TemplateEmail_InitiateChaseTask", "TemplateEmail_InitiateChaseTask", SqlDbType.VarChar, false);
                TemplateEmail_ExecChaseTaskDetail = new Field(this, "TemplateEmail_ExecChaseTaskDetail", "TemplateEmail_ExecChaseTaskDetail", SqlDbType.VarChar, false);
                TemplateEmail_TryDistill = new Field(this, "TemplateEmail_TryDistill", "TemplateEmail_TryDistill", SqlDbType.VarChar, false);
                TemplateEmail_DistillAccept = new Field(this, "TemplateEmail_DistillAccept", "TemplateEmail_DistillAccept", SqlDbType.VarChar, false);
                TemplateEmail_DistillNoAccept = new Field(this, "TemplateEmail_DistillNoAccept", "TemplateEmail_DistillNoAccept", SqlDbType.VarChar, false);
                TemplateEmail_Quash = new Field(this, "TemplateEmail_Quash", "TemplateEmail_Quash", SqlDbType.VarChar, false);
                TemplateEmail_QuashScheme = new Field(this, "TemplateEmail_QuashScheme", "TemplateEmail_QuashScheme", SqlDbType.VarChar, false);
                TemplateEmail_QuashChaseTaskDetail = new Field(this, "TemplateEmail_QuashChaseTaskDetail", "TemplateEmail_QuashChaseTaskDetail", SqlDbType.VarChar, false);
                TemplateEmail_QuashChaseTask = new Field(this, "TemplateEmail_QuashChaseTask", "TemplateEmail_QuashChaseTask", SqlDbType.VarChar, false);
                TemplateEmail_Win = new Field(this, "TemplateEmail_Win", "TemplateEmail_Win", SqlDbType.VarChar, false);
                TemplateEmail_MobileValid = new Field(this, "TemplateEmail_MobileValid", "TemplateEmail_MobileValid", SqlDbType.VarChar, false);
                TemplateEmail_MobileValided = new Field(this, "TemplateEmail_MobileValided", "TemplateEmail_MobileValided", SqlDbType.VarChar, false);
                TemplateStationSMS_Register = new Field(this, "TemplateStationSMS_Register", "TemplateStationSMS_Register", SqlDbType.VarChar, false);
                TemplateStationSMS_RegisterAdv = new Field(this, "TemplateStationSMS_RegisterAdv", "TemplateStationSMS_RegisterAdv", SqlDbType.VarChar, false);
                TemplateStationSMS_ForgetPassword = new Field(this, "TemplateStationSMS_ForgetPassword", "TemplateStationSMS_ForgetPassword", SqlDbType.VarChar, false);
                TemplateStationSMS_UserEdit = new Field(this, "TemplateStationSMS_UserEdit", "TemplateStationSMS_UserEdit", SqlDbType.VarChar, false);
                TemplateStationSMS_UserEditAdv = new Field(this, "TemplateStationSMS_UserEditAdv", "TemplateStationSMS_UserEditAdv", SqlDbType.VarChar, false);
                TemplateStationSMS_InitiateScheme = new Field(this, "TemplateStationSMS_InitiateScheme", "TemplateStationSMS_InitiateScheme", SqlDbType.VarChar, false);
                TemplateStationSMS_JoinScheme = new Field(this, "TemplateStationSMS_JoinScheme", "TemplateStationSMS_JoinScheme", SqlDbType.VarChar, false);
                TemplateStationSMS_InitiateChaseTask = new Field(this, "TemplateStationSMS_InitiateChaseTask", "TemplateStationSMS_InitiateChaseTask", SqlDbType.VarChar, false);
                TemplateStationSMS_ExecChaseTaskDetail = new Field(this, "TemplateStationSMS_ExecChaseTaskDetail", "TemplateStationSMS_ExecChaseTaskDetail", SqlDbType.VarChar, false);
                TemplateStationSMS_TryDistill = new Field(this, "TemplateStationSMS_TryDistill", "TemplateStationSMS_TryDistill", SqlDbType.VarChar, false);
                TemplateStationSMS_DistillAccept = new Field(this, "TemplateStationSMS_DistillAccept", "TemplateStationSMS_DistillAccept", SqlDbType.VarChar, false);
                TemplateStationSMS_DistillNoAccept = new Field(this, "TemplateStationSMS_DistillNoAccept", "TemplateStationSMS_DistillNoAccept", SqlDbType.VarChar, false);
                TemplateStationSMS_Quash = new Field(this, "TemplateStationSMS_Quash", "TemplateStationSMS_Quash", SqlDbType.VarChar, false);
                TemplateStationSMS_QuashScheme = new Field(this, "TemplateStationSMS_QuashScheme", "TemplateStationSMS_QuashScheme", SqlDbType.VarChar, false);
                TemplateStationSMS_QuashChaseTaskDetail = new Field(this, "TemplateStationSMS_QuashChaseTaskDetail", "TemplateStationSMS_QuashChaseTaskDetail", SqlDbType.VarChar, false);
                TemplateStationSMS_QuashChaseTask = new Field(this, "TemplateStationSMS_QuashChaseTask", "TemplateStationSMS_QuashChaseTask", SqlDbType.VarChar, false);
                TemplateStationSMS_Win = new Field(this, "TemplateStationSMS_Win", "TemplateStationSMS_Win", SqlDbType.VarChar, false);
                TemplateStationSMS_MobileValid = new Field(this, "TemplateStationSMS_MobileValid", "TemplateStationSMS_MobileValid", SqlDbType.VarChar, false);
                TemplateStationSMS_MobileValided = new Field(this, "TemplateStationSMS_MobileValided", "TemplateStationSMS_MobileValided", SqlDbType.VarChar, false);
                TemplateSMS_Register = new Field(this, "TemplateSMS_Register", "TemplateSMS_Register", SqlDbType.VarChar, false);
                TemplateSMS_RegisterAdv = new Field(this, "TemplateSMS_RegisterAdv", "TemplateSMS_RegisterAdv", SqlDbType.VarChar, false);
                TemplateSMS_ForgetPassword = new Field(this, "TemplateSMS_ForgetPassword", "TemplateSMS_ForgetPassword", SqlDbType.VarChar, false);
                TemplateSMS_UserEdit = new Field(this, "TemplateSMS_UserEdit", "TemplateSMS_UserEdit", SqlDbType.VarChar, false);
                TemplateSMS_UserEditAdv = new Field(this, "TemplateSMS_UserEditAdv", "TemplateSMS_UserEditAdv", SqlDbType.VarChar, false);
                TemplateSMS_InitiateScheme = new Field(this, "TemplateSMS_InitiateScheme", "TemplateSMS_InitiateScheme", SqlDbType.VarChar, false);
                TemplateSMS_JoinScheme = new Field(this, "TemplateSMS_JoinScheme", "TemplateSMS_JoinScheme", SqlDbType.VarChar, false);
                TemplateSMS_InitiateChaseTask = new Field(this, "TemplateSMS_InitiateChaseTask", "TemplateSMS_InitiateChaseTask", SqlDbType.VarChar, false);
                TemplateSMS_ExecChaseTaskDetail = new Field(this, "TemplateSMS_ExecChaseTaskDetail", "TemplateSMS_ExecChaseTaskDetail", SqlDbType.VarChar, false);
                TemplateSMS_TryDistill = new Field(this, "TemplateSMS_TryDistill", "TemplateSMS_TryDistill", SqlDbType.VarChar, false);
                TemplateSMS_DistillAccept = new Field(this, "TemplateSMS_DistillAccept", "TemplateSMS_DistillAccept", SqlDbType.VarChar, false);
                TemplateSMS_DistillNoAccept = new Field(this, "TemplateSMS_DistillNoAccept", "TemplateSMS_DistillNoAccept", SqlDbType.VarChar, false);
                TemplateSMS_Quash = new Field(this, "TemplateSMS_Quash", "TemplateSMS_Quash", SqlDbType.VarChar, false);
                TemplateSMS_QuashScheme = new Field(this, "TemplateSMS_QuashScheme", "TemplateSMS_QuashScheme", SqlDbType.VarChar, false);
                TemplateSMS_QuashChaseTaskDetail = new Field(this, "TemplateSMS_QuashChaseTaskDetail", "TemplateSMS_QuashChaseTaskDetail", SqlDbType.VarChar, false);
                TemplateSMS_QuashChaseTask = new Field(this, "TemplateSMS_QuashChaseTask", "TemplateSMS_QuashChaseTask", SqlDbType.VarChar, false);
                TemplateSMS_Win = new Field(this, "TemplateSMS_Win", "TemplateSMS_Win", SqlDbType.VarChar, false);
                TemplateSMS_MobileValid = new Field(this, "TemplateSMS_MobileValid", "TemplateSMS_MobileValid", SqlDbType.VarChar, false);
                TemplateSMS_MobileValided = new Field(this, "TemplateSMS_MobileValided", "TemplateSMS_MobileValided", SqlDbType.VarChar, false);
                Opt_CPSRegisterAgreement = new Field(this, "Opt_CPSRegisterAgreement", "Opt_CPSRegisterAgreement", SqlDbType.VarChar, false);
                Opt_PromotionMemberBonusScale = new Field(this, "Opt_PromotionMemberBonusScale", "Opt_PromotionMemberBonusScale", SqlDbType.Float, false);
                Opt_PromotionSiteBonusScale = new Field(this, "Opt_PromotionSiteBonusScale", "Opt_PromotionSiteBonusScale", SqlDbType.Float, false);
                Opt_Promotion_Status_ON = new Field(this, "Opt_Promotion_Status_ON", "Opt_Promotion_Status_ON", SqlDbType.Bit, false);
                Opt_FloatNotifiesTime = new Field(this, "Opt_FloatNotifiesTime", "Opt_FloatNotifiesTime", SqlDbType.SmallInt, false);
                Opt_Score_Compendium = new Field(this, "Opt_Score_Compendium", "Opt_Score_Compendium", SqlDbType.Decimal, false);
                Opt_Score_PrententType = new Field(this, "Opt_Score_PrententType", "Opt_Score_PrententType", SqlDbType.SmallInt, false);
            }
        }

        public class T_SiteSendNotificationTypes : TableBase
        {
            public Field SiteID;
            public Field Manner;
            public Field NotificationTypeID;

            public T_SiteSendNotificationTypes()
            {
                TableName = "T_SiteSendNotificationTypes";

                SiteID = new Field(this, "SiteID", "SiteID", SqlDbType.BigInt, false);
                Manner = new Field(this, "Manner", "Manner", SqlDbType.SmallInt, false);
                NotificationTypeID = new Field(this, "NotificationTypeID", "NotificationTypeID", SqlDbType.SmallInt, false);
            }
        }

        public class T_SiteUrls : TableBase
        {
            public Field ID;
            public Field SiteID;
            public Field Url;

            public T_SiteUrls()
            {
                TableName = "T_SiteUrls";

                ID = new Field(this, "ID", "ID", SqlDbType.BigInt, true);
                SiteID = new Field(this, "SiteID", "SiteID", SqlDbType.BigInt, false);
                Url = new Field(this, "Url", "Url", SqlDbType.VarChar, false);
            }
        }

        public class T_SMS : TableBase
        {
            public Field ID;
            public Field SiteID;
            public Field SMSID;
            public Field From;
            public Field To;
            public Field DateTime;
            public Field Content;
            public Field IsSent;

            public T_SMS()
            {
                TableName = "T_SMS";

                ID = new Field(this, "ID", "ID", SqlDbType.BigInt, true);
                SiteID = new Field(this, "SiteID", "SiteID", SqlDbType.BigInt, false);
                SMSID = new Field(this, "SMSID", "SMSID", SqlDbType.BigInt, false);
                From = new Field(this, "From", "From", SqlDbType.VarChar, false);
                To = new Field(this, "To", "To", SqlDbType.VarChar, false);
                DateTime = new Field(this, "DateTime", "DateTime", SqlDbType.DateTime, false);
                Content = new Field(this, "Content", "Content", SqlDbType.VarChar, false);
                IsSent = new Field(this, "IsSent", "IsSent", SqlDbType.Bit, false);
            }
        }

        public class T_SmsBettings : TableBase
        {
            public Field ID;
            public Field SiteID;
            public Field SMSID;
            public Field From;
            public Field DateTime;
            public Field Content;
            public Field SchemeID;
            public Field HandleResult;
            public Field HandleDescription;

            public T_SmsBettings()
            {
                TableName = "T_SmsBettings";

                ID = new Field(this, "ID", "ID", SqlDbType.BigInt, true);
                SiteID = new Field(this, "SiteID", "SiteID", SqlDbType.BigInt, false);
                SMSID = new Field(this, "SMSID", "SMSID", SqlDbType.BigInt, false);
                From = new Field(this, "From", "From", SqlDbType.VarChar, false);
                DateTime = new Field(this, "DateTime", "DateTime", SqlDbType.DateTime, false);
                Content = new Field(this, "Content", "Content", SqlDbType.VarChar, false);
                SchemeID = new Field(this, "SchemeID", "SchemeID", SqlDbType.BigInt, false);
                HandleResult = new Field(this, "HandleResult", "HandleResult", SqlDbType.Bit, false);
                HandleDescription = new Field(this, "HandleDescription", "HandleDescription", SqlDbType.VarChar, false);
            }
        }

        public class T_SoftDownloads : TableBase
        {
            public Field ID;
            public Field SiteID;
            public Field LotteryID;
            public Field DateTime;
            public Field Title;
            public Field FileUrl;
            public Field ImageUrl;
            public Field isHot;
            public Field isCommend;
            public Field isShow;
            public Field ReadCount;
            public Field Content;

            public T_SoftDownloads()
            {
                TableName = "T_SoftDownloads";

                ID = new Field(this, "ID", "ID", SqlDbType.BigInt, true);
                SiteID = new Field(this, "SiteID", "SiteID", SqlDbType.BigInt, false);
                LotteryID = new Field(this, "LotteryID", "LotteryID", SqlDbType.Int, false);
                DateTime = new Field(this, "DateTime", "DateTime", SqlDbType.DateTime, false);
                Title = new Field(this, "Title", "Title", SqlDbType.VarChar, false);
                FileUrl = new Field(this, "FileUrl", "FileUrl", SqlDbType.VarChar, false);
                ImageUrl = new Field(this, "ImageUrl", "ImageUrl", SqlDbType.VarChar, false);
                isHot = new Field(this, "isHot", "isHot", SqlDbType.Bit, false);
                isCommend = new Field(this, "isCommend", "isCommend", SqlDbType.Bit, false);
                isShow = new Field(this, "isShow", "isShow", SqlDbType.Bit, false);
                ReadCount = new Field(this, "ReadCount", "ReadCount", SqlDbType.Int, false);
                Content = new Field(this, "Content", "Content", SqlDbType.VarChar, false);
            }
        }

        public class T_StationSMS : TableBase
        {
            public Field ID;
            public Field SiteID;
            public Field SourceID;
            public Field AimID;
            public Field Type;
            public Field DateTime;
            public Field isShow;
            public Field Content;

            public T_StationSMS()
            {
                TableName = "T_StationSMS";

                ID = new Field(this, "ID", "ID", SqlDbType.BigInt, true);
                SiteID = new Field(this, "SiteID", "SiteID", SqlDbType.BigInt, false);
                SourceID = new Field(this, "SourceID", "SourceID", SqlDbType.BigInt, false);
                AimID = new Field(this, "AimID", "AimID", SqlDbType.BigInt, false);
                Type = new Field(this, "Type", "Type", SqlDbType.SmallInt, false);
                DateTime = new Field(this, "DateTime", "DateTime", SqlDbType.DateTime, false);
                isShow = new Field(this, "isShow", "isShow", SqlDbType.Bit, false);
                Content = new Field(this, "Content", "Content", SqlDbType.VarChar, false);
            }
        }

        public class T_SurrogateNotifications : TableBase
        {
            public Field ID;
            public Field SiteID;
            public Field DateTime;
            public Field Title;
            public Field isShow;
            public Field Content;

            public T_SurrogateNotifications()
            {
                TableName = "T_SurrogateNotifications";

                ID = new Field(this, "ID", "ID", SqlDbType.BigInt, true);
                SiteID = new Field(this, "SiteID", "SiteID", SqlDbType.BigInt, false);
                DateTime = new Field(this, "DateTime", "DateTime", SqlDbType.DateTime, false);
                Title = new Field(this, "Title", "Title", SqlDbType.VarChar, false);
                isShow = new Field(this, "isShow", "isShow", SqlDbType.Bit, false);
                Content = new Field(this, "Content", "Content", SqlDbType.VarChar, false);
            }
        }

        public class T_SurrogateTrys : TableBase
        {
            public Field ID;
            public Field SiteID;
            public Field UserID;
            public Field DateTime;
            public Field HandleResult;
            public Field HandlelDateTime;
            public Field Name;
            public Field LogoUrl;
            public Field Company;
            public Field Address;
            public Field PostCode;
            public Field ResponsiblePerson;
            public Field ContactPerson;
            public Field Telephone;
            public Field Fax;
            public Field Mobile;
            public Field Email;
            public Field QQ;
            public Field ServiceTelephone;
            public Field UseLotteryList;
            public Field Urls;
            public Field Content;

            public T_SurrogateTrys()
            {
                TableName = "T_SurrogateTrys";

                ID = new Field(this, "ID", "ID", SqlDbType.BigInt, true);
                SiteID = new Field(this, "SiteID", "SiteID", SqlDbType.BigInt, false);
                UserID = new Field(this, "UserID", "UserID", SqlDbType.BigInt, false);
                DateTime = new Field(this, "DateTime", "DateTime", SqlDbType.DateTime, false);
                HandleResult = new Field(this, "HandleResult", "HandleResult", SqlDbType.SmallInt, false);
                HandlelDateTime = new Field(this, "HandlelDateTime", "HandlelDateTime", SqlDbType.DateTime, false);
                Name = new Field(this, "Name", "Name", SqlDbType.VarChar, false);
                LogoUrl = new Field(this, "LogoUrl", "LogoUrl", SqlDbType.VarChar, false);
                Company = new Field(this, "Company", "Company", SqlDbType.VarChar, false);
                Address = new Field(this, "Address", "Address", SqlDbType.VarChar, false);
                PostCode = new Field(this, "PostCode", "PostCode", SqlDbType.VarChar, false);
                ResponsiblePerson = new Field(this, "ResponsiblePerson", "ResponsiblePerson", SqlDbType.VarChar, false);
                ContactPerson = new Field(this, "ContactPerson", "ContactPerson", SqlDbType.VarChar, false);
                Telephone = new Field(this, "Telephone", "Telephone", SqlDbType.VarChar, false);
                Fax = new Field(this, "Fax", "Fax", SqlDbType.VarChar, false);
                Mobile = new Field(this, "Mobile", "Mobile", SqlDbType.VarChar, false);
                Email = new Field(this, "Email", "Email", SqlDbType.VarChar, false);
                QQ = new Field(this, "QQ", "QQ", SqlDbType.VarChar, false);
                ServiceTelephone = new Field(this, "ServiceTelephone", "ServiceTelephone", SqlDbType.VarChar, false);
                UseLotteryList = new Field(this, "UseLotteryList", "UseLotteryList", SqlDbType.VarChar, false);
                Urls = new Field(this, "Urls", "Urls", SqlDbType.VarChar, false);
                Content = new Field(this, "Content", "Content", SqlDbType.VarChar, false);
            }
        }

        public class T_SystemLog : TableBase
        {
            public Field ID;
            public Field SiteID;
            public Field UserID;
            public Field DateTime;
            public Field IPAddress;
            public Field Description;

            public T_SystemLog()
            {
                TableName = "T_SystemLog";

                ID = new Field(this, "ID", "ID", SqlDbType.BigInt, true);
                SiteID = new Field(this, "SiteID", "SiteID", SqlDbType.BigInt, false);
                UserID = new Field(this, "UserID", "UserID", SqlDbType.BigInt, false);
                DateTime = new Field(this, "DateTime", "DateTime", SqlDbType.DateTime, false);
                IPAddress = new Field(this, "IPAddress", "IPAddress", SqlDbType.VarChar, false);
                Description = new Field(this, "Description", "Description", SqlDbType.SmallInt, false);
            }
        }

        public class T_TestNumber : TableBase
        {
            public Field ID;
            public Field IsuseID;
            public Field TestNumber;

            public T_TestNumber()
            {
                TableName = "T_TestNumber";

                ID = new Field(this, "ID", "ID", SqlDbType.BigInt, true);
                IsuseID = new Field(this, "IsuseID", "IsuseID", SqlDbType.BigInt, false);
                TestNumber = new Field(this, "TestNumber", "TestNumber", SqlDbType.VarChar, false);
            }
        }

        public class T_TomActivities : TableBase
        {
            public Field ID;
            public Field UserID;
            public Field DateTime;
            public Field AlipayName;
            public Field IsReward1;
            public Field DayBalanceAdd;
            public Field IsReward2;
            public Field DaySchemeMoney;
            public Field IsReward10;
            public Field DayWinMoney;
            public Field IsReward200;

            public T_TomActivities()
            {
                TableName = "T_TomActivities";

                ID = new Field(this, "ID", "ID", SqlDbType.BigInt, true);
                UserID = new Field(this, "UserID", "UserID", SqlDbType.BigInt, false);
                DateTime = new Field(this, "DateTime", "DateTime", SqlDbType.DateTime, false);
                AlipayName = new Field(this, "AlipayName", "AlipayName", SqlDbType.VarChar, false);
                IsReward1 = new Field(this, "IsReward1", "IsReward1", SqlDbType.Bit, false);
                DayBalanceAdd = new Field(this, "DayBalanceAdd", "DayBalanceAdd", SqlDbType.Money, false);
                IsReward2 = new Field(this, "IsReward2", "IsReward2", SqlDbType.Bit, false);
                DaySchemeMoney = new Field(this, "DaySchemeMoney", "DaySchemeMoney", SqlDbType.Money, false);
                IsReward10 = new Field(this, "IsReward10", "IsReward10", SqlDbType.Bit, false);
                DayWinMoney = new Field(this, "DayWinMoney", "DayWinMoney", SqlDbType.Money, false);
                IsReward200 = new Field(this, "IsReward200", "IsReward200", SqlDbType.Bit, false);
            }
        }

        public class T_TotalMoney : TableBase
        {
            public Field ID;
            public Field IsuseID;
            public Field TotalMoney;

            public T_TotalMoney()
            {
                TableName = "T_TotalMoney";

                ID = new Field(this, "ID", "ID", SqlDbType.BigInt, true);
                IsuseID = new Field(this, "IsuseID", "IsuseID", SqlDbType.BigInt, false);
                TotalMoney = new Field(this, "TotalMoney", "TotalMoney", SqlDbType.VarChar, false);
            }
        }

        public class T_TrendCharts : TableBase
        {
            public Field ID;
            public Field LotteryID;
            public Field TrendChartName;
            public Field TrendChartUrl;
            public Field Order;

            public T_TrendCharts()
            {
                TableName = "T_TrendCharts";

                ID = new Field(this, "ID", "ID", SqlDbType.Int, true);
                LotteryID = new Field(this, "LotteryID", "LotteryID", SqlDbType.Int, false);
                TrendChartName = new Field(this, "TrendChartName", "TrendChartName", SqlDbType.VarChar, false);
                TrendChartUrl = new Field(this, "TrendChartUrl", "TrendChartUrl", SqlDbType.VarChar, false);
                Order = new Field(this, "Order", "Order", SqlDbType.Int, false);
            }
        }

        public class T_UnionLinkScale : TableBase
        {
            public Field ID;
            public Field UnionID;
            public Field SiteLinkPID;
            public Field BonusScale;

            public T_UnionLinkScale()
            {
                TableName = "T_UnionLinkScale";

                ID = new Field(this, "ID", "ID", SqlDbType.BigInt, true);
                UnionID = new Field(this, "UnionID", "UnionID", SqlDbType.BigInt, false);
                SiteLinkPID = new Field(this, "SiteLinkPID", "SiteLinkPID", SqlDbType.VarChar, false);
                BonusScale = new Field(this, "BonusScale", "BonusScale", SqlDbType.Decimal, false);
            }
        }

        public class T_UserAcceptNotificationTypes : TableBase
        {
            public Field UserID;
            public Field Manner;
            public Field NotificationTypeID;

            public T_UserAcceptNotificationTypes()
            {
                TableName = "T_UserAcceptNotificationTypes";

                UserID = new Field(this, "UserID", "UserID", SqlDbType.BigInt, false);
                Manner = new Field(this, "Manner", "Manner", SqlDbType.SmallInt, false);
                NotificationTypeID = new Field(this, "NotificationTypeID", "NotificationTypeID", SqlDbType.SmallInt, false);
            }
        }

        public class T_UserActions : TableBase
        {
            public Field ID;
            public Field DateTime;
            public Field UserID;
            public Field Action;
            public Field LastSchemeID;

            public T_UserActions()
            {
                TableName = "T_UserActions";

                ID = new Field(this, "ID", "ID", SqlDbType.BigInt, true);
                DateTime = new Field(this, "DateTime", "DateTime", SqlDbType.DateTime, false);
                UserID = new Field(this, "UserID", "UserID", SqlDbType.BigInt, false);
                Action = new Field(this, "Action", "Action", SqlDbType.VarChar, false);
                LastSchemeID = new Field(this, "LastSchemeID", "LastSchemeID", SqlDbType.BigInt, false);
            }
        }

        public class T_UserBankBindDetails : TableBase
        {
            public Field UserID;
            public Field BankType;
            public Field BankName;
            public Field BankCardNumber;
            public Field BankInProvinceName;
            public Field BankInCityName;
            public Field BankUserName;
            public Field BankTypeName;

            public T_UserBankBindDetails()
            {
                TableName = "T_UserBankBindDetails";

                UserID = new Field(this, "UserID", "UserID", SqlDbType.BigInt, false);
                BankType = new Field(this, "BankType", "BankType", SqlDbType.BigInt, false);
                BankName = new Field(this, "BankName", "BankName", SqlDbType.VarChar, false);
                BankCardNumber = new Field(this, "BankCardNumber", "BankCardNumber", SqlDbType.VarChar, false);
                BankInProvinceName = new Field(this, "BankInProvinceName", "BankInProvinceName", SqlDbType.VarChar, false);
                BankInCityName = new Field(this, "BankInCityName", "BankInCityName", SqlDbType.VarChar, false);
                BankUserName = new Field(this, "BankUserName", "BankUserName", SqlDbType.VarChar, false);
                BankTypeName = new Field(this, "BankTypeName", "BankTypeName", SqlDbType.VarChar, false);
            }
        }

        public class T_UserDetails : TableBase
        {
            public Field ID;
            public Field SiteID;
            public Field UserID;
            public Field DateTime;
            public Field OperatorType;
            public Field Money;
            public Field FormalitiesFees;
            public Field SchemeID;
            public Field RelatedUserID;
            public Field PayNumber;
            public Field PayBank;
            public Field Memo;
            public Field OperatorID;
            public Field AlipayID;
            public Field AlipayName;

            public T_UserDetails()
            {
                TableName = "T_UserDetails";

                ID = new Field(this, "ID", "ID", SqlDbType.BigInt, true);
                SiteID = new Field(this, "SiteID", "SiteID", SqlDbType.BigInt, false);
                UserID = new Field(this, "UserID", "UserID", SqlDbType.BigInt, false);
                DateTime = new Field(this, "DateTime", "DateTime", SqlDbType.DateTime, false);
                OperatorType = new Field(this, "OperatorType", "OperatorType", SqlDbType.SmallInt, false);
                Money = new Field(this, "Money", "Money", SqlDbType.Money, false);
                FormalitiesFees = new Field(this, "FormalitiesFees", "FormalitiesFees", SqlDbType.Money, false);
                SchemeID = new Field(this, "SchemeID", "SchemeID", SqlDbType.BigInt, false);
                RelatedUserID = new Field(this, "RelatedUserID", "RelatedUserID", SqlDbType.BigInt, false);
                PayNumber = new Field(this, "PayNumber", "PayNumber", SqlDbType.VarChar, false);
                PayBank = new Field(this, "PayBank", "PayBank", SqlDbType.VarChar, false);
                Memo = new Field(this, "Memo", "Memo", SqlDbType.VarChar, false);
                OperatorID = new Field(this, "OperatorID", "OperatorID", SqlDbType.BigInt, false);
                AlipayID = new Field(this, "AlipayID", "AlipayID", SqlDbType.VarChar, false);
                AlipayName = new Field(this, "AlipayName", "AlipayName", SqlDbType.VarChar, false);
            }
        }

        public class T_UserDistillPayByAlipayLog : TableBase
        {
            public Field ID;
            public Field DateTime;
            public Field Content;

            public T_UserDistillPayByAlipayLog()
            {
                TableName = "T_UserDistillPayByAlipayLog";

                ID = new Field(this, "ID", "ID", SqlDbType.BigInt, true);
                DateTime = new Field(this, "DateTime", "DateTime", SqlDbType.SmallDateTime, false);
                Content = new Field(this, "Content", "Content", SqlDbType.VarChar, false);
            }
        }

        public class T_UserDistillPaymentFileDetaills : TableBase
        {
            public Field ID;
            public Field PaymentFileID;
            public Field SequenceNumber;
            public Field DistillID;

            public T_UserDistillPaymentFileDetaills()
            {
                TableName = "T_UserDistillPaymentFileDetaills";

                ID = new Field(this, "ID", "ID", SqlDbType.BigInt, true);
                PaymentFileID = new Field(this, "PaymentFileID", "PaymentFileID", SqlDbType.BigInt, false);
                SequenceNumber = new Field(this, "SequenceNumber", "SequenceNumber", SqlDbType.BigInt, false);
                DistillID = new Field(this, "DistillID", "DistillID", SqlDbType.BigInt, false);
            }
        }

        public class T_UserDistillPaymentFiles : TableBase
        {
            public Field id;
            public Field FileName;
            public Field DateTime;
            public Field Result;
            public Field HandleOperatorID;
            public Field Type;

            public T_UserDistillPaymentFiles()
            {
                TableName = "T_UserDistillPaymentFiles";

                id = new Field(this, "id", "id", SqlDbType.BigInt, true);
                FileName = new Field(this, "FileName", "FileName", SqlDbType.VarChar, false);
                DateTime = new Field(this, "DateTime", "DateTime", SqlDbType.DateTime, false);
                Result = new Field(this, "Result", "Result", SqlDbType.Int, false);
                HandleOperatorID = new Field(this, "HandleOperatorID", "HandleOperatorID", SqlDbType.BigInt, false);
                Type = new Field(this, "Type", "Type", SqlDbType.Int, false);
            }
        }

        public class T_UserDistills : TableBase
        {
            public Field ID;
            public Field SiteID;
            public Field UserID;
            public Field DateTime;
            public Field Money;
            public Field FormalitiesFees;
            public Field Result;
            public Field HandleDateTime;
            public Field BankName;
            public Field BankCardNumber;
            public Field Memo;
            public Field HandleOperatorID;
            public Field BankUserName;
            public Field AlipayID;
            public Field AlipayName;
            public Field DistillType;
            public Field BankTypeName;
            public Field BankInProvince;
            public Field BankInCity;
            public Field IsCps;

            public T_UserDistills()
            {
                TableName = "T_UserDistills";

                ID = new Field(this, "ID", "ID", SqlDbType.BigInt, true);
                SiteID = new Field(this, "SiteID", "SiteID", SqlDbType.BigInt, false);
                UserID = new Field(this, "UserID", "UserID", SqlDbType.BigInt, false);
                DateTime = new Field(this, "DateTime", "DateTime", SqlDbType.DateTime, false);
                Money = new Field(this, "Money", "Money", SqlDbType.Money, false);
                FormalitiesFees = new Field(this, "FormalitiesFees", "FormalitiesFees", SqlDbType.Money, false);
                Result = new Field(this, "Result", "Result", SqlDbType.SmallInt, false);
                HandleDateTime = new Field(this, "HandleDateTime", "HandleDateTime", SqlDbType.DateTime, false);
                BankName = new Field(this, "BankName", "BankName", SqlDbType.VarChar, false);
                BankCardNumber = new Field(this, "BankCardNumber", "BankCardNumber", SqlDbType.VarChar, false);
                Memo = new Field(this, "Memo", "Memo", SqlDbType.VarChar, false);
                HandleOperatorID = new Field(this, "HandleOperatorID", "HandleOperatorID", SqlDbType.BigInt, false);
                BankUserName = new Field(this, "BankUserName", "BankUserName", SqlDbType.VarChar, false);
                AlipayID = new Field(this, "AlipayID", "AlipayID", SqlDbType.VarChar, false);
                AlipayName = new Field(this, "AlipayName", "AlipayName", SqlDbType.VarChar, false);
                DistillType = new Field(this, "DistillType", "DistillType", SqlDbType.Int, false);
                BankTypeName = new Field(this, "BankTypeName", "BankTypeName", SqlDbType.VarChar, false);
                BankInProvince = new Field(this, "BankInProvince", "BankInProvince", SqlDbType.VarChar, false);
                BankInCity = new Field(this, "BankInCity", "BankInCity", SqlDbType.VarChar, false);
                IsCps = new Field(this, "IsCps", "IsCps", SqlDbType.Bit, false);
            }
        }

        public class T_UserEditQuestionAnswer : TableBase
        {
            public Field ID;
            public Field UserID;
            public Field QuestionAnswerState;
            public Field ValidedCount;
            public Field DateTime;

            public T_UserEditQuestionAnswer()
            {
                TableName = "T_UserEditQuestionAnswer";

                ID = new Field(this, "ID", "ID", SqlDbType.Int, true);
                UserID = new Field(this, "UserID", "UserID", SqlDbType.Int, false);
                QuestionAnswerState = new Field(this, "QuestionAnswerState", "QuestionAnswerState", SqlDbType.Int, false);
                ValidedCount = new Field(this, "ValidedCount", "ValidedCount", SqlDbType.Int, false);
                DateTime = new Field(this, "DateTime", "DateTime", SqlDbType.DateTime, false);
            }
        }

        public class T_UserForInitiateFollowSchemeTrys : TableBase
        {
            public Field ID;
            public Field SiteID;
            public Field UserID;
            public Field DateTime;
            public Field PlayTypeID;
            public Field Description;
            public Field HandleResult;
            public Field HandleDateTime;

            public T_UserForInitiateFollowSchemeTrys()
            {
                TableName = "T_UserForInitiateFollowSchemeTrys";

                ID = new Field(this, "ID", "ID", SqlDbType.BigInt, true);
                SiteID = new Field(this, "SiteID", "SiteID", SqlDbType.BigInt, false);
                UserID = new Field(this, "UserID", "UserID", SqlDbType.BigInt, false);
                DateTime = new Field(this, "DateTime", "DateTime", SqlDbType.DateTime, false);
                PlayTypeID = new Field(this, "PlayTypeID", "PlayTypeID", SqlDbType.Int, false);
                Description = new Field(this, "Description", "Description", SqlDbType.VarChar, false);
                HandleResult = new Field(this, "HandleResult", "HandleResult", SqlDbType.SmallInt, false);
                HandleDateTime = new Field(this, "HandleDateTime", "HandleDateTime", SqlDbType.DateTime, false);
            }
        }

        public class T_UserGroups : TableBase
        {
            public Field ID;
            public Field Name;
            public Field Description;

            public T_UserGroups()
            {
                TableName = "T_UserGroups";

                ID = new Field(this, "ID", "ID", SqlDbType.SmallInt, false);
                Name = new Field(this, "Name", "Name", SqlDbType.VarChar, false);
                Description = new Field(this, "Description", "Description", SqlDbType.VarChar, false);
            }
        }

        public class T_UserHongbaoPromotion : TableBase
        {
            public Field ID;
            public Field UserID;
            public Field CreateDate;
            public Field Money;
            public Field AcceptUserID;
            public Field UseDate;
            public Field ExpiryDate;
            public Field URL;

            public T_UserHongbaoPromotion()
            {
                TableName = "T_UserHongbaoPromotion";

                ID = new Field(this, "ID", "ID", SqlDbType.BigInt, true);
                UserID = new Field(this, "UserID", "UserID", SqlDbType.BigInt, false);
                CreateDate = new Field(this, "CreateDate", "CreateDate", SqlDbType.DateTime, false);
                Money = new Field(this, "Money", "Money", SqlDbType.Money, false);
                AcceptUserID = new Field(this, "AcceptUserID", "AcceptUserID", SqlDbType.BigInt, false);
                UseDate = new Field(this, "UseDate", "UseDate", SqlDbType.DateTime, false);
                ExpiryDate = new Field(this, "ExpiryDate", "ExpiryDate", SqlDbType.DateTime, false);
                URL = new Field(this, "URL", "URL", SqlDbType.NVarChar, false);
            }
        }

        public class T_UserHongbaoPromotionUsed : TableBase
        {
            public Field PromotionID;

            public T_UserHongbaoPromotionUsed()
            {
                TableName = "T_UserHongbaoPromotionUsed";

                PromotionID = new Field(this, "PromotionID", "PromotionID", SqlDbType.BigInt, false);
            }
        }

        public class T_UserInGroups : TableBase
        {
            public Field UserID;
            public Field GroupID;

            public T_UserInGroups()
            {
                TableName = "T_UserInGroups";

                UserID = new Field(this, "UserID", "UserID", SqlDbType.BigInt, false);
                GroupID = new Field(this, "GroupID", "GroupID", SqlDbType.SmallInt, false);
            }
        }

        public class T_UserInSchemeChatRooms : TableBase
        {
            public Field UserID;
            public Field SchemeID;
            public Field LastAccessTime;

            public T_UserInSchemeChatRooms()
            {
                TableName = "T_UserInSchemeChatRooms";

                UserID = new Field(this, "UserID", "UserID", SqlDbType.BigInt, false);
                SchemeID = new Field(this, "SchemeID", "SchemeID", SqlDbType.BigInt, false);
                LastAccessTime = new Field(this, "LastAccessTime", "LastAccessTime", SqlDbType.DateTime, false);
            }
        }

        public class T_UserPayDetails : TableBase
        {
            public Field ID;
            public Field SiteID;
            public Field UserID;
            public Field DateTime;
            public Field PayType;
            public Field Money;
            public Field FormalitiesFees;
            public Field Result;

            public T_UserPayDetails()
            {
                TableName = "T_UserPayDetails";

                ID = new Field(this, "ID", "ID", SqlDbType.BigInt, true);
                SiteID = new Field(this, "SiteID", "SiteID", SqlDbType.BigInt, false);
                UserID = new Field(this, "UserID", "UserID", SqlDbType.BigInt, false);
                DateTime = new Field(this, "DateTime", "DateTime", SqlDbType.DateTime, false);
                PayType = new Field(this, "PayType", "PayType", SqlDbType.VarChar, false);
                Money = new Field(this, "Money", "Money", SqlDbType.Money, false);
                FormalitiesFees = new Field(this, "FormalitiesFees", "FormalitiesFees", SqlDbType.Money, false);
                Result = new Field(this, "Result", "Result", SqlDbType.SmallInt, false);
            }
        }

        public class T_UserPayNumberList : TableBase
        {
            public Field PayNumber;
            public Field Money;

            public T_UserPayNumberList()
            {
                TableName = "T_UserPayNumberList";

                PayNumber = new Field(this, "PayNumber", "PayNumber", SqlDbType.BigInt, false);
                Money = new Field(this, "Money", "Money", SqlDbType.Money, false);
            }
        }

        public class T_UserPayOutDetails_99Bill : TableBase
        {
            public Field ID;
            public Field DistillID;
            public Field DealCharge;
            public Field DebitCharge;
            public Field CreditCharge;
            public Field DealID;
            public Field ResultFlag;
            public Field FailureCause;

            public T_UserPayOutDetails_99Bill()
            {
                TableName = "T_UserPayOutDetails_99Bill";

                ID = new Field(this, "ID", "ID", SqlDbType.BigInt, true);
                DistillID = new Field(this, "DistillID", "DistillID", SqlDbType.BigInt, false);
                DealCharge = new Field(this, "DealCharge", "DealCharge", SqlDbType.Money, false);
                DebitCharge = new Field(this, "DebitCharge", "DebitCharge", SqlDbType.Money, false);
                CreditCharge = new Field(this, "CreditCharge", "CreditCharge", SqlDbType.Money, false);
                DealID = new Field(this, "DealID", "DealID", SqlDbType.VarChar, false);
                ResultFlag = new Field(this, "ResultFlag", "ResultFlag", SqlDbType.Bit, false);
                FailureCause = new Field(this, "FailureCause", "FailureCause", SqlDbType.VarChar, false);
            }
        }

        public class T_Users : TableBase
        {
            public Field ID;
            public Field SiteID;
            public Field Name;
            public Field RealityName;
            public Field Password;
            public Field PasswordAdv;
            public Field CityID;
            public Field Sex;
            public Field BirthDay;
            public Field IDCardNumber;
            public Field Address;
            public Field Email;
            public Field isEmailValided;
            public Field QQ;
            public Field Telephone;
            public Field Mobile;
            public Field isMobileValided;
            public Field isPrivacy;
            public Field isCanLogin;
            public Field RegisterTime;
            public Field LastLoginTime;
            public Field LastLoginIP;
            public Field LoginCount;
            public Field UserType;
            public Field BankType;
            public Field BankName;
            public Field BankCardNumber;
            public Field Balance;
            public Field Freeze;
            public Field ScoringOfSelfBuy;
            public Field ScoringOfCommendBuy;
            public Field Scoring;
            public Field Level;
            public Field CommenderID;
            public Field CpsID;
            public Field AlipayID;
            public Field Bonus;
            public Field Reward;
            public Field AlipayName;
            public Field isAlipayNameValided;
            public Field isAlipayCps;
            public Field IsCrossLogin;
            public Field ComeFrom;
            public Field Memo;
            public Field BonusThisMonth;
            public Field BonusAllow;
            public Field BonusUse;
            public Field PromotionMemberBonusScale;
            public Field PromotionSiteBonusScale;
            public Field MaxFollowNumber;
            public Field VisitSource;
            public Field Key;
            public Field HeadUrl;
            public Field FriendList;
            public Field NickName;
            public Field SecurityQuestion;
            public Field SecurityAnswer;
            public Field Reason;

            public T_Users()
            {
                TableName = "T_Users";

                ID = new Field(this, "ID", "ID", SqlDbType.BigInt, false);
                SiteID = new Field(this, "SiteID", "SiteID", SqlDbType.BigInt, false);
                Name = new Field(this, "Name", "Name", SqlDbType.VarChar, false);
                RealityName = new Field(this, "RealityName", "RealityName", SqlDbType.VarChar, false);
                Password = new Field(this, "Password", "Password", SqlDbType.VarChar, false);
                PasswordAdv = new Field(this, "PasswordAdv", "PasswordAdv", SqlDbType.VarChar, false);
                CityID = new Field(this, "CityID", "CityID", SqlDbType.Int, false);
                Sex = new Field(this, "Sex", "Sex", SqlDbType.VarChar, false);
                BirthDay = new Field(this, "BirthDay", "BirthDay", SqlDbType.DateTime, false);
                IDCardNumber = new Field(this, "IDCardNumber", "IDCardNumber", SqlDbType.VarChar, false);
                Address = new Field(this, "Address", "Address", SqlDbType.VarChar, false);
                Email = new Field(this, "Email", "Email", SqlDbType.VarChar, false);
                isEmailValided = new Field(this, "isEmailValided", "isEmailValided", SqlDbType.Bit, false);
                QQ = new Field(this, "QQ", "QQ", SqlDbType.VarChar, false);
                Telephone = new Field(this, "Telephone", "Telephone", SqlDbType.VarChar, false);
                Mobile = new Field(this, "Mobile", "Mobile", SqlDbType.VarChar, false);
                isMobileValided = new Field(this, "isMobileValided", "isMobileValided", SqlDbType.Bit, false);
                isPrivacy = new Field(this, "isPrivacy", "isPrivacy", SqlDbType.Bit, false);
                isCanLogin = new Field(this, "isCanLogin", "isCanLogin", SqlDbType.Bit, false);
                RegisterTime = new Field(this, "RegisterTime", "RegisterTime", SqlDbType.DateTime, false);
                LastLoginTime = new Field(this, "LastLoginTime", "LastLoginTime", SqlDbType.DateTime, false);
                LastLoginIP = new Field(this, "LastLoginIP", "LastLoginIP", SqlDbType.VarChar, false);
                LoginCount = new Field(this, "LoginCount", "LoginCount", SqlDbType.Int, false);
                UserType = new Field(this, "UserType", "UserType", SqlDbType.SmallInt, false);
                BankType = new Field(this, "BankType", "BankType", SqlDbType.SmallInt, false);
                BankName = new Field(this, "BankName", "BankName", SqlDbType.VarChar, false);
                BankCardNumber = new Field(this, "BankCardNumber", "BankCardNumber", SqlDbType.VarChar, false);
                Balance = new Field(this, "Balance", "Balance", SqlDbType.Money, false);
                Freeze = new Field(this, "Freeze", "Freeze", SqlDbType.Money, false);
                ScoringOfSelfBuy = new Field(this, "ScoringOfSelfBuy", "ScoringOfSelfBuy", SqlDbType.Float, false);
                ScoringOfCommendBuy = new Field(this, "ScoringOfCommendBuy", "ScoringOfCommendBuy", SqlDbType.Float, false);
                Scoring = new Field(this, "Scoring", "Scoring", SqlDbType.Float, false);
                Level = new Field(this, "Level", "Level", SqlDbType.SmallInt, false);
                CommenderID = new Field(this, "CommenderID", "CommenderID", SqlDbType.BigInt, false);
                CpsID = new Field(this, "CpsID", "CpsID", SqlDbType.BigInt, false);
                AlipayID = new Field(this, "AlipayID", "AlipayID", SqlDbType.VarChar, false);
                Bonus = new Field(this, "Bonus", "Bonus", SqlDbType.Money, false);
                Reward = new Field(this, "Reward", "Reward", SqlDbType.Money, false);
                AlipayName = new Field(this, "AlipayName", "AlipayName", SqlDbType.VarChar, false);
                isAlipayNameValided = new Field(this, "isAlipayNameValided", "isAlipayNameValided", SqlDbType.Bit, false);
                isAlipayCps = new Field(this, "isAlipayCps", "isAlipayCps", SqlDbType.Bit, false);
                IsCrossLogin = new Field(this, "IsCrossLogin", "IsCrossLogin", SqlDbType.Bit, false);
                ComeFrom = new Field(this, "ComeFrom", "ComeFrom", SqlDbType.Int, false);
                Memo = new Field(this, "Memo", "Memo", SqlDbType.VarChar, false);
                BonusThisMonth = new Field(this, "BonusThisMonth", "BonusThisMonth", SqlDbType.Money, false);
                BonusAllow = new Field(this, "BonusAllow", "BonusAllow", SqlDbType.Money, false);
                BonusUse = new Field(this, "BonusUse", "BonusUse", SqlDbType.Money, false);
                PromotionMemberBonusScale = new Field(this, "PromotionMemberBonusScale", "PromotionMemberBonusScale", SqlDbType.Float, false);
                PromotionSiteBonusScale = new Field(this, "PromotionSiteBonusScale", "PromotionSiteBonusScale", SqlDbType.Float, false);
                MaxFollowNumber = new Field(this, "MaxFollowNumber", "MaxFollowNumber", SqlDbType.Int, false);
                VisitSource = new Field(this, "VisitSource", "VisitSource", SqlDbType.VarChar, false);
                Key = new Field(this, "Key", "Key", SqlDbType.VarChar, false);
                HeadUrl = new Field(this, "HeadUrl", "HeadUrl", SqlDbType.VarChar, false);
                FriendList = new Field(this, "FriendList", "FriendList", SqlDbType.VarChar, false);
                NickName = new Field(this, "NickName", "NickName", SqlDbType.VarChar, false);
                SecurityQuestion = new Field(this, "SecurityQuestion", "SecurityQuestion", SqlDbType.VarChar, false);
                SecurityAnswer = new Field(this, "SecurityAnswer", "SecurityAnswer", SqlDbType.NVarChar, false);
                Reason = new Field(this, "Reason", "Reason", SqlDbType.VarChar, false);
            }
        }

        public class T_UserScoringDetails : TableBase
        {
            public Field ID;
            public Field SiteID;
            public Field UserID;
            public Field DateTime;
            public Field OperatorType;
            public Field Scoring;
            public Field SchemeID;
            public Field RelatedUserID;
            public Field Memo;

            public T_UserScoringDetails()
            {
                TableName = "T_UserScoringDetails";

                ID = new Field(this, "ID", "ID", SqlDbType.BigInt, true);
                SiteID = new Field(this, "SiteID", "SiteID", SqlDbType.BigInt, false);
                UserID = new Field(this, "UserID", "UserID", SqlDbType.BigInt, false);
                DateTime = new Field(this, "DateTime", "DateTime", SqlDbType.DateTime, false);
                OperatorType = new Field(this, "OperatorType", "OperatorType", SqlDbType.SmallInt, false);
                Scoring = new Field(this, "Scoring", "Scoring", SqlDbType.Float, false);
                SchemeID = new Field(this, "SchemeID", "SchemeID", SqlDbType.BigInt, false);
                RelatedUserID = new Field(this, "RelatedUserID", "RelatedUserID", SqlDbType.BigInt, false);
                Memo = new Field(this, "Memo", "Memo", SqlDbType.VarChar, false);
            }
        }

        public class T_UsersForInitiateFollowScheme : TableBase
        {
            public Field ID;
            public Field SiteID;
            public Field UserID;
            public Field DateTime;
            public Field PlayTypeID;
            public Field Description;
            public Field MaxNumberOf;

            public T_UsersForInitiateFollowScheme()
            {
                TableName = "T_UsersForInitiateFollowScheme";

                ID = new Field(this, "ID", "ID", SqlDbType.BigInt, true);
                SiteID = new Field(this, "SiteID", "SiteID", SqlDbType.BigInt, false);
                UserID = new Field(this, "UserID", "UserID", SqlDbType.BigInt, false);
                DateTime = new Field(this, "DateTime", "DateTime", SqlDbType.DateTime, false);
                PlayTypeID = new Field(this, "PlayTypeID", "PlayTypeID", SqlDbType.Int, false);
                Description = new Field(this, "Description", "Description", SqlDbType.VarChar, false);
                MaxNumberOf = new Field(this, "MaxNumberOf", "MaxNumberOf", SqlDbType.Int, false);
            }
        }

        public class T_UserToCpsUId : TableBase
        {
            public Field ID;
            public Field Uid;
            public Field CpsID;
            public Field PID;

            public T_UserToCpsUId()
            {
                TableName = "T_UserToCpsUId";

                ID = new Field(this, "ID", "ID", SqlDbType.Int, false);
                Uid = new Field(this, "Uid", "Uid", SqlDbType.Int, false);
                CpsID = new Field(this, "CpsID", "CpsID", SqlDbType.Int, false);
                PID = new Field(this, "PID", "PID", SqlDbType.Int, false);
            }
        }

        public class T_WinTypes : TableBase
        {
            public Field ID;
            public Field LotteryID;
            public Field Name;
            public Field Order;
            public Field DefaultMoney;
            public Field DefaultMoneyNoWithTax;

            public T_WinTypes()
            {
                TableName = "T_WinTypes";

                ID = new Field(this, "ID", "ID", SqlDbType.Int, false);
                LotteryID = new Field(this, "LotteryID", "LotteryID", SqlDbType.Int, false);
                Name = new Field(this, "Name", "Name", SqlDbType.VarChar, false);
                Order = new Field(this, "Order", "Order", SqlDbType.Int, false);
                DefaultMoney = new Field(this, "DefaultMoney", "DefaultMoney", SqlDbType.Money, false);
                DefaultMoneyNoWithTax = new Field(this, "DefaultMoneyNoWithTax", "DefaultMoneyNoWithTax", SqlDbType.Money, false);
            }
        }
    }

    public class Views
    {
        public class ViewBase
        {
            public string ViewName = "";

            public DataTable Open(string ConnectionString, string FieldList, string Condition, string Order)
            {
                FieldList = FieldList.Trim();
                Condition = Condition.Trim();
                Order = Order.Trim();

                return MSSQL.Select(ConnectionString, "select " + (FieldList == "" ? "*" : FieldList) + " from [" + ViewName + "]" + (Condition == "" ? "" : " where " + Condition) + (Order == "" ? "" : " order by " + Order));
            }

            public long GetCount(string ConnectionString, string Condition)
            {
                Condition = Condition.Trim();

                object Result = MSSQL.ExecuteScalar(ConnectionString, "select count(*) from [" + ViewName + "]" + (Condition == "" ? "" : " where " + Condition));

                if (Result == null)
                {
                    return 0;
                }

                return long.Parse(Result.ToString());
            }
        }

        public class V_Advertisements : ViewBase
        {
            public V_Advertisements()
            {
                ViewName = "V_Advertisements";
            }
        }

        public class V_BuyDetails : ViewBase
        {
            public V_BuyDetails()
            {
                ViewName = "V_BuyDetails";
            }
        }

        public class V_BuyDetailsNonce : ViewBase
        {
            public V_BuyDetailsNonce()
            {
                ViewName = "V_BuyDetailsNonce";
            }
        }

        public class V_BuyDetailsToCenter : ViewBase
        {
            public V_BuyDetailsToCenter()
            {
                ViewName = "V_BuyDetailsToCenter";
            }
        }

        public class V_BuyDetailsWithQuashed : ViewBase
        {
            public V_BuyDetailsWithQuashed()
            {
                ViewName = "V_BuyDetailsWithQuashed";
            }
        }

        public class V_BuyDetailsWithQuashedAll : ViewBase
        {
            public V_BuyDetailsWithQuashedAll()
            {
                ViewName = "V_BuyDetailsWithQuashedAll";
            }
        }

        public class V_CardPasswordDetails : ViewBase
        {
            public V_CardPasswordDetails()
            {
                ViewName = "V_CardPasswordDetails";
            }
        }

        public class V_Celebs : ViewBase
        {
            public V_Celebs()
            {
                ViewName = "V_Celebs";
            }
        }

        public class V_ChaseTaskDetails : ViewBase
        {
            public V_ChaseTaskDetails()
            {
                ViewName = "V_ChaseTaskDetails";
            }
        }

        public class V_ChaseTasks : ViewBase
        {
            public V_ChaseTasks()
            {
                ViewName = "V_ChaseTasks";
            }
        }

        public class V_ChaseTasksTotal : ViewBase
        {
            public V_ChaseTasksTotal()
            {
                ViewName = "V_ChaseTasksTotal";
            }
        }

        public class V_Citys : ViewBase
        {
            public V_Citys()
            {
                ViewName = "V_Citys";
            }
        }

        public class V_Cps : ViewBase
        {
            public V_Cps()
            {
                ViewName = "V_Cps";
            }
        }

        public class V_CpsTrys : ViewBase
        {
            public V_CpsTrys()
            {
                ViewName = "V_CpsTrys";
            }
        }

        public class V_CpsWithTransactionMoney : ViewBase
        {
            public V_CpsWithTransactionMoney()
            {
                ViewName = "V_CpsWithTransactionMoney";
            }
        }

        public class V_CustomFollowSchemes : ViewBase
        {
            public V_CustomFollowSchemes()
            {
                ViewName = "V_CustomFollowSchemes";
            }
        }

        public class V_ElectronTicketAgentSchemes : ViewBase
        {
            public V_ElectronTicketAgentSchemes()
            {
                ViewName = "V_ElectronTicketAgentSchemes";
            }
        }

        public class V_ElectronTicketAgentSchemesElectronTickets : ViewBase
        {
            public V_ElectronTicketAgentSchemesElectronTickets()
            {
                ViewName = "V_ElectronTicketAgentSchemesElectronTickets";
            }
        }

        public class V_ElectronTicketAgentSchemesSendToCenter : ViewBase
        {
            public V_ElectronTicketAgentSchemesSendToCenter()
            {
                ViewName = "V_ElectronTicketAgentSchemesSendToCenter";
            }
        }

        public class V_Experts : ViewBase
        {
            public V_Experts()
            {
                ViewName = "V_Experts";
            }
        }

        public class V_ExpertsCommends : ViewBase
        {
            public V_ExpertsCommends()
            {
                ViewName = "V_ExpertsCommends";
            }
        }

        public class V_ExpertsPredictNews : ViewBase
        {
            public V_ExpertsPredictNews()
            {
                ViewName = "V_ExpertsPredictNews";
            }
        }

        public class V_ExpertsTrys : ViewBase
        {
            public V_ExpertsTrys()
            {
                ViewName = "V_ExpertsTrys";
            }
        }

        public class V_ExpertsWinCommends : ViewBase
        {
            public V_ExpertsWinCommends()
            {
                ViewName = "V_ExpertsWinCommends";
            }
        }

        public class V_FullSchemesCount : ViewBase
        {
            public V_FullSchemesCount()
            {
                ViewName = "V_FullSchemesCount";
            }
        }

        public class V_GetDate : ViewBase
        {
            public V_GetDate()
            {
                ViewName = "V_GetDate";
            }
        }

        public class V_IsuseForZCDC : ViewBase
        {
            public V_IsuseForZCDC()
            {
                ViewName = "V_IsuseForZCDC";
            }
        }

        public class V_Isuses : ViewBase
        {
            public V_Isuses()
            {
                ViewName = "V_Isuses";
            }
        }

        public class v_Isuses_JXSSC : ViewBase
        {
            public v_Isuses_JXSSC()
            {
                ViewName = "v_Isuses_JXSSC";
            }
        }

        public class v_Isuses_SYYDJ : ViewBase
        {
            public v_Isuses_SYYDJ()
            {
                ViewName = "v_Isuses_SYYDJ";
            }
        }

        public class V_News : ViewBase
        {
            public V_News()
            {
                ViewName = "V_News";
            }
        }

        public class V_NPIsuses : ViewBase
        {
            public V_NPIsuses()
            {
                ViewName = "V_NPIsuses";
            }
        }

        public class V_PlayTypes : ViewBase
        {
            public V_PlayTypes()
            {
                ViewName = "V_PlayTypes";
            }
        }

        public class V_Questions : ViewBase
        {
            public V_Questions()
            {
                ViewName = "V_Questions";
            }
        }

        public class V_SchemeChatContents : ViewBase
        {
            public V_SchemeChatContents()
            {
                ViewName = "V_SchemeChatContents";
            }
        }

        public class V_SchemeCount : ViewBase
        {
            public V_SchemeCount()
            {
                ViewName = "V_SchemeCount";
            }
        }

        public class V_SchemeForZCDC : ViewBase
        {
            public V_SchemeForZCDC()
            {
                ViewName = "V_SchemeForZCDC";
            }
        }

        public class V_Schemes : ViewBase
        {
            public V_Schemes()
            {
                ViewName = "V_Schemes";
            }
        }

        public class V_SchemeSchedules : ViewBase
        {
            public V_SchemeSchedules()
            {
                ViewName = "V_SchemeSchedules";
            }
        }

        public class V_SchemeSchedulesWithQuashed : ViewBase
        {
            public V_SchemeSchedulesWithQuashed()
            {
                ViewName = "V_SchemeSchedulesWithQuashed";
            }
        }

        public class V_SchemesSendToCenter : ViewBase
        {
            public V_SchemesSendToCenter()
            {
                ViewName = "V_SchemesSendToCenter";
            }
        }

        public class V_ScoreCommodities : ViewBase
        {
            public V_ScoreCommodities()
            {
                ViewName = "V_ScoreCommodities";
            }
        }

        public class V_SiteSendNotificationTypes : ViewBase
        {
            public V_SiteSendNotificationTypes()
            {
                ViewName = "V_SiteSendNotificationTypes";
            }
        }

        public class V_StationSMS : ViewBase
        {
            public V_StationSMS()
            {
                ViewName = "V_StationSMS";
            }
        }

        public class V_SurrogateTrys : ViewBase
        {
            public V_SurrogateTrys()
            {
                ViewName = "V_SurrogateTrys";
            }
        }

        public class V_SystemLog : ViewBase
        {
            public V_SystemLog()
            {
                ViewName = "V_SystemLog";
            }
        }

        public class V_UserAcceptNotificationTypes : ViewBase
        {
            public V_UserAcceptNotificationTypes()
            {
                ViewName = "V_UserAcceptNotificationTypes";
            }
        }

        public class V_UserActions : ViewBase
        {
            public V_UserActions()
            {
                ViewName = "V_UserActions";
            }
        }

        public class V_UserDetails : ViewBase
        {
            public V_UserDetails()
            {
                ViewName = "V_UserDetails";
            }
        }

        public class V_UserDetailsWithSchemes : ViewBase
        {
            public V_UserDetailsWithSchemes()
            {
                ViewName = "V_UserDetailsWithSchemes";
            }
        }

        public class V_UserDistills : ViewBase
        {
            public V_UserDistills()
            {
                ViewName = "V_UserDistills";
            }
        }

        public class V_UserForInitiateFollowSchemeTrys : ViewBase
        {
            public V_UserForInitiateFollowSchemeTrys()
            {
                ViewName = "V_UserForInitiateFollowSchemeTrys";
            }
        }

        public class V_UserInGroups : ViewBase
        {
            public V_UserInGroups()
            {
                ViewName = "V_UserInGroups";
            }
        }

        public class V_UserInSchemeChatRooms : ViewBase
        {
            public V_UserInSchemeChatRooms()
            {
                ViewName = "V_UserInSchemeChatRooms";
            }
        }

        public class V_UserPayDetails : ViewBase
        {
            public V_UserPayDetails()
            {
                ViewName = "V_UserPayDetails";
            }
        }

        public class V_Users : ViewBase
        {
            public V_Users()
            {
                ViewName = "V_Users";
            }
        }

        public class V_UserScoringDetails : ViewBase
        {
            public V_UserScoringDetails()
            {
                ViewName = "V_UserScoringDetails";
            }
        }

        public class V_UsersForInitiateFollowScheme : ViewBase
        {
            public V_UsersForInitiateFollowScheme()
            {
                ViewName = "V_UsersForInitiateFollowScheme";
            }
        }

        public class V_UsersWithSumWinMoney : ViewBase
        {
            public V_UsersWithSumWinMoney()
            {
                ViewName = "V_UsersWithSumWinMoney";
            }
        }

        public class V_UsersWithSumWinMoneyThisWeek : ViewBase
        {
            public V_UsersWithSumWinMoneyThisWeek()
            {
                ViewName = "V_UsersWithSumWinMoneyThisWeek";
            }
        }

        public class V_UsersWithSumWinMoneyToday : ViewBase
        {
            public V_UsersWithSumWinMoneyToday()
            {
                ViewName = "V_UsersWithSumWinMoneyToday";
            }
        }
    }

    public class Functions
    {
        public static int F_AccumulateMember(string ConnectionString, long SiteID, long UserID, DateTime StartTime, DateTime EndTime, int type)
        {
            object Result = MSSQL.ExecuteFunction(ConnectionString, "F_AccumulateMember",
                new MSSQL.Parameter("SiteID", SqlDbType.BigInt, 0, ParameterDirection.Input, SiteID),
                new MSSQL.Parameter("UserID", SqlDbType.BigInt, 0, ParameterDirection.Input, UserID),
                new MSSQL.Parameter("StartTime", SqlDbType.DateTime, 0, ParameterDirection.Input, StartTime),
                new MSSQL.Parameter("EndTime", SqlDbType.DateTime, 0, ParameterDirection.Input, EndTime),
                new MSSQL.Parameter("type", SqlDbType.Int, 0, ParameterDirection.Input, type)
                );

            return System.Convert.ToInt32(Result);
        }

        public static double F_CalculationFollowSchemesMoney(string ConnectionString, double RemainingMoney, int RemainingShare, double FollowSchemesMoney)
        {
            object Result = MSSQL.ExecuteFunction(ConnectionString, "F_CalculationFollowSchemesMoney",
                new MSSQL.Parameter("RemainingMoney", SqlDbType.Money, 0, ParameterDirection.Input, RemainingMoney),
                new MSSQL.Parameter("RemainingShare", SqlDbType.Int, 0, ParameterDirection.Input, RemainingShare),
                new MSSQL.Parameter("FollowSchemesMoney", SqlDbType.Money, 0, ParameterDirection.Input, FollowSchemesMoney)
                );

            return System.Convert.ToDouble(Result);
        }

        public static bool F_ComparisonLotteryList(string ConnectionString, string ParentLotteryList, string SubLotteryList)
        {
            object Result = MSSQL.ExecuteFunction(ConnectionString, "F_ComparisonLotteryList",
                new MSSQL.Parameter("ParentLotteryList", SqlDbType.VarChar, 0, ParameterDirection.Input, ParentLotteryList),
                new MSSQL.Parameter("SubLotteryList", SqlDbType.VarChar, 0, ParameterDirection.Input, SubLotteryList)
                );

            return System.Convert.ToBoolean(Result);
        }

        public static int F_CpsMemberAccumulateBuyerMember(string ConnectionString, long SiteID, long UserID)
        {
            object Result = MSSQL.ExecuteFunction(ConnectionString, "F_CpsMemberAccumulateBuyerMember",
                new MSSQL.Parameter("SiteID", SqlDbType.BigInt, 0, ParameterDirection.Input, SiteID),
                new MSSQL.Parameter("UserID", SqlDbType.BigInt, 0, ParameterDirection.Input, UserID)
                );

            return System.Convert.ToInt32(Result);
        }

        public static int F_CpsMemberAccumulateWebSite(string ConnectionString, long SiteID, long UserID)
        {
            object Result = MSSQL.ExecuteFunction(ConnectionString, "F_CpsMemberAccumulateWebSite",
                new MSSQL.Parameter("SiteID", SqlDbType.BigInt, 0, ParameterDirection.Input, SiteID),
                new MSSQL.Parameter("UserID", SqlDbType.BigInt, 0, ParameterDirection.Input, UserID)
                );

            return System.Convert.ToInt32(Result);
        }

        public static int F_CurrentDateRegMember(string ConnectionString, long SiteID, long UserID, DateTime StartTime, DateTime EndTime, int type)
        {
            object Result = MSSQL.ExecuteFunction(ConnectionString, "F_CurrentDateRegMember",
                new MSSQL.Parameter("SiteID", SqlDbType.BigInt, 0, ParameterDirection.Input, SiteID),
                new MSSQL.Parameter("UserID", SqlDbType.BigInt, 0, ParameterDirection.Input, UserID),
                new MSSQL.Parameter("StartTime", SqlDbType.DateTime, 0, ParameterDirection.Input, StartTime),
                new MSSQL.Parameter("EndTime", SqlDbType.DateTime, 0, ParameterDirection.Input, EndTime),
                new MSSQL.Parameter("type", SqlDbType.Int, 0, ParameterDirection.Input, type)
                );

            return System.Convert.ToInt32(Result);
        }

        public static double F_CurrentDateRegMemberPayMoney(string ConnectionString, long SiteID, long UserID, DateTime CurrentDate, int type)
        {
            object Result = MSSQL.ExecuteFunction(ConnectionString, "F_CurrentDateRegMemberPayMoney",
                new MSSQL.Parameter("SiteID", SqlDbType.BigInt, 0, ParameterDirection.Input, SiteID),
                new MSSQL.Parameter("UserID", SqlDbType.BigInt, 0, ParameterDirection.Input, UserID),
                new MSSQL.Parameter("CurrentDate", SqlDbType.DateTime, 0, ParameterDirection.Input, CurrentDate),
                new MSSQL.Parameter("type", SqlDbType.Int, 0, ParameterDirection.Input, type)
                );

            return System.Convert.ToDouble(Result);
        }

        public static double F_CurrentDateRegMemberPayMoneyBonusScale(string ConnectionString, long SiteID, long UserID, DateTime CurrentDate, int type)
        {
            object Result = MSSQL.ExecuteFunction(ConnectionString, "F_CurrentDateRegMemberPayMoneyBonusScale",
                new MSSQL.Parameter("SiteID", SqlDbType.BigInt, 0, ParameterDirection.Input, SiteID),
                new MSSQL.Parameter("UserID", SqlDbType.BigInt, 0, ParameterDirection.Input, UserID),
                new MSSQL.Parameter("CurrentDate", SqlDbType.DateTime, 0, ParameterDirection.Input, CurrentDate),
                new MSSQL.Parameter("type", SqlDbType.Int, 0, ParameterDirection.Input, type)
                );

            return System.Convert.ToDouble(Result);
        }

        public static double F_CurrentDateRegMemberPayMoneyBonusScale_all(string ConnectionString, long SiteID, long UserID, DateTime CurrentDate)
        {
            object Result = MSSQL.ExecuteFunction(ConnectionString, "F_CurrentDateRegMemberPayMoneyBonusScale_all",
                new MSSQL.Parameter("SiteID", SqlDbType.BigInt, 0, ParameterDirection.Input, SiteID),
                new MSSQL.Parameter("UserID", SqlDbType.BigInt, 0, ParameterDirection.Input, UserID),
                new MSSQL.Parameter("CurrentDate", SqlDbType.DateTime, 0, ParameterDirection.Input, CurrentDate)
                );

            return System.Convert.ToDouble(Result);
        }

        public static double F_CurrentDateRegMemberPayMoneyBonusScale_today(string ConnectionString, long SiteID, long UserID, DateTime CurrentDate)
        {
            object Result = MSSQL.ExecuteFunction(ConnectionString, "F_CurrentDateRegMemberPayMoneyBonusScale_today",
                new MSSQL.Parameter("SiteID", SqlDbType.BigInt, 0, ParameterDirection.Input, SiteID),
                new MSSQL.Parameter("UserID", SqlDbType.BigInt, 0, ParameterDirection.Input, UserID),
                new MSSQL.Parameter("CurrentDate", SqlDbType.DateTime, 0, ParameterDirection.Input, CurrentDate)
                );

            return System.Convert.ToDouble(Result);
        }

        public static double F_CurrentDateRegMemberPayMoneyBonusScaleSite(string ConnectionString, long SiteID, long UserID, DateTime CurrentDate)
        {
            object Result = MSSQL.ExecuteFunction(ConnectionString, "F_CurrentDateRegMemberPayMoneyBonusScaleSite",
                new MSSQL.Parameter("SiteID", SqlDbType.BigInt, 0, ParameterDirection.Input, SiteID),
                new MSSQL.Parameter("UserID", SqlDbType.BigInt, 0, ParameterDirection.Input, UserID),
                new MSSQL.Parameter("CurrentDate", SqlDbType.DateTime, 0, ParameterDirection.Input, CurrentDate)
                );

            return System.Convert.ToDouble(Result);
        }

        public static double F_CurrentDateRegMemberPayMoneyBonusScaleSite_all(string ConnectionString, long SiteID, long UserID, DateTime CurrentDate)
        {
            object Result = MSSQL.ExecuteFunction(ConnectionString, "F_CurrentDateRegMemberPayMoneyBonusScaleSite_all",
                new MSSQL.Parameter("SiteID", SqlDbType.BigInt, 0, ParameterDirection.Input, SiteID),
                new MSSQL.Parameter("UserID", SqlDbType.BigInt, 0, ParameterDirection.Input, UserID),
                new MSSQL.Parameter("CurrentDate", SqlDbType.DateTime, 0, ParameterDirection.Input, CurrentDate)
                );

            return System.Convert.ToDouble(Result);
        }

        public static double F_CurrentDateRegMemberPayMoneyBonusScaleSite_today(string ConnectionString, long SiteID, long UserID, DateTime CurrentDate)
        {
            object Result = MSSQL.ExecuteFunction(ConnectionString, "F_CurrentDateRegMemberPayMoneyBonusScaleSite_today",
                new MSSQL.Parameter("SiteID", SqlDbType.BigInt, 0, ParameterDirection.Input, SiteID),
                new MSSQL.Parameter("UserID", SqlDbType.BigInt, 0, ParameterDirection.Input, UserID),
                new MSSQL.Parameter("CurrentDate", SqlDbType.DateTime, 0, ParameterDirection.Input, CurrentDate)
                );

            return System.Convert.ToDouble(Result);
        }

        public static int F_CurrentDateRegPayMember(string ConnectionString, long SiteID, long UserID, DateTime CurrentDate, int type)
        {
            object Result = MSSQL.ExecuteFunction(ConnectionString, "F_CurrentDateRegPayMember",
                new MSSQL.Parameter("SiteID", SqlDbType.BigInt, 0, ParameterDirection.Input, SiteID),
                new MSSQL.Parameter("UserID", SqlDbType.BigInt, 0, ParameterDirection.Input, UserID),
                new MSSQL.Parameter("CurrentDate", SqlDbType.DateTime, 0, ParameterDirection.Input, CurrentDate),
                new MSSQL.Parameter("type", SqlDbType.Int, 0, ParameterDirection.Input, type)
                );

            return System.Convert.ToInt32(Result);
        }

        public static double F_CurrentMonthMemberRecWebSitePayMoney(string ConnectionString, long SiteID, long UserID, DateTime CurrentDate, int type)
        {
            object Result = MSSQL.ExecuteFunction(ConnectionString, "F_CurrentMonthMemberRecWebSitePayMoney",
                new MSSQL.Parameter("SiteID", SqlDbType.BigInt, 0, ParameterDirection.Input, SiteID),
                new MSSQL.Parameter("UserID", SqlDbType.BigInt, 0, ParameterDirection.Input, UserID),
                new MSSQL.Parameter("CurrentDate", SqlDbType.DateTime, 0, ParameterDirection.Input, CurrentDate),
                new MSSQL.Parameter("type", SqlDbType.Int, 0, ParameterDirection.Input, type)
                );

            return System.Convert.ToDouble(Result);
        }

        public static string F_DateTimeToYYMMDD(string ConnectionString, DateTime Dt)
        {
            object Result = MSSQL.ExecuteFunction(ConnectionString, "F_DateTimeToYYMMDD",
                new MSSQL.Parameter("Dt", SqlDbType.DateTime, 0, ParameterDirection.Input, Dt)
                );

            return System.Convert.ToString(Result);
        }

        public static string F_DateTimeToYYMMDDHHMMSS(string ConnectionString, DateTime Dt)
        {
            object Result = MSSQL.ExecuteFunction(ConnectionString, "F_DateTimeToYYMMDDHHMMSS",
                new MSSQL.Parameter("Dt", SqlDbType.DateTime, 0, ParameterDirection.Input, Dt)
                );

            return System.Convert.ToString(Result);
        }

        public static string F_GetBankTypeName(string ConnectionString, short BankTypeID)
        {
            object Result = MSSQL.ExecuteFunction(ConnectionString, "F_GetBankTypeName",
                new MSSQL.Parameter("BankTypeID", SqlDbType.SmallInt, 0, ParameterDirection.Input, BankTypeID)
                );

            return System.Convert.ToString(Result);
        }

        public static short F_GetDetailsOperatorType(string ConnectionString, string OperatorType)
        {
            object Result = MSSQL.ExecuteFunction(ConnectionString, "F_GetDetailsOperatorType",
                new MSSQL.Parameter("OperatorType", SqlDbType.VarChar, 0, ParameterDirection.Input, OperatorType)
                );

            return System.Convert.ToInt16(Result);
        }

        public static string F_GetExpertsLotteryList(string ConnectionString, long SiteID, long UserID)
        {
            object Result = MSSQL.ExecuteFunction(ConnectionString, "F_GetExpertsLotteryList",
                new MSSQL.Parameter("SiteID", SqlDbType.BigInt, 0, ParameterDirection.Input, SiteID),
                new MSSQL.Parameter("UserID", SqlDbType.BigInt, 0, ParameterDirection.Input, UserID)
                );

            return System.Convert.ToString(Result);
        }

        public static bool F_GetIsAdministrator(string ConnectionString, long SiteID, long UserID)
        {
            object Result = MSSQL.ExecuteFunction(ConnectionString, "F_GetIsAdministrator",
                new MSSQL.Parameter("SiteID", SqlDbType.BigInt, 0, ParameterDirection.Input, SiteID),
                new MSSQL.Parameter("UserID", SqlDbType.BigInt, 0, ParameterDirection.Input, UserID)
                );

            return System.Convert.ToBoolean(Result);
        }

        public static bool F_GetIsSendNotification(string ConnectionString, long SiteID, short Manner, string NotificationCode, long UserID)
        {
            object Result = MSSQL.ExecuteFunction(ConnectionString, "F_GetIsSendNotification",
                new MSSQL.Parameter("SiteID", SqlDbType.BigInt, 0, ParameterDirection.Input, SiteID),
                new MSSQL.Parameter("Manner", SqlDbType.SmallInt, 0, ParameterDirection.Input, Manner),
                new MSSQL.Parameter("NotificationCode", SqlDbType.VarChar, 0, ParameterDirection.Input, NotificationCode),
                new MSSQL.Parameter("UserID", SqlDbType.BigInt, 0, ParameterDirection.Input, UserID)
                );

            return System.Convert.ToBoolean(Result);
        }

        public static DateTime F_GetIsuseChaseExecuteTime(string ConnectionString, long IsuseID)
        {
            object Result = MSSQL.ExecuteFunction(ConnectionString, "F_GetIsuseChaseExecuteTime",
                new MSSQL.Parameter("IsuseID", SqlDbType.BigInt, 0, ParameterDirection.Input, IsuseID)
                );

            return System.Convert.ToDateTime(Result);
        }

        public static string F_GetIsuseCount(string ConnectionString, int LotteryID)
        {
            object Result = MSSQL.ExecuteFunction(ConnectionString, "F_GetIsuseCount",
                new MSSQL.Parameter("LotteryID", SqlDbType.Int, 0, ParameterDirection.Input, LotteryID)
                );

            return System.Convert.ToString(Result);
        }

        public static DateTime F_GetIsuseEndTime(string ConnectionString, long IsuseID)
        {
            object Result = MSSQL.ExecuteFunction(ConnectionString, "F_GetIsuseEndTime",
                new MSSQL.Parameter("IsuseID", SqlDbType.BigInt, 0, ParameterDirection.Input, IsuseID)
                );

            return System.Convert.ToDateTime(Result);
        }

        public static DateTime F_GetIsuseStartTime(string ConnectionString, long IsuseID)
        {
            object Result = MSSQL.ExecuteFunction(ConnectionString, "F_GetIsuseStartTime",
                new MSSQL.Parameter("IsuseID", SqlDbType.BigInt, 0, ParameterDirection.Input, IsuseID)
                );

            return System.Convert.ToDateTime(Result);
        }

        public static DateTime F_GetIsuseSystemEndTime(string ConnectionString, long IsuseID, int PlayTypeID)
        {
            object Result = MSSQL.ExecuteFunction(ConnectionString, "F_GetIsuseSystemEndTime",
                new MSSQL.Parameter("IsuseID", SqlDbType.BigInt, 0, ParameterDirection.Input, IsuseID),
                new MSSQL.Parameter("PlayTypeID", SqlDbType.Int, 0, ParameterDirection.Input, PlayTypeID)
                );

            return System.Convert.ToDateTime(Result);
        }

        public static DataTable F_GetLotteryCanChaseIsuses(string ConnectionString, int LotteryID, int PlayType)
        {
            return MSSQL.Select(ConnectionString, "select * from F_GetLotteryCanChaseIsuses(@LotteryID, @PlayType)",
                new MSSQL.Parameter("LotteryID", SqlDbType.Int, 0, ParameterDirection.Input, LotteryID),
                new MSSQL.Parameter("PlayType", SqlDbType.Int, 0, ParameterDirection.Input, PlayType)
                );
        }

        public static string F_GetLotteryCode(string ConnectionString, int LotteryID)
        {
            object Result = MSSQL.ExecuteFunction(ConnectionString, "F_GetLotteryCode",
                new MSSQL.Parameter("LotteryID", SqlDbType.Int, 0, ParameterDirection.Input, LotteryID)
                );

            return System.Convert.ToString(Result);
        }

        public static string F_GetLotteryIntervalType(string ConnectionString, int LotteryID)
        {
            object Result = MSSQL.ExecuteFunction(ConnectionString, "F_GetLotteryIntervalType",
                new MSSQL.Parameter("LotteryID", SqlDbType.Int, 0, ParameterDirection.Input, LotteryID)
                );

            return System.Convert.ToString(Result);
        }

        public static bool F_GetLotteryIsUsed(string ConnectionString, long SiteID, int LotteryID)
        {
            object Result = MSSQL.ExecuteFunction(ConnectionString, "F_GetLotteryIsUsed",
                new MSSQL.Parameter("SiteID", SqlDbType.BigInt, 0, ParameterDirection.Input, SiteID),
                new MSSQL.Parameter("LotteryID", SqlDbType.Int, 0, ParameterDirection.Input, LotteryID)
                );

            return System.Convert.ToBoolean(Result);
        }

        public static string F_GetLotteryName(string ConnectionString, int LotteryID)
        {
            object Result = MSSQL.ExecuteFunction(ConnectionString, "F_GetLotteryName",
                new MSSQL.Parameter("LotteryID", SqlDbType.Int, 0, ParameterDirection.Input, LotteryID)
                );

            return System.Convert.ToString(Result);
        }

        public static short F_GetLotteryPrintOutType(string ConnectionString, int LotteryID)
        {
            object Result = MSSQL.ExecuteFunction(ConnectionString, "F_GetLotteryPrintOutType",
                new MSSQL.Parameter("LotteryID", SqlDbType.Int, 0, ParameterDirection.Input, LotteryID)
                );

            return System.Convert.ToInt16(Result);
        }

        public static string F_GetLotteryType2Name(string ConnectionString, short Type2)
        {
            object Result = MSSQL.ExecuteFunction(ConnectionString, "F_GetLotteryType2Name",
                new MSSQL.Parameter("Type2", SqlDbType.SmallInt, 0, ParameterDirection.Input, Type2)
                );

            return System.Convert.ToString(Result);
        }

        public static string F_GetLotteryWinNumberExemple(string ConnectionString, int LotteryID)
        {
            object Result = MSSQL.ExecuteFunction(ConnectionString, "F_GetLotteryWinNumberExemple",
                new MSSQL.Parameter("LotteryID", SqlDbType.Int, 0, ParameterDirection.Input, LotteryID)
                );

            return System.Convert.ToString(Result);
        }

        public static DataTable F_GetManagers(string ConnectionString, long SiteID)
        {
            return MSSQL.Select(ConnectionString, "select * from F_GetManagers(@SiteID)",
                new MSSQL.Parameter("SiteID", SqlDbType.BigInt, 0, ParameterDirection.Input, SiteID)
                );
        }

        public static long F_GetMasterSiteID(string ConnectionString)
        {
            object Result = MSSQL.ExecuteFunction(ConnectionString, "F_GetMasterSiteID");

            return System.Convert.ToInt64(Result);
        }

        public static int F_GetMaxMultiple(string ConnectionString, long IsuseID, int PlayTypeID)
        {
            object Result = MSSQL.ExecuteFunction(ConnectionString, "F_GetMaxMultiple",
                new MSSQL.Parameter("IsuseID", SqlDbType.BigInt, 0, ParameterDirection.Input, IsuseID),
                new MSSQL.Parameter("PlayTypeID", SqlDbType.Int, 0, ParameterDirection.Input, PlayTypeID)
                );

            return System.Convert.ToInt32(Result);
        }

        public static string F_GetOptions(string ConnectionString, string Key)
        {
            object Result = MSSQL.ExecuteFunction(ConnectionString, "F_GetOptions",
                new MSSQL.Parameter("Key", SqlDbType.VarChar, 0, ParameterDirection.Input, Key)
                );

            return System.Convert.ToString(Result);
        }

        public static string F_GetPlaceFromIPAddress(string ConnectionString, string IPAddress)
        {
            object Result = MSSQL.ExecuteFunction(ConnectionString, "F_GetPlaceFromIPAddress",
                new MSSQL.Parameter("IPAddress", SqlDbType.VarChar, 0, ParameterDirection.Input, IPAddress)
                );

            return System.Convert.ToString(Result);
        }

        public static string F_GetPlayTypeName(string ConnectionString, int PlayTypeID)
        {
            object Result = MSSQL.ExecuteFunction(ConnectionString, "F_GetPlayTypeName",
                new MSSQL.Parameter("PlayTypeID", SqlDbType.Int, 0, ParameterDirection.Input, PlayTypeID)
                );

            return System.Convert.ToString(Result);
        }

        public static string F_GetProvinceCity(string ConnectionString, int CityID)
        {
            object Result = MSSQL.ExecuteFunction(ConnectionString, "F_GetProvinceCity",
                new MSSQL.Parameter("CityID", SqlDbType.Int, 0, ParameterDirection.Input, CityID)
                );

            return System.Convert.ToString(Result);
        }

        public static long F_GetSchemeInitiateUserID(string ConnectionString, long SiteID, long SchemeID)
        {
            object Result = MSSQL.ExecuteFunction(ConnectionString, "F_GetSchemeInitiateUserID",
                new MSSQL.Parameter("SiteID", SqlDbType.BigInt, 0, ParameterDirection.Input, SiteID),
                new MSSQL.Parameter("SchemeID", SqlDbType.BigInt, 0, ParameterDirection.Input, SchemeID)
                );

            return System.Convert.ToInt64(Result);
        }

        public static string F_GetSchemeOpenUsers(string ConnectionString, long SchemeID)
        {
            object Result = MSSQL.ExecuteFunction(ConnectionString, "F_GetSchemeOpenUsers",
                new MSSQL.Parameter("SchemeID", SqlDbType.BigInt, 0, ParameterDirection.Input, SchemeID)
                );

            return System.Convert.ToString(Result);
        }

        public static short F_GetScoringDetailsOperatorType(string ConnectionString, string OperatorType)
        {
            object Result = MSSQL.ExecuteFunction(ConnectionString, "F_GetScoringDetailsOperatorType",
                new MSSQL.Parameter("OperatorType", SqlDbType.VarChar, 0, ParameterDirection.Input, OperatorType)
                );

            return System.Convert.ToInt16(Result);
        }

        public static DataTable F_GetSiteAdministrator(string ConnectionString, long SiteID)
        {
            return MSSQL.Select(ConnectionString, "select * from F_GetSiteAdministrator(@SiteID)",
                new MSSQL.Parameter("SiteID", SqlDbType.BigInt, 0, ParameterDirection.Input, SiteID)
                );
        }

        public static DataTable F_GetSiteAdministrators(string ConnectionString, long SiteID)
        {
            return MSSQL.Select(ConnectionString, "select * from F_GetSiteAdministrators(@SiteID)",
                new MSSQL.Parameter("SiteID", SqlDbType.BigInt, 0, ParameterDirection.Input, SiteID)
                );
        }

        public static long F_GetSiteOwnerUserID(string ConnectionString, long SiteID)
        {
            object Result = MSSQL.ExecuteFunction(ConnectionString, "F_GetSiteOwnerUserID",
                new MSSQL.Parameter("SiteID", SqlDbType.BigInt, 0, ParameterDirection.Input, SiteID)
                );

            return System.Convert.ToInt64(Result);
        }

        public static long F_GetSiteParentID(string ConnectionString, long SiteID)
        {
            object Result = MSSQL.ExecuteFunction(ConnectionString, "F_GetSiteParentID",
                new MSSQL.Parameter("SiteID", SqlDbType.BigInt, 0, ParameterDirection.Input, SiteID)
                );

            return System.Convert.ToInt64(Result);
        }

        public static string F_GetSiteSendNotificationTypes(string ConnectionString, long SiteID, short Manner)
        {
            object Result = MSSQL.ExecuteFunction(ConnectionString, "F_GetSiteSendNotificationTypes",
                new MSSQL.Parameter("SiteID", SqlDbType.BigInt, 0, ParameterDirection.Input, SiteID),
                new MSSQL.Parameter("Manner", SqlDbType.SmallInt, 0, ParameterDirection.Input, Manner)
                );

            return System.Convert.ToString(Result);
        }

        public static string F_GetSiteUrls(string ConnectionString, long SiteID)
        {
            object Result = MSSQL.ExecuteFunction(ConnectionString, "F_GetSiteUrls",
                new MSSQL.Parameter("SiteID", SqlDbType.BigInt, 0, ParameterDirection.Input, SiteID)
                );

            return System.Convert.ToString(Result);
        }

        public static string F_GetUsedLotteryList(string ConnectionString, long SiteID)
        {
            object Result = MSSQL.ExecuteFunction(ConnectionString, "F_GetUsedLotteryList",
                new MSSQL.Parameter("SiteID", SqlDbType.BigInt, 0, ParameterDirection.Input, SiteID)
                );

            return System.Convert.ToString(Result);
        }

        public static string F_GetUsedLotteryListQuickBuy(string ConnectionString, long SiteID)
        {
            object Result = MSSQL.ExecuteFunction(ConnectionString, "F_GetUsedLotteryListQuickBuy",
                new MSSQL.Parameter("SiteID", SqlDbType.BigInt, 0, ParameterDirection.Input, SiteID)
                );

            return System.Convert.ToString(Result);
        }

        public static string F_GetUsedLotteryListRestrictions(string ConnectionString, long SiteID)
        {
            object Result = MSSQL.ExecuteFunction(ConnectionString, "F_GetUsedLotteryListRestrictions",
                new MSSQL.Parameter("SiteID", SqlDbType.BigInt, 0, ParameterDirection.Input, SiteID)
                );

            return System.Convert.ToString(Result);
        }

        public static string F_GetUsedLotteryNameList(string ConnectionString, long SiteID)
        {
            object Result = MSSQL.ExecuteFunction(ConnectionString, "F_GetUsedLotteryNameList",
                new MSSQL.Parameter("SiteID", SqlDbType.BigInt, 0, ParameterDirection.Input, SiteID)
                );

            return System.Convert.ToString(Result);
        }

        public static string F_GetUsedLotteryNameListQuickBuy(string ConnectionString, long SiteID)
        {
            object Result = MSSQL.ExecuteFunction(ConnectionString, "F_GetUsedLotteryNameListQuickBuy",
                new MSSQL.Parameter("SiteID", SqlDbType.BigInt, 0, ParameterDirection.Input, SiteID)
                );

            return System.Convert.ToString(Result);
        }

        public static string F_GetUsedLotteryNameListRestrictions(string ConnectionString, long SiteID)
        {
            object Result = MSSQL.ExecuteFunction(ConnectionString, "F_GetUsedLotteryNameListRestrictions",
                new MSSQL.Parameter("SiteID", SqlDbType.BigInt, 0, ParameterDirection.Input, SiteID)
                );

            return System.Convert.ToString(Result);
        }

        public static string F_GetUserAcceptNotificationTypes(string ConnectionString, long UserID, short Manner)
        {
            object Result = MSSQL.ExecuteFunction(ConnectionString, "F_GetUserAcceptNotificationTypes",
                new MSSQL.Parameter("UserID", SqlDbType.BigInt, 0, ParameterDirection.Input, UserID),
                new MSSQL.Parameter("Manner", SqlDbType.SmallInt, 0, ParameterDirection.Input, Manner)
                );

            return System.Convert.ToString(Result);
        }

        public static long F_GetUserCommenderID(string ConnectionString, long UserID)
        {
            object Result = MSSQL.ExecuteFunction(ConnectionString, "F_GetUserCommenderID",
                new MSSQL.Parameter("UserID", SqlDbType.BigInt, 0, ParameterDirection.Input, UserID)
                );

            return System.Convert.ToInt64(Result);
        }

        public static string F_GetUserCompetencesList(string ConnectionString, long UserID)
        {
            object Result = MSSQL.ExecuteFunction(ConnectionString, "F_GetUserCompetencesList",
                new MSSQL.Parameter("UserID", SqlDbType.BigInt, 0, ParameterDirection.Input, UserID)
                );

            return System.Convert.ToString(Result);
        }

        public static long F_GetUserIDByName(string ConnectionString, long SiteID, string Name)
        {
            object Result = MSSQL.ExecuteFunction(ConnectionString, "F_GetUserIDByName",
                new MSSQL.Parameter("SiteID", SqlDbType.BigInt, 0, ParameterDirection.Input, SiteID),
                new MSSQL.Parameter("Name", SqlDbType.VarChar, 0, ParameterDirection.Input, Name)
                );

            return System.Convert.ToInt64(Result);
        }

        public static string F_GetUserNameByID(string ConnectionString, long SiteID, long ID)
        {
            object Result = MSSQL.ExecuteFunction(ConnectionString, "F_GetUserNameByID",
                new MSSQL.Parameter("SiteID", SqlDbType.BigInt, 0, ParameterDirection.Input, SiteID),
                new MSSQL.Parameter("ID", SqlDbType.BigInt, 0, ParameterDirection.Input, ID)
                );

            return System.Convert.ToString(Result);
        }

        public static string F_GetUserOwnerSitesList(string ConnectionString, long UserID)
        {
            object Result = MSSQL.ExecuteFunction(ConnectionString, "F_GetUserOwnerSitesList",
                new MSSQL.Parameter("UserID", SqlDbType.BigInt, 0, ParameterDirection.Input, UserID)
                );

            return System.Convert.ToString(Result);
        }

        public static DataTable F_GetWinLotteryNumber(string ConnectionString, long SiteID, int LotteryID)
        {
            return MSSQL.Select(ConnectionString, "select * from F_GetWinLotteryNumber(@SiteID, @LotteryID)",
                new MSSQL.Parameter("SiteID", SqlDbType.BigInt, 0, ParameterDirection.Input, SiteID),
                new MSSQL.Parameter("LotteryID", SqlDbType.Int, 0, ParameterDirection.Input, LotteryID)
                );
        }

        public static long F_IPAddressToInt64(string ConnectionString, string IPAddress)
        {
            object Result = MSSQL.ExecuteFunction(ConnectionString, "F_IPAddressToInt64",
                new MSSQL.Parameter("IPAddress", SqlDbType.VarChar, 0, ParameterDirection.Input, IPAddress)
                );

            return System.Convert.ToInt64(Result);
        }

        public static bool F_IsDivisibility(string ConnectionString, double Dividend, double Divisor)
        {
            object Result = MSSQL.ExecuteFunction(ConnectionString, "F_IsDivisibility",
                new MSSQL.Parameter("Dividend", SqlDbType.Float, 0, ParameterDirection.Input, Dividend),
                new MSSQL.Parameter("Divisor", SqlDbType.Float, 0, ParameterDirection.Input, Divisor)
                );

            return System.Convert.ToBoolean(Result);
        }

        public static double F_MonthPayMoneyShopBonusScale(string ConnectionString, long SiteID, long UserID, DateTime StartTime, DateTime EndTime, int type)
        {
            object Result = MSSQL.ExecuteFunction(ConnectionString, "F_MonthPayMoneyShopBonusScale",
                new MSSQL.Parameter("SiteID", SqlDbType.BigInt, 0, ParameterDirection.Input, SiteID),
                new MSSQL.Parameter("UserID", SqlDbType.BigInt, 0, ParameterDirection.Input, UserID),
                new MSSQL.Parameter("StartTime", SqlDbType.DateTime, 0, ParameterDirection.Input, StartTime),
                new MSSQL.Parameter("EndTime", SqlDbType.DateTime, 0, ParameterDirection.Input, EndTime),
                new MSSQL.Parameter("type", SqlDbType.Int, 0, ParameterDirection.Input, type)
                );

            return System.Convert.ToDouble(Result);
        }

        public static double F_MonthShopPayMoney(string ConnectionString, long SiteID, long UserID, DateTime StartTime, DateTime EndTime, int type)
        {
            object Result = MSSQL.ExecuteFunction(ConnectionString, "F_MonthShopPayMoney",
                new MSSQL.Parameter("SiteID", SqlDbType.BigInt, 0, ParameterDirection.Input, SiteID),
                new MSSQL.Parameter("UserID", SqlDbType.BigInt, 0, ParameterDirection.Input, UserID),
                new MSSQL.Parameter("StartTime", SqlDbType.DateTime, 0, ParameterDirection.Input, StartTime),
                new MSSQL.Parameter("EndTime", SqlDbType.DateTime, 0, ParameterDirection.Input, EndTime),
                new MSSQL.Parameter("type", SqlDbType.Int, 0, ParameterDirection.Input, type)
                );

            return System.Convert.ToDouble(Result);
        }

        public static string F_PadLeft(string ConnectionString, string str, string FillChar, int Len)
        {
            object Result = MSSQL.ExecuteFunction(ConnectionString, "F_PadLeft",
                new MSSQL.Parameter("str", SqlDbType.VarChar, 0, ParameterDirection.Input, str),
                new MSSQL.Parameter("FillChar", SqlDbType.Char, 0, ParameterDirection.Input, FillChar),
                new MSSQL.Parameter("Len", SqlDbType.Int, 0, ParameterDirection.Input, Len)
                );

            return System.Convert.ToString(Result);
        }

        public static DataTable F_SplitString(string ConnectionString, string SplitString, string Separator)
        {
            return MSSQL.Select(ConnectionString, "select * from F_SplitString(@SplitString, @Separator)",
                new MSSQL.Parameter("SplitString", SqlDbType.VarChar, 0, ParameterDirection.Input, SplitString),
                new MSSQL.Parameter("Separator", SqlDbType.VarChar, 0, ParameterDirection.Input, Separator)
                );
        }

        public static double F_UnionSitePayMoney(string ConnectionString, long SiteID, long UserID, DateTime StartTime, DateTime EndTime)
        {
            object Result = MSSQL.ExecuteFunction(ConnectionString, "F_UnionSitePayMoney",
                new MSSQL.Parameter("SiteID", SqlDbType.BigInt, 0, ParameterDirection.Input, SiteID),
                new MSSQL.Parameter("UserID", SqlDbType.BigInt, 0, ParameterDirection.Input, UserID),
                new MSSQL.Parameter("StartTime", SqlDbType.DateTime, 0, ParameterDirection.Input, StartTime),
                new MSSQL.Parameter("EndTime", SqlDbType.DateTime, 0, ParameterDirection.Input, EndTime)
                );

            return System.Convert.ToDouble(Result);
        }

        public static double F_WebSitePayMoney(string ConnectionString, long SiteID, long UserID, DateTime StartDate, DateTime EndDate, int type)
        {
            object Result = MSSQL.ExecuteFunction(ConnectionString, "F_WebSitePayMoney",
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
        public static int P_AcceptUserHongbaoPromotion(string ConnectionString, long FromUserID, long ToUserID, long UserHongbaoPromotionID, ref int ReturnValue, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_AcceptUserHongbaoPromotion(ConnectionString, ref ds, FromUserID, ToUserID, UserHongbaoPromotionID, ref ReturnValue, ref ReturnDescription);
        }

        public static int P_AcceptUserHongbaoPromotion(string ConnectionString, ref DataSet ds, long FromUserID, long ToUserID, long UserHongbaoPromotionID, ref int ReturnValue, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(ConnectionString, "P_AcceptUserHongbaoPromotion", ref ds, ref Outputs,
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

        public static int P_AddUserToCpsUId(string ConnectionString, int ID, int Uid, int CpsID, int PID, ref long ReturnValue, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_AddUserToCpsUId(ConnectionString, ref ds, ID, Uid, CpsID, PID, ref ReturnValue, ref ReturnDescription);
        }

        public static int P_AddUserToCpsUId(string ConnectionString, ref DataSet ds, int ID, int Uid, int CpsID, int PID, ref long ReturnValue, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(ConnectionString, "P_AddUserToCpsUId", ref ds, ref Outputs,
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

        public static int P_AdvertisementsEdit(string ConnectionString, int ID, string Title, string Url, int Order, bool isShow, ref int ReturnValue, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_AdvertisementsEdit(ConnectionString, ref ds, ID, Title, Url, Order, isShow, ref ReturnValue, ref ReturnDescription);
        }

        public static int P_AdvertisementsEdit(string ConnectionString, ref DataSet ds, int ID, string Title, string Url, int Order, bool isShow, ref int ReturnValue, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(ConnectionString, "P_AdvertisementsEdit", ref ds, ref Outputs,
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

        public static int P_Analysis_3D_Miss(string ConnectionString, ref int ReturnValue, ref string ReturnDescptrion)
        {
            DataSet ds = null;

            return P_Analysis_3D_Miss(ConnectionString, ref ds, ref ReturnValue, ref ReturnDescptrion);
        }

        public static int P_Analysis_3D_Miss(string ConnectionString, ref DataSet ds, ref int ReturnValue, ref string ReturnDescptrion)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(ConnectionString, "P_Analysis_3D_Miss", ref ds, ref Outputs,
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

        public static int P_Analysis_SHSSL_HotAndCoolAndMiss(string ConnectionString, ref int ReturnValue, ref string ReturnDescptrion)
        {
            DataSet ds = null;

            return P_Analysis_SHSSL_HotAndCoolAndMiss(ConnectionString, ref ds, ref ReturnValue, ref ReturnDescptrion);
        }

        public static int P_Analysis_SHSSL_HotAndCoolAndMiss(string ConnectionString, ref DataSet ds, ref int ReturnValue, ref string ReturnDescptrion)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(ConnectionString, "P_Analysis_SHSSL_HotAndCoolAndMiss", ref ds, ref Outputs,
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

        public static int P_Analysis_SHSSL_WinUsersList(string ConnectionString, int LotteryID, ref int ReturnValue, ref string ReturnDescptrion)
        {
            DataSet ds = null;

            return P_Analysis_SHSSL_WinUsersList(ConnectionString, ref ds, LotteryID, ref ReturnValue, ref ReturnDescptrion);
        }

        public static int P_Analysis_SHSSL_WinUsersList(string ConnectionString, ref DataSet ds, int LotteryID, ref int ReturnValue, ref string ReturnDescptrion)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(ConnectionString, "P_Analysis_SHSSL_WinUsersList", ref ds, ref Outputs,
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

        public static int P_CalculateUserLevel(string ConnectionString)
        {
            DataSet ds = null;

            return P_CalculateUserLevel(ConnectionString, ref ds);
        }

        public static int P_CalculateUserLevel(string ConnectionString, ref DataSet ds)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(ConnectionString, "P_CalculateUserLevel", ref ds, ref Outputs);

            return CallResult;
        }

        public static int P_CanExpenseBonus(string ConnectionString, ref int ReturnValue, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_CanExpenseBonus(ConnectionString, ref ds, ref ReturnValue, ref ReturnDescription);
        }

        public static int P_CanExpenseBonus(string ConnectionString, ref DataSet ds, ref int ReturnValue, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(ConnectionString, "P_CanExpenseBonus", ref ds, ref Outputs,
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

        public static int P_CardPasswordAdd(string ConnectionString, int AgentID, int Period, double Money, int Count, ref int ReturnValue, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_CardPasswordAdd(ConnectionString, ref ds, AgentID, Period, Money, Count, ref ReturnValue, ref ReturnDescription);
        }

        public static int P_CardPasswordAdd(string ConnectionString, ref DataSet ds, int AgentID, int Period, double Money, int Count, ref int ReturnValue, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(ConnectionString, "P_CardPasswordAdd", ref ds, ref Outputs,
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

        public static int P_CardPasswordAgentAddMoney(string ConnectionString, long AgentID, double Amount, string Memo, ref int ReturnValue, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_CardPasswordAgentAddMoney(ConnectionString, ref ds, AgentID, Amount, Memo, ref ReturnValue, ref ReturnDescription);
        }

        public static int P_CardPasswordAgentAddMoney(string ConnectionString, ref DataSet ds, long AgentID, double Amount, string Memo, ref int ReturnValue, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(ConnectionString, "P_CardPasswordAgentAddMoney", ref ds, ref Outputs,
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

        public static int P_CardPasswordExchange(string ConnectionString, int AgentID, string CardsXml, ref int ReturnValue, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_CardPasswordExchange(ConnectionString, ref ds, AgentID, CardsXml, ref ReturnValue, ref ReturnDescription);
        }

        public static int P_CardPasswordExchange(string ConnectionString, ref DataSet ds, int AgentID, string CardsXml, ref int ReturnValue, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(ConnectionString, "P_CardPasswordExchange", ref ds, ref Outputs,
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

        public static int P_CardPasswordGet(string ConnectionString, int AgentID, long CardPasswordID, ref DateTime DateTime, ref DateTime Period, ref double Money, ref short State, ref long UserID, ref DateTime UseDateTime, ref int ReturnValue, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_CardPasswordGet(ConnectionString, ref ds, AgentID, CardPasswordID, ref DateTime, ref Period, ref Money, ref State, ref UserID, ref UseDateTime, ref ReturnValue, ref ReturnDescription);
        }

        public static int P_CardPasswordGet(string ConnectionString, ref DataSet ds, int AgentID, long CardPasswordID, ref DateTime DateTime, ref DateTime Period, ref double Money, ref short State, ref long UserID, ref DateTime UseDateTime, ref int ReturnValue, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(ConnectionString, "P_CardPasswordGet", ref ds, ref Outputs,
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

        public static int P_CardPasswordSetPeriod(string ConnectionString, int AgentID, long CardPasswordID, DateTime Period, ref int ReturnValue, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_CardPasswordSetPeriod(ConnectionString, ref ds, AgentID, CardPasswordID, Period, ref ReturnValue, ref ReturnDescription);
        }

        public static int P_CardPasswordSetPeriod(string ConnectionString, ref DataSet ds, int AgentID, long CardPasswordID, DateTime Period, ref int ReturnValue, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(ConnectionString, "P_CardPasswordSetPeriod", ref ds, ref Outputs,
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

        public static int P_CardPasswordTryErrorAdd(string ConnectionString, long UserID, string Number, ref int ReturnValue, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_CardPasswordTryErrorAdd(ConnectionString, ref ds, UserID, Number, ref ReturnValue, ref ReturnDescription);
        }

        public static int P_CardPasswordTryErrorAdd(string ConnectionString, ref DataSet ds, long UserID, string Number, ref int ReturnValue, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(ConnectionString, "P_CardPasswordTryErrorAdd", ref ds, ref Outputs,
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

        public static int P_CardPasswordTryErrorFreeze(string ConnectionString, long SiteID, long UserID, ref int Freeze, ref int ReturnValue, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_CardPasswordTryErrorFreeze(ConnectionString, ref ds, SiteID, UserID, ref Freeze, ref ReturnValue, ref ReturnDescription);
        }

        public static int P_CardPasswordTryErrorFreeze(string ConnectionString, ref DataSet ds, long SiteID, long UserID, ref int Freeze, ref int ReturnValue, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(ConnectionString, "P_CardPasswordTryErrorFreeze", ref ds, ref Outputs,
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

        public static int P_CardPasswordUse(string ConnectionString, int AgentID, long CardPasswordID, string Number, long SiteID, long UserID, ref int ReturnValue, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_CardPasswordUse(ConnectionString, ref ds, AgentID, CardPasswordID, Number, SiteID, UserID, ref ReturnValue, ref ReturnDescription);
        }

        public static int P_CardPasswordUse(string ConnectionString, ref DataSet ds, int AgentID, long CardPasswordID, string Number, long SiteID, long UserID, ref int ReturnValue, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(ConnectionString, "P_CardPasswordUse", ref ds, ref Outputs,
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

        public static int P_CelebAdd(string ConnectionString, long SiteID, long UserID, string Title, string Intro, string Say, string Comment, string Score, long Order, bool isRecommended, ref long CelebID, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_CelebAdd(ConnectionString, ref ds, SiteID, UserID, Title, Intro, Say, Comment, Score, Order, isRecommended, ref CelebID, ref ReturnDescription);
        }

        public static int P_CelebAdd(string ConnectionString, ref DataSet ds, long SiteID, long UserID, string Title, string Intro, string Say, string Comment, string Score, long Order, bool isRecommended, ref long CelebID, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(ConnectionString, "P_CelebAdd", ref ds, ref Outputs,
                new MSSQL.Parameter("SiteID", SqlDbType.BigInt, 0, ParameterDirection.Input, SiteID),
                new MSSQL.Parameter("UserID", SqlDbType.BigInt, 0, ParameterDirection.Input, UserID),
                new MSSQL.Parameter("Title", SqlDbType.VarChar, 0, ParameterDirection.Input, Title),
                new MSSQL.Parameter("Intro", SqlDbType.VarChar, 0, ParameterDirection.Input, Intro),
                new MSSQL.Parameter("Say", SqlDbType.VarChar, 0, ParameterDirection.Input, Say),
                new MSSQL.Parameter("Comment", SqlDbType.VarChar, 0, ParameterDirection.Input, Comment),
                new MSSQL.Parameter("Score", SqlDbType.VarChar, 0, ParameterDirection.Input, Score),
                new MSSQL.Parameter("Order", SqlDbType.BigInt, 0, ParameterDirection.Input, Order),
                new MSSQL.Parameter("isRecommended", SqlDbType.Bit, 0, ParameterDirection.Input, isRecommended),
                new MSSQL.Parameter("CelebID", SqlDbType.BigInt, 8, ParameterDirection.Output, CelebID),
                new MSSQL.Parameter("ReturnDescription", SqlDbType.VarChar, 100, ParameterDirection.Output, ReturnDescription)
                );

            try
            {
                CelebID = System.Convert.ToInt64(Outputs["CelebID"]);
            }
            catch { }

            try
            {
                ReturnDescription = System.Convert.ToString(Outputs["ReturnDescription"]);
            }
            catch { }

            return CallResult;
        }

        public static int P_CelebDelete(string ConnectionString, long SiteID, long ID, ref int ReturnValue, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_CelebDelete(ConnectionString, ref ds, SiteID, ID, ref ReturnValue, ref ReturnDescription);
        }

        public static int P_CelebDelete(string ConnectionString, ref DataSet ds, long SiteID, long ID, ref int ReturnValue, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(ConnectionString, "P_CelebDelete", ref ds, ref Outputs,
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

        public static int P_CelebEdit(string ConnectionString, long SiteID, long ID, string Title, string Intro, string Say, string Comment, string Score, long Order, bool isRecommended, ref int ReturnValue, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_CelebEdit(ConnectionString, ref ds, SiteID, ID, Title, Intro, Say, Comment, Score, Order, isRecommended, ref ReturnValue, ref ReturnDescription);
        }

        public static int P_CelebEdit(string ConnectionString, ref DataSet ds, long SiteID, long ID, string Title, string Intro, string Say, string Comment, string Score, long Order, bool isRecommended, ref int ReturnValue, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(ConnectionString, "P_CelebEdit", ref ds, ref Outputs,
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

        public static int P_ChasesAdd(string ConnectionString, long UserID, int LotteryID, short Type, DateTime StartTime, DateTime EndTime, int IsuseCount, int Multiple, int Nums, short BetType, string LotteryNumber, short StopTypeWhenWin, double StopTypeWhenMoney, double Money, string Title, string ChaseXML, ref int ChaseID, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_ChasesAdd(ConnectionString, ref ds, UserID, LotteryID, Type, StartTime, EndTime, IsuseCount, Multiple, Nums, BetType, LotteryNumber, StopTypeWhenWin, StopTypeWhenMoney, Money, Title, ChaseXML, ref ChaseID, ref ReturnDescription);
        }

        public static int P_ChasesAdd(string ConnectionString, ref DataSet ds, long UserID, int LotteryID, short Type, DateTime StartTime, DateTime EndTime, int IsuseCount, int Multiple, int Nums, short BetType, string LotteryNumber, short StopTypeWhenWin, double StopTypeWhenMoney, double Money, string Title, string ChaseXML, ref int ChaseID, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(ConnectionString, "P_ChasesAdd", ref ds, ref Outputs,
                new MSSQL.Parameter("UserID", SqlDbType.BigInt, 0, ParameterDirection.Input, UserID),
                new MSSQL.Parameter("LotteryID", SqlDbType.Int, 0, ParameterDirection.Input, LotteryID),
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

        public static int P_ChaseStopWhenWin(string ConnectionString, long SchemeID, double WinMoney, ref int ReturnValue, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_ChaseStopWhenWin(ConnectionString, ref ds, SchemeID, WinMoney, ref ReturnValue, ref ReturnDescription);
        }

        public static int P_ChaseStopWhenWin(string ConnectionString, ref DataSet ds, long SchemeID, double WinMoney, ref int ReturnValue, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(ConnectionString, "P_ChaseStopWhenWin", ref ds, ref Outputs,
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

        public static int P_ChaseTaskStopWhenWin(string ConnectionString, long SiteID, long SchemeID, double WinMoney, ref int ReturnValue, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_ChaseTaskStopWhenWin(ConnectionString, ref ds, SiteID, SchemeID, WinMoney, ref ReturnValue, ref ReturnDescription);
        }

        public static int P_ChaseTaskStopWhenWin(string ConnectionString, ref DataSet ds, long SiteID, long SchemeID, double WinMoney, ref int ReturnValue, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(ConnectionString, "P_ChaseTaskStopWhenWin", ref ds, ref Outputs,
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

        public static int P_CheckChaseTasks(string ConnectionString, ref int ReturnValue, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_CheckChaseTasks(ConnectionString, ref ds, ref ReturnValue, ref ReturnDescription);
        }

        public static int P_CheckChaseTasks(string ConnectionString, ref DataSet ds, ref int ReturnValue, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(ConnectionString, "P_CheckChaseTasks", ref ds, ref Outputs,
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

        public static int P_ClearSystemLog(string ConnectionString, long SiteID, long UserID, string IPAddress, ref int ReturnValue, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_ClearSystemLog(ConnectionString, ref ds, SiteID, UserID, IPAddress, ref ReturnValue, ref ReturnDescription);
        }

        public static int P_ClearSystemLog(string ConnectionString, ref DataSet ds, long SiteID, long UserID, string IPAddress, ref int ReturnValue, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(ConnectionString, "P_ClearSystemLog", ref ds, ref Outputs,
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

        public static int P_CpsAdd(string ConnectionString, long SiteID, long OwnerUserID, string Name, string Url, string LogoUrl, double BonusScale, bool ON, string Company, string Address, string PostCode, string ResponsiblePerson, string ContactPerson, string Telephone, string Fax, string Mobile, string Email, string QQ, string ServiceTelephone, string MD5Key, short Type, long ParentID, string DomainName, long OperatorID, long CommendID, ref long ID, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_CpsAdd(ConnectionString, ref ds, SiteID, OwnerUserID, Name, Url, LogoUrl, BonusScale, ON, Company, Address, PostCode, ResponsiblePerson, ContactPerson, Telephone, Fax, Mobile, Email, QQ, ServiceTelephone, MD5Key, Type, ParentID, DomainName, OperatorID, CommendID, ref ID, ref ReturnDescription);
        }

        public static int P_CpsAdd(string ConnectionString, ref DataSet ds, long SiteID, long OwnerUserID, string Name, string Url, string LogoUrl, double BonusScale, bool ON, string Company, string Address, string PostCode, string ResponsiblePerson, string ContactPerson, string Telephone, string Fax, string Mobile, string Email, string QQ, string ServiceTelephone, string MD5Key, short Type, long ParentID, string DomainName, long OperatorID, long CommendID, ref long ID, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(ConnectionString, "P_CpsAdd", ref ds, ref Outputs,
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

        public static int P_CpsApp(string ConnectionString, long SiteID, string UserName, string PassWord, string Content, string Name, string Url, string ContactPerson, string Telephone, string Mobile, string Email, string QQ, string DomainName, long CpsID, double BonusScale, ref int ID, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_CpsApp(ConnectionString, ref ds, SiteID, UserName, PassWord, Content, Name, Url, ContactPerson, Telephone, Mobile, Email, QQ, DomainName, CpsID, BonusScale, ref ID, ref ReturnDescription);
        }

        public static int P_CpsApp(string ConnectionString, ref DataSet ds, long SiteID, string UserName, string PassWord, string Content, string Name, string Url, string ContactPerson, string Telephone, string Mobile, string Email, string QQ, string DomainName, long CpsID, double BonusScale, ref int ID, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(ConnectionString, "P_CpsApp", ref ds, ref Outputs,
                new MSSQL.Parameter("SiteID", SqlDbType.BigInt, 0, ParameterDirection.Input, SiteID),
                new MSSQL.Parameter("UserName", SqlDbType.VarChar, 0, ParameterDirection.Input, UserName),
                new MSSQL.Parameter("PassWord", SqlDbType.VarChar, 0, ParameterDirection.Input, PassWord),
                new MSSQL.Parameter("Content", SqlDbType.VarChar, 0, ParameterDirection.Input, Content),
                new MSSQL.Parameter("Name", SqlDbType.VarChar, 0, ParameterDirection.Input, Name),
                new MSSQL.Parameter("Url", SqlDbType.VarChar, 0, ParameterDirection.Input, Url),
                new MSSQL.Parameter("ContactPerson", SqlDbType.VarChar, 0, ParameterDirection.Input, ContactPerson),
                new MSSQL.Parameter("Telephone", SqlDbType.VarChar, 0, ParameterDirection.Input, Telephone),
                new MSSQL.Parameter("Mobile", SqlDbType.VarChar, 0, ParameterDirection.Input, Mobile),
                new MSSQL.Parameter("Email", SqlDbType.VarChar, 0, ParameterDirection.Input, Email),
                new MSSQL.Parameter("QQ", SqlDbType.VarChar, 0, ParameterDirection.Input, QQ),
                new MSSQL.Parameter("DomainName", SqlDbType.VarChar, 0, ParameterDirection.Input, DomainName),
                new MSSQL.Parameter("CpsID", SqlDbType.BigInt, 0, ParameterDirection.Input, CpsID),
                new MSSQL.Parameter("BonusScale", SqlDbType.Float, 0, ParameterDirection.Input, BonusScale),
                new MSSQL.Parameter("ID", SqlDbType.Int, 4, ParameterDirection.Output, ID),
                new MSSQL.Parameter("ReturnDescription", SqlDbType.VarChar, 100, ParameterDirection.Output, ReturnDescription)
                );

            try
            {
                ID = System.Convert.ToInt32(Outputs["ID"]);
            }
            catch { }

            try
            {
                ReturnDescription = System.Convert.ToString(Outputs["ReturnDescription"]);
            }
            catch { }

            return CallResult;
        }

        public static int P_CpsCalculateAllowBonus(string ConnectionString, int Year, int Month, ref int ReturnValue, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_CpsCalculateAllowBonus(ConnectionString, ref ds, Year, Month, ref ReturnValue, ref ReturnDescription);
        }

        public static int P_CpsCalculateAllowBonus(string ConnectionString, ref DataSet ds, int Year, int Month, ref int ReturnValue, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(ConnectionString, "P_CpsCalculateAllowBonus", ref ds, ref Outputs,
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

        public static int P_CpsCalculateBonus(string ConnectionString, ref int ReturnValue, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_CpsCalculateBonus(ConnectionString, ref ds, ref ReturnValue, ref ReturnDescription);
        }

        public static int P_CpsCalculateBonus(string ConnectionString, ref DataSet ds, ref int ReturnValue, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(ConnectionString, "P_CpsCalculateBonus", ref ds, ref Outputs,
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

        public static int P_CpsDistill(string ConnectionString, long SiteID, long UserID, double Money, double FormalitiesFees, string BankUserName, string BankName, string BankCardNumber, string Memo, ref int ReturnValue, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_CpsDistill(ConnectionString, ref ds, SiteID, UserID, Money, FormalitiesFees, BankUserName, BankName, BankCardNumber, Memo, ref ReturnValue, ref ReturnDescription);
        }

        public static int P_CpsDistill(string ConnectionString, ref DataSet ds, long SiteID, long UserID, double Money, double FormalitiesFees, string BankUserName, string BankName, string BankCardNumber, string Memo, ref int ReturnValue, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(ConnectionString, "P_CpsDistill", ref ds, ref Outputs,
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

        public static int P_CpsDistillAccept(string ConnectionString, long SiteID, long UserID, long DistillID, string PayName, string PayBank, string PayCardNumber, string Memo, long HandleOperatorID, ref int ReturnValue, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_CpsDistillAccept(ConnectionString, ref ds, SiteID, UserID, DistillID, PayName, PayBank, PayCardNumber, Memo, HandleOperatorID, ref ReturnValue, ref ReturnDescription);
        }

        public static int P_CpsDistillAccept(string ConnectionString, ref DataSet ds, long SiteID, long UserID, long DistillID, string PayName, string PayBank, string PayCardNumber, string Memo, long HandleOperatorID, ref int ReturnValue, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(ConnectionString, "P_CpsDistillAccept", ref ds, ref Outputs,
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

        public static int P_CpsDistillNoAccept(string ConnectionString, long SiteID, long UserID, long DistillID, string Memo, long HandleOperatorID, ref int ReturnValue, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_CpsDistillNoAccept(ConnectionString, ref ds, SiteID, UserID, DistillID, Memo, HandleOperatorID, ref ReturnValue, ref ReturnDescription);
        }

        public static int P_CpsDistillNoAccept(string ConnectionString, ref DataSet ds, long SiteID, long UserID, long DistillID, string Memo, long HandleOperatorID, ref int ReturnValue, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(ConnectionString, "P_CpsDistillNoAccept", ref ds, ref Outputs,
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

        public static int P_CpsDistillQuash(string ConnectionString, long SiteID, long UserID, long DistillID, ref int ReturnValue, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_CpsDistillQuash(ConnectionString, ref ds, SiteID, UserID, DistillID, ref ReturnValue, ref ReturnDescription);
        }

        public static int P_CpsDistillQuash(string ConnectionString, ref DataSet ds, long SiteID, long UserID, long DistillID, ref int ReturnValue, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(ConnectionString, "P_CpsDistillQuash", ref ds, ref Outputs,
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

        public static int P_CpsEdit(string ConnectionString, long SiteID, long CpsID, string UrlName, string Url, string LogoUrl, double BonusScale, bool ON, string Company, string Address, string PostCode, string ResponsiblePerson, string ContactPerson, string Telephone, string Fax, string Mobile, string Email, string QQ, string ServiceTelephone, string MD5Key, short Type, string DomainName, bool IsShow, ref int ReturnValue, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_CpsEdit(ConnectionString, ref ds, SiteID, CpsID, UrlName, Url, LogoUrl, BonusScale, ON, Company, Address, PostCode, ResponsiblePerson, ContactPerson, Telephone, Fax, Mobile, Email, QQ, ServiceTelephone, MD5Key, Type, DomainName, IsShow, ref ReturnValue, ref ReturnDescription);
        }

        public static int P_CpsEdit(string ConnectionString, ref DataSet ds, long SiteID, long CpsID, string UrlName, string Url, string LogoUrl, double BonusScale, bool ON, string Company, string Address, string PostCode, string ResponsiblePerson, string ContactPerson, string Telephone, string Fax, string Mobile, string Email, string QQ, string ServiceTelephone, string MD5Key, short Type, string DomainName, bool IsShow, ref int ReturnValue, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(ConnectionString, "P_CpsEdit", ref ds, ref Outputs,
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

        public static int P_CpsGetCommendMemberBuyDetail(string ConnectionString, long SiteID, long CommenderID, long MemberID, DateTime FromTime, DateTime ToTime, ref int ReturnValue, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_CpsGetCommendMemberBuyDetail(ConnectionString, ref ds, SiteID, CommenderID, MemberID, FromTime, ToTime, ref ReturnValue, ref ReturnDescription);
        }

        public static int P_CpsGetCommendMemberBuyDetail(string ConnectionString, ref DataSet ds, long SiteID, long CommenderID, long MemberID, DateTime FromTime, DateTime ToTime, ref int ReturnValue, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(ConnectionString, "P_CpsGetCommendMemberBuyDetail", ref ds, ref Outputs,
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

        public static int P_CpsGetCommendMemberList(string ConnectionString, long CommmenderID, ref int ReturnValue, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_CpsGetCommendMemberList(ConnectionString, ref ds, CommmenderID, ref ReturnValue, ref ReturnDescription);
        }

        public static int P_CpsGetCommendMemberList(string ConnectionString, ref DataSet ds, long CommmenderID, ref int ReturnValue, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(ConnectionString, "P_CpsGetCommendMemberList", ref ds, ref Outputs,
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

        public static int P_CpsGetCommendSiteBuyDetail(string ConnectionString, long SiteID, long CommenderID, long CpsID, long MemberID, DateTime FromTime, DateTime ToTime, ref int ReturnValue, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_CpsGetCommendSiteBuyDetail(ConnectionString, ref ds, SiteID, CommenderID, CpsID, MemberID, FromTime, ToTime, ref ReturnValue, ref ReturnDescription);
        }

        public static int P_CpsGetCommendSiteBuyDetail(string ConnectionString, ref DataSet ds, long SiteID, long CommenderID, long CpsID, long MemberID, DateTime FromTime, DateTime ToTime, ref int ReturnValue, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(ConnectionString, "P_CpsGetCommendSiteBuyDetail", ref ds, ref Outputs,
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

        public static int P_CpsMemRecommendWebsiteList(string ConnectionString, long userid, long siteid, ref int ReturnValue, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_CpsMemRecommendWebsiteList(ConnectionString, ref ds, userid, siteid, ref ReturnValue, ref ReturnDescription);
        }

        public static int P_CpsMemRecommendWebsiteList(string ConnectionString, ref DataSet ds, long userid, long siteid, ref int ReturnValue, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(ConnectionString, "P_CpsMemRecommendWebsiteList", ref ds, ref Outputs,
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

        public static int P_CpsPromoterList(string ConnectionString, long SiteID, ref int ReturnValue, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_CpsPromoterList(ConnectionString, ref ds, SiteID, ref ReturnValue, ref ReturnDescription);
        }

        public static int P_CpsPromoterList(string ConnectionString, ref DataSet ds, long SiteID, ref int ReturnValue, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(ConnectionString, "P_CpsPromoterList", ref ds, ref Outputs,
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

        public static int P_CpsTry(string ConnectionString, long SiteID, long UserID, string Content, string Name, string Url, string LogoUrl, string Company, string Address, string PostCode, string ResponsiblePerson, string ContactPerson, string Telephone, string Fax, string Mobile, string Email, string QQ, string ServiceTelephone, string MD5Key, short Type, string DomainName, long ParentID, double BonusScale, long CommendID, ref long ReturnValue, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_CpsTry(ConnectionString, ref ds, SiteID, UserID, Content, Name, Url, LogoUrl, Company, Address, PostCode, ResponsiblePerson, ContactPerson, Telephone, Fax, Mobile, Email, QQ, ServiceTelephone, MD5Key, Type, DomainName, ParentID, BonusScale, CommendID, ref ReturnValue, ref ReturnDescription);
        }

        public static int P_CpsTry(string ConnectionString, ref DataSet ds, long SiteID, long UserID, string Content, string Name, string Url, string LogoUrl, string Company, string Address, string PostCode, string ResponsiblePerson, string ContactPerson, string Telephone, string Fax, string Mobile, string Email, string QQ, string ServiceTelephone, string MD5Key, short Type, string DomainName, long ParentID, double BonusScale, long CommendID, ref long ReturnValue, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(ConnectionString, "P_CpsTry", ref ds, ref Outputs,
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

        public static int P_CpsTryHandle(string ConnectionString, long SiteID, long TryID, long OperatorID, short HandleResult, double BonusScale, bool ON, ref long CpsID, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_CpsTryHandle(ConnectionString, ref ds, SiteID, TryID, OperatorID, HandleResult, BonusScale, ON, ref CpsID, ref ReturnDescription);
        }

        public static int P_CpsTryHandle(string ConnectionString, ref DataSet ds, long SiteID, long TryID, long OperatorID, short HandleResult, double BonusScale, bool ON, ref long CpsID, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(ConnectionString, "P_CpsTryHandle", ref ds, ref Outputs,
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

        public static int P_CustomFollowSchemesAdd(string ConnectionString, long SiteID, long UserID, long FollowSchemeID, double MoneyStart, double MoneyEnd, int BuyShareStart, int BuyShareEnd, short Type, ref int ReturnValue, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_CustomFollowSchemesAdd(ConnectionString, ref ds, SiteID, UserID, FollowSchemeID, MoneyStart, MoneyEnd, BuyShareStart, BuyShareEnd, Type, ref ReturnValue, ref ReturnDescription);
        }

        public static int P_CustomFollowSchemesAdd(string ConnectionString, ref DataSet ds, long SiteID, long UserID, long FollowSchemeID, double MoneyStart, double MoneyEnd, int BuyShareStart, int BuyShareEnd, short Type, ref int ReturnValue, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(ConnectionString, "P_CustomFollowSchemesAdd", ref ds, ref Outputs,
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

        public static int P_CustomFollowSchemesDelete(string ConnectionString, long SiteID, long UserID, long FollowSchemeID, ref int ReturnValue, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_CustomFollowSchemesDelete(ConnectionString, ref ds, SiteID, UserID, FollowSchemeID, ref ReturnValue, ref ReturnDescription);
        }

        public static int P_CustomFollowSchemesDelete(string ConnectionString, ref DataSet ds, long SiteID, long UserID, long FollowSchemeID, ref int ReturnValue, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(ConnectionString, "P_CustomFollowSchemesDelete", ref ds, ref Outputs,
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

        public static int P_Distill(string ConnectionString, long SiteID, long UserID, int DistillType, double Money, double FormalitiesFees, string BankUserName, string BankName, string BankCardNumber, string AlipayID, string AlipayName, string Memo, ref int ReturnValue, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_Distill(ConnectionString, ref ds, SiteID, UserID, DistillType, Money, FormalitiesFees, BankUserName, BankName, BankCardNumber, AlipayID, AlipayName, Memo, ref ReturnValue, ref ReturnDescription);
        }

        public static int P_Distill(string ConnectionString, ref DataSet ds, long SiteID, long UserID, int DistillType, double Money, double FormalitiesFees, string BankUserName, string BankName, string BankCardNumber, string AlipayID, string AlipayName, string Memo, ref int ReturnValue, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(ConnectionString, "P_Distill", ref ds, ref Outputs,
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

        public static int P_DistillAccept(string ConnectionString, long SiteID, long UserID, long DistillID, string PayName, string PayBank, string PayCardNumber, string AlipayID, string AlipayName, string Memo, long HandleOperatorID, ref int ReturnValue, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_DistillAccept(ConnectionString, ref ds, SiteID, UserID, DistillID, PayName, PayBank, PayCardNumber, AlipayID, AlipayName, Memo, HandleOperatorID, ref ReturnValue, ref ReturnDescription);
        }

        public static int P_DistillAccept(string ConnectionString, ref DataSet ds, long SiteID, long UserID, long DistillID, string PayName, string PayBank, string PayCardNumber, string AlipayID, string AlipayName, string Memo, long HandleOperatorID, ref int ReturnValue, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(ConnectionString, "P_DistillAccept", ref ds, ref Outputs,
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

        public static int P_DistillNoAccept(string ConnectionString, long SiteID, long UserID, long DistillID, string Memo, long HandleOperatorID, ref int ReturnValue, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_DistillNoAccept(ConnectionString, ref ds, SiteID, UserID, DistillID, Memo, HandleOperatorID, ref ReturnValue, ref ReturnDescription);
        }

        public static int P_DistillNoAccept(string ConnectionString, ref DataSet ds, long SiteID, long UserID, long DistillID, string Memo, long HandleOperatorID, ref int ReturnValue, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(ConnectionString, "P_DistillNoAccept", ref ds, ref Outputs,
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

        public static int P_DistillPaySuccess(string ConnectionString, long SiteID, long UserID, long DistillID, string Memo, long HandleOperatorID, ref int ReturnValue, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_DistillPaySuccess(ConnectionString, ref ds, SiteID, UserID, DistillID, Memo, HandleOperatorID, ref ReturnValue, ref ReturnDescription);
        }

        public static int P_DistillPaySuccess(string ConnectionString, ref DataSet ds, long SiteID, long UserID, long DistillID, string Memo, long HandleOperatorID, ref int ReturnValue, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(ConnectionString, "P_DistillPaySuccess", ref ds, ref Outputs,
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

        public static int P_DistillQuash(string ConnectionString, long SiteID, long UserID, long DistillID, ref int ReturnValue, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_DistillQuash(ConnectionString, ref ds, SiteID, UserID, DistillID, ref ReturnValue, ref ReturnDescription);
        }

        public static int P_DistillQuash(string ConnectionString, ref DataSet ds, long SiteID, long UserID, long DistillID, ref int ReturnValue, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(ConnectionString, "P_DistillQuash", ref ds, ref Outputs,
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

        public static int P_DownloadAdd(string ConnectionString, long SiteID, DateTime DateTime, string Title, string FileUrl, bool isShow, ref long NewDownloadID, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_DownloadAdd(ConnectionString, ref ds, SiteID, DateTime, Title, FileUrl, isShow, ref NewDownloadID, ref ReturnDescription);
        }

        public static int P_DownloadAdd(string ConnectionString, ref DataSet ds, long SiteID, DateTime DateTime, string Title, string FileUrl, bool isShow, ref long NewDownloadID, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(ConnectionString, "P_DownloadAdd", ref ds, ref Outputs,
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

        public static int P_DownloadDelete(string ConnectionString, long SiteID, long ID, ref int ReturnValue, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_DownloadDelete(ConnectionString, ref ds, SiteID, ID, ref ReturnValue, ref ReturnDescription);
        }

        public static int P_DownloadDelete(string ConnectionString, ref DataSet ds, long SiteID, long ID, ref int ReturnValue, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(ConnectionString, "P_DownloadDelete", ref ds, ref Outputs,
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

        public static int P_DownloadEdit(string ConnectionString, long SiteID, long ID, DateTime DateTime, string Title, string FileUrl, bool isShow, ref int ReturnValue, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_DownloadEdit(ConnectionString, ref ds, SiteID, ID, DateTime, Title, FileUrl, isShow, ref ReturnValue, ref ReturnDescription);
        }

        public static int P_DownloadEdit(string ConnectionString, ref DataSet ds, long SiteID, long ID, DateTime DateTime, string Title, string FileUrl, bool isShow, ref int ReturnValue, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(ConnectionString, "P_DownloadEdit", ref ds, ref Outputs,
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

        public static int P_ElectronTicketAgentAddMoney(string ConnectionString, long AgentID, double Amount, string Memo, ref int ReturnValue, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_ElectronTicketAgentAddMoney(ConnectionString, ref ds, AgentID, Amount, Memo, ref ReturnValue, ref ReturnDescription);
        }

        public static int P_ElectronTicketAgentAddMoney(string ConnectionString, ref DataSet ds, long AgentID, double Amount, string Memo, ref int ReturnValue, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(ConnectionString, "P_ElectronTicketAgentAddMoney", ref ds, ref Outputs,
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

        public static int P_ElectronTicketAgentSchemeAdd(string ConnectionString, long AgentID, string SchemeNumber, int LotteryID, int PlayTypeID, long IsuseID, string LotteryNumber, double Amount, int Multiple, int Share, string InitiateName, string InitiateAlipayName, string InitiateAlipayID, string InitiateRealityName, string InitiateIDCard, string InitiateTelephone, string InitiateMobile, string InitiateEmail, double InitiateBonusScale, double InitiateBonusLimitLower, double InitiateBonusLimitUpper, string DetailXML, ref long SchemeID, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_ElectronTicketAgentSchemeAdd(ConnectionString, ref ds, AgentID, SchemeNumber, LotteryID, PlayTypeID, IsuseID, LotteryNumber, Amount, Multiple, Share, InitiateName, InitiateAlipayName, InitiateAlipayID, InitiateRealityName, InitiateIDCard, InitiateTelephone, InitiateMobile, InitiateEmail, InitiateBonusScale, InitiateBonusLimitLower, InitiateBonusLimitUpper, DetailXML, ref SchemeID, ref ReturnDescription);
        }

        public static int P_ElectronTicketAgentSchemeAdd(string ConnectionString, ref DataSet ds, long AgentID, string SchemeNumber, int LotteryID, int PlayTypeID, long IsuseID, string LotteryNumber, double Amount, int Multiple, int Share, string InitiateName, string InitiateAlipayName, string InitiateAlipayID, string InitiateRealityName, string InitiateIDCard, string InitiateTelephone, string InitiateMobile, string InitiateEmail, double InitiateBonusScale, double InitiateBonusLimitLower, double InitiateBonusLimitUpper, string DetailXML, ref long SchemeID, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(ConnectionString, "P_ElectronTicketAgentSchemeAdd", ref ds, ref Outputs,
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

        public static int P_ElectronTicketAgentSchemeQuash(string ConnectionString, long IsuseID, ref int ReturnValue, ref string ReturnDescptrion)
        {
            DataSet ds = null;

            return P_ElectronTicketAgentSchemeQuash(ConnectionString, ref ds, IsuseID, ref ReturnValue, ref ReturnDescptrion);
        }

        public static int P_ElectronTicketAgentSchemeQuash(string ConnectionString, ref DataSet ds, long IsuseID, ref int ReturnValue, ref string ReturnDescptrion)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(ConnectionString, "P_ElectronTicketAgentSchemeQuash", ref ds, ref Outputs,
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

        public static int P_ElectronTicketAgentSchemeSendToCenterAdd(string ConnectionString, long SchemeID, int PlayTypeID, string AgrentNo, string TicketXML, ref int ReturnValue, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_ElectronTicketAgentSchemeSendToCenterAdd(ConnectionString, ref ds, SchemeID, PlayTypeID, AgrentNo, TicketXML, ref ReturnValue, ref ReturnDescription);
        }

        public static int P_ElectronTicketAgentSchemeSendToCenterAdd(string ConnectionString, ref DataSet ds, long SchemeID, int PlayTypeID, string AgrentNo, string TicketXML, ref int ReturnValue, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(ConnectionString, "P_ElectronTicketAgentSchemeSendToCenterAdd", ref ds, ref Outputs,
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

        public static int P_ElectronTicketAgentSchemeSendToCenterAdd_Single(string ConnectionString, long SchemeID, int PlayTypeID, double Money, int Multiple, string Ticket, bool isFirstWrite, string AgrentNo, ref long ID, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_ElectronTicketAgentSchemeSendToCenterAdd_Single(ConnectionString, ref ds, SchemeID, PlayTypeID, Money, Multiple, Ticket, isFirstWrite, AgrentNo, ref ID, ref ReturnDescription);
        }

        public static int P_ElectronTicketAgentSchemeSendToCenterAdd_Single(string ConnectionString, ref DataSet ds, long SchemeID, int PlayTypeID, double Money, int Multiple, string Ticket, bool isFirstWrite, string AgrentNo, ref long ID, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(ConnectionString, "P_ElectronTicketAgentSchemeSendToCenterAdd_Single", ref ds, ref Outputs,
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

        public static int P_ElectronTicketAgentSchemesSendToCenterHandleUniteAnte(string ConnectionString, long SchemeID, DateTime DealTime, ref int ReturnValue, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_ElectronTicketAgentSchemesSendToCenterHandleUniteAnte(ConnectionString, ref ds, SchemeID, DealTime, ref ReturnValue, ref ReturnDescription);
        }

        public static int P_ElectronTicketAgentSchemesSendToCenterHandleUniteAnte(string ConnectionString, ref DataSet ds, long SchemeID, DateTime DealTime, ref int ReturnValue, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(ConnectionString, "P_ElectronTicketAgentSchemesSendToCenterHandleUniteAnte", ref ds, ref Outputs,
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

        public static int P_ElectronTicketWin(string ConnectionString, long IsuseID, string BonusXML, string AgentBonusXML, ref int ReturnValue, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_ElectronTicketWin(ConnectionString, ref ds, IsuseID, BonusXML, AgentBonusXML, ref ReturnValue, ref ReturnDescription);
        }

        public static int P_ElectronTicketWin(string ConnectionString, ref DataSet ds, long IsuseID, string BonusXML, string AgentBonusXML, ref int ReturnValue, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(ConnectionString, "P_ElectronTicketWin", ref ds, ref Outputs,
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

        public static int P_EnterSchemeChatRoom(string ConnectionString, long SiteID, long UserID, long SchemeID, ref int ReturnValue, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_EnterSchemeChatRoom(ConnectionString, ref ds, SiteID, UserID, SchemeID, ref ReturnValue, ref ReturnDescription);
        }

        public static int P_EnterSchemeChatRoom(string ConnectionString, ref DataSet ds, long SiteID, long UserID, long SchemeID, ref int ReturnValue, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(ConnectionString, "P_EnterSchemeChatRoom", ref ds, ref Outputs,
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

        public static int P_ExecChases(string ConnectionString, ref int ReturnValue, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_ExecChases(ConnectionString, ref ds, ref ReturnValue, ref ReturnDescription);
        }

        public static int P_ExecChases(string ConnectionString, ref DataSet ds, ref int ReturnValue, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(ConnectionString, "P_ExecChases", ref ds, ref Outputs,
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

        public static int P_ExecChaseTaskDetail(string ConnectionString, long SiteID, long ChaseTaskDetailID, ref int ReturnValue, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_ExecChaseTaskDetail(ConnectionString, ref ds, SiteID, ChaseTaskDetailID, ref ReturnValue, ref ReturnDescription);
        }

        public static int P_ExecChaseTaskDetail(string ConnectionString, ref DataSet ds, long SiteID, long ChaseTaskDetailID, ref int ReturnValue, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(ConnectionString, "P_ExecChaseTaskDetail", ref ds, ref Outputs,
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

        public static int P_ExecChaseTasks(string ConnectionString, ref int ReturnValue, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_ExecChaseTasks(ConnectionString, ref ds, ref ReturnValue, ref ReturnDescription);
        }

        public static int P_ExecChaseTasks(string ConnectionString, ref DataSet ds, ref int ReturnValue, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(ConnectionString, "P_ExecChaseTasks", ref ds, ref Outputs,
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

        public static int P_ExpertsCommendAdd(string ConnectionString, long SiteID, long ExpertsID, DateTime DateTime, long IsuseID, string Title, string Content, string Number, double Price, ref long NewExpertsCommendID, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_ExpertsCommendAdd(ConnectionString, ref ds, SiteID, ExpertsID, DateTime, IsuseID, Title, Content, Number, Price, ref NewExpertsCommendID, ref ReturnDescription);
        }

        public static int P_ExpertsCommendAdd(string ConnectionString, ref DataSet ds, long SiteID, long ExpertsID, DateTime DateTime, long IsuseID, string Title, string Content, string Number, double Price, ref long NewExpertsCommendID, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(ConnectionString, "P_ExpertsCommendAdd", ref ds, ref Outputs,
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

        public static int P_ExpertsCommendDelete(string ConnectionString, long SiteID, long ExpertsCommendID, ref int ReturnValue, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_ExpertsCommendDelete(ConnectionString, ref ds, SiteID, ExpertsCommendID, ref ReturnValue, ref ReturnDescription);
        }

        public static int P_ExpertsCommendDelete(string ConnectionString, ref DataSet ds, long SiteID, long ExpertsCommendID, ref int ReturnValue, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(ConnectionString, "P_ExpertsCommendDelete", ref ds, ref Outputs,
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

        public static int P_ExpertsCommendEdit(string ConnectionString, long SiteID, long ExpertsCommendID, DateTime DateTime, long IsuseID, string Title, string Content, string Number, double Price, double WinMoney, bool isCommend, ref int ReturnValue, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_ExpertsCommendEdit(ConnectionString, ref ds, SiteID, ExpertsCommendID, DateTime, IsuseID, Title, Content, Number, Price, WinMoney, isCommend, ref ReturnValue, ref ReturnDescription);
        }

        public static int P_ExpertsCommendEdit(string ConnectionString, ref DataSet ds, long SiteID, long ExpertsCommendID, DateTime DateTime, long IsuseID, string Title, string Content, string Number, double Price, double WinMoney, bool isCommend, ref int ReturnValue, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(ConnectionString, "P_ExpertsCommendEdit", ref ds, ref Outputs,
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

        public static int P_ExpertsCommendRead(string ConnectionString, long SiteID, long ExpertsCommendID, long UserID, ref int ReturnValue, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_ExpertsCommendRead(ConnectionString, ref ds, SiteID, ExpertsCommendID, UserID, ref ReturnValue, ref ReturnDescription);
        }

        public static int P_ExpertsCommendRead(string ConnectionString, ref DataSet ds, long SiteID, long ExpertsCommendID, long UserID, ref int ReturnValue, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(ConnectionString, "P_ExpertsCommendRead", ref ds, ref Outputs,
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

        public static int P_ExpertsDelete(string ConnectionString, long SiteID, long ExpertsID, ref int ReturnValue, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_ExpertsDelete(ConnectionString, ref ds, SiteID, ExpertsID, ref ReturnValue, ref ReturnDescription);
        }

        public static int P_ExpertsDelete(string ConnectionString, ref DataSet ds, long SiteID, long ExpertsID, ref int ReturnValue, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(ConnectionString, "P_ExpertsDelete", ref ds, ref Outputs,
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

        public static int P_ExpertsEdit(string ConnectionString, long SiteID, long ExpertsID, string Description, double MaxPrice, double BonusScale, bool ON, bool isCommend, int ReadCount, ref int ReturnValue, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_ExpertsEdit(ConnectionString, ref ds, SiteID, ExpertsID, Description, MaxPrice, BonusScale, ON, isCommend, ReadCount, ref ReturnValue, ref ReturnDescription);
        }

        public static int P_ExpertsEdit(string ConnectionString, ref DataSet ds, long SiteID, long ExpertsID, string Description, double MaxPrice, double BonusScale, bool ON, bool isCommend, int ReadCount, ref int ReturnValue, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(ConnectionString, "P_ExpertsEdit", ref ds, ref Outputs,
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

        public static int P_ExpertsTry(string ConnectionString, long SiteID, long UserID, int LotteryID, string Description, double MaxPrice, double BonusScale, ref long NewExpertsTryID, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_ExpertsTry(ConnectionString, ref ds, SiteID, UserID, LotteryID, Description, MaxPrice, BonusScale, ref NewExpertsTryID, ref ReturnDescription);
        }

        public static int P_ExpertsTry(string ConnectionString, ref DataSet ds, long SiteID, long UserID, int LotteryID, string Description, double MaxPrice, double BonusScale, ref long NewExpertsTryID, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(ConnectionString, "P_ExpertsTry", ref ds, ref Outputs,
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

        public static int P_ExpertsTryHandle(string ConnectionString, long SiteID, long ExpertsTryID, short HandleResult, string Description, double MaxPrice, double BonusScale, bool isCommend, ref int ReturnValue, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_ExpertsTryHandle(ConnectionString, ref ds, SiteID, ExpertsTryID, HandleResult, Description, MaxPrice, BonusScale, isCommend, ref ReturnValue, ref ReturnDescription);
        }

        public static int P_ExpertsTryHandle(string ConnectionString, ref DataSet ds, long SiteID, long ExpertsTryID, short HandleResult, string Description, double MaxPrice, double BonusScale, bool isCommend, ref int ReturnValue, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(ConnectionString, "P_ExpertsTryHandle", ref ds, ref Outputs,
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

        public static int P_ExpertsWinCommendAdd(string ConnectionString, long SiteID, long UserID, DateTime DateTime, long IsuseID, string Title, string Content, bool isShow, ref long NewExpertsWinCommendID, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_ExpertsWinCommendAdd(ConnectionString, ref ds, SiteID, UserID, DateTime, IsuseID, Title, Content, isShow, ref NewExpertsWinCommendID, ref ReturnDescription);
        }

        public static int P_ExpertsWinCommendAdd(string ConnectionString, ref DataSet ds, long SiteID, long UserID, DateTime DateTime, long IsuseID, string Title, string Content, bool isShow, ref long NewExpertsWinCommendID, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(ConnectionString, "P_ExpertsWinCommendAdd", ref ds, ref Outputs,
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

        public static int P_ExpertsWinCommendDelete(string ConnectionString, long SiteID, long ExpertsWinCommendsID, ref int ReturnValue, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_ExpertsWinCommendDelete(ConnectionString, ref ds, SiteID, ExpertsWinCommendsID, ref ReturnValue, ref ReturnDescription);
        }

        public static int P_ExpertsWinCommendDelete(string ConnectionString, ref DataSet ds, long SiteID, long ExpertsWinCommendsID, ref int ReturnValue, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(ConnectionString, "P_ExpertsWinCommendDelete", ref ds, ref Outputs,
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

        public static int P_ExpertsWinCommendEdit(string ConnectionString, long SiteID, long ExpertsWinCommendsID, DateTime DateTime, long IsuseID, string Title, string Content, bool isShow, bool ON, bool isCommend, bool isAdmin, ref int ReturnValue, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_ExpertsWinCommendEdit(ConnectionString, ref ds, SiteID, ExpertsWinCommendsID, DateTime, IsuseID, Title, Content, isShow, ON, isCommend, isAdmin, ref ReturnValue, ref ReturnDescription);
        }

        public static int P_ExpertsWinCommendEdit(string ConnectionString, ref DataSet ds, long SiteID, long ExpertsWinCommendsID, DateTime DateTime, long IsuseID, string Title, string Content, bool isShow, bool ON, bool isCommend, bool isAdmin, ref int ReturnValue, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(ConnectionString, "P_ExpertsWinCommendEdit", ref ds, ref Outputs,
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

        public static int P_FootballLeagueTypeAdd(string ConnectionString, string Name, string Code, string MarkersColor, string Description, int Order, bool isUse, ref int FootballLeagueTypeID, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_FootballLeagueTypeAdd(ConnectionString, ref ds, Name, Code, MarkersColor, Description, Order, isUse, ref FootballLeagueTypeID, ref ReturnDescription);
        }

        public static int P_FootballLeagueTypeAdd(string ConnectionString, ref DataSet ds, string Name, string Code, string MarkersColor, string Description, int Order, bool isUse, ref int FootballLeagueTypeID, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(ConnectionString, "P_FootballLeagueTypeAdd", ref ds, ref Outputs,
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

        public static int P_FootballLeagueTypeEdit(string ConnectionString, int ID, string Name, string Code, string MarkersColor, string Description, int Order, bool isUse, ref int ReturnValue, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_FootballLeagueTypeEdit(ConnectionString, ref ds, ID, Name, Code, MarkersColor, Description, Order, isUse, ref ReturnValue, ref ReturnDescription);
        }

        public static int P_FootballLeagueTypeEdit(string ConnectionString, ref DataSet ds, int ID, string Name, string Code, string MarkersColor, string Description, int Order, bool isUse, ref int ReturnValue, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(ConnectionString, "P_FootballLeagueTypeEdit", ref ds, ref Outputs,
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

        public static int P_FriendshipLinkAdd(string ConnectionString, long SiteID, string LinkName, string LogoUrl, string Url, int Order, bool isShow, ref long NewFriendshipLinkID, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_FriendshipLinkAdd(ConnectionString, ref ds, SiteID, LinkName, LogoUrl, Url, Order, isShow, ref NewFriendshipLinkID, ref ReturnDescription);
        }

        public static int P_FriendshipLinkAdd(string ConnectionString, ref DataSet ds, long SiteID, string LinkName, string LogoUrl, string Url, int Order, bool isShow, ref long NewFriendshipLinkID, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(ConnectionString, "P_FriendshipLinkAdd", ref ds, ref Outputs,
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

        public static int P_FriendshipLinkDelete(string ConnectionString, long SiteID, long ID, ref int ReturnValue, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_FriendshipLinkDelete(ConnectionString, ref ds, SiteID, ID, ref ReturnValue, ref ReturnDescription);
        }

        public static int P_FriendshipLinkDelete(string ConnectionString, ref DataSet ds, long SiteID, long ID, ref int ReturnValue, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(ConnectionString, "P_FriendshipLinkDelete", ref ds, ref Outputs,
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

        public static int P_FriendshipLinkEdit(string ConnectionString, long SiteID, long ID, string LinkName, string LogoUrl, string Url, int Order, bool isShow, ref int ReturnValue, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_FriendshipLinkEdit(ConnectionString, ref ds, SiteID, ID, LinkName, LogoUrl, Url, Order, isShow, ref ReturnValue, ref ReturnDescription);
        }

        public static int P_FriendshipLinkEdit(string ConnectionString, ref DataSet ds, long SiteID, long ID, string LinkName, string LogoUrl, string Url, int Order, bool isShow, ref int ReturnValue, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(ConnectionString, "P_FriendshipLinkEdit", ref ds, ref Outputs,
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

        public static int P_GetAccount(string ConnectionString, long SiteID, int Year, int Month, ref int ReturnValue, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_GetAccount(ConnectionString, ref ds, SiteID, Year, Month, ref ReturnValue, ref ReturnDescription);
        }

        public static int P_GetAccount(string ConnectionString, ref DataSet ds, long SiteID, int Year, int Month, ref int ReturnValue, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(ConnectionString, "P_GetAccount", ref ds, ref Outputs,
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

        public static int P_GetCpsAccount(string ConnectionString, int Year, int Month, long CpsID, ref int ReturnValue, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_GetCpsAccount(ConnectionString, ref ds, Year, Month, CpsID, ref ReturnValue, ref ReturnDescription);
        }

        public static int P_GetCpsAccount(string ConnectionString, ref DataSet ds, int Year, int Month, long CpsID, ref int ReturnValue, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(ConnectionString, "P_GetCpsAccount", ref ds, ref Outputs,
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

        public static int P_GetCpsAccountByPid(string ConnectionString, long SiteID, long UserID, DateTime StartTime, DateTime EndTime, string pid, ref int ReturnValue, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_GetCpsAccountByPid(ConnectionString, ref ds, SiteID, UserID, StartTime, EndTime, pid, ref ReturnValue, ref ReturnDescription);
        }

        public static int P_GetCpsAccountByPid(string ConnectionString, ref DataSet ds, long SiteID, long UserID, DateTime StartTime, DateTime EndTime, string pid, ref int ReturnValue, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(ConnectionString, "P_GetCpsAccountByPid", ref ds, ref Outputs,
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

        public static int P_GetCpsAccountDetail(string ConnectionString, long SiteID, long UserID, int Year, int Month, ref int ReturnValue, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_GetCpsAccountDetail(ConnectionString, ref ds, SiteID, UserID, Year, Month, ref ReturnValue, ref ReturnDescription);
        }

        public static int P_GetCpsAccountDetail(string ConnectionString, ref DataSet ds, long SiteID, long UserID, int Year, int Month, ref int ReturnValue, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(ConnectionString, "P_GetCpsAccountDetail", ref ds, ref Outputs,
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

        public static int P_GetCpsAccountDetailWithUser(string ConnectionString, long CpsID, DateTime StartTime, DateTime EndTime, string KeyWord, ref int ReturnValue, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_GetCpsAccountDetailWithUser(ConnectionString, ref ds, CpsID, StartTime, EndTime, KeyWord, ref ReturnValue, ref ReturnDescription);
        }

        public static int P_GetCpsAccountDetailWithUser(string ConnectionString, ref DataSet ds, long CpsID, DateTime StartTime, DateTime EndTime, string KeyWord, ref int ReturnValue, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(ConnectionString, "P_GetCpsAccountDetailWithUser", ref ds, ref Outputs,
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

        public static int P_GetCpsAccountRevenue(string ConnectionString, long SiteID, long UserID, DateTime StartTime, DateTime EndTime, ref int ReturnValue, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_GetCpsAccountRevenue(ConnectionString, ref ds, SiteID, UserID, StartTime, EndTime, ref ReturnValue, ref ReturnDescription);
        }

        public static int P_GetCpsAccountRevenue(string ConnectionString, ref DataSet ds, long SiteID, long UserID, DateTime StartTime, DateTime EndTime, ref int ReturnValue, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(ConnectionString, "P_GetCpsAccountRevenue", ref ds, ref Outputs,
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

        public static int P_GetCpsAccountRevenue_center(string ConnectionString, long SiteID, long UserID, DateTime StartTime, DateTime EndTime, ref int ReturnValue, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_GetCpsAccountRevenue_center(ConnectionString, ref ds, SiteID, UserID, StartTime, EndTime, ref ReturnValue, ref ReturnDescription);
        }

        public static int P_GetCpsAccountRevenue_center(string ConnectionString, ref DataSet ds, long SiteID, long UserID, DateTime StartTime, DateTime EndTime, ref int ReturnValue, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(ConnectionString, "P_GetCpsAccountRevenue_center", ref ds, ref Outputs,
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

        public static int P_GetCpsAccountRevenueSite(string ConnectionString, long SiteID, long UserID, DateTime StartTime, DateTime EndTime, ref int ReturnValue, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_GetCpsAccountRevenueSite(ConnectionString, ref ds, SiteID, UserID, StartTime, EndTime, ref ReturnValue, ref ReturnDescription);
        }

        public static int P_GetCpsAccountRevenueSite(string ConnectionString, ref DataSet ds, long SiteID, long UserID, DateTime StartTime, DateTime EndTime, ref int ReturnValue, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(ConnectionString, "P_GetCpsAccountRevenueSite", ref ds, ref Outputs,
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

        public static int P_GetCpsAccountRevenueSite_center(string ConnectionString, long SiteID, long UserID, DateTime StartTime, DateTime EndTime, ref int ReturnValue, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_GetCpsAccountRevenueSite_center(ConnectionString, ref ds, SiteID, UserID, StartTime, EndTime, ref ReturnValue, ref ReturnDescription);
        }

        public static int P_GetCpsAccountRevenueSite_center(string ConnectionString, ref DataSet ds, long SiteID, long UserID, DateTime StartTime, DateTime EndTime, ref int ReturnValue, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(ConnectionString, "P_GetCpsAccountRevenueSite_center", ref ds, ref Outputs,
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

        public static int P_GetCpsAccountWithCpsID(string ConnectionString, DateTime StartTime, DateTime EndTime, long CpsID, ref int ReturnValue, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_GetCpsAccountWithCpsID(ConnectionString, ref ds, StartTime, EndTime, CpsID, ref ReturnValue, ref ReturnDescription);
        }

        public static int P_GetCpsAccountWithCpsID(string ConnectionString, ref DataSet ds, DateTime StartTime, DateTime EndTime, long CpsID, ref int ReturnValue, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(ConnectionString, "P_GetCpsAccountWithCpsID", ref ds, ref Outputs,
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

        public static int P_GetCpsAccountWithCpsUser(string ConnectionString, long CpsID, DateTime StartTime, DateTime EndTime, string KeyWord, ref int ReturnValue, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_GetCpsAccountWithCpsUser(ConnectionString, ref ds, CpsID, StartTime, EndTime, KeyWord, ref ReturnValue, ref ReturnDescription);
        }

        public static int P_GetCpsAccountWithCpsUser(string ConnectionString, ref DataSet ds, long CpsID, DateTime StartTime, DateTime EndTime, string KeyWord, ref int ReturnValue, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(ConnectionString, "P_GetCpsAccountWithCpsUser", ref ds, ref Outputs,
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

        public static int P_GetCpsAccountWithMonth(string ConnectionString, DateTime StartTime, DateTime EndTime, string UserName, long CpsID, ref int ReturnValue, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_GetCpsAccountWithMonth(ConnectionString, ref ds, StartTime, EndTime, UserName, CpsID, ref ReturnValue, ref ReturnDescription);
        }

        public static int P_GetCpsAccountWithMonth(string ConnectionString, ref DataSet ds, DateTime StartTime, DateTime EndTime, string UserName, long CpsID, ref int ReturnValue, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(ConnectionString, "P_GetCpsAccountWithMonth", ref ds, ref Outputs,
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

        public static int P_GetCpsBuyDetailByDate(string ConnectionString, long SiteID, long CpsID, DateTime FromTime, DateTime ToTime, ref int ReturnValue, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_GetCpsBuyDetailByDate(ConnectionString, ref ds, SiteID, CpsID, FromTime, ToTime, ref ReturnValue, ref ReturnDescription);
        }

        public static int P_GetCpsBuyDetailByDate(string ConnectionString, ref DataSet ds, long SiteID, long CpsID, DateTime FromTime, DateTime ToTime, ref int ReturnValue, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(ConnectionString, "P_GetCpsBuyDetailByDate", ref ds, ref Outputs,
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

        public static int P_GetCpsByDay(string ConnectionString, long CpsID, ref int Members, ref int SumMembers, ref double TodayIncome, ref int ReturnValue, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_GetCpsByDay(ConnectionString, ref ds, CpsID, ref Members, ref SumMembers, ref TodayIncome, ref ReturnValue, ref ReturnDescription);
        }

        public static int P_GetCpsByDay(string ConnectionString, ref DataSet ds, long CpsID, ref int Members, ref int SumMembers, ref double TodayIncome, ref int ReturnValue, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(ConnectionString, "P_GetCpsByDay", ref ds, ref Outputs,
                new MSSQL.Parameter("CpsID", SqlDbType.BigInt, 0, ParameterDirection.Input, CpsID),
                new MSSQL.Parameter("Members", SqlDbType.Int, 4, ParameterDirection.Output, Members),
                new MSSQL.Parameter("SumMembers", SqlDbType.Int, 4, ParameterDirection.Output, SumMembers),
                new MSSQL.Parameter("TodayIncome", SqlDbType.Money, 8, ParameterDirection.Output, TodayIncome),
                new MSSQL.Parameter("ReturnValue", SqlDbType.Int, 4, ParameterDirection.Output, ReturnValue),
                new MSSQL.Parameter("ReturnDescription", SqlDbType.VarChar, 100, ParameterDirection.Output, ReturnDescription)
                );

            try
            {
                Members = System.Convert.ToInt32(Outputs["Members"]);
            }
            catch { }

            try
            {
                SumMembers = System.Convert.ToInt32(Outputs["SumMembers"]);
            }
            catch { }

            try
            {
                TodayIncome = System.Convert.ToDouble(Outputs["TodayIncome"]);
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

        public static int P_GetCpsInformationByID(string ConnectionString, long SiteID, long CpsID, ref long OwnerUserID, ref string Name, ref DateTime Datetime, ref string Url, ref string LogoUrl, ref double BonusScale, ref bool ON, ref string Company, ref string Address, ref string PostCode, ref string ResponsiblePerson, ref string ContactPerson, ref string Telephone, ref string Fax, ref string Mobile, ref string Email, ref string QQ, ref string ServiceTelephone, ref string MD5Key, ref short Type, ref long ParentID, ref string DomainName, ref int ReturnValue, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_GetCpsInformationByID(ConnectionString, ref ds, SiteID, CpsID, ref OwnerUserID, ref Name, ref Datetime, ref Url, ref LogoUrl, ref BonusScale, ref ON, ref Company, ref Address, ref PostCode, ref ResponsiblePerson, ref ContactPerson, ref Telephone, ref Fax, ref Mobile, ref Email, ref QQ, ref ServiceTelephone, ref MD5Key, ref Type, ref ParentID, ref DomainName, ref ReturnValue, ref ReturnDescription);
        }

        public static int P_GetCpsInformationByID(string ConnectionString, ref DataSet ds, long SiteID, long CpsID, ref long OwnerUserID, ref string Name, ref DateTime Datetime, ref string Url, ref string LogoUrl, ref double BonusScale, ref bool ON, ref string Company, ref string Address, ref string PostCode, ref string ResponsiblePerson, ref string ContactPerson, ref string Telephone, ref string Fax, ref string Mobile, ref string Email, ref string QQ, ref string ServiceTelephone, ref string MD5Key, ref short Type, ref long ParentID, ref string DomainName, ref int ReturnValue, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(ConnectionString, "P_GetCpsInformationByID", ref ds, ref Outputs,
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

        public static int P_GetCpsInformationByOwnerUserID(string ConnectionString, long SiteID, long OwnerUserID, ref long CpsID, ref string Name, ref DateTime Datetime, ref string Url, ref string LogoUrl, ref double BonusScale, ref bool ON, ref string Company, ref string Address, ref string PostCode, ref string ResponsiblePerson, ref string ContactPerson, ref string Telephone, ref string Fax, ref string Mobile, ref string Email, ref string QQ, ref string ServiceTelephone, ref string MD5Key, ref short Type, ref long ParentID, ref string DomainName, ref int ReturnValue, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_GetCpsInformationByOwnerUserID(ConnectionString, ref ds, SiteID, OwnerUserID, ref CpsID, ref Name, ref Datetime, ref Url, ref LogoUrl, ref BonusScale, ref ON, ref Company, ref Address, ref PostCode, ref ResponsiblePerson, ref ContactPerson, ref Telephone, ref Fax, ref Mobile, ref Email, ref QQ, ref ServiceTelephone, ref MD5Key, ref Type, ref ParentID, ref DomainName, ref ReturnValue, ref ReturnDescription);
        }

        public static int P_GetCpsInformationByOwnerUserID(string ConnectionString, ref DataSet ds, long SiteID, long OwnerUserID, ref long CpsID, ref string Name, ref DateTime Datetime, ref string Url, ref string LogoUrl, ref double BonusScale, ref bool ON, ref string Company, ref string Address, ref string PostCode, ref string ResponsiblePerson, ref string ContactPerson, ref string Telephone, ref string Fax, ref string Mobile, ref string Email, ref string QQ, ref string ServiceTelephone, ref string MD5Key, ref short Type, ref long ParentID, ref string DomainName, ref int ReturnValue, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(ConnectionString, "P_GetCpsInformationByOwnerUserID", ref ds, ref Outputs,
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

        public static int P_GetCpsLinkList(string ConnectionString, long CpsID, DateTime StartTime, DateTime EndTime, ref int ReturnValue, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_GetCpsLinkList(ConnectionString, ref ds, CpsID, StartTime, EndTime, ref ReturnValue, ref ReturnDescription);
        }

        public static int P_GetCpsLinkList(string ConnectionString, ref DataSet ds, long CpsID, DateTime StartTime, DateTime EndTime, ref int ReturnValue, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(ConnectionString, "P_GetCpsLinkList", ref ds, ref Outputs,
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

        public static int P_GetCpsList(string ConnectionString, ref int ReturnValue, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_GetCpsList(ConnectionString, ref ds, ref ReturnValue, ref ReturnDescription);
        }

        public static int P_GetCpsList(string ConnectionString, ref DataSet ds, ref int ReturnValue, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(ConnectionString, "P_GetCpsList", ref ds, ref Outputs,
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

        public static int P_GetCpsMemberBuyDetail(string ConnectionString, long SiteID, long UserID, DateTime FromTime, DateTime ToTime, ref int ReturnValue, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_GetCpsMemberBuyDetail(ConnectionString, ref ds, SiteID, UserID, FromTime, ToTime, ref ReturnValue, ref ReturnDescription);
        }

        public static int P_GetCpsMemberBuyDetail(string ConnectionString, ref DataSet ds, long SiteID, long UserID, DateTime FromTime, DateTime ToTime, ref int ReturnValue, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(ConnectionString, "P_GetCpsMemberBuyDetail", ref ds, ref Outputs,
                new MSSQL.Parameter("SiteID", SqlDbType.BigInt, 0, ParameterDirection.Input, SiteID),
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

        public static int P_GetCpsMemberList(string ConnectionString, long CpsID, DateTime FromTime, DateTime ToTime, ref int ReturnValue, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_GetCpsMemberList(ConnectionString, ref ds, CpsID, FromTime, ToTime, ref ReturnValue, ref ReturnDescription);
        }

        public static int P_GetCpsMemberList(string ConnectionString, ref DataSet ds, long CpsID, DateTime FromTime, DateTime ToTime, ref int ReturnValue, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(ConnectionString, "P_GetCpsMemberList", ref ds, ref Outputs,
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

        public static int P_GetCpsPopularizeAccountRevenue(string ConnectionString, long SiteID, long UserID, DateTime StartTime, DateTime EndTime, int type, ref int ReturnValue, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_GetCpsPopularizeAccountRevenue(ConnectionString, ref ds, SiteID, UserID, StartTime, EndTime, type, ref ReturnValue, ref ReturnDescription);
        }

        public static int P_GetCpsPopularizeAccountRevenue(string ConnectionString, ref DataSet ds, long SiteID, long UserID, DateTime StartTime, DateTime EndTime, int type, ref int ReturnValue, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(ConnectionString, "P_GetCpsPopularizeAccountRevenue", ref ds, ref Outputs,
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

        public static int P_GetCpsUnionBusinessList(string ConnectionString, ref int ReturnValue, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_GetCpsUnionBusinessList(ConnectionString, ref ds, ref ReturnValue, ref ReturnDescription);
        }

        public static int P_GetCpsUnionBusinessList(string ConnectionString, ref DataSet ds, ref int ReturnValue, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(ConnectionString, "P_GetCpsUnionBusinessList", ref ds, ref Outputs,
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

        public static int P_GetCpsWebSiteList(string ConnectionString, ref int ReturnValue, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_GetCpsWebSiteList(ConnectionString, ref ds, ref ReturnValue, ref ReturnDescription);
        }

        public static int P_GetCpsWebSiteList(string ConnectionString, ref DataSet ds, ref int ReturnValue, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(ConnectionString, "P_GetCpsWebSiteList", ref ds, ref Outputs,
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

        public static int P_GetDistillMoneyAndAddMoney(string ConnectionString, long SiteID, DateTime FromDate, DateTime ToDate, int DistillType, ref int ReturnValue, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_GetDistillMoneyAndAddMoney(ConnectionString, ref ds, SiteID, FromDate, ToDate, DistillType, ref ReturnValue, ref ReturnDescription);
        }

        public static int P_GetDistillMoneyAndAddMoney(string ConnectionString, ref DataSet ds, long SiteID, DateTime FromDate, DateTime ToDate, int DistillType, ref int ReturnValue, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(ConnectionString, "P_GetDistillMoneyAndAddMoney", ref ds, ref Outputs,
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

        public static int P_GetDistillStatisticByAccount(string ConnectionString, long SiteID, DateTime FromDate, DateTime ToDate, string AccountName, ref int ReturnValue, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_GetDistillStatisticByAccount(ConnectionString, ref ds, SiteID, FromDate, ToDate, AccountName, ref ReturnValue, ref ReturnDescription);
        }

        public static int P_GetDistillStatisticByAccount(string ConnectionString, ref DataSet ds, long SiteID, DateTime FromDate, DateTime ToDate, string AccountName, ref int ReturnValue, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(ConnectionString, "P_GetDistillStatisticByAccount", ref ds, ref Outputs,
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

        public static int P_GetDistillStatisticByDistillType(string ConnectionString, long SiteID, DateTime FromDate, DateTime ToDate, ref int ReturnValue, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_GetDistillStatisticByDistillType(ConnectionString, ref ds, SiteID, FromDate, ToDate, ref ReturnValue, ref ReturnDescription);
        }

        public static int P_GetDistillStatisticByDistillType(string ConnectionString, ref DataSet ds, long SiteID, DateTime FromDate, DateTime ToDate, ref int ReturnValue, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(ConnectionString, "P_GetDistillStatisticByDistillType", ref ds, ref Outputs,
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

        public static int P_GetExpertsAccount(string ConnectionString, long SiteID, int Year, int Month, long ExpertsID, ref int ReturnValue, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_GetExpertsAccount(ConnectionString, ref ds, SiteID, Year, Month, ExpertsID, ref ReturnValue, ref ReturnDescription);
        }

        public static int P_GetExpertsAccount(string ConnectionString, ref DataSet ds, long SiteID, int Year, int Month, long ExpertsID, ref int ReturnValue, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(ConnectionString, "P_GetExpertsAccount", ref ds, ref Outputs,
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

        public static int P_GetExpertsAccountDetail(string ConnectionString, long SiteID, long UserID, int Year, int Month, ref int ReturnValue, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_GetExpertsAccountDetail(ConnectionString, ref ds, SiteID, UserID, Year, Month, ref ReturnValue, ref ReturnDescription);
        }

        public static int P_GetExpertsAccountDetail(string ConnectionString, ref DataSet ds, long SiteID, long UserID, int Year, int Month, ref int ReturnValue, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(ConnectionString, "P_GetExpertsAccountDetail", ref ds, ref Outputs,
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

        public static int P_GetExpertsGroupUserID(string ConnectionString, int ExpertsCount)
        {
            DataSet ds = null;

            return P_GetExpertsGroupUserID(ConnectionString, ref ds, ExpertsCount);
        }

        public static int P_GetExpertsGroupUserID(string ConnectionString, ref DataSet ds, int ExpertsCount)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(ConnectionString, "P_GetExpertsGroupUserID", ref ds, ref Outputs,
                new MSSQL.Parameter("ExpertsCount", SqlDbType.Int, 0, ParameterDirection.Input, ExpertsCount)
                );

            return CallResult;
        }

        public static int P_GetFinanceAddMoney(string ConnectionString, long SiteID, long UserID, int Year, int Month, ref int ReturnValue, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_GetFinanceAddMoney(ConnectionString, ref ds, SiteID, UserID, Year, Month, ref ReturnValue, ref ReturnDescription);
        }

        public static int P_GetFinanceAddMoney(string ConnectionString, ref DataSet ds, long SiteID, long UserID, int Year, int Month, ref int ReturnValue, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(ConnectionString, "P_GetFinanceAddMoney", ref ds, ref Outputs,
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

        public static int P_GetFriendsWinInfo(string ConnectionString, long UserID, string SnsName, ref int ReturnValue, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_GetFriendsWinInfo(ConnectionString, ref ds, UserID, SnsName, ref ReturnValue, ref ReturnDescription);
        }

        public static int P_GetFriendsWinInfo(string ConnectionString, ref DataSet ds, long UserID, string SnsName, ref int ReturnValue, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(ConnectionString, "P_GetFriendsWinInfo", ref ds, ref Outputs,
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

        public static int P_GetIsuseSalesAmount(string ConnectionString, long SiteID)
        {
            DataSet ds = null;

            return P_GetIsuseSalesAmount(ConnectionString, ref ds, SiteID);
        }

        public static int P_GetIsuseSalesAmount(string ConnectionString, ref DataSet ds, long SiteID)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(ConnectionString, "P_GetIsuseSalesAmount", ref ds, ref Outputs,
                new MSSQL.Parameter("SiteID", SqlDbType.BigInt, 0, ParameterDirection.Input, SiteID)
                );

            return CallResult;
        }

        public static int P_GetLoginCount(string ConnectionString, int Year, int Month)
        {
            DataSet ds = null;

            return P_GetLoginCount(ConnectionString, ref ds, Year, Month);
        }

        public static int P_GetLoginCount(string ConnectionString, ref DataSet ds, int Year, int Month)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(ConnectionString, "P_GetLoginCount", ref ds, ref Outputs,
                new MSSQL.Parameter("Year", SqlDbType.Int, 0, ParameterDirection.Input, Year),
                new MSSQL.Parameter("Month", SqlDbType.Int, 0, ParameterDirection.Input, Month)
                );

            return CallResult;
        }

        public static int P_GetNewPayNumber(string ConnectionString, long SiteID, long UserID, string PayType, double Money, double FormalitiesFees, ref long NewPayNumber, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_GetNewPayNumber(ConnectionString, ref ds, SiteID, UserID, PayType, Money, FormalitiesFees, ref NewPayNumber, ref ReturnDescription);
        }

        public static int P_GetNewPayNumber(string ConnectionString, ref DataSet ds, long SiteID, long UserID, string PayType, double Money, double FormalitiesFees, ref long NewPayNumber, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(ConnectionString, "P_GetNewPayNumber", ref ds, ref Outputs,
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

        public static int P_GetPromoterInfoByIDList(string ConnectionString, long SiteID, string UserIDList, ref int ReturnValue, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_GetPromoterInfoByIDList(ConnectionString, ref ds, SiteID, UserIDList, ref ReturnValue, ref ReturnDescription);
        }

        public static int P_GetPromoterInfoByIDList(string ConnectionString, ref DataSet ds, long SiteID, string UserIDList, ref int ReturnValue, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(ConnectionString, "P_GetPromoterInfoByIDList", ref ds, ref Outputs,
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

        public static int P_GetSchemeChatContents(string ConnectionString, long SiteID, long UserID, long SchemeID, ref int ReturnValue, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_GetSchemeChatContents(ConnectionString, ref ds, SiteID, UserID, SchemeID, ref ReturnValue, ref ReturnDescription);
        }

        public static int P_GetSchemeChatContents(string ConnectionString, ref DataSet ds, long SiteID, long UserID, long SchemeID, ref int ReturnValue, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(ConnectionString, "P_GetSchemeChatContents", ref ds, ref Outputs,
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

        public static int P_GetSedimentBalance(string ConnectionString, ref double SedimentBalance)
        {
            DataSet ds = null;

            return P_GetSedimentBalance(ConnectionString, ref ds, ref SedimentBalance);
        }

        public static int P_GetSedimentBalance(string ConnectionString, ref DataSet ds, ref double SedimentBalance)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(ConnectionString, "P_GetSedimentBalance", ref ds, ref Outputs,
                new MSSQL.Parameter("SedimentBalance", SqlDbType.Money, 8, ParameterDirection.Output, SedimentBalance)
                );

            try
            {
                SedimentBalance = System.Convert.ToDouble(Outputs["SedimentBalance"]);
            }
            catch { }

            return CallResult;
        }

        public static int P_GetSiteInformationByID(string ConnectionString, long SiteID, ref long ParentID, ref long OwnerUserID, ref string Name, ref string LogoUrl, ref string Company, ref string Address, ref string PostCode, ref string ResponsiblePerson, ref string ContactPerson, ref string Telephone, ref string Fax, ref string Mobile, ref string Email, ref string QQ, ref string ServiceTelephone, ref string ICPCert, ref short Level, ref bool ON, ref double BonusScale, ref int MaxSubSites, ref string UseLotteryListRestrictions, ref string UseLotteryList, ref string UseLotteryListQuickBuy, ref string Urls, ref long AdministratorID, ref string PageTitle, ref string PageKeywords, ref int ReturnValue, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_GetSiteInformationByID(ConnectionString, ref ds, SiteID, ref ParentID, ref OwnerUserID, ref Name, ref LogoUrl, ref Company, ref Address, ref PostCode, ref ResponsiblePerson, ref ContactPerson, ref Telephone, ref Fax, ref Mobile, ref Email, ref QQ, ref ServiceTelephone, ref ICPCert, ref Level, ref ON, ref BonusScale, ref MaxSubSites, ref UseLotteryListRestrictions, ref UseLotteryList, ref UseLotteryListQuickBuy, ref Urls, ref AdministratorID, ref PageTitle, ref PageKeywords, ref ReturnValue, ref ReturnDescription);
        }

        public static int P_GetSiteInformationByID(string ConnectionString, ref DataSet ds, long SiteID, ref long ParentID, ref long OwnerUserID, ref string Name, ref string LogoUrl, ref string Company, ref string Address, ref string PostCode, ref string ResponsiblePerson, ref string ContactPerson, ref string Telephone, ref string Fax, ref string Mobile, ref string Email, ref string QQ, ref string ServiceTelephone, ref string ICPCert, ref short Level, ref bool ON, ref double BonusScale, ref int MaxSubSites, ref string UseLotteryListRestrictions, ref string UseLotteryList, ref string UseLotteryListQuickBuy, ref string Urls, ref long AdministratorID, ref string PageTitle, ref string PageKeywords, ref int ReturnValue, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(ConnectionString, "P_GetSiteInformationByID", ref ds, ref Outputs,
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

        public static int P_GetSiteInformationByUrl(string ConnectionString, string Url, ref long SiteID, ref long ParentID, ref long OwnerUserID, ref string Name, ref string LogoUrl, ref string Company, ref string Address, ref string PostCode, ref string ResponsiblePerson, ref string ContactPerson, ref string Telephone, ref string Fax, ref string Mobile, ref string Email, ref string QQ, ref string ServiceTelephone, ref string ICPCert, ref short Level, ref bool ON, ref double BonusScale, ref int MaxSubSites, ref string UseLotteryListRestrictions, ref string UseLotteryList, ref string UseLotteryListQuickBuy, ref string Urls, ref long AdministratorID, ref string PageTitle, ref string PageKeywords, ref int ReturnValue, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_GetSiteInformationByUrl(ConnectionString, ref ds, Url, ref SiteID, ref ParentID, ref OwnerUserID, ref Name, ref LogoUrl, ref Company, ref Address, ref PostCode, ref ResponsiblePerson, ref ContactPerson, ref Telephone, ref Fax, ref Mobile, ref Email, ref QQ, ref ServiceTelephone, ref ICPCert, ref Level, ref ON, ref BonusScale, ref MaxSubSites, ref UseLotteryListRestrictions, ref UseLotteryList, ref UseLotteryListQuickBuy, ref Urls, ref AdministratorID, ref PageTitle, ref PageKeywords, ref ReturnValue, ref ReturnDescription);
        }

        public static int P_GetSiteInformationByUrl(string ConnectionString, ref DataSet ds, string Url, ref long SiteID, ref long ParentID, ref long OwnerUserID, ref string Name, ref string LogoUrl, ref string Company, ref string Address, ref string PostCode, ref string ResponsiblePerson, ref string ContactPerson, ref string Telephone, ref string Fax, ref string Mobile, ref string Email, ref string QQ, ref string ServiceTelephone, ref string ICPCert, ref short Level, ref bool ON, ref double BonusScale, ref int MaxSubSites, ref string UseLotteryListRestrictions, ref string UseLotteryList, ref string UseLotteryListQuickBuy, ref string Urls, ref long AdministratorID, ref string PageTitle, ref string PageKeywords, ref int ReturnValue, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(ConnectionString, "P_GetSiteInformationByUrl", ref ds, ref Outputs,
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

        public static int P_GetSiteNotificationTemplate(string ConnectionString, long SiteID, short Manner, string NotificationType, ref string Value)
        {
            DataSet ds = null;

            return P_GetSiteNotificationTemplate(ConnectionString, ref ds, SiteID, Manner, NotificationType, ref Value);
        }

        public static int P_GetSiteNotificationTemplate(string ConnectionString, ref DataSet ds, long SiteID, short Manner, string NotificationType, ref string Value)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(ConnectionString, "P_GetSiteNotificationTemplate", ref ds, ref Outputs,
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

        public static int P_GetSurrogateAccount(string ConnectionString, long SiteID, int Year, int Month, long SubSiteID, ref int ReturnValue, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_GetSurrogateAccount(ConnectionString, ref ds, SiteID, Year, Month, SubSiteID, ref ReturnValue, ref ReturnDescription);
        }

        public static int P_GetSurrogateAccount(string ConnectionString, ref DataSet ds, long SiteID, int Year, int Month, long SubSiteID, ref int ReturnValue, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(ConnectionString, "P_GetSurrogateAccount", ref ds, ref Outputs,
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

        public static int P_GetSurrogateSalesRanked(string ConnectionString, long SiteID, int RanksType, int Year, int Month, int Top, ref int ReturnValue, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_GetSurrogateSalesRanked(ConnectionString, ref ds, SiteID, RanksType, Year, Month, Top, ref ReturnValue, ref ReturnDescription);
        }

        public static int P_GetSurrogateSalesRanked(string ConnectionString, ref DataSet ds, long SiteID, int RanksType, int Year, int Month, int Top, ref int ReturnValue, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(ConnectionString, "P_GetSurrogateSalesRanked", ref ds, ref Outputs,
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

        public static int P_GetUserAccountDetail(string ConnectionString, long SiteID, long UserID, int Year, int Month, int Day, ref int ReturnValue, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_GetUserAccountDetail(ConnectionString, ref ds, SiteID, UserID, Year, Month, Day, ref ReturnValue, ref ReturnDescription);
        }

        public static int P_GetUserAccountDetail(string ConnectionString, ref DataSet ds, long SiteID, long UserID, int Year, int Month, int Day, ref int ReturnValue, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(ConnectionString, "P_GetUserAccountDetail", ref ds, ref Outputs,
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

        public static int P_GetUserAccountDetails(string ConnectionString, long SiteID, long UserID, DateTime StartTime, DateTime EndTime, ref int ReturnValue, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_GetUserAccountDetails(ConnectionString, ref ds, SiteID, UserID, StartTime, EndTime, ref ReturnValue, ref ReturnDescription);
        }

        public static int P_GetUserAccountDetails(string ConnectionString, ref DataSet ds, long SiteID, long UserID, DateTime StartTime, DateTime EndTime, ref int ReturnValue, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(ConnectionString, "P_GetUserAccountDetails", ref ds, ref Outputs,
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

        public static int P_GetUserBankDetail(string ConnectionString, long SiteID, long UserID, ref string BankTypeName, ref string BankName, ref string BankCardNumber, ref string BankInProvinceName, ref string BankInCityName, ref string BankUserName, ref int ReturnValue, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_GetUserBankDetail(ConnectionString, ref ds, SiteID, UserID, ref BankTypeName, ref BankName, ref BankCardNumber, ref BankInProvinceName, ref BankInCityName, ref BankUserName, ref ReturnValue, ref ReturnDescription);
        }

        public static int P_GetUserBankDetail(string ConnectionString, ref DataSet ds, long SiteID, long UserID, ref string BankTypeName, ref string BankName, ref string BankCardNumber, ref string BankInProvinceName, ref string BankInCityName, ref string BankUserName, ref int ReturnValue, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(ConnectionString, "P_GetUserBankDetail", ref ds, ref Outputs,
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

        public static int P_GetUserCustomFollowSchemes(string ConnectionString, long SiteID, long UserID, int PlayTypeID, ref int ReturnValue, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_GetUserCustomFollowSchemes(ConnectionString, ref ds, SiteID, UserID, PlayTypeID, ref ReturnValue, ref ReturnDescription);
        }

        public static int P_GetUserCustomFollowSchemes(string ConnectionString, ref DataSet ds, long SiteID, long UserID, int PlayTypeID, ref int ReturnValue, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(ConnectionString, "P_GetUserCustomFollowSchemes", ref ds, ref Outputs,
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

        public static int P_GetUserFreezeDetail(string ConnectionString, long SiteID, long UserID, ref int ReturnValue, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_GetUserFreezeDetail(ConnectionString, ref ds, SiteID, UserID, ref ReturnValue, ref ReturnDescription);
        }

        public static int P_GetUserFreezeDetail(string ConnectionString, ref DataSet ds, long SiteID, long UserID, ref int ReturnValue, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(ConnectionString, "P_GetUserFreezeDetail", ref ds, ref Outputs,
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

        public static int P_GetUserInformationByID(string ConnectionString, long UserID, long SiteID, ref string Name, ref string NickName, ref string RealityName, ref string Password, ref string PasswordAdv, ref int CityID, ref string Sex, ref DateTime BirthDay, ref string IDCardNumber, ref string Address, ref string Email, ref bool isEmailValided, ref string QQ, ref string Telephone, ref string Mobile, ref bool isMobileValided, ref bool isPrivacy, ref bool isCanLogin, ref DateTime RegisterTime, ref DateTime LastLoginTime, ref string LastLoginIP, ref int LoginCount, ref short UserType, ref short BankType, ref string BankName, ref string BankCardNumber, ref double Balance, ref double Freeze, ref double ScoringOfSelfBuy, ref double ScoringOfCommendBuy, ref double Scoring, ref short Level, ref long CommenderID, ref long CpsID, ref string OwnerSites, ref string AlipayID, ref double Bonus, ref double Reward, ref string AlipayName, ref bool isAlipayNameValided, ref int ComeFrom, ref bool IsCrossLogin, ref string Memo, ref double BonusThisMonth, ref double BonusAllow, ref double BonusUse, ref double PromotionMemberBonusScale, ref double PromotionSiteBonusScale, ref string VisitSource, ref string HeadUrl, ref string SecurityQuestion, ref string SecurityAnswer, ref string FriendList, ref int ReturnValue, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_GetUserInformationByID(ConnectionString, ref ds, UserID, SiteID, ref Name, ref NickName, ref RealityName, ref Password, ref PasswordAdv, ref CityID, ref Sex, ref BirthDay, ref IDCardNumber, ref Address, ref Email, ref isEmailValided, ref QQ, ref Telephone, ref Mobile, ref isMobileValided, ref isPrivacy, ref isCanLogin, ref RegisterTime, ref LastLoginTime, ref LastLoginIP, ref LoginCount, ref UserType, ref BankType, ref BankName, ref BankCardNumber, ref Balance, ref Freeze, ref ScoringOfSelfBuy, ref ScoringOfCommendBuy, ref Scoring, ref Level, ref CommenderID, ref CpsID, ref OwnerSites, ref AlipayID, ref Bonus, ref Reward, ref AlipayName, ref isAlipayNameValided, ref ComeFrom, ref IsCrossLogin, ref Memo, ref BonusThisMonth, ref BonusAllow, ref BonusUse, ref PromotionMemberBonusScale, ref PromotionSiteBonusScale, ref VisitSource, ref HeadUrl, ref SecurityQuestion, ref SecurityAnswer, ref FriendList, ref ReturnValue, ref ReturnDescription);
        }

        public static int P_GetUserInformationByID(string ConnectionString, ref DataSet ds, long UserID, long SiteID, ref string Name, ref string NickName, ref string RealityName, ref string Password, ref string PasswordAdv, ref int CityID, ref string Sex, ref DateTime BirthDay, ref string IDCardNumber, ref string Address, ref string Email, ref bool isEmailValided, ref string QQ, ref string Telephone, ref string Mobile, ref bool isMobileValided, ref bool isPrivacy, ref bool isCanLogin, ref DateTime RegisterTime, ref DateTime LastLoginTime, ref string LastLoginIP, ref int LoginCount, ref short UserType, ref short BankType, ref string BankName, ref string BankCardNumber, ref double Balance, ref double Freeze, ref double ScoringOfSelfBuy, ref double ScoringOfCommendBuy, ref double Scoring, ref short Level, ref long CommenderID, ref long CpsID, ref string OwnerSites, ref string AlipayID, ref double Bonus, ref double Reward, ref string AlipayName, ref bool isAlipayNameValided, ref int ComeFrom, ref bool IsCrossLogin, ref string Memo, ref double BonusThisMonth, ref double BonusAllow, ref double BonusUse, ref double PromotionMemberBonusScale, ref double PromotionSiteBonusScale, ref string VisitSource, ref string HeadUrl, ref string SecurityQuestion, ref string SecurityAnswer, ref string FriendList, ref int ReturnValue, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(ConnectionString, "P_GetUserInformationByID", ref ds, ref Outputs,
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
                new MSSQL.Parameter("Telephone", SqlDbType.VarChar, 50, ParameterDirection.Output, Telephone),
                new MSSQL.Parameter("Mobile", SqlDbType.VarChar, 50, ParameterDirection.Output, Mobile),
                new MSSQL.Parameter("isMobileValided", SqlDbType.Bit, 1, ParameterDirection.Output, isMobileValided),
                new MSSQL.Parameter("isPrivacy", SqlDbType.Bit, 1, ParameterDirection.Output, isPrivacy),
                new MSSQL.Parameter("isCanLogin", SqlDbType.Bit, 1, ParameterDirection.Output, isCanLogin),
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

        public static int P_GetUserInformationByName(string ConnectionString, string Name, long SiteID, ref long UserID, ref string NickName, ref string RealityName, ref string Password, ref string PasswordAdv, ref int CityID, ref string Sex, ref DateTime BirthDay, ref string IDCardNumber, ref string Address, ref string Email, ref bool isEmailValided, ref string QQ, ref string Telephone, ref string Mobile, ref bool isMobileValided, ref bool isPrivacy, ref bool isCanLogin, ref DateTime RegisterTime, ref DateTime LastLoginTime, ref string LastLoginIP, ref int LoginCount, ref short UserType, ref short BankType, ref string BankName, ref string BankCardNumber, ref double Balance, ref double Freeze, ref double ScoringOfSelfBuy, ref double ScoringOfCommendBuy, ref double Scoring, ref short Level, ref long CommenderID, ref long CpsID, ref string OwnerSites, ref string AlipayID, ref double Bonus, ref double Reward, ref string AlipayName, ref bool isAlipayNameValided, ref int ComeFrom, ref bool IsCrossLogin, ref string Memo, ref double BonusThisMonth, ref double BonusAllow, ref double BonusUse, ref double PromotionMemberBonusScale, ref double PromotionSiteBonusScale, ref string VisitSource, ref string HeadUrl, ref string SecurityQuestion, ref string SecurityAnswer, ref string FriendList, ref int ReturnValue, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_GetUserInformationByName(ConnectionString, ref ds, Name, SiteID, ref UserID, ref NickName, ref RealityName, ref Password, ref PasswordAdv, ref CityID, ref Sex, ref BirthDay, ref IDCardNumber, ref Address, ref Email, ref isEmailValided, ref QQ, ref Telephone, ref Mobile, ref isMobileValided, ref isPrivacy, ref isCanLogin, ref RegisterTime, ref LastLoginTime, ref LastLoginIP, ref LoginCount, ref UserType, ref BankType, ref BankName, ref BankCardNumber, ref Balance, ref Freeze, ref ScoringOfSelfBuy, ref ScoringOfCommendBuy, ref Scoring, ref Level, ref CommenderID, ref CpsID, ref OwnerSites, ref AlipayID, ref Bonus, ref Reward, ref AlipayName, ref isAlipayNameValided, ref ComeFrom, ref IsCrossLogin, ref Memo, ref BonusThisMonth, ref BonusAllow, ref BonusUse, ref PromotionMemberBonusScale, ref PromotionSiteBonusScale, ref VisitSource, ref HeadUrl, ref SecurityQuestion, ref SecurityAnswer, ref FriendList, ref ReturnValue, ref ReturnDescription);
        }

        public static int P_GetUserInformationByName(string ConnectionString, ref DataSet ds, string Name, long SiteID, ref long UserID, ref string NickName, ref string RealityName, ref string Password, ref string PasswordAdv, ref int CityID, ref string Sex, ref DateTime BirthDay, ref string IDCardNumber, ref string Address, ref string Email, ref bool isEmailValided, ref string QQ, ref string Telephone, ref string Mobile, ref bool isMobileValided, ref bool isPrivacy, ref bool isCanLogin, ref DateTime RegisterTime, ref DateTime LastLoginTime, ref string LastLoginIP, ref int LoginCount, ref short UserType, ref short BankType, ref string BankName, ref string BankCardNumber, ref double Balance, ref double Freeze, ref double ScoringOfSelfBuy, ref double ScoringOfCommendBuy, ref double Scoring, ref short Level, ref long CommenderID, ref long CpsID, ref string OwnerSites, ref string AlipayID, ref double Bonus, ref double Reward, ref string AlipayName, ref bool isAlipayNameValided, ref int ComeFrom, ref bool IsCrossLogin, ref string Memo, ref double BonusThisMonth, ref double BonusAllow, ref double BonusUse, ref double PromotionMemberBonusScale, ref double PromotionSiteBonusScale, ref string VisitSource, ref string HeadUrl, ref string SecurityQuestion, ref string SecurityAnswer, ref string FriendList, ref int ReturnValue, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(ConnectionString, "P_GetUserInformationByName", ref ds, ref Outputs,
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
                new MSSQL.Parameter("Telephone", SqlDbType.VarChar, 50, ParameterDirection.Output, Telephone),
                new MSSQL.Parameter("Mobile", SqlDbType.VarChar, 50, ParameterDirection.Output, Mobile),
                new MSSQL.Parameter("isMobileValided", SqlDbType.Bit, 1, ParameterDirection.Output, isMobileValided),
                new MSSQL.Parameter("isPrivacy", SqlDbType.Bit, 1, ParameterDirection.Output, isPrivacy),
                new MSSQL.Parameter("isCanLogin", SqlDbType.Bit, 1, ParameterDirection.Output, isCanLogin),
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

        public static int P_GetUserIsAwardwinning(string ConnectionString, string AlipayName, ref int ReturnValue, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_GetUserIsAwardwinning(ConnectionString, ref ds, AlipayName, ref ReturnValue, ref ReturnDescription);
        }

        public static int P_GetUserIsAwardwinning(string ConnectionString, ref DataSet ds, string AlipayName, ref int ReturnValue, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(ConnectionString, "P_GetUserIsAwardwinning", ref ds, ref Outputs,
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

        public static int P_GetUserScoringDetail(string ConnectionString, long SiteID, long UserID, int Year, int Month, ref int ReturnValue, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_GetUserScoringDetail(ConnectionString, ref ds, SiteID, UserID, Year, Month, ref ReturnValue, ref ReturnDescription);
        }

        public static int P_GetUserScoringDetail(string ConnectionString, ref DataSet ds, long SiteID, long UserID, int Year, int Month, ref int ReturnValue, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(ConnectionString, "P_GetUserScoringDetail", ref ds, ref Outputs,
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

        public static int P_GetUserSMSDetail(string ConnectionString, long SiteID, long UserID, int Year, int Month, ref int ReturnValue, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_GetUserSMSDetail(ConnectionString, ref ds, SiteID, UserID, Year, Month, ref ReturnValue, ref ReturnDescription);
        }

        public static int P_GetUserSMSDetail(string ConnectionString, ref DataSet ds, long SiteID, long UserID, int Year, int Month, ref int ReturnValue, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(ConnectionString, "P_GetUserSMSDetail", ref ds, ref Outputs,
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

        public static int P_GetWinLotteryNumber(string ConnectionString, long SiteID, int LotteryID, int IsuseCount, ref int ReturnValue, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_GetWinLotteryNumber(ConnectionString, ref ds, SiteID, LotteryID, IsuseCount, ref ReturnValue, ref ReturnDescription);
        }

        public static int P_GetWinLotteryNumber(string ConnectionString, ref DataSet ds, long SiteID, int LotteryID, int IsuseCount, ref int ReturnValue, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(ConnectionString, "P_GetWinLotteryNumber", ref ds, ref Outputs,
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

        public static int P_InitializationData(string ConnectionString, string CallPassword, ref int ReturnValue, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_InitializationData(ConnectionString, ref ds, CallPassword, ref ReturnValue, ref ReturnDescription);
        }

        public static int P_InitializationData(string ConnectionString, ref DataSet ds, string CallPassword, ref int ReturnValue, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(ConnectionString, "P_InitializationData", ref ds, ref Outputs,
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

        public static int P_InitializationSiteTemplates(string ConnectionString, long SiteID, ref int ReturnValue, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_InitializationSiteTemplates(ConnectionString, ref ds, SiteID, ref ReturnValue, ref ReturnDescription);
        }

        public static int P_InitializationSiteTemplates(string ConnectionString, ref DataSet ds, long SiteID, ref int ReturnValue, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(ConnectionString, "P_InitializationSiteTemplates", ref ds, ref Outputs,
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

        public static int P_InitiateChaseTask(string ConnectionString, long SiteID, long UserID, string Title, string Description, int LotteryID, double StopWhenWinMoney, string DetailXML, string LotteryNumber, double SchemeBonusScale, ref long ChaseTaskID, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_InitiateChaseTask(ConnectionString, ref ds, SiteID, UserID, Title, Description, LotteryID, StopWhenWinMoney, DetailXML, LotteryNumber, SchemeBonusScale, ref ChaseTaskID, ref ReturnDescription);
        }

        public static int P_InitiateChaseTask(string ConnectionString, ref DataSet ds, long SiteID, long UserID, string Title, string Description, int LotteryID, double StopWhenWinMoney, string DetailXML, string LotteryNumber, double SchemeBonusScale, ref long ChaseTaskID, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(ConnectionString, "P_InitiateChaseTask", ref ds, ref Outputs,
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

        public static int P_InitiateScheme(string ConnectionString, long SiteID, long UserID, long IsuseID, int PlayTypeID, string Title, string Description, string LotteryNumber, string UploadFileContent, int Multiple, double Money, double AssureMoney, int Share, int BuyShare, string OpenUsers, short SecrecyLevel, double SchemeBonusScale, ref long SchemeID, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_InitiateScheme(ConnectionString, ref ds, SiteID, UserID, IsuseID, PlayTypeID, Title, Description, LotteryNumber, UploadFileContent, Multiple, Money, AssureMoney, Share, BuyShare, OpenUsers, SecrecyLevel, SchemeBonusScale, ref SchemeID, ref ReturnDescription);
        }

        public static int P_InitiateScheme(string ConnectionString, ref DataSet ds, long SiteID, long UserID, long IsuseID, int PlayTypeID, string Title, string Description, string LotteryNumber, string UploadFileContent, int Multiple, double Money, double AssureMoney, int Share, int BuyShare, string OpenUsers, short SecrecyLevel, double SchemeBonusScale, ref long SchemeID, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(ConnectionString, "P_InitiateScheme", ref ds, ref Outputs,
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

        public static int P_InquirySchemesHandle(string ConnectionString, string CounterAnteId, string DealTime, short HandleResult, string HandleDescription, short PrintOutType, ref int ReturnValue, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_InquirySchemesHandle(ConnectionString, ref ds, CounterAnteId, DealTime, HandleResult, HandleDescription, PrintOutType, ref ReturnValue, ref ReturnDescription);
        }

        public static int P_InquirySchemesHandle(string ConnectionString, ref DataSet ds, string CounterAnteId, string DealTime, short HandleResult, string HandleDescription, short PrintOutType, ref int ReturnValue, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(ConnectionString, "P_InquirySchemesHandle", ref ds, ref Outputs,
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

        public static int P_IsuseAdd(string ConnectionString, int LotteryID, string Name, DateTime StartTime, DateTime EndTime, string AdditionalXML, ref long IsuseID, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_IsuseAdd(ConnectionString, ref ds, LotteryID, Name, StartTime, EndTime, AdditionalXML, ref IsuseID, ref ReturnDescription);
        }

        public static int P_IsuseAdd(string ConnectionString, ref DataSet ds, int LotteryID, string Name, DateTime StartTime, DateTime EndTime, string AdditionalXML, ref long IsuseID, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(ConnectionString, "P_IsuseAdd", ref ds, ref Outputs,
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

        public static int P_IsuseBonusesAdd(string ConnectionString, long IsuseId, long UserID, string WinListXML, ref int ReturnValue, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_IsuseBonusesAdd(ConnectionString, ref ds, IsuseId, UserID, WinListXML, ref ReturnValue, ref ReturnDescription);
        }

        public static int P_IsuseBonusesAdd(string ConnectionString, ref DataSet ds, long IsuseId, long UserID, string WinListXML, ref int ReturnValue, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(ConnectionString, "P_IsuseBonusesAdd", ref ds, ref Outputs,
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

        public static int P_IsuseEdit(string ConnectionString, long IsuseID, string Name, DateTime StartTime, DateTime EndTime, string AdditionalXML, ref int ReturnValue, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_IsuseEdit(ConnectionString, ref ds, IsuseID, Name, StartTime, EndTime, AdditionalXML, ref ReturnValue, ref ReturnDescription);
        }

        public static int P_IsuseEdit(string ConnectionString, ref DataSet ds, long IsuseID, string Name, DateTime StartTime, DateTime EndTime, string AdditionalXML, ref int ReturnValue, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(ConnectionString, "P_IsuseEdit", ref ds, ref Outputs,
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

        public static int P_IsuseUpdate(string ConnectionString, int LotteryID, string IsuseName, short State, DateTime StartTime, DateTime EndTime, DateTime StateUpdateTime, string WinLotteryNumber, ref int ReturnValue, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_IsuseUpdate(ConnectionString, ref ds, LotteryID, IsuseName, State, StartTime, EndTime, StateUpdateTime, WinLotteryNumber, ref ReturnValue, ref ReturnDescription);
        }

        public static int P_IsuseUpdate(string ConnectionString, ref DataSet ds, int LotteryID, string IsuseName, short State, DateTime StartTime, DateTime EndTime, DateTime StateUpdateTime, string WinLotteryNumber, ref int ReturnValue, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(ConnectionString, "P_IsuseUpdate", ref ds, ref Outputs,
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

        public static int P_JoinScheme(string ConnectionString, long SiteID, long UserID, long SchemeID, int Share, bool isAutoFollowScheme, ref int ReturnValue, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_JoinScheme(ConnectionString, ref ds, SiteID, UserID, SchemeID, Share, isAutoFollowScheme, ref ReturnValue, ref ReturnDescription);
        }

        public static int P_JoinScheme(string ConnectionString, ref DataSet ds, long SiteID, long UserID, long SchemeID, int Share, bool isAutoFollowScheme, ref int ReturnValue, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(ConnectionString, "P_JoinScheme", ref ds, ref Outputs,
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

        public static int P_LeaveSchemeChatRoom(string ConnectionString, long SiteID, long UserID, long SchemeID, ref int ReturnValue, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_LeaveSchemeChatRoom(ConnectionString, ref ds, SiteID, UserID, SchemeID, ref ReturnValue, ref ReturnDescription);
        }

        public static int P_LeaveSchemeChatRoom(string ConnectionString, ref DataSet ds, long SiteID, long UserID, long SchemeID, ref int ReturnValue, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(ConnectionString, "P_LeaveSchemeChatRoom", ref ds, ref Outputs,
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

        public static int P_Login(string ConnectionString, long SiteID, string Name, string InputPassword, string LoginIP, ref long UserID, ref string PasswordAdv, ref string RealityName, ref int CityID, ref string Sex, ref DateTime BirthDay, ref string IDCardNumber, ref string Address, ref string Email, ref bool isEmailValided, ref string QQ, ref string Telephone, ref string Mobile, ref bool isMobileValided, ref bool isPrivacy, ref bool isCanLogin, ref DateTime RegisterTime, ref DateTime LastLoginTime, ref string LastLoginIP, ref int LoginCount, ref short UserType, ref short BankType, ref string BankName, ref string BankCardNumber, ref double Balance, ref double Freeze, ref double ScoringOfSelfBuy, ref double ScoringOfCommendBuy, ref double Scoring, ref short Level, ref long CommenderID, ref long CpsID, ref string AlipayID, ref string AlipayName, ref bool isAlipayNameValided, ref double Bonus, ref double Reward, ref string Memo, ref double BonusThisMonth, ref double BonusAllow, ref double BonusUse, ref double PromotionMemberBonusScale, ref double PromotionSiteBonusScale, ref int ReturnValue, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_Login(ConnectionString, ref ds, SiteID, Name, InputPassword, LoginIP, ref UserID, ref PasswordAdv, ref RealityName, ref CityID, ref Sex, ref BirthDay, ref IDCardNumber, ref Address, ref Email, ref isEmailValided, ref QQ, ref Telephone, ref Mobile, ref isMobileValided, ref isPrivacy, ref isCanLogin, ref RegisterTime, ref LastLoginTime, ref LastLoginIP, ref LoginCount, ref UserType, ref BankType, ref BankName, ref BankCardNumber, ref Balance, ref Freeze, ref ScoringOfSelfBuy, ref ScoringOfCommendBuy, ref Scoring, ref Level, ref CommenderID, ref CpsID, ref AlipayID, ref AlipayName, ref isAlipayNameValided, ref Bonus, ref Reward, ref Memo, ref BonusThisMonth, ref BonusAllow, ref BonusUse, ref PromotionMemberBonusScale, ref PromotionSiteBonusScale, ref ReturnValue, ref ReturnDescription);
        }

        public static int P_Login(string ConnectionString, ref DataSet ds, long SiteID, string Name, string InputPassword, string LoginIP, ref long UserID, ref string PasswordAdv, ref string RealityName, ref int CityID, ref string Sex, ref DateTime BirthDay, ref string IDCardNumber, ref string Address, ref string Email, ref bool isEmailValided, ref string QQ, ref string Telephone, ref string Mobile, ref bool isMobileValided, ref bool isPrivacy, ref bool isCanLogin, ref DateTime RegisterTime, ref DateTime LastLoginTime, ref string LastLoginIP, ref int LoginCount, ref short UserType, ref short BankType, ref string BankName, ref string BankCardNumber, ref double Balance, ref double Freeze, ref double ScoringOfSelfBuy, ref double ScoringOfCommendBuy, ref double Scoring, ref short Level, ref long CommenderID, ref long CpsID, ref string AlipayID, ref string AlipayName, ref bool isAlipayNameValided, ref double Bonus, ref double Reward, ref string Memo, ref double BonusThisMonth, ref double BonusAllow, ref double BonusUse, ref double PromotionMemberBonusScale, ref double PromotionSiteBonusScale, ref int ReturnValue, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(ConnectionString, "P_Login", ref ds, ref Outputs,
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
                new MSSQL.Parameter("Telephone", SqlDbType.VarChar, 50, ParameterDirection.Output, Telephone),
                new MSSQL.Parameter("Mobile", SqlDbType.VarChar, 50, ParameterDirection.Output, Mobile),
                new MSSQL.Parameter("isMobileValided", SqlDbType.Bit, 1, ParameterDirection.Output, isMobileValided),
                new MSSQL.Parameter("isPrivacy", SqlDbType.Bit, 1, ParameterDirection.Output, isPrivacy),
                new MSSQL.Parameter("isCanLogin", SqlDbType.Bit, 1, ParameterDirection.Output, isCanLogin),
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

        public static int P_LotteryToolLinkAdd(string ConnectionString, long SiteID, int LotteryID, string LinkName, string LogoUrl, string Url, int Order, bool isShow, ref long NewLotteryToolLinkID, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_LotteryToolLinkAdd(ConnectionString, ref ds, SiteID, LotteryID, LinkName, LogoUrl, Url, Order, isShow, ref NewLotteryToolLinkID, ref ReturnDescription);
        }

        public static int P_LotteryToolLinkAdd(string ConnectionString, ref DataSet ds, long SiteID, int LotteryID, string LinkName, string LogoUrl, string Url, int Order, bool isShow, ref long NewLotteryToolLinkID, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(ConnectionString, "P_LotteryToolLinkAdd", ref ds, ref Outputs,
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

        public static int P_LotteryToolLinkDelete(string ConnectionString, long SiteID, long ID, ref int ReturnValue, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_LotteryToolLinkDelete(ConnectionString, ref ds, SiteID, ID, ref ReturnValue, ref ReturnDescription);
        }

        public static int P_LotteryToolLinkDelete(string ConnectionString, ref DataSet ds, long SiteID, long ID, ref int ReturnValue, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(ConnectionString, "P_LotteryToolLinkDelete", ref ds, ref Outputs,
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

        public static int P_LotteryToolLinkEdit(string ConnectionString, long SiteID, long ID, int LotteryID, string LinkName, string LogoUrl, string Url, int Order, bool isShow, ref int ReturnValue, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_LotteryToolLinkEdit(ConnectionString, ref ds, SiteID, ID, LotteryID, LinkName, LogoUrl, Url, Order, isShow, ref ReturnValue, ref ReturnDescription);
        }

        public static int P_LotteryToolLinkEdit(string ConnectionString, ref DataSet ds, long SiteID, long ID, int LotteryID, string LinkName, string LogoUrl, string Url, int Order, bool isShow, ref int ReturnValue, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(ConnectionString, "P_LotteryToolLinkEdit", ref ds, ref Outputs,
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

        public static int P_MarketOutlookAdd(string ConnectionString, DateTime DateTime, string Title, string Content, bool isShow, ref long NewMarketOutlookID, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_MarketOutlookAdd(ConnectionString, ref ds, DateTime, Title, Content, isShow, ref NewMarketOutlookID, ref ReturnDescription);
        }

        public static int P_MarketOutlookAdd(string ConnectionString, ref DataSet ds, DateTime DateTime, string Title, string Content, bool isShow, ref long NewMarketOutlookID, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(ConnectionString, "P_MarketOutlookAdd", ref ds, ref Outputs,
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

        public static int P_MarketOutlookDelete(string ConnectionString, long ID, ref int ReturnValue, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_MarketOutlookDelete(ConnectionString, ref ds, ID, ref ReturnValue, ref ReturnDescription);
        }

        public static int P_MarketOutlookDelete(string ConnectionString, ref DataSet ds, long ID, ref int ReturnValue, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(ConnectionString, "P_MarketOutlookDelete", ref ds, ref Outputs,
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

        public static int P_MarketOutlookEdit(string ConnectionString, long ID, DateTime DateTime, string Title, string Content, bool isShow, ref int ReturnValue, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_MarketOutlookEdit(ConnectionString, ref ds, ID, DateTime, Title, Content, isShow, ref ReturnValue, ref ReturnDescription);
        }

        public static int P_MarketOutlookEdit(string ConnectionString, ref DataSet ds, long ID, DateTime DateTime, string Title, string Content, bool isShow, ref int ReturnValue, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(ConnectionString, "P_MarketOutlookEdit", ref ds, ref Outputs,
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

        public static int P_MergeUserDetails(string ConnectionString, string CallPassword, ref int ReturnValue, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_MergeUserDetails(ConnectionString, ref ds, CallPassword, ref ReturnValue, ref ReturnDescription);
        }

        public static int P_MergeUserDetails(string ConnectionString, ref DataSet ds, string CallPassword, ref int ReturnValue, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(ConnectionString, "P_MergeUserDetails", ref ds, ref Outputs,
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

        public static int P_NewsAdd(string ConnectionString, long SiteID, int TypeID, DateTime DateTime, string Title, string Content, string ImageUrl, bool isShow, bool isHasImage, bool isCanComments, bool isCommend, bool isHot, long ReadCount, int IsusesId, ref long NewsID, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_NewsAdd(ConnectionString, ref ds, SiteID, TypeID, DateTime, Title, Content, ImageUrl, isShow, isHasImage, isCanComments, isCommend, isHot, ReadCount, IsusesId, ref NewsID, ref ReturnDescription);
        }

        public static int P_NewsAdd(string ConnectionString, ref DataSet ds, long SiteID, int TypeID, DateTime DateTime, string Title, string Content, string ImageUrl, bool isShow, bool isHasImage, bool isCanComments, bool isCommend, bool isHot, long ReadCount, int IsusesId, ref long NewsID, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(ConnectionString, "P_NewsAdd", ref ds, ref Outputs,
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
                new MSSQL.Parameter("IsusesId", SqlDbType.Int, 0, ParameterDirection.Input, IsusesId),
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

        public static int P_NewsAddComments(string ConnectionString, long SiteID, long NewsID, DateTime DateTime, long CommentserID, string CommentserName, string Content, bool isShow, ref long NewNewsCommentsID, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_NewsAddComments(ConnectionString, ref ds, SiteID, NewsID, DateTime, CommentserID, CommentserName, Content, isShow, ref NewNewsCommentsID, ref ReturnDescription);
        }

        public static int P_NewsAddComments(string ConnectionString, ref DataSet ds, long SiteID, long NewsID, DateTime DateTime, long CommentserID, string CommentserName, string Content, bool isShow, ref long NewNewsCommentsID, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(ConnectionString, "P_NewsAddComments", ref ds, ref Outputs,
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

        public static int P_NewsDelete(string ConnectionString, long SiteID, long ID, ref int ReturnValue, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_NewsDelete(ConnectionString, ref ds, SiteID, ID, ref ReturnValue, ref ReturnDescription);
        }

        public static int P_NewsDelete(string ConnectionString, ref DataSet ds, long SiteID, long ID, ref int ReturnValue, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(ConnectionString, "P_NewsDelete", ref ds, ref Outputs,
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

        public static int P_NewsDeleteComments(string ConnectionString, long SiteID, long NewsCommentsID, ref long ReturnValue, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_NewsDeleteComments(ConnectionString, ref ds, SiteID, NewsCommentsID, ref ReturnValue, ref ReturnDescription);
        }

        public static int P_NewsDeleteComments(string ConnectionString, ref DataSet ds, long SiteID, long NewsCommentsID, ref long ReturnValue, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(ConnectionString, "P_NewsDeleteComments", ref ds, ref Outputs,
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

        public static int P_NewsEdit(string ConnectionString, long SiteID, long ID, int TypeID, DateTime DateTime, string Title, string Content, string ImageUrl, bool isShow, bool isHasImage, bool isCanComments, bool isCommend, bool isHot, long ReadCount, ref int ReturnValue, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_NewsEdit(ConnectionString, ref ds, SiteID, ID, TypeID, DateTime, Title, Content, ImageUrl, isShow, isHasImage, isCanComments, isCommend, isHot, ReadCount, ref ReturnValue, ref ReturnDescription);
        }

        public static int P_NewsEdit(string ConnectionString, ref DataSet ds, long SiteID, long ID, int TypeID, DateTime DateTime, string Title, string Content, string ImageUrl, bool isShow, bool isHasImage, bool isCanComments, bool isCommend, bool isHot, long ReadCount, ref int ReturnValue, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(ConnectionString, "P_NewsEdit", ref ds, ref Outputs,
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

        public static int P_NewsEditComments(string ConnectionString, long SiteID, long NewsCommentsID, DateTime DateTime, long CommentserID, string CommentserName, string Content, bool isShow, ref long ReturnValue, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_NewsEditComments(ConnectionString, ref ds, SiteID, NewsCommentsID, DateTime, CommentserID, CommentserName, Content, isShow, ref ReturnValue, ref ReturnDescription);
        }

        public static int P_NewsEditComments(string ConnectionString, ref DataSet ds, long SiteID, long NewsCommentsID, DateTime DateTime, long CommentserID, string CommentserName, string Content, bool isShow, ref long ReturnValue, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(ConnectionString, "P_NewsEditComments", ref ds, ref Outputs,
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

        public static int P_NewsRead(string ConnectionString, long SiteID, long NewsID, ref int ReturnValue, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_NewsRead(ConnectionString, ref ds, SiteID, NewsID, ref ReturnValue, ref ReturnDescription);
        }

        public static int P_NewsRead(string ConnectionString, ref DataSet ds, long SiteID, long NewsID, ref int ReturnValue, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(ConnectionString, "P_NewsRead", ref ds, ref Outputs,
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

        public static int P_PoliciesAndRegulationAdd(string ConnectionString, DateTime DateTime, string Title, string Content, bool isShow, ref long NewPoliciesAndRegulationID, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_PoliciesAndRegulationAdd(ConnectionString, ref ds, DateTime, Title, Content, isShow, ref NewPoliciesAndRegulationID, ref ReturnDescription);
        }

        public static int P_PoliciesAndRegulationAdd(string ConnectionString, ref DataSet ds, DateTime DateTime, string Title, string Content, bool isShow, ref long NewPoliciesAndRegulationID, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(ConnectionString, "P_PoliciesAndRegulationAdd", ref ds, ref Outputs,
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

        public static int P_PoliciesAndRegulationDelete(string ConnectionString, long ID, ref int ReturnValue, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_PoliciesAndRegulationDelete(ConnectionString, ref ds, ID, ref ReturnValue, ref ReturnDescription);
        }

        public static int P_PoliciesAndRegulationDelete(string ConnectionString, ref DataSet ds, long ID, ref int ReturnValue, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(ConnectionString, "P_PoliciesAndRegulationDelete", ref ds, ref Outputs,
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

        public static int P_PoliciesAndRegulationEdit(string ConnectionString, long ID, DateTime DateTime, string Title, string Content, bool isShow, ref int ReturnValue, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_PoliciesAndRegulationEdit(ConnectionString, ref ds, ID, DateTime, Title, Content, isShow, ref ReturnValue, ref ReturnDescription);
        }

        public static int P_PoliciesAndRegulationEdit(string ConnectionString, ref DataSet ds, long ID, DateTime DateTime, string Title, string Content, bool isShow, ref int ReturnValue, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(ConnectionString, "P_PoliciesAndRegulationEdit", ref ds, ref Outputs,
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

        public static int P_PopUserBonus(string ConnectionString, long Id, ref int ReturnValue, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_PopUserBonus(ConnectionString, ref ds, Id, ref ReturnValue, ref ReturnDescription);
        }

        public static int P_PopUserBonus(string ConnectionString, ref DataSet ds, long Id, ref int ReturnValue, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(ConnectionString, "P_PopUserBonus", ref ds, ref Outputs,
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

        public static int P_Quash(string ConnectionString, long SiteID, long BuyDetailID, ref int ReturnValue, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_Quash(ConnectionString, ref ds, SiteID, BuyDetailID, ref ReturnValue, ref ReturnDescription);
        }

        public static int P_Quash(string ConnectionString, ref DataSet ds, long SiteID, long BuyDetailID, ref int ReturnValue, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(ConnectionString, "P_Quash", ref ds, ref Outputs,
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

        public static int P_QuashChaseTask(string ConnectionString, long SiteID, long ChaseTaskID, bool isSystemQuash, ref int ReturnValue, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_QuashChaseTask(ConnectionString, ref ds, SiteID, ChaseTaskID, isSystemQuash, ref ReturnValue, ref ReturnDescription);
        }

        public static int P_QuashChaseTask(string ConnectionString, ref DataSet ds, long SiteID, long ChaseTaskID, bool isSystemQuash, ref int ReturnValue, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(ConnectionString, "P_QuashChaseTask", ref ds, ref Outputs,
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

        public static int P_QuashChaseTaskDetail(string ConnectionString, long SiteID, long ChaseTaskDetailID, bool isSystemQuash, ref int ReturnValue, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_QuashChaseTaskDetail(ConnectionString, ref ds, SiteID, ChaseTaskDetailID, isSystemQuash, ref ReturnValue, ref ReturnDescription);
        }

        public static int P_QuashChaseTaskDetail(string ConnectionString, ref DataSet ds, long SiteID, long ChaseTaskDetailID, bool isSystemQuash, ref int ReturnValue, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(ConnectionString, "P_QuashChaseTaskDetail", ref ds, ref Outputs,
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

        public static int P_QuashScheme(string ConnectionString, long SiteID, long SchemeID, bool isSystemQuash, bool isRelation, ref int ReturnValue, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_QuashScheme(ConnectionString, ref ds, SiteID, SchemeID, isSystemQuash, isRelation, ref ReturnValue, ref ReturnDescription);
        }

        public static int P_QuashScheme(string ConnectionString, ref DataSet ds, long SiteID, long SchemeID, bool isSystemQuash, bool isRelation, ref int ReturnValue, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(ConnectionString, "P_QuashScheme", ref ds, ref Outputs,
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

        public static int P_QuashSchemeNoLotteryNumber(string ConnectionString, ref int ReturnValue, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_QuashSchemeNoLotteryNumber(ConnectionString, ref ds, ref ReturnValue, ref ReturnDescription);
        }

        public static int P_QuashSchemeNoLotteryNumber(string ConnectionString, ref DataSet ds, ref int ReturnValue, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(ConnectionString, "P_QuashSchemeNoLotteryNumber", ref ds, ref Outputs,
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

        public static int P_QuashTemp12345(string ConnectionString, long SiteID, long BuyDetailID, ref int ReturnValue, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_QuashTemp12345(ConnectionString, ref ds, SiteID, BuyDetailID, ref ReturnValue, ref ReturnDescription);
        }

        public static int P_QuashTemp12345(string ConnectionString, ref DataSet ds, long SiteID, long BuyDetailID, ref int ReturnValue, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(ConnectionString, "P_QuashTemp12345", ref ds, ref Outputs,
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

        public static int P_QuestionsAdd(string ConnectionString, long SiteID, long UserID, short TypeID, string Telephone, string Content, ref long NewQuestionID, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_QuestionsAdd(ConnectionString, ref ds, SiteID, UserID, TypeID, Telephone, Content, ref NewQuestionID, ref ReturnDescription);
        }

        public static int P_QuestionsAdd(string ConnectionString, ref DataSet ds, long SiteID, long UserID, short TypeID, string Telephone, string Content, ref long NewQuestionID, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(ConnectionString, "P_QuestionsAdd", ref ds, ref Outputs,
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

        public static int P_QuestionsAnswer(string ConnectionString, long SiteID, long QuestionID, string Answer, long AnswerOperatorID, ref int ReturnValue, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_QuestionsAnswer(ConnectionString, ref ds, SiteID, QuestionID, Answer, AnswerOperatorID, ref ReturnValue, ref ReturnDescription);
        }

        public static int P_QuestionsAnswer(string ConnectionString, ref DataSet ds, long SiteID, long QuestionID, string Answer, long AnswerOperatorID, ref int ReturnValue, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(ConnectionString, "P_QuestionsAnswer", ref ds, ref Outputs,
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

        public static int P_QuestionsDelete(string ConnectionString, long SiteID, long QuestionID, ref int ReturnValue, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_QuestionsDelete(ConnectionString, ref ds, SiteID, QuestionID, ref ReturnValue, ref ReturnDescription);
        }

        public static int P_QuestionsDelete(string ConnectionString, ref DataSet ds, long SiteID, long QuestionID, ref int ReturnValue, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(ConnectionString, "P_QuestionsDelete", ref ds, ref Outputs,
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

        public static int P_QuestionsHandling(string ConnectionString, long SiteID, long QuestionID, long HandleOperatorID, ref int ReturnValue, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_QuestionsHandling(ConnectionString, ref ds, SiteID, QuestionID, HandleOperatorID, ref ReturnValue, ref ReturnDescription);
        }

        public static int P_QuestionsHandling(string ConnectionString, ref DataSet ds, long SiteID, long QuestionID, long HandleOperatorID, ref int ReturnValue, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(ConnectionString, "P_QuestionsHandling", ref ds, ref Outputs,
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

        public static int P_RebonusShares(string ConnectionString, ref int ReturnValue, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_RebonusShares(ConnectionString, ref ds, ref ReturnValue, ref ReturnDescription);
        }

        public static int P_RebonusShares(string ConnectionString, ref DataSet ds, ref int ReturnValue, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(ConnectionString, "P_RebonusShares", ref ds, ref Outputs,
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

        public static int P_SchemeAssure(string ConnectionString, ref int ReturnValue, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_SchemeAssure(ConnectionString, ref ds, ref ReturnValue, ref ReturnDescription);
        }

        public static int P_SchemeAssure(string ConnectionString, ref DataSet ds, ref int ReturnValue, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(ConnectionString, "P_SchemeAssure", ref ds, ref Outputs,
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

        public static int P_SchemeCalculatedBonus(string ConnectionString, ref bool ReturnBool, ref int ReturnValue, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_SchemeCalculatedBonus(ConnectionString, ref ds, ref ReturnBool, ref ReturnValue, ref ReturnDescription);
        }

        public static int P_SchemeCalculatedBonus(string ConnectionString, ref DataSet ds, ref bool ReturnBool, ref int ReturnValue, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(ConnectionString, "P_SchemeCalculatedBonus", ref ds, ref Outputs,
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

        public static int P_SchemePost(string ConnectionString, int posterid, string poster, int fid, string title, string ip, string message, long schemeid, int typeid, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_SchemePost(ConnectionString, ref ds, posterid, poster, fid, title, ip, message, schemeid, typeid, ref ReturnDescription);
        }

        public static int P_SchemePost(string ConnectionString, ref DataSet ds, int posterid, string poster, int fid, string title, string ip, string message, long schemeid, int typeid, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(ConnectionString, "P_SchemePost", ref ds, ref Outputs,
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

        public static int P_SchemePrintOut(string ConnectionString, long SiteID, long SchemeID, long BuyOperatorID, short PrintOutType, string Identifiers, bool isOt, ref int ReturnValue, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_SchemePrintOut(ConnectionString, ref ds, SiteID, SchemeID, BuyOperatorID, PrintOutType, Identifiers, isOt, ref ReturnValue, ref ReturnDescription);
        }

        public static int P_SchemePrintOut(string ConnectionString, ref DataSet ds, long SiteID, long SchemeID, long BuyOperatorID, short PrintOutType, string Identifiers, bool isOt, ref int ReturnValue, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(ConnectionString, "P_SchemePrintOut", ref ds, ref Outputs,
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

        public static int P_SchemesSendToCenterAdd(string ConnectionString, long SchemeID, int PlayTypeID, string TicketXML, ref int ReturnValue, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_SchemesSendToCenterAdd(ConnectionString, ref ds, SchemeID, PlayTypeID, TicketXML, ref ReturnValue, ref ReturnDescription);
        }

        public static int P_SchemesSendToCenterAdd(string ConnectionString, ref DataSet ds, long SchemeID, int PlayTypeID, string TicketXML, ref int ReturnValue, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(ConnectionString, "P_SchemesSendToCenterAdd", ref ds, ref Outputs,
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

        public static int P_SchemesSendToCenterAdd_Single(string ConnectionString, long SchemeID, int PlayTypeID, double Money, int Multiple, string Ticket, bool isFirstWrite, ref long ID, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_SchemesSendToCenterAdd_Single(ConnectionString, ref ds, SchemeID, PlayTypeID, Money, Multiple, Ticket, isFirstWrite, ref ID, ref ReturnDescription);
        }

        public static int P_SchemesSendToCenterAdd_Single(string ConnectionString, ref DataSet ds, long SchemeID, int PlayTypeID, double Money, int Multiple, string Ticket, bool isFirstWrite, ref long ID, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(ConnectionString, "P_SchemesSendToCenterAdd_Single", ref ds, ref Outputs,
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

        public static int P_SchemesSendToCenterHandle(string ConnectionString, string Identifiers, DateTime DealTime, bool IsSuccess, string Status, ref int ReturnValue, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_SchemesSendToCenterHandle(ConnectionString, ref ds, Identifiers, DealTime, IsSuccess, Status, ref ReturnValue, ref ReturnDescription);
        }

        public static int P_SchemesSendToCenterHandle(string ConnectionString, ref DataSet ds, string Identifiers, DateTime DealTime, bool IsSuccess, string Status, ref int ReturnValue, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(ConnectionString, "P_SchemesSendToCenterHandle", ref ds, ref Outputs,
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

        public static int P_SchemesSendToCenterHandleUniteAnte(string ConnectionString, long SchemeID, DateTime DealTime, bool isOt, ref int ReturnValue, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_SchemesSendToCenterHandleUniteAnte(ConnectionString, ref ds, SchemeID, DealTime, isOt, ref ReturnValue, ref ReturnDescription);
        }

        public static int P_SchemesSendToCenterHandleUniteAnte(string ConnectionString, ref DataSet ds, long SchemeID, DateTime DealTime, bool isOt, ref int ReturnValue, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(ConnectionString, "P_SchemesSendToCenterHandleUniteAnte", ref ds, ref Outputs,
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

        public static int P_ScoringExchange(string ConnectionString, long SiteID, long UserID, double Scoring, ref int ReturnValue, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_ScoringExchange(ConnectionString, ref ds, SiteID, UserID, Scoring, ref ReturnValue, ref ReturnDescription);
        }

        public static int P_ScoringExchange(string ConnectionString, ref DataSet ds, long SiteID, long UserID, double Scoring, ref int ReturnValue, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(ConnectionString, "P_ScoringExchange", ref ds, ref Outputs,
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

        public static int P_SelectPaging(string ConnectionString, string TableOrViewName, string FieldList, string OrderFieldList, string Condition, int PageSize, int PageIndex, ref long RowCount, ref int PageCount, ref int CurrentPageIndex, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_SelectPaging(ConnectionString, ref ds, TableOrViewName, FieldList, OrderFieldList, Condition, PageSize, PageIndex, ref RowCount, ref PageCount, ref CurrentPageIndex, ref ReturnDescription);
        }

        public static int P_SelectPaging(string ConnectionString, ref DataSet ds, string TableOrViewName, string FieldList, string OrderFieldList, string Condition, int PageSize, int PageIndex, ref long RowCount, ref int PageCount, ref int CurrentPageIndex, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(ConnectionString, "P_SelectPaging", ref ds, ref Outputs,
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

        public static int P_SetFriendsWinInfo(string ConnectionString, string SnsName, ref int ReturnValue, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_SetFriendsWinInfo(ConnectionString, ref ds, SnsName, ref ReturnValue, ref ReturnDescription);
        }

        public static int P_SetFriendsWinInfo(string ConnectionString, ref DataSet ds, string SnsName, ref int ReturnValue, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(ConnectionString, "P_SetFriendsWinInfo", ref ds, ref Outputs,
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

        public static int P_SetMaxMultiple(string ConnectionString, long IsuseID, int PlayTypeID, int MaxMultiple, ref int ReturnValue, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_SetMaxMultiple(ConnectionString, ref ds, IsuseID, PlayTypeID, MaxMultiple, ref ReturnValue, ref ReturnDescription);
        }

        public static int P_SetMaxMultiple(string ConnectionString, ref DataSet ds, long IsuseID, int PlayTypeID, int MaxMultiple, ref int ReturnValue, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(ConnectionString, "P_SetMaxMultiple", ref ds, ref Outputs,
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

        public static int P_SetOptions(string ConnectionString, string Key, string Value)
        {
            DataSet ds = null;

            return P_SetOptions(ConnectionString, ref ds, Key, Value);
        }

        public static int P_SetOptions(string ConnectionString, ref DataSet ds, string Key, string Value)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(ConnectionString, "P_SetOptions", ref ds, ref Outputs,
                new MSSQL.Parameter("Key", SqlDbType.VarChar, 0, ParameterDirection.Input, Key),
                new MSSQL.Parameter("Value", SqlDbType.VarChar, 0, ParameterDirection.Input, Value)
                );

            return CallResult;
        }

        public static int P_SetSchemeOpenUsers(string ConnectionString, long SiteID, long SchemeID, string UserList, ref int ReturnValue, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_SetSchemeOpenUsers(ConnectionString, ref ds, SiteID, SchemeID, UserList, ref ReturnValue, ref ReturnDescription);
        }

        public static int P_SetSchemeOpenUsers(string ConnectionString, ref DataSet ds, long SiteID, long SchemeID, string UserList, ref int ReturnValue, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(ConnectionString, "P_SetSchemeOpenUsers", ref ds, ref Outputs,
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

        public static int P_SetSiteNotificationTemplate(string ConnectionString, long SiteID, short Manner, string NotificationType, string Value)
        {
            DataSet ds = null;

            return P_SetSiteNotificationTemplate(ConnectionString, ref ds, SiteID, Manner, NotificationType, Value);
        }

        public static int P_SetSiteNotificationTemplate(string ConnectionString, ref DataSet ds, long SiteID, short Manner, string NotificationType, string Value)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(ConnectionString, "P_SetSiteNotificationTemplate", ref ds, ref Outputs,
                new MSSQL.Parameter("SiteID", SqlDbType.BigInt, 0, ParameterDirection.Input, SiteID),
                new MSSQL.Parameter("Manner", SqlDbType.SmallInt, 0, ParameterDirection.Input, Manner),
                new MSSQL.Parameter("NotificationType", SqlDbType.VarChar, 0, ParameterDirection.Input, NotificationType),
                new MSSQL.Parameter("Value", SqlDbType.VarChar, 0, ParameterDirection.Input, Value)
                );

            return CallResult;
        }

        public static int P_SetSiteONState(string ConnectionString, long SiteID, bool ON)
        {
            DataSet ds = null;

            return P_SetSiteONState(ConnectionString, ref ds, SiteID, ON);
        }

        public static int P_SetSiteONState(string ConnectionString, ref DataSet ds, long SiteID, bool ON)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(ConnectionString, "P_SetSiteONState", ref ds, ref Outputs,
                new MSSQL.Parameter("SiteID", SqlDbType.BigInt, 0, ParameterDirection.Input, SiteID),
                new MSSQL.Parameter("ON", SqlDbType.Bit, 0, ParameterDirection.Input, ON)
                );

            return CallResult;
        }

        public static int P_SetSiteSendNotificationTypes(string ConnectionString, long SiteID, short Manner, string SendNotificationTypeList, ref int ReturnValue, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_SetSiteSendNotificationTypes(ConnectionString, ref ds, SiteID, Manner, SendNotificationTypeList, ref ReturnValue, ref ReturnDescription);
        }

        public static int P_SetSiteSendNotificationTypes(string ConnectionString, ref DataSet ds, long SiteID, short Manner, string SendNotificationTypeList, ref int ReturnValue, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(ConnectionString, "P_SetSiteSendNotificationTypes", ref ds, ref Outputs,
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

        public static int P_SetSiteUrls(string ConnectionString, long SiteID, string Urls, ref int ReturnValue, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_SetSiteUrls(ConnectionString, ref ds, SiteID, Urls, ref ReturnValue, ref ReturnDescription);
        }

        public static int P_SetSiteUrls(string ConnectionString, ref DataSet ds, long SiteID, string Urls, ref int ReturnValue, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(ConnectionString, "P_SetSiteUrls", ref ds, ref Outputs,
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

        public static int P_SetUserAcceptNotificationTypes(string ConnectionString, long SiteID, long UserID, short Manner, string AcceptNotificationTypeList, ref int ReturnValue, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_SetUserAcceptNotificationTypes(ConnectionString, ref ds, SiteID, UserID, Manner, AcceptNotificationTypeList, ref ReturnValue, ref ReturnDescription);
        }

        public static int P_SetUserAcceptNotificationTypes(string ConnectionString, ref DataSet ds, long SiteID, long UserID, short Manner, string AcceptNotificationTypeList, ref int ReturnValue, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(ConnectionString, "P_SetUserAcceptNotificationTypes", ref ds, ref Outputs,
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

        public static int P_SetUserCompetences(string ConnectionString, long SiteID, long UserID, string CompetencesList, string GroupsList, ref int ReturnValue, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_SetUserCompetences(ConnectionString, ref ds, SiteID, UserID, CompetencesList, GroupsList, ref ReturnValue, ref ReturnDescription);
        }

        public static int P_SetUserCompetences(string ConnectionString, ref DataSet ds, long SiteID, long UserID, string CompetencesList, string GroupsList, ref int ReturnValue, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(ConnectionString, "P_SetUserCompetences", ref ds, ref Outputs,
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

        public static int P_SiteAdd(string ConnectionString, long SiteParentID, long OwnerUserID, string Name, string LogoUrl, string Company, string Address, string PostCode, string ResponsiblePerson, string ContactPerson, string Telephone, string Fax, string Mobile, string Email, string QQ, string ServiceTelephone, string ICPCert, short Level, bool ON, double BonusScale, int MaxSubSites, string UseLotteryListRestrictions, string UseLotteryList, string UseLotteryListQuickBuy, string Urls, ref long AdministratorID, ref long SiteID, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_SiteAdd(ConnectionString, ref ds, SiteParentID, OwnerUserID, Name, LogoUrl, Company, Address, PostCode, ResponsiblePerson, ContactPerson, Telephone, Fax, Mobile, Email, QQ, ServiceTelephone, ICPCert, Level, ON, BonusScale, MaxSubSites, UseLotteryListRestrictions, UseLotteryList, UseLotteryListQuickBuy, Urls, ref AdministratorID, ref SiteID, ref ReturnDescription);
        }

        public static int P_SiteAdd(string ConnectionString, ref DataSet ds, long SiteParentID, long OwnerUserID, string Name, string LogoUrl, string Company, string Address, string PostCode, string ResponsiblePerson, string ContactPerson, string Telephone, string Fax, string Mobile, string Email, string QQ, string ServiceTelephone, string ICPCert, short Level, bool ON, double BonusScale, int MaxSubSites, string UseLotteryListRestrictions, string UseLotteryList, string UseLotteryListQuickBuy, string Urls, ref long AdministratorID, ref long SiteID, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(ConnectionString, "P_SiteAdd", ref ds, ref Outputs,
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

        public static int P_SiteAfficheAdd(string ConnectionString, long SiteID, DateTime DateTime, string Title, string Content, bool isShow, bool isCommend, ref long NewAfficheID, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_SiteAfficheAdd(ConnectionString, ref ds, SiteID, DateTime, Title, Content, isShow, isCommend, ref NewAfficheID, ref ReturnDescription);
        }

        public static int P_SiteAfficheAdd(string ConnectionString, ref DataSet ds, long SiteID, DateTime DateTime, string Title, string Content, bool isShow, bool isCommend, ref long NewAfficheID, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(ConnectionString, "P_SiteAfficheAdd", ref ds, ref Outputs,
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

        public static int P_SiteAfficheDelete(string ConnectionString, long SiteID, long ID, ref int ReturnValue, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_SiteAfficheDelete(ConnectionString, ref ds, SiteID, ID, ref ReturnValue, ref ReturnDescription);
        }

        public static int P_SiteAfficheDelete(string ConnectionString, ref DataSet ds, long SiteID, long ID, ref int ReturnValue, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(ConnectionString, "P_SiteAfficheDelete", ref ds, ref Outputs,
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

        public static int P_SiteAfficheEdit(string ConnectionString, long SiteID, long ID, DateTime DateTime, string Title, string Content, bool isShow, bool isCommend, ref int ReturnValue, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_SiteAfficheEdit(ConnectionString, ref ds, SiteID, ID, DateTime, Title, Content, isShow, isCommend, ref ReturnValue, ref ReturnDescription);
        }

        public static int P_SiteAfficheEdit(string ConnectionString, ref DataSet ds, long SiteID, long ID, DateTime DateTime, string Title, string Content, bool isShow, bool isCommend, ref int ReturnValue, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(ConnectionString, "P_SiteAfficheEdit", ref ds, ref Outputs,
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

        public static int P_SiteEdit(string ConnectionString, long SiteID, string Name, string LogoUrl, string Company, string Address, string PostCode, string ResponsiblePerson, string ContactPerson, string Telephone, string Fax, string Mobile, string Email, string QQ, string ServiceTelephone, string ICPCert, bool ON, double BonusScale, int MaxSubSites, string UseLotteryListRestrictions, string UseLotteryList, string UseLotteryListQuickBuy, string Urls, ref int ReturnValue, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_SiteEdit(ConnectionString, ref ds, SiteID, Name, LogoUrl, Company, Address, PostCode, ResponsiblePerson, ContactPerson, Telephone, Fax, Mobile, Email, QQ, ServiceTelephone, ICPCert, ON, BonusScale, MaxSubSites, UseLotteryListRestrictions, UseLotteryList, UseLotteryListQuickBuy, Urls, ref ReturnValue, ref ReturnDescription);
        }

        public static int P_SiteEdit(string ConnectionString, ref DataSet ds, long SiteID, string Name, string LogoUrl, string Company, string Address, string PostCode, string ResponsiblePerson, string ContactPerson, string Telephone, string Fax, string Mobile, string Email, string QQ, string ServiceTelephone, string ICPCert, bool ON, double BonusScale, int MaxSubSites, string UseLotteryListRestrictions, string UseLotteryList, string UseLotteryListQuickBuy, string Urls, ref int ReturnValue, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(ConnectionString, "P_SiteEdit", ref ds, ref Outputs,
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

        public static int P_SoftDownloadAdd(string ConnectionString, long SiteID, int LotteryID, DateTime DateTime, string Title, string FileUrl, string ImageUrl, string Content, bool isHot, bool isCommend, bool isShow, int ReadCount, ref long NewSoftDownloadID, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_SoftDownloadAdd(ConnectionString, ref ds, SiteID, LotteryID, DateTime, Title, FileUrl, ImageUrl, Content, isHot, isCommend, isShow, ReadCount, ref NewSoftDownloadID, ref ReturnDescription);
        }

        public static int P_SoftDownloadAdd(string ConnectionString, ref DataSet ds, long SiteID, int LotteryID, DateTime DateTime, string Title, string FileUrl, string ImageUrl, string Content, bool isHot, bool isCommend, bool isShow, int ReadCount, ref long NewSoftDownloadID, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(ConnectionString, "P_SoftDownloadAdd", ref ds, ref Outputs,
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

        public static int P_SoftDownloadDelete(string ConnectionString, long SiteID, long ID, ref int ReturnValue, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_SoftDownloadDelete(ConnectionString, ref ds, SiteID, ID, ref ReturnValue, ref ReturnDescription);
        }

        public static int P_SoftDownloadDelete(string ConnectionString, ref DataSet ds, long SiteID, long ID, ref int ReturnValue, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(ConnectionString, "P_SoftDownloadDelete", ref ds, ref Outputs,
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

        public static int P_SoftDownloadEdit(string ConnectionString, long SiteID, long ID, int LotteryID, DateTime DateTime, string Title, string FileUrl, string ImageUrl, string Content, bool isHot, bool isCommend, bool isShow, int ReadCount, ref int ReturnValue, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_SoftDownloadEdit(ConnectionString, ref ds, SiteID, ID, LotteryID, DateTime, Title, FileUrl, ImageUrl, Content, isHot, isCommend, isShow, ReadCount, ref ReturnValue, ref ReturnDescription);
        }

        public static int P_SoftDownloadEdit(string ConnectionString, ref DataSet ds, long SiteID, long ID, int LotteryID, DateTime DateTime, string Title, string FileUrl, string ImageUrl, string Content, bool isHot, bool isCommend, bool isShow, int ReadCount, ref int ReturnValue, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(ConnectionString, "P_SoftDownloadEdit", ref ds, ref Outputs,
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

        public static int P_SurrogateNotificationAdd(string ConnectionString, long SiteID, string Title, string Content, bool isShow, ref long SurrogateNotificationID, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_SurrogateNotificationAdd(ConnectionString, ref ds, SiteID, Title, Content, isShow, ref SurrogateNotificationID, ref ReturnDescription);
        }

        public static int P_SurrogateNotificationAdd(string ConnectionString, ref DataSet ds, long SiteID, string Title, string Content, bool isShow, ref long SurrogateNotificationID, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(ConnectionString, "P_SurrogateNotificationAdd", ref ds, ref Outputs,
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

        public static int P_SurrogateNotificationDelete(string ConnectionString, long SiteID, long SurrogateNotificationID, ref int ReturnValue, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_SurrogateNotificationDelete(ConnectionString, ref ds, SiteID, SurrogateNotificationID, ref ReturnValue, ref ReturnDescription);
        }

        public static int P_SurrogateNotificationDelete(string ConnectionString, ref DataSet ds, long SiteID, long SurrogateNotificationID, ref int ReturnValue, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(ConnectionString, "P_SurrogateNotificationDelete", ref ds, ref Outputs,
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

        public static int P_SurrogateNotificationEdit(string ConnectionString, long SiteID, long SurrogateNotificationID, string Title, string Content, bool isShow, ref int ReturnValue, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_SurrogateNotificationEdit(ConnectionString, ref ds, SiteID, SurrogateNotificationID, Title, Content, isShow, ref ReturnValue, ref ReturnDescription);
        }

        public static int P_SurrogateNotificationEdit(string ConnectionString, ref DataSet ds, long SiteID, long SurrogateNotificationID, string Title, string Content, bool isShow, ref int ReturnValue, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(ConnectionString, "P_SurrogateNotificationEdit", ref ds, ref Outputs,
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

        public static int P_SurrogateTry(string ConnectionString, long SiteID, long UserID, string Content, string Name, string LogoUrl, string Company, string Address, string PostCode, string ResponsiblePerson, string ContactPerson, string Telephone, string Fax, string Mobile, string Email, string QQ, string ServiceTelephone, string UseLotteryList, string Urls, ref int ReturnValue, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_SurrogateTry(ConnectionString, ref ds, SiteID, UserID, Content, Name, LogoUrl, Company, Address, PostCode, ResponsiblePerson, ContactPerson, Telephone, Fax, Mobile, Email, QQ, ServiceTelephone, UseLotteryList, Urls, ref ReturnValue, ref ReturnDescription);
        }

        public static int P_SurrogateTry(string ConnectionString, ref DataSet ds, long SiteID, long UserID, string Content, string Name, string LogoUrl, string Company, string Address, string PostCode, string ResponsiblePerson, string ContactPerson, string Telephone, string Fax, string Mobile, string Email, string QQ, string ServiceTelephone, string UseLotteryList, string Urls, ref int ReturnValue, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(ConnectionString, "P_SurrogateTry", ref ds, ref Outputs,
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

        public static int P_SurrogateTryHandle(string ConnectionString, long SiteID, long TryID, short HandleResult, ref int ReturnValue, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_SurrogateTryHandle(ConnectionString, ref ds, SiteID, TryID, HandleResult, ref ReturnValue, ref ReturnDescription);
        }

        public static int P_SurrogateTryHandle(string ConnectionString, ref DataSet ds, long SiteID, long TryID, short HandleResult, ref int ReturnValue, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(ConnectionString, "P_SurrogateTryHandle", ref ds, ref Outputs,
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

        public static int P_SystemEnd(string ConnectionString, ref int ReturnValue, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_SystemEnd(ConnectionString, ref ds, ref ReturnValue, ref ReturnDescription);
        }

        public static int P_SystemEnd(string ConnectionString, ref DataSet ds, ref int ReturnValue, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(ConnectionString, "P_SystemEnd", ref ds, ref Outputs,
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

        public static int P_TrendChart_11YDJ_WINNUM(string ConnectionString, DateTime DateTime, long LotteryID, ref int ReturnValue, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_TrendChart_11YDJ_WINNUM(ConnectionString, ref ds, DateTime, LotteryID, ref ReturnValue, ref ReturnDescription);
        }

        public static int P_TrendChart_11YDJ_WINNUM(string ConnectionString, ref DataSet ds, DateTime DateTime, long LotteryID, ref int ReturnValue, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(ConnectionString, "P_TrendChart_11YDJ_WINNUM", ref ds, ref Outputs,
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

        public static int P_TrendChart_15X5_CGXMB(string ConnectionString)
        {
            DataSet ds = null;

            return P_TrendChart_15X5_CGXMB(ConnectionString, ref ds);
        }

        public static int P_TrendChart_15X5_CGXMB(string ConnectionString, ref DataSet ds)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(ConnectionString, "P_TrendChart_15X5_CGXMB", ref ds, ref Outputs);

            return CallResult;
        }

        public static int P_TrendChart_15X5_HMFB(string ConnectionString)
        {
            DataSet ds = null;

            return P_TrendChart_15X5_HMFB(ConnectionString, ref ds);
        }

        public static int P_TrendChart_15X5_HMFB(string ConnectionString, ref DataSet ds)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(ConnectionString, "P_TrendChart_15X5_HMFB", ref ds, ref Outputs);

            return CallResult;
        }

        public static int P_TrendChart_22X5_HMFB(string ConnectionString, int IsuseNum)
        {
            DataSet ds = null;

            return P_TrendChart_22X5_HMFB(ConnectionString, ref ds, IsuseNum);
        }

        public static int P_TrendChart_22X5_HMFB(string ConnectionString, ref DataSet ds, int IsuseNum)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(ConnectionString, "P_TrendChart_22X5_HMFB", ref ds, ref Outputs,
                new MSSQL.Parameter("IsuseNum", SqlDbType.Int, 0, ParameterDirection.Input, IsuseNum)
                );

            return CallResult;
        }

        public static int P_TrendChart_22X5_HMLR(string ConnectionString, int IsuseNum)
        {
            DataSet ds = null;

            return P_TrendChart_22X5_HMLR(ConnectionString, ref ds, IsuseNum);
        }

        public static int P_TrendChart_22X5_HMLR(string ConnectionString, ref DataSet ds, int IsuseNum)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(ConnectionString, "P_TrendChart_22X5_HMLR", ref ds, ref Outputs,
                new MSSQL.Parameter("IsuseNum", SqlDbType.Int, 0, ParameterDirection.Input, IsuseNum)
                );

            return CallResult;
        }

        public static int P_TrendChart_22X5_HMLRjj(string ConnectionString, int IsuseNum)
        {
            DataSet ds = null;

            return P_TrendChart_22X5_HMLRjj(ConnectionString, ref ds, IsuseNum);
        }

        public static int P_TrendChart_22X5_HMLRjj(string ConnectionString, ref DataSet ds, int IsuseNum)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(ConnectionString, "P_TrendChart_22X5_HMLRjj", ref ds, ref Outputs,
                new MSSQL.Parameter("IsuseNum", SqlDbType.Int, 0, ParameterDirection.Input, IsuseNum)
                );

            return CallResult;
        }

        public static int P_TrendChart_22X5_HZ_Heng(string ConnectionString)
        {
            DataSet ds = null;

            return P_TrendChart_22X5_HZ_Heng(ConnectionString, ref ds);
        }

        public static int P_TrendChart_22X5_HZ_Heng(string ConnectionString, ref DataSet ds)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(ConnectionString, "P_TrendChart_22X5_HZ_Heng", ref ds, ref Outputs);

            return CallResult;
        }

        public static int P_TrendChart_22X5_HZzong(string ConnectionString)
        {
            DataSet ds = null;

            return P_TrendChart_22X5_HZzong(ConnectionString, ref ds);
        }

        public static int P_TrendChart_22X5_HZzong(string ConnectionString, ref DataSet ds)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(ConnectionString, "P_TrendChart_22X5_HZzong", ref ds, ref Outputs);

            return CallResult;
        }

        public static int P_TrendChart_22X5_JO(string ConnectionString)
        {
            DataSet ds = null;

            return P_TrendChart_22X5_JO(ConnectionString, ref ds);
        }

        public static int P_TrendChart_22X5_JO(string ConnectionString, ref DataSet ds)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(ConnectionString, "P_TrendChart_22X5_JO", ref ds, ref Outputs);

            return CallResult;
        }

        public static int P_TrendChart_22X5_LH(string ConnectionString)
        {
            DataSet ds = null;

            return P_TrendChart_22X5_LH(ConnectionString, ref ds);
        }

        public static int P_TrendChart_22X5_LH(string ConnectionString, ref DataSet ds)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(ConnectionString, "P_TrendChart_22X5_LH", ref ds, ref Outputs);

            return CallResult;
        }

        public static int P_TrendChart_22X5_WeiHaoCF(string ConnectionString)
        {
            DataSet ds = null;

            return P_TrendChart_22X5_WeiHaoCF(ConnectionString, ref ds);
        }

        public static int P_TrendChart_22X5_WeiHaoCF(string ConnectionString, ref DataSet ds)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(ConnectionString, "P_TrendChart_22X5_WeiHaoCF", ref ds, ref Outputs);

            return CallResult;
        }

        public static int P_TrendChart_22X5_WH(string ConnectionString)
        {
            DataSet ds = null;

            return P_TrendChart_22X5_WH(ConnectionString, ref ds);
        }

        public static int P_TrendChart_22X5_WH(string ConnectionString, ref DataSet ds)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(ConnectionString, "P_TrendChart_22X5_WH", ref ds, ref Outputs);

            return CallResult;
        }

        public static int P_TrendChart_22X5_YS(string ConnectionString)
        {
            DataSet ds = null;

            return P_TrendChart_22X5_YS(ConnectionString, ref ds);
        }

        public static int P_TrendChart_22X5_YS(string ConnectionString, ref DataSet ds)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(ConnectionString, "P_TrendChart_22X5_YS", ref ds, ref Outputs);

            return CallResult;
        }

        public static int P_TrendChart_3D_C3YS(string ConnectionString)
        {
            DataSet ds = null;

            return P_TrendChart_3D_C3YS(ConnectionString, ref ds);
        }

        public static int P_TrendChart_3D_C3YS(string ConnectionString, ref DataSet ds)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(ConnectionString, "P_TrendChart_3D_C3YS", ref ds, ref Outputs);

            return CallResult;
        }

        public static int P_TrendChart_3D_DZX(string ConnectionString)
        {
            DataSet ds = null;

            return P_TrendChart_3D_DZX(ConnectionString, ref ds);
        }

        public static int P_TrendChart_3D_DZX(string ConnectionString, ref DataSet ds)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(ConnectionString, "P_TrendChart_3D_DZX", ref ds, ref Outputs);

            return CallResult;
        }

        public static int P_TrendChart_3D_HZ(string ConnectionString)
        {
            DataSet ds = null;

            return P_TrendChart_3D_HZ(ConnectionString, ref ds);
        }

        public static int P_TrendChart_3D_HZ(string ConnectionString, ref DataSet ds)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(ConnectionString, "P_TrendChart_3D_HZ", ref ds, ref Outputs);

            return CallResult;
        }

        public static int P_TrendChart_3D_KD(string ConnectionString)
        {
            DataSet ds = null;

            return P_TrendChart_3D_KD(ConnectionString, ref ds);
        }

        public static int P_TrendChart_3D_KD(string ConnectionString, ref DataSet ds)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(ConnectionString, "P_TrendChart_3D_KD", ref ds, ref Outputs);

            return CallResult;
        }

        public static int P_TrendChart_3D_XTZST(string ConnectionString)
        {
            DataSet ds = null;

            return P_TrendChart_3D_XTZST(ConnectionString, ref ds);
        }

        public static int P_TrendChart_3D_XTZST(string ConnectionString, ref DataSet ds)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(ConnectionString, "P_TrendChart_3D_XTZST", ref ds, ref Outputs);

            return CallResult;
        }

        public static int P_TrendChart_3D_ZHFB(string ConnectionString)
        {
            DataSet ds = null;

            return P_TrendChart_3D_ZHFB(ConnectionString, ref ds);
        }

        public static int P_TrendChart_3D_ZHFB(string ConnectionString, ref DataSet ds)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(ConnectionString, "P_TrendChart_3D_ZHFB", ref ds, ref Outputs);

            return CallResult;
        }

        public static int P_TrendChart_3D_ZHXT(string ConnectionString)
        {
            DataSet ds = null;

            return P_TrendChart_3D_ZHXT(ConnectionString, ref ds);
        }

        public static int P_TrendChart_3D_ZHXT(string ConnectionString, ref DataSet ds)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(ConnectionString, "P_TrendChart_3D_ZHXT", ref ds, ref Outputs);

            return CallResult;
        }

        public static int P_TrendChart_4D_CGXMB(string ConnectionString)
        {
            DataSet ds = null;

            return P_TrendChart_4D_CGXMB(ConnectionString, ref ds);
        }

        public static int P_TrendChart_4D_CGXMB(string ConnectionString, ref DataSet ds)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(ConnectionString, "P_TrendChart_4D_CGXMB", ref ds, ref Outputs);

            return CallResult;
        }

        public static int P_TrendChart_4D_ZHFB(string ConnectionString)
        {
            DataSet ds = null;

            return P_TrendChart_4D_ZHFB(ConnectionString, ref ds);
        }

        public static int P_TrendChart_4D_ZHFB(string ConnectionString, ref DataSet ds)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(ConnectionString, "P_TrendChart_4D_ZHFB", ref ds, ref Outputs);

            return CallResult;
        }

        public static int P_TrendChart_7CL_HMFB(string ConnectionString)
        {
            DataSet ds = null;

            return P_TrendChart_7CL_HMFB(ConnectionString, ref ds);
        }

        public static int P_TrendChart_7CL_HMFB(string ConnectionString, ref DataSet ds)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(ConnectionString, "P_TrendChart_7CL_HMFB", ref ds, ref Outputs);

            return CallResult;
        }

        public static int P_TrendChart_7LC_CGXMB(string ConnectionString)
        {
            DataSet ds = null;

            return P_TrendChart_7LC_CGXMB(ConnectionString, ref ds);
        }

        public static int P_TrendChart_7LC_CGXMB(string ConnectionString, ref DataSet ds)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(ConnectionString, "P_TrendChart_7LC_CGXMB", ref ds, ref Outputs);

            return CallResult;
        }

        public static int P_TrendChart_7X_012(string ConnectionString)
        {
            DataSet ds = null;

            return P_TrendChart_7X_012(ConnectionString, ref ds);
        }

        public static int P_TrendChart_7X_012(string ConnectionString, ref DataSet ds)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(ConnectionString, "P_TrendChart_7X_012", ref ds, ref Outputs);

            return CallResult;
        }

        public static int P_TrendChart_7X_CF(string ConnectionString)
        {
            DataSet ds = null;

            return P_TrendChart_7X_CF(ConnectionString, ref ds);
        }

        public static int P_TrendChart_7X_CF(string ConnectionString, ref DataSet ds)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(ConnectionString, "P_TrendChart_7X_CF", ref ds, ref Outputs);

            return CallResult;
        }

        public static int P_TrendChart_7X_DX(string ConnectionString)
        {
            DataSet ds = null;

            return P_TrendChart_7X_DX(ConnectionString, ref ds);
        }

        public static int P_TrendChart_7X_DX(string ConnectionString, ref DataSet ds)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(ConnectionString, "P_TrendChart_7X_DX", ref ds, ref Outputs);

            return CallResult;
        }

        public static int P_TrendChart_7X_DZX(string ConnectionString)
        {
            DataSet ds = null;

            return P_TrendChart_7X_DZX(ConnectionString, ref ds);
        }

        public static int P_TrendChart_7X_DZX(string ConnectionString, ref DataSet ds)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(ConnectionString, "P_TrendChart_7X_DZX", ref ds, ref Outputs);

            return CallResult;
        }

        public static int P_TrendChart_7X_HMFB(string ConnectionString)
        {
            DataSet ds = null;

            return P_TrendChart_7X_HMFB(ConnectionString, ref ds);
        }

        public static int P_TrendChart_7X_HMFB(string ConnectionString, ref DataSet ds)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(ConnectionString, "P_TrendChart_7X_HMFB", ref ds, ref Outputs);

            return CallResult;
        }

        public static int P_TrendChart_7X_HZHeng(string ConnectionString)
        {
            DataSet ds = null;

            return P_TrendChart_7X_HZHeng(ConnectionString, ref ds);
        }

        public static int P_TrendChart_7X_HZHeng(string ConnectionString, ref DataSet ds)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(ConnectionString, "P_TrendChart_7X_HZHeng", ref ds, ref Outputs);

            return CallResult;
        }

        public static int P_TrendChart_7X_HZzhong(string ConnectionString)
        {
            DataSet ds = null;

            return P_TrendChart_7X_HZzhong(ConnectionString, ref ds);
        }

        public static int P_TrendChart_7X_HZzhong(string ConnectionString, ref DataSet ds)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(ConnectionString, "P_TrendChart_7X_HZzhong", ref ds, ref Outputs);

            return CallResult;
        }

        public static int P_TrendChart_7X_JO(string ConnectionString)
        {
            DataSet ds = null;

            return P_TrendChart_7X_JO(ConnectionString, ref ds);
        }

        public static int P_TrendChart_7X_JO(string ConnectionString, ref DataSet ds)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(ConnectionString, "P_TrendChart_7X_JO", ref ds, ref Outputs);

            return CallResult;
        }

        public static int P_TrendChart_7X_LH(string ConnectionString)
        {
            DataSet ds = null;

            return P_TrendChart_7X_LH(ConnectionString, ref ds);
        }

        public static int P_TrendChart_7X_LH(string ConnectionString, ref DataSet ds)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(ConnectionString, "P_TrendChart_7X_LH", ref ds, ref Outputs);

            return CallResult;
        }

        public static int P_TrendChart_7X_YS(string ConnectionString)
        {
            DataSet ds = null;

            return P_TrendChart_7X_YS(ConnectionString, ref ds);
        }

        public static int P_TrendChart_7X_YS(string ConnectionString, ref DataSet ds)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(ConnectionString, "P_TrendChart_7X_YS", ref ds, ref Outputs);

            return CallResult;
        }

        public static int P_TrendChart_7X_ZH(string ConnectionString)
        {
            DataSet ds = null;

            return P_TrendChart_7X_ZH(ConnectionString, ref ds);
        }

        public static int P_TrendChart_7X_ZH(string ConnectionString, ref DataSet ds)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(ConnectionString, "P_TrendChart_7X_ZH", ref ds, ref Outputs);

            return CallResult;
        }

        public static int P_TrendChart_7XC_CGXMB(string ConnectionString)
        {
            DataSet ds = null;

            return P_TrendChart_7XC_CGXMB(ConnectionString, ref ds);
        }

        public static int P_TrendChart_7XC_CGXMB(string ConnectionString, ref DataSet ds)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(ConnectionString, "P_TrendChart_7XC_CGXMB", ref ds, ref Outputs);

            return CallResult;
        }

        public static int P_TrendChart_CJDLT_HMFB(string ConnectionString, int IsuseNum)
        {
            DataSet ds = null;

            return P_TrendChart_CJDLT_HMFB(ConnectionString, ref ds, IsuseNum);
        }

        public static int P_TrendChart_CJDLT_HMFB(string ConnectionString, ref DataSet ds, int IsuseNum)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(ConnectionString, "P_TrendChart_CJDLT_HMFB", ref ds, ref Outputs,
                new MSSQL.Parameter("IsuseNum", SqlDbType.Int, 0, ParameterDirection.Input, IsuseNum)
                );

            return CallResult;
        }

        public static int P_TrendChart_CJDLT_HMLR_JiMa(string ConnectionString, int IsuseNum)
        {
            DataSet ds = null;

            return P_TrendChart_CJDLT_HMLR_JiMa(ConnectionString, ref ds, IsuseNum);
        }

        public static int P_TrendChart_CJDLT_HMLR_JiMa(string ConnectionString, ref DataSet ds, int IsuseNum)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(ConnectionString, "P_TrendChart_CJDLT_HMLR_JiMa", ref ds, ref Outputs,
                new MSSQL.Parameter("IsuseNum", SqlDbType.Int, 0, ParameterDirection.Input, IsuseNum)
                );

            return CallResult;
        }

        public static int P_TrendChart_CJDLT_HMLR_JiMajj(string ConnectionString, int IsuseNum)
        {
            DataSet ds = null;

            return P_TrendChart_CJDLT_HMLR_JiMajj(ConnectionString, ref ds, IsuseNum);
        }

        public static int P_TrendChart_CJDLT_HMLR_JiMajj(string ConnectionString, ref DataSet ds, int IsuseNum)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(ConnectionString, "P_TrendChart_CJDLT_HMLR_JiMajj", ref ds, ref Outputs,
                new MSSQL.Parameter("IsuseNum", SqlDbType.Int, 0, ParameterDirection.Input, IsuseNum)
                );

            return CallResult;
        }

        public static int P_TrendChart_CJDLT_HMLR_Tema(string ConnectionString, int IsuseNum)
        {
            DataSet ds = null;

            return P_TrendChart_CJDLT_HMLR_Tema(ConnectionString, ref ds, IsuseNum);
        }

        public static int P_TrendChart_CJDLT_HMLR_Tema(string ConnectionString, ref DataSet ds, int IsuseNum)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(ConnectionString, "P_TrendChart_CJDLT_HMLR_Tema", ref ds, ref Outputs,
                new MSSQL.Parameter("IsuseNum", SqlDbType.Int, 0, ParameterDirection.Input, IsuseNum)
                );

            return CallResult;
        }

        public static int P_TrendChart_CJDLT_HMLR_Temajj(string ConnectionString, int IsuseNum)
        {
            DataSet ds = null;

            return P_TrendChart_CJDLT_HMLR_Temajj(ConnectionString, ref ds, IsuseNum);
        }

        public static int P_TrendChart_CJDLT_HMLR_Temajj(string ConnectionString, ref DataSet ds, int IsuseNum)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(ConnectionString, "P_TrendChart_CJDLT_HMLR_Temajj", ref ds, ref Outputs,
                new MSSQL.Parameter("IsuseNum", SqlDbType.Int, 0, ParameterDirection.Input, IsuseNum)
                );

            return CallResult;
        }

        public static int P_TrendChart_CJDLT_HZ_Heng(string ConnectionString)
        {
            DataSet ds = null;

            return P_TrendChart_CJDLT_HZ_Heng(ConnectionString, ref ds);
        }

        public static int P_TrendChart_CJDLT_HZ_Heng(string ConnectionString, ref DataSet ds)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(ConnectionString, "P_TrendChart_CJDLT_HZ_Heng", ref ds, ref Outputs);

            return CallResult;
        }

        public static int P_TrendChart_CJDLT_HZzong(string ConnectionString)
        {
            DataSet ds = null;

            return P_TrendChart_CJDLT_HZzong(ConnectionString, ref ds);
        }

        public static int P_TrendChart_CJDLT_HZzong(string ConnectionString, ref DataSet ds)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(ConnectionString, "P_TrendChart_CJDLT_HZzong", ref ds, ref Outputs);

            return CallResult;
        }

        public static int P_TrendChart_CJDLT_jima(string ConnectionString)
        {
            DataSet ds = null;

            return P_TrendChart_CJDLT_jima(ConnectionString, ref ds);
        }

        public static int P_TrendChart_CJDLT_jima(string ConnectionString, ref DataSet ds)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(ConnectionString, "P_TrendChart_CJDLT_jima", ref ds, ref Outputs);

            return CallResult;
        }

        public static int P_TrendChart_CJDLT_jimaYL(string ConnectionString, int IsuseNum)
        {
            DataSet ds = null;

            return P_TrendChart_CJDLT_jimaYL(ConnectionString, ref ds, IsuseNum);
        }

        public static int P_TrendChart_CJDLT_jimaYL(string ConnectionString, ref DataSet ds, int IsuseNum)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(ConnectionString, "P_TrendChart_CJDLT_jimaYL", ref ds, ref Outputs,
                new MSSQL.Parameter("IsuseNum", SqlDbType.Int, 0, ParameterDirection.Input, IsuseNum)
                );

            return CallResult;
        }

        public static int P_TrendChart_CJDLT_Jiou(string ConnectionString)
        {
            DataSet ds = null;

            return P_TrendChart_CJDLT_Jiou(ConnectionString, ref ds);
        }

        public static int P_TrendChart_CJDLT_Jiou(string ConnectionString, ref DataSet ds)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(ConnectionString, "P_TrendChart_CJDLT_Jiou", ref ds, ref Outputs);

            return CallResult;
        }

        public static int P_TrendChart_CJDLT_LH(string ConnectionString)
        {
            DataSet ds = null;

            return P_TrendChart_CJDLT_LH(ConnectionString, ref ds);
        }

        public static int P_TrendChart_CJDLT_LH(string ConnectionString, ref DataSet ds)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(ConnectionString, "P_TrendChart_CJDLT_LH", ref ds, ref Outputs);

            return CallResult;
        }

        public static int P_TrendChart_CJDLT_tema(string ConnectionString)
        {
            DataSet ds = null;

            return P_TrendChart_CJDLT_tema(ConnectionString, ref ds);
        }

        public static int P_TrendChart_CJDLT_tema(string ConnectionString, ref DataSet ds)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(ConnectionString, "P_TrendChart_CJDLT_tema", ref ds, ref Outputs);

            return CallResult;
        }

        public static int P_TrendChart_CJDLT_TeMa_WH(string ConnectionString)
        {
            DataSet ds = null;

            return P_TrendChart_CJDLT_TeMa_WH(ConnectionString, ref ds);
        }

        public static int P_TrendChart_CJDLT_TeMa_WH(string ConnectionString, ref DataSet ds)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(ConnectionString, "P_TrendChart_CJDLT_TeMa_WH", ref ds, ref Outputs);

            return CallResult;
        }

        public static int P_TrendChart_CJDLT_TemaYL(string ConnectionString, int IsuseNum)
        {
            DataSet ds = null;

            return P_TrendChart_CJDLT_TemaYL(ConnectionString, ref ds, IsuseNum);
        }

        public static int P_TrendChart_CJDLT_TemaYL(string ConnectionString, ref DataSet ds, int IsuseNum)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(ConnectionString, "P_TrendChart_CJDLT_TemaYL", ref ds, ref Outputs,
                new MSSQL.Parameter("IsuseNum", SqlDbType.Int, 0, ParameterDirection.Input, IsuseNum)
                );

            return CallResult;
        }

        public static int P_TrendChart_CJDLT_WH(string ConnectionString)
        {
            DataSet ds = null;

            return P_TrendChart_CJDLT_WH(ConnectionString, ref ds);
        }

        public static int P_TrendChart_CJDLT_WH(string ConnectionString, ref DataSet ds)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(ConnectionString, "P_TrendChart_CJDLT_WH", ref ds, ref Outputs);

            return CallResult;
        }

        public static int P_TrendChart_CJDLT_YS(string ConnectionString)
        {
            DataSet ds = null;

            return P_TrendChart_CJDLT_YS(ConnectionString, ref ds);
        }

        public static int P_TrendChart_CJDLT_YS(string ConnectionString, ref DataSet ds)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(ConnectionString, "P_TrendChart_CJDLT_YS", ref ds, ref Outputs);

            return CallResult;
        }

        public static int P_TrendChart_DF6J1_ZHFB(string ConnectionString)
        {
            DataSet ds = null;

            return P_TrendChart_DF6J1_ZHFB(ConnectionString, ref ds);
        }

        public static int P_TrendChart_DF6J1_ZHFB(string ConnectionString, ref DataSet ds)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(ConnectionString, "P_TrendChart_DF6J1_ZHFB", ref ds, ref Outputs);

            return CallResult;
        }

        public static int P_TrendChart_FC3D(string ConnectionString, int IsuseNum)
        {
            DataSet ds = null;

            return P_TrendChart_FC3D(ConnectionString, ref ds, IsuseNum);
        }

        public static int P_TrendChart_FC3D(string ConnectionString, ref DataSet ds, int IsuseNum)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(ConnectionString, "P_TrendChart_FC3D", ref ds, ref Outputs,
                new MSSQL.Parameter("IsuseNum", SqlDbType.Int, 0, ParameterDirection.Input, IsuseNum)
                );

            return CallResult;
        }

        public static int P_TrendChart_KLPK_012(string ConnectionString)
        {
            DataSet ds = null;

            return P_TrendChart_KLPK_012(ConnectionString, ref ds);
        }

        public static int P_TrendChart_KLPK_012(string ConnectionString, ref DataSet ds)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(ConnectionString, "P_TrendChart_KLPK_012", ref ds, ref Outputs);

            return CallResult;
        }

        public static int P_TrendChart_KLPK_DX(string ConnectionString)
        {
            DataSet ds = null;

            return P_TrendChart_KLPK_DX(ConnectionString, ref ds);
        }

        public static int P_TrendChart_KLPK_DX(string ConnectionString, ref DataSet ds)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(ConnectionString, "P_TrendChart_KLPK_DX", ref ds, ref Outputs);

            return CallResult;
        }

        public static int P_TrendChart_KLPK_DZX(string ConnectionString)
        {
            DataSet ds = null;

            return P_TrendChart_KLPK_DZX(ConnectionString, ref ds);
        }

        public static int P_TrendChart_KLPK_DZX(string ConnectionString, ref DataSet ds)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(ConnectionString, "P_TrendChart_KLPK_DZX", ref ds, ref Outputs);

            return CallResult;
        }

        public static int P_TrendChart_KLPK_KJFB(string ConnectionString)
        {
            DataSet ds = null;

            return P_TrendChart_KLPK_KJFB(ConnectionString, ref ds);
        }

        public static int P_TrendChart_KLPK_KJFB(string ConnectionString, ref DataSet ds)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(ConnectionString, "P_TrendChart_KLPK_KJFB", ref ds, ref Outputs);

            return CallResult;
        }

        public static int P_TrendChart_KLPK_ZH(string ConnectionString)
        {
            DataSet ds = null;

            return P_TrendChart_KLPK_ZH(ConnectionString, ref ds);
        }

        public static int P_TrendChart_KLPK_ZH(string ConnectionString, ref DataSet ds)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(ConnectionString, "P_TrendChart_KLPK_ZH", ref ds, ref Outputs);

            return CallResult;
        }

        public static int P_TrendChart_PL3(string ConnectionString)
        {
            DataSet ds = null;

            return P_TrendChart_PL3(ConnectionString, ref ds);
        }

        public static int P_TrendChart_PL3(string ConnectionString, ref DataSet ds)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(ConnectionString, "P_TrendChart_PL3", ref ds, ref Outputs);

            return CallResult;
        }

        public static int P_TrendChart_PL3_012(string ConnectionString, int IsuseNum)
        {
            DataSet ds = null;

            return P_TrendChart_PL3_012(ConnectionString, ref ds, IsuseNum);
        }

        public static int P_TrendChart_PL3_012(string ConnectionString, ref DataSet ds, int IsuseNum)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(ConnectionString, "P_TrendChart_PL3_012", ref ds, ref Outputs,
                new MSSQL.Parameter("IsuseNum", SqlDbType.Int, 0, ParameterDirection.Input, IsuseNum)
                );

            return CallResult;
        }

        public static int P_TrendChart_PL3_DX(string ConnectionString, int IsuseNum)
        {
            DataSet ds = null;

            return P_TrendChart_PL3_DX(ConnectionString, ref ds, IsuseNum);
        }

        public static int P_TrendChart_PL3_DX(string ConnectionString, ref DataSet ds, int IsuseNum)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(ConnectionString, "P_TrendChart_PL3_DX", ref ds, ref Outputs,
                new MSSQL.Parameter("IsuseNum", SqlDbType.Int, 0, ParameterDirection.Input, IsuseNum)
                );

            return CallResult;
        }

        public static int P_TrendChart_PL3_DZX(string ConnectionString, int IsuseNum)
        {
            DataSet ds = null;

            return P_TrendChart_PL3_DZX(ConnectionString, ref ds, IsuseNum);
        }

        public static int P_TrendChart_PL3_DZX(string ConnectionString, ref DataSet ds, int IsuseNum)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(ConnectionString, "P_TrendChart_PL3_DZX", ref ds, ref Outputs,
                new MSSQL.Parameter("IsuseNum", SqlDbType.Int, 0, ParameterDirection.Input, IsuseNum)
                );

            return CallResult;
        }

        public static int P_TrendChart_PL3_HMFB(string ConnectionString, int IsuseNum)
        {
            DataSet ds = null;

            return P_TrendChart_PL3_HMFB(ConnectionString, ref ds, IsuseNum);
        }

        public static int P_TrendChart_PL3_HMFB(string ConnectionString, ref DataSet ds, int IsuseNum)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(ConnectionString, "P_TrendChart_PL3_HMFB", ref ds, ref Outputs,
                new MSSQL.Parameter("IsuseNum", SqlDbType.Int, 0, ParameterDirection.Input, IsuseNum)
                );

            return CallResult;
        }

        public static int P_TrendChart_PL3_HZ(string ConnectionString, int IsuseNum)
        {
            DataSet ds = null;

            return P_TrendChart_PL3_HZ(ConnectionString, ref ds, IsuseNum);
        }

        public static int P_TrendChart_PL3_HZ(string ConnectionString, ref DataSet ds, int IsuseNum)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(ConnectionString, "P_TrendChart_PL3_HZ", ref ds, ref Outputs,
                new MSSQL.Parameter("IsuseNum", SqlDbType.Int, 0, ParameterDirection.Input, IsuseNum)
                );

            return CallResult;
        }

        public static int P_TrendChart_PL3_JO(string ConnectionString, int IsuseNum)
        {
            DataSet ds = null;

            return P_TrendChart_PL3_JO(ConnectionString, ref ds, IsuseNum);
        }

        public static int P_TrendChart_PL3_JO(string ConnectionString, ref DataSet ds, int IsuseNum)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(ConnectionString, "P_TrendChart_PL3_JO", ref ds, ref Outputs,
                new MSSQL.Parameter("IsuseNum", SqlDbType.Int, 0, ParameterDirection.Input, IsuseNum)
                );

            return CallResult;
        }

        public static int P_TrendChart_PL3_KD(string ConnectionString, int IsuseNum)
        {
            DataSet ds = null;

            return P_TrendChart_PL3_KD(ConnectionString, ref ds, IsuseNum);
        }

        public static int P_TrendChart_PL3_KD(string ConnectionString, ref DataSet ds, int IsuseNum)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(ConnectionString, "P_TrendChart_PL3_KD", ref ds, ref Outputs,
                new MSSQL.Parameter("IsuseNum", SqlDbType.Int, 0, ParameterDirection.Input, IsuseNum)
                );

            return CallResult;
        }

        public static int P_TrendChart_PL3_LH(string ConnectionString)
        {
            DataSet ds = null;

            return P_TrendChart_PL3_LH(ConnectionString, ref ds);
        }

        public static int P_TrendChart_PL3_LH(string ConnectionString, ref DataSet ds)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(ConnectionString, "P_TrendChart_PL3_LH", ref ds, ref Outputs);

            return CallResult;
        }

        public static int P_TrendChart_PL3_WH(string ConnectionString, int IsuseNum)
        {
            DataSet ds = null;

            return P_TrendChart_PL3_WH(ConnectionString, ref ds, IsuseNum);
        }

        public static int P_TrendChart_PL3_WH(string ConnectionString, ref DataSet ds, int IsuseNum)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(ConnectionString, "P_TrendChart_PL3_WH", ref ds, ref Outputs,
                new MSSQL.Parameter("IsuseNum", SqlDbType.Int, 0, ParameterDirection.Input, IsuseNum)
                );

            return CallResult;
        }

        public static int P_TrendChart_PL3_YS(string ConnectionString)
        {
            DataSet ds = null;

            return P_TrendChart_PL3_YS(ConnectionString, ref ds);
        }

        public static int P_TrendChart_PL3_YS(string ConnectionString, ref DataSet ds)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(ConnectionString, "P_TrendChart_PL3_YS", ref ds, ref Outputs);

            return CallResult;
        }

        public static int P_TrendChart_PL3_ZH(string ConnectionString, int IsuseNum)
        {
            DataSet ds = null;

            return P_TrendChart_PL3_ZH(ConnectionString, ref ds, IsuseNum);
        }

        public static int P_TrendChart_PL3_ZH(string ConnectionString, ref DataSet ds, int IsuseNum)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(ConnectionString, "P_TrendChart_PL3_ZH", ref ds, ref Outputs,
                new MSSQL.Parameter("IsuseNum", SqlDbType.Int, 0, ParameterDirection.Input, IsuseNum)
                );

            return CallResult;
        }

        public static int P_TrendChart_PL3_ZX(string ConnectionString, int IsuseNum)
        {
            DataSet ds = null;

            return P_TrendChart_PL3_ZX(ConnectionString, ref ds, IsuseNum);
        }

        public static int P_TrendChart_PL3_ZX(string ConnectionString, ref DataSet ds, int IsuseNum)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(ConnectionString, "P_TrendChart_PL3_ZX", ref ds, ref Outputs,
                new MSSQL.Parameter("IsuseNum", SqlDbType.Int, 0, ParameterDirection.Input, IsuseNum)
                );

            return CallResult;
        }

        public static int P_TrendChart_PL5_012(string ConnectionString)
        {
            DataSet ds = null;

            return P_TrendChart_PL5_012(ConnectionString, ref ds);
        }

        public static int P_TrendChart_PL5_012(string ConnectionString, ref DataSet ds)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(ConnectionString, "P_TrendChart_PL5_012", ref ds, ref Outputs);

            return CallResult;
        }

        public static int P_TrendChart_PL5_CF(string ConnectionString)
        {
            DataSet ds = null;

            return P_TrendChart_PL5_CF(ConnectionString, ref ds);
        }

        public static int P_TrendChart_PL5_CF(string ConnectionString, ref DataSet ds)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(ConnectionString, "P_TrendChart_PL5_CF", ref ds, ref Outputs);

            return CallResult;
        }

        public static int P_TrendChart_PL5_DX(string ConnectionString)
        {
            DataSet ds = null;

            return P_TrendChart_PL5_DX(ConnectionString, ref ds);
        }

        public static int P_TrendChart_PL5_DX(string ConnectionString, ref DataSet ds)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(ConnectionString, "P_TrendChart_PL5_DX", ref ds, ref Outputs);

            return CallResult;
        }

        public static int P_TrendChart_PL5_DZX(string ConnectionString)
        {
            DataSet ds = null;

            return P_TrendChart_PL5_DZX(ConnectionString, ref ds);
        }

        public static int P_TrendChart_PL5_DZX(string ConnectionString, ref DataSet ds)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(ConnectionString, "P_TrendChart_PL5_DZX", ref ds, ref Outputs);

            return CallResult;
        }

        public static int P_TrendChart_PL5_HMFB(string ConnectionString, int IsuseNum)
        {
            DataSet ds = null;

            return P_TrendChart_PL5_HMFB(ConnectionString, ref ds, IsuseNum);
        }

        public static int P_TrendChart_PL5_HMFB(string ConnectionString, ref DataSet ds, int IsuseNum)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(ConnectionString, "P_TrendChart_PL5_HMFB", ref ds, ref Outputs,
                new MSSQL.Parameter("IsuseNum", SqlDbType.Int, 0, ParameterDirection.Input, IsuseNum)
                );

            return CallResult;
        }

        public static int P_TrendChart_PL5_HZ(string ConnectionString, int IsuseNum)
        {
            DataSet ds = null;

            return P_TrendChart_PL5_HZ(ConnectionString, ref ds, IsuseNum);
        }

        public static int P_TrendChart_PL5_HZ(string ConnectionString, ref DataSet ds, int IsuseNum)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(ConnectionString, "P_TrendChart_PL5_HZ", ref ds, ref Outputs,
                new MSSQL.Parameter("IsuseNum", SqlDbType.Int, 0, ParameterDirection.Input, IsuseNum)
                );

            return CallResult;
        }

        public static int P_TrendChart_PL5_JO(string ConnectionString)
        {
            DataSet ds = null;

            return P_TrendChart_PL5_JO(ConnectionString, ref ds);
        }

        public static int P_TrendChart_PL5_JO(string ConnectionString, ref DataSet ds)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(ConnectionString, "P_TrendChart_PL5_JO", ref ds, ref Outputs);

            return CallResult;
        }

        public static int P_TrendChart_PL5_LH(string ConnectionString)
        {
            DataSet ds = null;

            return P_TrendChart_PL5_LH(ConnectionString, ref ds);
        }

        public static int P_TrendChart_PL5_LH(string ConnectionString, ref DataSet ds)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(ConnectionString, "P_TrendChart_PL5_LH", ref ds, ref Outputs);

            return CallResult;
        }

        public static int P_TrendChart_PL5_YS(string ConnectionString)
        {
            DataSet ds = null;

            return P_TrendChart_PL5_YS(ConnectionString, ref ds);
        }

        public static int P_TrendChart_PL5_YS(string ConnectionString, ref DataSet ds)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(ConnectionString, "P_TrendChart_PL5_YS", ref ds, ref Outputs);

            return CallResult;
        }

        public static int P_TrendChart_PL5_ZH(string ConnectionString)
        {
            DataSet ds = null;

            return P_TrendChart_PL5_ZH(ConnectionString, ref ds);
        }

        public static int P_TrendChart_PL5_ZH(string ConnectionString, ref DataSet ds)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(ConnectionString, "P_TrendChart_PL5_ZH", ref ds, ref Outputs);

            return CallResult;
        }

        public static int P_TrendChart_SHSSL_012(string ConnectionString)
        {
            DataSet ds = null;

            return P_TrendChart_SHSSL_012(ConnectionString, ref ds);
        }

        public static int P_TrendChart_SHSSL_012(string ConnectionString, ref DataSet ds)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(ConnectionString, "P_TrendChart_SHSSL_012", ref ds, ref Outputs);

            return CallResult;
        }

        public static int P_TrendChart_SHSSL_DX(string ConnectionString)
        {
            DataSet ds = null;

            return P_TrendChart_SHSSL_DX(ConnectionString, ref ds);
        }

        public static int P_TrendChart_SHSSL_DX(string ConnectionString, ref DataSet ds)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(ConnectionString, "P_TrendChart_SHSSL_DX", ref ds, ref Outputs);

            return CallResult;
        }

        public static int P_TrendChart_SHSSL_HZ(string ConnectionString)
        {
            DataSet ds = null;

            return P_TrendChart_SHSSL_HZ(ConnectionString, ref ds);
        }

        public static int P_TrendChart_SHSSL_HZ(string ConnectionString, ref DataSet ds)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(ConnectionString, "P_TrendChart_SHSSL_HZ", ref ds, ref Outputs);

            return CallResult;
        }

        public static int P_TrendChart_SHSSL_JO(string ConnectionString)
        {
            DataSet ds = null;

            return P_TrendChart_SHSSL_JO(ConnectionString, ref ds);
        }

        public static int P_TrendChart_SHSSL_JO(string ConnectionString, ref DataSet ds)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(ConnectionString, "P_TrendChart_SHSSL_JO", ref ds, ref Outputs);

            return CallResult;
        }

        public static int P_TrendChart_SHSSL_ZH(string ConnectionString)
        {
            DataSet ds = null;

            return P_TrendChart_SHSSL_ZH(ConnectionString, ref ds);
        }

        public static int P_TrendChart_SHSSL_ZH(string ConnectionString, ref DataSet ds)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(ConnectionString, "P_TrendChart_SHSSL_ZH", ref ds, ref Outputs);

            return CallResult;
        }

        public static int P_TrendChart_SHSSL_ZHFB(string ConnectionString)
        {
            DataSet ds = null;

            return P_TrendChart_SHSSL_ZHFB(ConnectionString, ref ds);
        }

        public static int P_TrendChart_SHSSL_ZHFB(string ConnectionString, ref DataSet ds)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(ConnectionString, "P_TrendChart_SHSSL_ZHFB", ref ds, ref Outputs);

            return CallResult;
        }

        public static int P_TrendChart_SSC_2X_012_ZST(string ConnectionString)
        {
            DataSet ds = null;

            return P_TrendChart_SSC_2X_012_ZST(ConnectionString, ref ds);
        }

        public static int P_TrendChart_SSC_2X_012_ZST(string ConnectionString, ref DataSet ds)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(ConnectionString, "P_TrendChart_SSC_2X_012_ZST", ref ds, ref Outputs);

            return CallResult;
        }

        public static int P_TrendChart_SSC_2XDXDSZST(string ConnectionString)
        {
            DataSet ds = null;

            return P_TrendChart_SSC_2XDXDSZST(ConnectionString, ref ds);
        }

        public static int P_TrendChart_SSC_2XDXDSZST(string ConnectionString, ref DataSet ds)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(ConnectionString, "P_TrendChart_SSC_2XDXDSZST", ref ds, ref Outputs);

            return CallResult;
        }

        public static int P_TrendChart_SSC_2XHZWZST(string ConnectionString)
        {
            DataSet ds = null;

            return P_TrendChart_SSC_2XHZWZST(ConnectionString, ref ds);
        }

        public static int P_TrendChart_SSC_2XHZWZST(string ConnectionString, ref DataSet ds)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(ConnectionString, "P_TrendChart_SSC_2XHZWZST", ref ds, ref Outputs);

            return CallResult;
        }

        public static int P_TrendChart_SSC_2XHZZST(string ConnectionString)
        {
            DataSet ds = null;

            return P_TrendChart_SSC_2XHZZST(ConnectionString, ref ds);
        }

        public static int P_TrendChart_SSC_2XHZZST(string ConnectionString, ref DataSet ds)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(ConnectionString, "P_TrendChart_SSC_2XHZZST", ref ds, ref Outputs);

            return CallResult;
        }

        public static int P_TrendChart_SSC_2XKDZST(string ConnectionString)
        {
            DataSet ds = null;

            return P_TrendChart_SSC_2XKDZST(ConnectionString, ref ds);
        }

        public static int P_TrendChart_SSC_2XKDZST(string ConnectionString, ref DataSet ds)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(ConnectionString, "P_TrendChart_SSC_2XKDZST", ref ds, ref Outputs);

            return CallResult;
        }

        public static int P_TrendChart_SSC_2XMaxZST(string ConnectionString)
        {
            DataSet ds = null;

            return P_TrendChart_SSC_2XMaxZST(ConnectionString, ref ds);
        }

        public static int P_TrendChart_SSC_2XMaxZST(string ConnectionString, ref DataSet ds)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(ConnectionString, "P_TrendChart_SSC_2XMaxZST", ref ds, ref Outputs);

            return CallResult;
        }

        public static int P_TrendChart_SSC_2XMINZST(string ConnectionString)
        {
            DataSet ds = null;

            return P_TrendChart_SSC_2XMINZST(ConnectionString, ref ds);
        }

        public static int P_TrendChart_SSC_2XMINZST(string ConnectionString, ref DataSet ds)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(ConnectionString, "P_TrendChart_SSC_2XMINZST", ref ds, ref Outputs);

            return CallResult;
        }

        public static int P_TrendChart_SSC_2XPJZZST(string ConnectionString)
        {
            DataSet ds = null;

            return P_TrendChart_SSC_2XPJZZST(ConnectionString, ref ds);
        }

        public static int P_TrendChart_SSC_2XPJZZST(string ConnectionString, ref DataSet ds)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(ConnectionString, "P_TrendChart_SSC_2XPJZZST", ref ds, ref Outputs);

            return CallResult;
        }

        public static int P_TrendChart_SSC_2XZHFBZST(string ConnectionString)
        {
            DataSet ds = null;

            return P_TrendChart_SSC_2XZHFBZST(ConnectionString, ref ds);
        }

        public static int P_TrendChart_SSC_2XZHFBZST(string ConnectionString, ref DataSet ds)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(ConnectionString, "P_TrendChart_SSC_2XZHFBZST", ref ds, ref Outputs);

            return CallResult;
        }

        public static int P_TrendChart_SSC_3X_DX012_ZST(string ConnectionString)
        {
            DataSet ds = null;

            return P_TrendChart_SSC_3X_DX012_ZST(ConnectionString, ref ds);
        }

        public static int P_TrendChart_SSC_3X_DX012_ZST(string ConnectionString, ref DataSet ds)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(ConnectionString, "P_TrendChart_SSC_3X_DX012_ZST", ref ds, ref Outputs);

            return CallResult;
        }

        public static int P_TrendChart_SSC_3X_ZX012_ZST(string ConnectionString)
        {
            DataSet ds = null;

            return P_TrendChart_SSC_3X_ZX012_ZST(ConnectionString, ref ds);
        }

        public static int P_TrendChart_SSC_3X_ZX012_ZST(string ConnectionString, ref DataSet ds)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(ConnectionString, "P_TrendChart_SSC_3X_ZX012_ZST", ref ds, ref Outputs);

            return CallResult;
        }

        public static int P_TrendChart_SSC_3XDXZST(string ConnectionString)
        {
            DataSet ds = null;

            return P_TrendChart_SSC_3XDXZST(ConnectionString, ref ds);
        }

        public static int P_TrendChart_SSC_3XDXZST(string ConnectionString, ref DataSet ds)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(ConnectionString, "P_TrendChart_SSC_3XDXZST", ref ds, ref Outputs);

            return CallResult;
        }

        public static int P_TrendChart_SSC_3XHZWZST(string ConnectionString)
        {
            DataSet ds = null;

            return P_TrendChart_SSC_3XHZWZST(ConnectionString, ref ds);
        }

        public static int P_TrendChart_SSC_3XHZWZST(string ConnectionString, ref DataSet ds)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(ConnectionString, "P_TrendChart_SSC_3XHZWZST", ref ds, ref Outputs);

            return CallResult;
        }

        public static int P_TrendChart_SSC_3XHZZST(string ConnectionString)
        {
            DataSet ds = null;

            return P_TrendChart_SSC_3XHZZST(ConnectionString, ref ds);
        }

        public static int P_TrendChart_SSC_3XHZZST(string ConnectionString, ref DataSet ds)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(ConnectionString, "P_TrendChart_SSC_3XHZZST", ref ds, ref Outputs);

            return CallResult;
        }

        public static int P_TrendChart_SSC_3XJOZST(string ConnectionString)
        {
            DataSet ds = null;

            return P_TrendChart_SSC_3XJOZST(ConnectionString, ref ds);
        }

        public static int P_TrendChart_SSC_3XJOZST(string ConnectionString, ref DataSet ds)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(ConnectionString, "P_TrendChart_SSC_3XJOZST", ref ds, ref Outputs);

            return CallResult;
        }

        public static int P_TrendChart_SSC_3XKDZST(string ConnectionString)
        {
            DataSet ds = null;

            return P_TrendChart_SSC_3XKDZST(ConnectionString, ref ds);
        }

        public static int P_TrendChart_SSC_3XKDZST(string ConnectionString, ref DataSet ds)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(ConnectionString, "P_TrendChart_SSC_3XKDZST", ref ds, ref Outputs);

            return CallResult;
        }

        public static int P_TrendChart_SSC_3XPJZZST(string ConnectionString)
        {
            DataSet ds = null;

            return P_TrendChart_SSC_3XPJZZST(ConnectionString, ref ds);
        }

        public static int P_TrendChart_SSC_3XPJZZST(string ConnectionString, ref DataSet ds)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(ConnectionString, "P_TrendChart_SSC_3XPJZZST", ref ds, ref Outputs);

            return CallResult;
        }

        public static int P_TrendChart_SSC_3XZHFBZST(string ConnectionString)
        {
            DataSet ds = null;

            return P_TrendChart_SSC_3XZHFBZST(ConnectionString, ref ds);
        }

        public static int P_TrendChart_SSC_3XZHFBZST(string ConnectionString, ref DataSet ds)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(ConnectionString, "P_TrendChart_SSC_3XZHFBZST", ref ds, ref Outputs);

            return CallResult;
        }

        public static int P_TrendChart_SSC_3XZHZST(string ConnectionString)
        {
            DataSet ds = null;

            return P_TrendChart_SSC_3XZHZST(ConnectionString, ref ds);
        }

        public static int P_TrendChart_SSC_3XZHZST(string ConnectionString, ref DataSet ds)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(ConnectionString, "P_TrendChart_SSC_3XZHZST", ref ds, ref Outputs);

            return CallResult;
        }

        public static int P_TrendChart_SSC_3XZST(string ConnectionString)
        {
            DataSet ds = null;

            return P_TrendChart_SSC_3XZST(ConnectionString, ref ds);
        }

        public static int P_TrendChart_SSC_3XZST(string ConnectionString, ref DataSet ds)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(ConnectionString, "P_TrendChart_SSC_3XZST", ref ds, ref Outputs);

            return CallResult;
        }

        public static int P_TrendChart_SSC_4XDXZST(string ConnectionString)
        {
            DataSet ds = null;

            return P_TrendChart_SSC_4XDXZST(ConnectionString, ref ds);
        }

        public static int P_TrendChart_SSC_4XDXZST(string ConnectionString, ref DataSet ds)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(ConnectionString, "P_TrendChart_SSC_4XDXZST", ref ds, ref Outputs);

            return CallResult;
        }

        public static int P_TrendChart_SSC_4XHZZST(string ConnectionString)
        {
            DataSet ds = null;

            return P_TrendChart_SSC_4XHZZST(ConnectionString, ref ds);
        }

        public static int P_TrendChart_SSC_4XHZZST(string ConnectionString, ref DataSet ds)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(ConnectionString, "P_TrendChart_SSC_4XHZZST", ref ds, ref Outputs);

            return CallResult;
        }

        public static int P_TrendChart_SSC_4XJOZST(string ConnectionString)
        {
            DataSet ds = null;

            return P_TrendChart_SSC_4XJOZST(ConnectionString, ref ds);
        }

        public static int P_TrendChart_SSC_4XJOZST(string ConnectionString, ref DataSet ds)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(ConnectionString, "P_TrendChart_SSC_4XJOZST", ref ds, ref Outputs);

            return CallResult;
        }

        public static int P_TrendChart_SSC_4XKDZST(string ConnectionString)
        {
            DataSet ds = null;

            return P_TrendChart_SSC_4XKDZST(ConnectionString, ref ds);
        }

        public static int P_TrendChart_SSC_4XKDZST(string ConnectionString, ref DataSet ds)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(ConnectionString, "P_TrendChart_SSC_4XKDZST", ref ds, ref Outputs);

            return CallResult;
        }

        public static int P_TrendChart_SSC_4XPJZZST(string ConnectionString)
        {
            DataSet ds = null;

            return P_TrendChart_SSC_4XPJZZST(ConnectionString, ref ds);
        }

        public static int P_TrendChart_SSC_4XPJZZST(string ConnectionString, ref DataSet ds)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(ConnectionString, "P_TrendChart_SSC_4XPJZZST", ref ds, ref Outputs);

            return CallResult;
        }

        public static int P_TrendChart_SSC_4XZHFBZST(string ConnectionString)
        {
            DataSet ds = null;

            return P_TrendChart_SSC_4XZHFBZST(ConnectionString, ref ds);
        }

        public static int P_TrendChart_SSC_4XZHFBZST(string ConnectionString, ref DataSet ds)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(ConnectionString, "P_TrendChart_SSC_4XZHFBZST", ref ds, ref Outputs);

            return CallResult;
        }

        public static int P_TrendChart_SSC_4XZHZST(string ConnectionString)
        {
            DataSet ds = null;

            return P_TrendChart_SSC_4XZHZST(ConnectionString, ref ds);
        }

        public static int P_TrendChart_SSC_4XZHZST(string ConnectionString, ref DataSet ds)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(ConnectionString, "P_TrendChart_SSC_4XZHZST", ref ds, ref Outputs);

            return CallResult;
        }

        public static int P_TrendChart_SSC_4XZST(string ConnectionString)
        {
            DataSet ds = null;

            return P_TrendChart_SSC_4XZST(ConnectionString, ref ds);
        }

        public static int P_TrendChart_SSC_4XZST(string ConnectionString, ref DataSet ds)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(ConnectionString, "P_TrendChart_SSC_4XZST", ref ds, ref Outputs);

            return CallResult;
        }

        public static int P_TrendChart_SSC_5XDXZST(string ConnectionString)
        {
            DataSet ds = null;

            return P_TrendChart_SSC_5XDXZST(ConnectionString, ref ds);
        }

        public static int P_TrendChart_SSC_5XDXZST(string ConnectionString, ref DataSet ds)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(ConnectionString, "P_TrendChart_SSC_5XDXZST", ref ds, ref Outputs);

            return CallResult;
        }

        public static int P_TrendChart_SSC_5XHZZST(string ConnectionString)
        {
            DataSet ds = null;

            return P_TrendChart_SSC_5XHZZST(ConnectionString, ref ds);
        }

        public static int P_TrendChart_SSC_5XHZZST(string ConnectionString, ref DataSet ds)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(ConnectionString, "P_TrendChart_SSC_5XHZZST", ref ds, ref Outputs);

            return CallResult;
        }

        public static int P_TrendChart_SSC_5XJOZST(string ConnectionString)
        {
            DataSet ds = null;

            return P_TrendChart_SSC_5XJOZST(ConnectionString, ref ds);
        }

        public static int P_TrendChart_SSC_5XJOZST(string ConnectionString, ref DataSet ds)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(ConnectionString, "P_TrendChart_SSC_5XJOZST", ref ds, ref Outputs);

            return CallResult;
        }

        public static int P_TrendChart_SSC_5XKDZST(string ConnectionString)
        {
            DataSet ds = null;

            return P_TrendChart_SSC_5XKDZST(ConnectionString, ref ds);
        }

        public static int P_TrendChart_SSC_5XKDZST(string ConnectionString, ref DataSet ds)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(ConnectionString, "P_TrendChart_SSC_5XKDZST", ref ds, ref Outputs);

            return CallResult;
        }

        public static int P_TrendChart_SSC_5XPJZZST(string ConnectionString)
        {
            DataSet ds = null;

            return P_TrendChart_SSC_5XPJZZST(ConnectionString, ref ds);
        }

        public static int P_TrendChart_SSC_5XPJZZST(string ConnectionString, ref DataSet ds)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(ConnectionString, "P_TrendChart_SSC_5XPJZZST", ref ds, ref Outputs);

            return CallResult;
        }

        public static int P_TrendChart_SSC_5XZHFBZST(string ConnectionString)
        {
            DataSet ds = null;

            return P_TrendChart_SSC_5XZHFBZST(ConnectionString, ref ds);
        }

        public static int P_TrendChart_SSC_5XZHFBZST(string ConnectionString, ref DataSet ds)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(ConnectionString, "P_TrendChart_SSC_5XZHFBZST", ref ds, ref Outputs);

            return CallResult;
        }

        public static int P_TrendChart_SSC_5XZHZST(string ConnectionString)
        {
            DataSet ds = null;

            return P_TrendChart_SSC_5XZHZST(ConnectionString, ref ds);
        }

        public static int P_TrendChart_SSC_5XZHZST(string ConnectionString, ref DataSet ds)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(ConnectionString, "P_TrendChart_SSC_5XZHZST", ref ds, ref Outputs);

            return CallResult;
        }

        public static int P_TrendChart_SSC_5XZST(string ConnectionString)
        {
            DataSet ds = null;

            return P_TrendChart_SSC_5XZST(ConnectionString, ref ds);
        }

        public static int P_TrendChart_SSC_5XZST(string ConnectionString, ref DataSet ds)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(ConnectionString, "P_TrendChart_SSC_5XZST", ref ds, ref Outputs);

            return CallResult;
        }

        public static int P_TrendChart_SSQ_3FQ(string ConnectionString)
        {
            DataSet ds = null;

            return P_TrendChart_SSQ_3FQ(ConnectionString, ref ds);
        }

        public static int P_TrendChart_SSQ_3FQ(string ConnectionString, ref DataSet ds)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(ConnectionString, "P_TrendChart_SSQ_3FQ", ref ds, ref Outputs);

            return CallResult;
        }

        public static int P_TrendChart_SSQ_BQZH(string ConnectionString)
        {
            DataSet ds = null;

            return P_TrendChart_SSQ_BQZH(ConnectionString, ref ds);
        }

        public static int P_TrendChart_SSQ_BQZH(string ConnectionString, ref DataSet ds)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(ConnectionString, "P_TrendChart_SSQ_BQZH", ref ds, ref Outputs);

            return CallResult;
        }

        public static int P_TrendChart_SSQ_CGXMB(string ConnectionString)
        {
            DataSet ds = null;

            return P_TrendChart_SSQ_CGXMB(ConnectionString, ref ds);
        }

        public static int P_TrendChart_SSQ_CGXMB(string ConnectionString, ref DataSet ds)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(ConnectionString, "P_TrendChart_SSQ_CGXMB", ref ds, ref Outputs);

            return CallResult;
        }

        public static int P_TrendChart_SSQ_DX(string ConnectionString)
        {
            DataSet ds = null;

            return P_TrendChart_SSQ_DX(ConnectionString, ref ds);
        }

        public static int P_TrendChart_SSQ_DX(string ConnectionString, ref DataSet ds)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(ConnectionString, "P_TrendChart_SSQ_DX", ref ds, ref Outputs);

            return CallResult;
        }

        public static int P_TrendChart_SSQ_HL(string ConnectionString)
        {
            DataSet ds = null;

            return P_TrendChart_SSQ_HL(ConnectionString, ref ds);
        }

        public static int P_TrendChart_SSQ_HL(string ConnectionString, ref DataSet ds)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(ConnectionString, "P_TrendChart_SSQ_HL", ref ds, ref Outputs);

            return CallResult;
        }

        public static int P_TrendChart_SSQ_HMFB(string ConnectionString)
        {
            DataSet ds = null;

            return P_TrendChart_SSQ_HMFB(ConnectionString, ref ds);
        }

        public static int P_TrendChart_SSQ_HMFB(string ConnectionString, ref DataSet ds)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(ConnectionString, "P_TrendChart_SSQ_HMFB", ref ds, ref Outputs);

            return CallResult;
        }

        public static int P_TrendChart_SSQ_JO(string ConnectionString)
        {
            DataSet ds = null;

            return P_TrendChart_SSQ_JO(ConnectionString, ref ds);
        }

        public static int P_TrendChart_SSQ_JO(string ConnectionString, ref DataSet ds)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(ConnectionString, "P_TrendChart_SSQ_JO", ref ds, ref Outputs);

            return CallResult;
        }

        public static int P_TrendChart_SSQ_ZH(string ConnectionString)
        {
            DataSet ds = null;

            return P_TrendChart_SSQ_ZH(ConnectionString, ref ds);
        }

        public static int P_TrendChart_SSQ_ZH(string ConnectionString, ref DataSet ds)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(ConnectionString, "P_TrendChart_SSQ_ZH", ref ds, ref Outputs);

            return CallResult;
        }

        public static int P_UserAdd(string ConnectionString, long SiteID, string Name, string RealityName, string Password, string PasswordAdv, int CityID, string Sex, DateTime BirthDay, string IDCardNumber, string Address, string Email, bool isEmailValided, string QQ, string Telephone, string Mobile, bool isMobileValided, bool isPrivacy, short UserType, short BankType, string BankName, string BankCardNumber, long CommenderID, long CpsID, string AlipayName, string Memo, string VisitSource, ref long UserID, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_UserAdd(ConnectionString, ref ds, SiteID, Name, RealityName, Password, PasswordAdv, CityID, Sex, BirthDay, IDCardNumber, Address, Email, isEmailValided, QQ, Telephone, Mobile, isMobileValided, isPrivacy, UserType, BankType, BankName, BankCardNumber, CommenderID, CpsID, AlipayName, Memo, VisitSource, ref UserID, ref ReturnDescription);
        }

        public static int P_UserAdd(string ConnectionString, ref DataSet ds, long SiteID, string Name, string RealityName, string Password, string PasswordAdv, int CityID, string Sex, DateTime BirthDay, string IDCardNumber, string Address, string Email, bool isEmailValided, string QQ, string Telephone, string Mobile, bool isMobileValided, bool isPrivacy, short UserType, short BankType, string BankName, string BankCardNumber, long CommenderID, long CpsID, string AlipayName, string Memo, string VisitSource, ref long UserID, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(ConnectionString, "P_UserAdd", ref ds, ref Outputs,
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

        public static int P_UserAddMoney(string ConnectionString, long SiteID, long UserID, double Money, double FormalitiesFees, string PayNumber, string PayBank, string Memo, ref int ReturnValue, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_UserAddMoney(ConnectionString, ref ds, SiteID, UserID, Money, FormalitiesFees, PayNumber, PayBank, Memo, ref ReturnValue, ref ReturnDescription);
        }

        public static int P_UserAddMoney(string ConnectionString, ref DataSet ds, long SiteID, long UserID, double Money, double FormalitiesFees, string PayNumber, string PayBank, string Memo, ref int ReturnValue, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(ConnectionString, "P_UserAddMoney", ref ds, ref Outputs,
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

        public static int P_UserAddMoneyManual(string ConnectionString, long SiteID, long UserID, double Money, string Memo, long OperatorID, ref int ReturnValue, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_UserAddMoneyManual(ConnectionString, ref ds, SiteID, UserID, Money, Memo, OperatorID, ref ReturnValue, ref ReturnDescription);
        }

        public static int P_UserAddMoneyManual(string ConnectionString, ref DataSet ds, long SiteID, long UserID, double Money, string Memo, long OperatorID, ref int ReturnValue, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(ConnectionString, "P_UserAddMoneyManual", ref ds, ref Outputs,
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

        public static int P_UserBankDetailEdit(string ConnectionString, long SiteID, long UserID, string BankTypeName, string BankName, string BankCardNumber, string BankInProvinceName, string BankInCityName, string BankUserName, ref int ReturnValue, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_UserBankDetailEdit(ConnectionString, ref ds, SiteID, UserID, BankTypeName, BankName, BankCardNumber, BankInProvinceName, BankInCityName, BankUserName, ref ReturnValue, ref ReturnDescription);
        }

        public static int P_UserBankDetailEdit(string ConnectionString, ref DataSet ds, long SiteID, long UserID, string BankTypeName, string BankName, string BankCardNumber, string BankInProvinceName, string BankInCityName, string BankUserName, ref int ReturnValue, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(ConnectionString, "P_UserBankDetailEdit", ref ds, ref Outputs,
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

        public static int P_UserDistillPayByAlipay(string ConnectionString, long HandleOperatorID, string FileName, string IDs, int PaymentType, ref int ReturnValue, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_UserDistillPayByAlipay(ConnectionString, ref ds, HandleOperatorID, FileName, IDs, PaymentType, ref ReturnValue, ref ReturnDescription);
        }

        public static int P_UserDistillPayByAlipay(string ConnectionString, ref DataSet ds, long HandleOperatorID, string FileName, string IDs, int PaymentType, ref int ReturnValue, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(ConnectionString, "P_UserDistillPayByAlipay", ref ds, ref Outputs,
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

        public static int P_UserDistillPayByAlipaySuccess(string ConnectionString, long SiteID, long DistillID, long HandleOperatorID, ref int ReturnValue, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_UserDistillPayByAlipaySuccess(ConnectionString, ref ds, SiteID, DistillID, HandleOperatorID, ref ReturnValue, ref ReturnDescription);
        }

        public static int P_UserDistillPayByAlipaySuccess(string ConnectionString, ref DataSet ds, long SiteID, long DistillID, long HandleOperatorID, ref int ReturnValue, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(ConnectionString, "P_UserDistillPayByAlipaySuccess", ref ds, ref Outputs,
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

        public static int P_UserDistillPayByAlipayUnsuccess(string ConnectionString, long SiteID, long DistillID, string Memo, ref int ReturnValue, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_UserDistillPayByAlipayUnsuccess(ConnectionString, ref ds, SiteID, DistillID, Memo, ref ReturnValue, ref ReturnDescription);
        }

        public static int P_UserDistillPayByAlipayUnsuccess(string ConnectionString, ref DataSet ds, long SiteID, long DistillID, string Memo, ref int ReturnValue, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(ConnectionString, "P_UserDistillPayByAlipayUnsuccess", ref ds, ref Outputs,
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

        public static int P_UserDistillPayByAlipayWriteLog(string ConnectionString, string Content)
        {
            DataSet ds = null;

            return P_UserDistillPayByAlipayWriteLog(ConnectionString, ref ds, Content);
        }

        public static int P_UserDistillPayByAlipayWriteLog(string ConnectionString, ref DataSet ds, string Content)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(ConnectionString, "P_UserDistillPayByAlipayWriteLog", ref ds, ref Outputs,
                new MSSQL.Parameter("Content", SqlDbType.VarChar, 0, ParameterDirection.Input, Content)
                );

            return CallResult;
        }

        public static int P_UserEditByID(string ConnectionString, long SiteID, long UserID, string Name, string RealityName, string Password, string PasswordAdv, int CityID, string Sex, DateTime BirthDay, string IDCardNumber, string Address, string Email, bool isEmailValided, string QQ, string Telephone, string Mobile, bool isMobileValided, bool isPrivacy, bool isCanLogin, short UserType, short BankType, string BankName, string BankCardNumber, double ScoringOfSelfBuy, double ScoringOfCommendBuy, short Level, string AlipayID, string AlipayName, bool isAlipayNameValided, double PromotionMemberBonusScale, double PromotionSiteBonusScale, bool IsCrossLogin, ref int ReturnValue, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_UserEditByID(ConnectionString, ref ds, SiteID, UserID, Name, RealityName, Password, PasswordAdv, CityID, Sex, BirthDay, IDCardNumber, Address, Email, isEmailValided, QQ, Telephone, Mobile, isMobileValided, isPrivacy, isCanLogin, UserType, BankType, BankName, BankCardNumber, ScoringOfSelfBuy, ScoringOfCommendBuy, Level, AlipayID, AlipayName, isAlipayNameValided, PromotionMemberBonusScale, PromotionSiteBonusScale, IsCrossLogin, ref ReturnValue, ref ReturnDescription);
        }

        public static int P_UserEditByID(string ConnectionString, ref DataSet ds, long SiteID, long UserID, string Name, string RealityName, string Password, string PasswordAdv, int CityID, string Sex, DateTime BirthDay, string IDCardNumber, string Address, string Email, bool isEmailValided, string QQ, string Telephone, string Mobile, bool isMobileValided, bool isPrivacy, bool isCanLogin, short UserType, short BankType, string BankName, string BankCardNumber, double ScoringOfSelfBuy, double ScoringOfCommendBuy, short Level, string AlipayID, string AlipayName, bool isAlipayNameValided, double PromotionMemberBonusScale, double PromotionSiteBonusScale, bool IsCrossLogin, ref int ReturnValue, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(ConnectionString, "P_UserEditByID", ref ds, ref Outputs,
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
                new MSSQL.Parameter("Telephone", SqlDbType.VarChar, 0, ParameterDirection.Input, Telephone),
                new MSSQL.Parameter("Mobile", SqlDbType.VarChar, 0, ParameterDirection.Input, Mobile),
                new MSSQL.Parameter("isMobileValided", SqlDbType.Bit, 0, ParameterDirection.Input, isMobileValided),
                new MSSQL.Parameter("isPrivacy", SqlDbType.Bit, 0, ParameterDirection.Input, isPrivacy),
                new MSSQL.Parameter("isCanLogin", SqlDbType.Bit, 0, ParameterDirection.Input, isCanLogin),
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

        public static int P_UserEditByName(string ConnectionString, long SiteID, long UserID, string Name, string RealityName, string Password, string PasswordAdv, int CityID, string Sex, DateTime BirthDay, string IDCardNumber, string Address, string Email, bool isEmailValided, string QQ, string Telephone, string Mobile, bool isMobileValided, bool isPrivacy, bool isCanLogin, short UserType, short BankType, string BankName, string BankCardNumber, double ScoringOfSelfBuy, double ScoringOfCommendBuy, short Level, string AlipayID, string AlipayName, bool isAlipayNameValided, double PromotionMemberBonusScale, double PromotionSiteBonusScale, bool IsCrossLogin, ref int ReturnValue, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_UserEditByName(ConnectionString, ref ds, SiteID, UserID, Name, RealityName, Password, PasswordAdv, CityID, Sex, BirthDay, IDCardNumber, Address, Email, isEmailValided, QQ, Telephone, Mobile, isMobileValided, isPrivacy, isCanLogin, UserType, BankType, BankName, BankCardNumber, ScoringOfSelfBuy, ScoringOfCommendBuy, Level, AlipayID, AlipayName, isAlipayNameValided, PromotionMemberBonusScale, PromotionSiteBonusScale, IsCrossLogin, ref ReturnValue, ref ReturnDescription);
        }

        public static int P_UserEditByName(string ConnectionString, ref DataSet ds, long SiteID, long UserID, string Name, string RealityName, string Password, string PasswordAdv, int CityID, string Sex, DateTime BirthDay, string IDCardNumber, string Address, string Email, bool isEmailValided, string QQ, string Telephone, string Mobile, bool isMobileValided, bool isPrivacy, bool isCanLogin, short UserType, short BankType, string BankName, string BankCardNumber, double ScoringOfSelfBuy, double ScoringOfCommendBuy, short Level, string AlipayID, string AlipayName, bool isAlipayNameValided, double PromotionMemberBonusScale, double PromotionSiteBonusScale, bool IsCrossLogin, ref int ReturnValue, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(ConnectionString, "P_UserEditByName", ref ds, ref Outputs,
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
                new MSSQL.Parameter("Telephone", SqlDbType.VarChar, 0, ParameterDirection.Input, Telephone),
                new MSSQL.Parameter("Mobile", SqlDbType.VarChar, 0, ParameterDirection.Input, Mobile),
                new MSSQL.Parameter("isMobileValided", SqlDbType.Bit, 0, ParameterDirection.Input, isMobileValided),
                new MSSQL.Parameter("isPrivacy", SqlDbType.Bit, 0, ParameterDirection.Input, isPrivacy),
                new MSSQL.Parameter("isCanLogin", SqlDbType.Bit, 0, ParameterDirection.Input, isCanLogin),
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

        public static int P_UserForInitiateFollowSchemeDelete(string ConnectionString, long SiteID, long UsersForInitiateFollowSchemeID, ref int ReturnValue, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_UserForInitiateFollowSchemeDelete(ConnectionString, ref ds, SiteID, UsersForInitiateFollowSchemeID, ref ReturnValue, ref ReturnDescription);
        }

        public static int P_UserForInitiateFollowSchemeDelete(string ConnectionString, ref DataSet ds, long SiteID, long UsersForInitiateFollowSchemeID, ref int ReturnValue, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(ConnectionString, "P_UserForInitiateFollowSchemeDelete", ref ds, ref Outputs,
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

        public static int P_UserForInitiateFollowSchemeEdit(string ConnectionString, long SiteID, long UsersForInitiateFollowSchemeID, string Description, int MaxNumberOf, ref int ReturnValue, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_UserForInitiateFollowSchemeEdit(ConnectionString, ref ds, SiteID, UsersForInitiateFollowSchemeID, Description, MaxNumberOf, ref ReturnValue, ref ReturnDescription);
        }

        public static int P_UserForInitiateFollowSchemeEdit(string ConnectionString, ref DataSet ds, long SiteID, long UsersForInitiateFollowSchemeID, string Description, int MaxNumberOf, ref int ReturnValue, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(ConnectionString, "P_UserForInitiateFollowSchemeEdit", ref ds, ref Outputs,
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

        public static int P_UserForInitiateFollowSchemeTry(string ConnectionString, long SiteID, long UserID, int PlayTypeID, string Description, ref long NewUserForInitiateFollowSchemeTryID, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_UserForInitiateFollowSchemeTry(ConnectionString, ref ds, SiteID, UserID, PlayTypeID, Description, ref NewUserForInitiateFollowSchemeTryID, ref ReturnDescription);
        }

        public static int P_UserForInitiateFollowSchemeTry(string ConnectionString, ref DataSet ds, long SiteID, long UserID, int PlayTypeID, string Description, ref long NewUserForInitiateFollowSchemeTryID, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(ConnectionString, "P_UserForInitiateFollowSchemeTry", ref ds, ref Outputs,
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

        public static int P_UserForInitiateFollowSchemeTryHandle(string ConnectionString, long SiteID, long UserForInitiateFollowSchemeTryID, short HandleResult, string Description, int MaxNumberOf, ref int ReturnValue, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_UserForInitiateFollowSchemeTryHandle(ConnectionString, ref ds, SiteID, UserForInitiateFollowSchemeTryID, HandleResult, Description, MaxNumberOf, ref ReturnValue, ref ReturnDescription);
        }

        public static int P_UserForInitiateFollowSchemeTryHandle(string ConnectionString, ref DataSet ds, long SiteID, long UserForInitiateFollowSchemeTryID, short HandleResult, string Description, int MaxNumberOf, ref int ReturnValue, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(ConnectionString, "P_UserForInitiateFollowSchemeTryHandle", ref ds, ref Outputs,
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

        public static int P_UserLogOut(string ConnectionString, long SiteID, long UserID, string Reason, ref int ReturnValue, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_UserLogOut(ConnectionString, ref ds, SiteID, UserID, Reason, ref ReturnValue, ref ReturnDescription);
        }

        public static int P_UserLogOut(string ConnectionString, ref DataSet ds, long SiteID, long UserID, string Reason, ref int ReturnValue, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(ConnectionString, "P_UserLogOut", ref ds, ref Outputs,
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

        public static int P_UserPaySMSCost(string ConnectionString, long SiteID, long UserID, string Mobile, int Num, ref int ReturnValue, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_UserPaySMSCost(ConnectionString, ref ds, SiteID, UserID, Mobile, Num, ref ReturnValue, ref ReturnDescription);
        }

        public static int P_UserPaySMSCost(string ConnectionString, ref DataSet ds, long SiteID, long UserID, string Mobile, int Num, ref int ReturnValue, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(ConnectionString, "P_UserPaySMSCost", ref ds, ref Outputs,
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

        public static int P_ViewUserBonus(string ConnectionString, long userid, ref int ReturnValue, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_ViewUserBonus(ConnectionString, ref ds, userid, ref ReturnValue, ref ReturnDescription);
        }

        public static int P_ViewUserBonus(string ConnectionString, ref DataSet ds, long userid, ref int ReturnValue, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(ConnectionString, "P_ViewUserBonus", ref ds, ref Outputs,
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

        public static int P_Win(string ConnectionString, long IsuseID, string WinLotteryNumber, string OpenAffiche, long OpenOperatorID, bool isEndTheIsuse, ref int SchemeCount, ref int QuashCount, ref int WinCount, ref int WinNoBuyCount, ref bool isEndOpen, ref int ReturnValue, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_Win(ConnectionString, ref ds, IsuseID, WinLotteryNumber, OpenAffiche, OpenOperatorID, isEndTheIsuse, ref SchemeCount, ref QuashCount, ref WinCount, ref WinNoBuyCount, ref isEndOpen, ref ReturnValue, ref ReturnDescription);
        }

        public static int P_Win(string ConnectionString, ref DataSet ds, long IsuseID, string WinLotteryNumber, string OpenAffiche, long OpenOperatorID, bool isEndTheIsuse, ref int SchemeCount, ref int QuashCount, ref int WinCount, ref int WinNoBuyCount, ref bool isEndOpen, ref int ReturnValue, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(ConnectionString, "P_Win", ref ds, ref Outputs,
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

        public static int P_WinByOpenManual(string ConnectionString, long SiteID, long SchemeID, double WinMoney, double WinMoneyNoWithTax, string WinDescription, long OpenOperatorID, ref int ReturnValue, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_WinByOpenManual(ConnectionString, ref ds, SiteID, SchemeID, WinMoney, WinMoneyNoWithTax, WinDescription, OpenOperatorID, ref ReturnValue, ref ReturnDescription);
        }

        public static int P_WinByOpenManual(string ConnectionString, ref DataSet ds, long SiteID, long SchemeID, double WinMoney, double WinMoneyNoWithTax, string WinDescription, long OpenOperatorID, ref int ReturnValue, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(ConnectionString, "P_WinByOpenManual", ref ds, ref Outputs,
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

        public static int P_WriteSchemeChatContents(string ConnectionString, long SiteID, long SchemeID, long FromUserID, long ToUserID, short Type, string Content, ref int ReturnValue, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_WriteSchemeChatContents(ConnectionString, ref ds, SiteID, SchemeID, FromUserID, ToUserID, Type, Content, ref ReturnValue, ref ReturnDescription);
        }

        public static int P_WriteSchemeChatContents(string ConnectionString, ref DataSet ds, long SiteID, long SchemeID, long FromUserID, long ToUserID, short Type, string Content, ref int ReturnValue, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(ConnectionString, "P_WriteSchemeChatContents", ref ds, ref Outputs,
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

        public static int P_WriteSMS(string ConnectionString, long SiteID, long SMSID, string From, string To, string Content, ref long NewSMSID)
        {
            DataSet ds = null;

            return P_WriteSMS(ConnectionString, ref ds, SiteID, SMSID, From, To, Content, ref NewSMSID);
        }

        public static int P_WriteSMS(string ConnectionString, ref DataSet ds, long SiteID, long SMSID, string From, string To, string Content, ref long NewSMSID)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(ConnectionString, "P_WriteSMS", ref ds, ref Outputs,
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

        public static int P_WriteStationSMS(string ConnectionString, long SiteID, long SourceID, long AimID, short Type, string Content, ref long NewSMSID)
        {
            DataSet ds = null;

            return P_WriteStationSMS(ConnectionString, ref ds, SiteID, SourceID, AimID, Type, Content, ref NewSMSID);
        }

        public static int P_WriteStationSMS(string ConnectionString, ref DataSet ds, long SiteID, long SourceID, long AimID, short Type, string Content, ref long NewSMSID)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(ConnectionString, "P_WriteStationSMS", ref ds, ref Outputs,
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

        public static int P_WriteSystemLog(string ConnectionString, long SiteID, long UserID, string IPAddress, short Description)
        {
            DataSet ds = null;

            return P_WriteSystemLog(ConnectionString, ref ds, SiteID, UserID, IPAddress, Description);
        }

        public static int P_WriteSystemLog(string ConnectionString, ref DataSet ds, long SiteID, long UserID, string IPAddress, short Description)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery(ConnectionString, "P_WriteSystemLog", ref ds, ref Outputs,
                new MSSQL.Parameter("SiteID", SqlDbType.BigInt, 0, ParameterDirection.Input, SiteID),
                new MSSQL.Parameter("UserID", SqlDbType.BigInt, 0, ParameterDirection.Input, UserID),
                new MSSQL.Parameter("IPAddress", SqlDbType.VarChar, 0, ParameterDirection.Input, IPAddress),
                new MSSQL.Parameter("Description", SqlDbType.SmallInt, 0, ParameterDirection.Input, Description)
                );

            return CallResult;
        }
    }
}
