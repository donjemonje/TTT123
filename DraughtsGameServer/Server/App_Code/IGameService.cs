using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

// NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IGameService" in both code and config file together.
[ServiceContract(CallbackContract = typeof(ICallBack))]
public interface IGameService
{
    [OperationContract]
    GroupData LogIn(int gameCode);
    [OperationContract(IsOneWay = true)]
    void Status(int gameCode);
    [OperationContract(IsOneWay = true)]
    void Move(int gameCode, int moveFrom, int moveTo, bool win);
    [OperationContract(IsOneWay = true)]
    void WaitingMyTurn(int gameCode);
}

public interface ICallBack
{
    [OperationContract(IsOneWay = true)]
    void Waiting();
    [OperationContract(IsOneWay = true)]
    void InitBoard(GroupData rival, bool yourTurn);
    [OperationContract(IsOneWay = true)]
    void UpdateBoard(int moveFrom, int moveTo);
    [OperationContract(IsOneWay = true)]
    void YouLost();
}

[DataContract]
public class GroupData
{
    [DataMember]
    public string GroupName { get; set; }
    [DataMember]
    public IEnumerable<string> PlayersNames { get; set; }
    [DataMember]
    public int PlayerNo { get; set; }
}
