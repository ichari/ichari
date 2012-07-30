using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using Shove.Database;

public class Model : IModel
{
    public void DoWork()
    {
        // 在此处添加操作实现
        return;
    }

    public List<T_Model> GetModel(long UserID, string PlayType, string TypeName)
    {
        ModelDataContext db1 = new ModelDataContext(Shove._Web.WebConfig.GetAppSettingsString("ConnectionString"));
        var Model = from s in db1.T_Model
                    where s.UserID.Equals(UserID) && s.PlayTypeID.Equals(PlayType) && s.TypeName.Equals(TypeName)
                       select s;
        return Model.ToList<T_Model>();
    }

    public int InsertModel(long UserID, string PlayType, string Name, string Content, string Descption, string TypeName)
    {
        int Result = MSSQL.ExecuteNonQuery("insert into T_Model values (" + UserID + ", '" + Name + "', '" + PlayType + "', '" + Content + "', '" + Descption + "', '" + TypeName + "')");

        return Result;
    }

    public List<T_Model> GetModelByID(long ID)
    {
        ModelDataContext db1 = new ModelDataContext(Shove._Web.WebConfig.GetAppSettingsString("ConnectionString"));
        var Model = from s in db1.T_Model
                    where s.id.Equals(ID)
                    select s;
        return Model.ToList<T_Model>();
    }

    public int DelModelByID(long ID)
    {
        int Result = MSSQL.ExecuteNonQuery("Delete from T_Model where id=" + ID.ToString());

        return Result;
    }
}
