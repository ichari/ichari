using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

[ServiceContract]
public interface IModel
{
    [OperationContract]
    void DoWork();

    [OperationContract]
    List<T_Model> GetModel(long UserID, string PlayType, string TypeName);

    [OperationContract]
    int InsertModel(long UserID, string PlayType, string Name, string Content, string Descption, string TypeName);

    [OperationContract]
    List<T_Model> GetModelByID(long ID);

    [OperationContract]
    int DelModelByID(long ID);
}
